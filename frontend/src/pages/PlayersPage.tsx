import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col, InputNumber, DatePicker, Tag, Radio,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, UnorderedListOutlined, AppstoreOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { playersApi, playerStatsApi } from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type { Player, LookupDto, LookupItemDto, PlayerStats } from '../types/index';
import PlayerCard from '../components/PlayerCard';
import PlayerRadarChart from '../components/PlayerRadarChart';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;

// цвета статусов игрока
const STATUS_COLORS: Record<string, string> = {
  Активный: 'green',
  Дисквалифицирован: 'red',
  Травмирован: 'orange',
  Завершил: 'default',
};

// страница управления игроками
const PlayersPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  const [players, setPlayers] = useState<Player[]>([]);
  const [amplua, setAmplua] = useState<LookupDto[]>([]);
  const [statuses, setStatuses] = useState<LookupDto[]>([]);
  const [teams, setTeams] = useState<LookupItemDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [viewMode, setViewMode] = useState<'table' | 'cards'>('table');
  const [draggedPlayer, setDraggedPlayer] = useState<Player | null>(null);
  const [radarPlayer, setRadarPlayer] = useState<Player | null>(null);
  const [radarStats, setRadarStats] = useState<PlayerStats | null>(null);
  const [radarOpen, setRadarOpen] = useState(false);

  // фильтры
  const [searchText, setSearchText] = useState('');
  const [filterTeam, setFilterTeam] = useState<number | undefined>(undefined);
  const [filterAmplua, setFilterAmplua] = useState<number | undefined>(undefined);
  const [filterStatus, setFilterStatus] = useState<number | undefined>(undefined);

  // модальное окно
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Player | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      const [playersData, ampluaData, statusesData, teamsData] = await Promise.all([
        playersApi.getAll(),
        lookupsApi.getAmplua(),
        lookupsApi.getPlayerStatuses(),
        lookupsApi.getTeams(),
      ]);
      setPlayers(playersData);
      setAmplua(ampluaData);
      setStatuses(statusesData);
      setTeams(teamsData);
    } catch {
      message.error('Ошибка загрузки данных');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  // клиентская фильтрация
  const filteredPlayers = useMemo(() => {
    return players.filter((p) => {
      const fullName = p.fullName ?? `${p.lastName} ${p.firstName}`;
      const matchesSearch = !searchText || fullName.toLowerCase().includes(searchText.toLowerCase());
      const matchesTeam = !filterTeam || p.teamId === filterTeam;
      const matchesAmplua = !filterAmplua || p.ampluaCode === filterAmplua;
      const matchesStatus = !filterStatus || p.statusCode === filterStatus;
      return matchesSearch && matchesTeam && matchesAmplua && matchesStatus;
    });
  }, [players, searchText, filterTeam, filterAmplua, filterStatus]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Player) => {
    setEditRecord(record);
    form.setFieldsValue({
      lastName: record.lastName,
      firstName: record.firstName,
      middleName: record.middleName,
      teamId: record.teamId,
      ampluaCode: record.ampluaCode,
      statusCode: record.statusCode,
      shirtNumber: record.shirtNumber,
      height: record.height,
      weight: record.weight,
      birthDate: record.birthDate ? dayjs(record.birthDate) : undefined,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await playersApi.delete(id);
      message.success('Игрок удалён');
      loadData();
    } catch {
      message.error('Ошибка удаления');
    }
  };

  const handleSave = async () => {
    let values: Record<string, unknown>;
    try {
      values = await form.validateFields();
    } catch {
      return;
    }
    // форматируем дату
    if (values.birthDate) {
      values.birthDate = (values.birthDate as dayjs.Dayjs).format('YYYY-MM-DD');
    }
    setSaving(true);
    try {
      if (editRecord) {
        await playersApi.update(editRecord.id, values);
        message.success('Игрок обновлён');
      } else {
        await playersApi.create(values);
        message.success('Игрок добавлен');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const handlePlayerClick = async (player: Player) => {
    setRadarPlayer(player);
    setRadarOpen(true);
    try {
      // загрузить все матчи игрока из всех матчей — упрощённо суммируем статистику
      // на практике нужен отдельный endpoint; здесь используем что есть
      const allStats = await playerStatsApi.getAll(0).catch(() => [] as PlayerStats[]);
      const playerStats = allStats.filter(s => s.playerId === player.id);
      if (playerStats.length > 0) {
        const summed: PlayerStats = playerStats.reduce((acc, s) => ({
          ...acc,
          servesTotal: acc.servesTotal + s.servesTotal,
          aces: acc.aces + s.aces,
          serveErrors: acc.serveErrors + s.serveErrors,
          receptionsTotal: acc.receptionsTotal + s.receptionsTotal,
          positiveReceptions: acc.positiveReceptions + s.positiveReceptions,
          receptionErrors: acc.receptionErrors + s.receptionErrors,
          attacksTotal: acc.attacksTotal + s.attacksTotal,
          attackPoints: acc.attackPoints + s.attackPoints,
          attackErrors: acc.attackErrors + s.attackErrors,
          blocks: acc.blocks + s.blocks,
          totalPoints: acc.totalPoints + s.totalPoints,
        }), { ...playerStats[0] });
        setRadarStats(summed);
      } else {
        setRadarStats(null);
      }
    } catch {
      setRadarStats(null);
    }
  };

  const handleTeamDrop = async (teamId: number, teamName: string) => {
    if (!draggedPlayer || !can('managePlayers')) return;
    Modal.confirm({
      title: `Перевести игрока в ${teamName}?`,
      content: `${draggedPlayer.fullName ?? draggedPlayer.lastName} будет переведён в команду ${teamName}`,
      okText: 'Перевести',
      cancelText: 'Отмена',
      onOk: async () => {
        try {
          await playersApi.update(draggedPlayer.id, { teamId });
          message.success('Игрок переведён');
          loadData();
        } catch {
          message.error('Ошибка перевода');
        }
      },
    });
    setDraggedPlayer(null);
  };

  const columns: ColumnsType<Player> = [
    {
      title: 'ФИО',
      key: 'fullName',
      render: (_: unknown, r: Player) => r.fullName ?? `${r.lastName} ${r.firstName} ${r.middleName ?? ''}`.trim(),
      sorter: (a, b) => (a.fullName ?? a.lastName).localeCompare(b.fullName ?? b.lastName),
    },
    {
      title: 'Команда',
      dataIndex: 'teamName',
      key: 'teamName',
      render: (name: string) => name ?? '—',
    },
    {
      title: 'Амплуа',
      dataIndex: 'ampluaName',
      key: 'ampluaName',
      render: (name: string) => name ?? '—',
    },
    {
      title: 'Статус',
      dataIndex: 'statusName',
      key: 'statusName',
      render: (name: string) => (
        <Tag color={STATUS_COLORS[name] ?? 'default'}>{name ?? '—'}</Tag>
      ),
    },
    {
      title: '№',
      dataIndex: 'shirtNumber',
      key: 'shirtNumber',
      width: 60,
      render: (n: number) => n ?? '—',
    },
    {
      title: 'Рост/Вес',
      key: 'sizes',
      render: (_: unknown, r: Player) => {
        const parts = [];
        if (r.height) parts.push(`${r.height} см`);
        if (r.weight) parts.push(`${r.weight} кг`);
        return parts.join(' / ') || '—';
      },
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: Player) => (
        <Space>
          <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          <Popconfirm
            title="Удалить игрока?"
            onConfirm={() => handleDelete(record.id)}
            okText="Удалить"
            cancelText="Отмена"
            okButtonProps={{ danger: true }}
          >
            <Button type="text" icon={<DeleteOutlined />} danger size="small" />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16, flexWrap: 'wrap', gap: 8 }}>
        <Title level={3} style={{ margin: 0 }}>Игроки</Title>
        <Space wrap>
          <Radio.Group value={viewMode} onChange={e => setViewMode(e.target.value)} buttonStyle="solid" size="small">
            <Radio.Button value="table"><UnorderedListOutlined /> Список</Radio.Button>
            <Radio.Button value="cards"><AppstoreOutlined /> Карточки</Radio.Button>
          </Radio.Group>
          <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
            Добавить
          </Button>
        </Space>
      </div>

      {/* фильтры */}
      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={6}>
          <Search
            placeholder="Поиск по ФИО"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Select
            placeholder="Фильтр по команде"
            allowClear
            style={{ width: '100%' }}
            onChange={setFilterTeam}
            options={teams.map((t) => ({ value: Number(t.id), label: t.name }))}
          />
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Select
            placeholder="Фильтр по амплуа"
            allowClear
            style={{ width: '100%' }}
            onChange={setFilterAmplua}
            options={amplua.map((a) => ({ value: a.code, label: a.name }))}
          />
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Select
            placeholder="Фильтр по статусу"
            allowClear
            style={{ width: '100%' }}
            onChange={setFilterStatus}
            options={statuses.map((s) => ({ value: s.code, label: s.name }))}
          />
        </Col>
      </Row>

      {viewMode === 'table' && (
        <Table
          rowKey="id"
          dataSource={filteredPlayers}
          columns={columns}
          loading={loading}
          scroll={{ x: 'max-content' }}
          pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
          onRow={record => ({ onClick: () => handlePlayerClick(record) })}
        />
      )}

      {viewMode === 'cards' && (() => {
        const grouped = new Map<string, Player[]>();
        filteredPlayers.forEach(p => {
          const team = p.teamName ?? 'Без команды';
          if (!grouped.has(team)) grouped.set(team, []);
          grouped.get(team)!.push(p);
        });
        return (
          <div>
            {Array.from(grouped.entries()).map(([teamName, teamPlayers]) => {
              const teamId = teamPlayers[0]?.teamId ?? 0;
              return (
                <div
                  key={teamName}
                  onDragOver={e => e.preventDefault()}
                  onDrop={() => handleTeamDrop(teamId, teamName)}
                  style={{ marginBottom: 24 }}
                >
                  <Typography.Text strong style={{ fontSize: 15, display: 'block', marginBottom: 8 }}>
                    {teamName}
                  </Typography.Text>
                  <div style={{ display: 'flex', flexWrap: 'wrap', gap: 12 }}>
                    {teamPlayers.map(p => (
                      <PlayerCard
                        key={p.id}
                        player={p}
                        draggable={can('managePlayers')}
                        onDragStart={setDraggedPlayer}
                        onClick={handlePlayerClick}
                      />
                    ))}
                  </div>
                </div>
              );
            })}
          </div>
        );
      })()}

      {/* радар-карта игрока */}
      <Modal
        open={radarOpen}
        title={radarPlayer ? `${radarPlayer.fullName ?? radarPlayer.lastName} — статистика` : 'Статистика'}
        onCancel={() => { setRadarOpen(false); setRadarPlayer(null); setRadarStats(null); }}
        footer={null}
        width={320}
      >
        {radarStats ? (
          <div style={{ display: 'flex', justifyContent: 'center', padding: '12px 0' }}>
            <PlayerRadarChart stats={radarStats} size={240} />
          </div>
        ) : (
          <Typography.Text type="secondary">Статистика не найдена</Typography.Text>
        )}
      </Modal>

      {/* модальное окно */}
      <Modal
        title={editRecord ? 'Редактировать игрока' : 'Новый игрок'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={700}
        destroyOnClose
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={16}>
            <Col xs={24} sm={8}>
              <Form.Item name="lastName" label="Фамилия" rules={[{ required: true, message: 'Обязательное поле' }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="firstName" label="Имя" rules={[{ required: true, message: 'Обязательное поле' }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="middleName" label="Отчество">
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={8}>
              <Form.Item name="teamId" label="Команда" rules={[{ required: true, message: 'Выберите команду' }]}>
                <Select
                  placeholder="Выберите команду"
                  options={teams.map((t) => ({ value: Number(t.id), label: t.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="ampluaCode" label="Амплуа" rules={[{ required: true, message: 'Выберите амплуа' }]}>
                <Select
                  placeholder="Амплуа"
                  options={amplua.map((a) => ({ value: a.code, label: a.name }))}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="statusCode" label="Статус" rules={[{ required: true, message: 'Выберите статус' }]}>
                <Select
                  placeholder="Статус"
                  options={statuses.map((s) => ({ value: s.code, label: s.name }))}
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={6}>
              <Form.Item name="shirtNumber" label="Номер (футболка)">
                <InputNumber min={1} max={99} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="height" label="Рост (см)">
                <InputNumber min={150} max={230} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="weight" label="Вес (кг)">
                <InputNumber min={50} max={160} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="birthDate" label="Дата рождения">
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default PlayersPage;
