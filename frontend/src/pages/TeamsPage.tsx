import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col, Avatar, Tree, Tag, Tooltip,
  Segmented, DatePicker,
} from 'antd';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, UserOutlined,
  TeamOutlined, TableOutlined, ApartmentOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import type { DataNode } from 'antd/es/tree';
import dayjs from 'dayjs';
import { teamsApi, playersApi } from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type { Team, Player } from '../types/index';
import type { LookupItemDto } from '../types/index';
import { useAuth } from '../context/AuthContext';

const { Title, Text } = Typography;
const { Search } = Input;

// страница управления командами
const TeamsPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  // данные
  const [teams, setTeams] = useState<Team[]>([]);
  const [players, setPlayers] = useState<Player[]>([]);
  const [regions, setRegions] = useState<LookupItemDto[]>([]);
  const [coaches, setCoaches] = useState<LookupItemDto[]>([]);
  const [venues, setVenues] = useState<LookupItemDto[]>([]);
  const [loading, setLoading] = useState(false);

  // вид: таблица или дерево
  const [viewMode, setViewMode] = useState<string>('table');

  // фильтры
  const [searchText, setSearchText] = useState('');
  const [filterRegion, setFilterRegion] = useState<string | undefined>(undefined);

  // модальное окно команды
  const [teamModalOpen, setTeamModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Team | null>(null);
  const [savingTeam, setSavingTeam] = useState(false);
  const [teamForm] = Form.useForm();

  // модальное окно игрока (для дерева)
  const [playerModalOpen, setPlayerModalOpen] = useState(false);
  const [editPlayer, setEditPlayer] = useState<Player | null>(null);
  const [savingPlayer, setSavingPlayer] = useState(false);
  const [playerForm] = Form.useForm();
  const [ampluaLookup, setAmpluaLookup] = useState<Array<{ code: number; name: string }>>([]);
  const [playerStatusLookup, setPlayerStatusLookup] = useState<Array<{ code: number; name: string }>>([]);

  const loadData = async () => {
    setLoading(true);
    try {
      const [teamsData, regionsData, coachesData, venuesData] = await Promise.all([
        teamsApi.getAll(),
        lookupsApi.getRegions(),
        lookupsApi.getCoaches(),
        lookupsApi.getVenues(),
      ]);
      setTeams(teamsData);
      setRegions(regionsData);
      setCoaches(coachesData);
      setVenues(venuesData);
    } catch {
      message.error('Ошибка загрузки данных');
    } finally {
      setLoading(false);
    }
  };

  const loadPlayers = async () => {
    try {
      const [playersData, ampluaData, statusData] = await Promise.all([
        playersApi.getAll(),
        lookupsApi.getAmplua(),
        lookupsApi.getPlayerStatuses(),
      ]);
      setPlayers(playersData);
      setAmpluaLookup(ampluaData);
      setPlayerStatusLookup(statusData);
    } catch {
      message.error('Ошибка загрузки игроков');
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (viewMode === 'tree') {
      loadPlayers();
    }
  }, [viewMode]);

  const filteredTeams = useMemo(() => {
    return teams.filter((t) => {
      const matchesSearch = !searchText || t.name.toLowerCase().includes(searchText.toLowerCase());
      const matchesRegion = !filterRegion || t.regionOktmo === filterRegion;
      return matchesSearch && matchesRegion;
    });
  }, [teams, searchText, filterRegion]);

  // операции с командами
  const handleCreateTeam = () => {
    setEditRecord(null);
    teamForm.resetFields();
    setTeamModalOpen(true);
  };

  const handleEditTeam = (record: Team) => {
    setEditRecord(record);
    teamForm.setFieldsValue({
      name: record.name,
      logoUrl: record.logoUrl,
      regionOktmo: record.regionOktmo,
      headCoachId: record.headCoachId,
      homeVenueId: record.homeVenueId,
    });
    setTeamModalOpen(true);
  };

  const handleDeleteTeam = async (id: number) => {
    try {
      await teamsApi.delete(id);
      message.success('Команда удалена');
      loadData();
      if (viewMode === 'tree') loadPlayers();
    } catch {
      message.error('Ошибка удаления');
    }
  };

  const handleSaveTeam = async () => {
    let values: Record<string, unknown>;
    try {
      values = await teamForm.validateFields();
    } catch { return; }
    setSavingTeam(true);
    try {
      if (editRecord) {
        await teamsApi.update(editRecord.id, values);
        message.success('Команда обновлена');
      } else {
        await teamsApi.create(values);
        message.success('Команда создана');
      }
      setTeamModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSavingTeam(false);
    }
  };

  // операции с игроками (для дерева)
  const handleCreatePlayer = (teamId: number) => {
    setEditPlayer(null);
    playerForm.resetFields();
    playerForm.setFieldsValue({ teamId, statusCode: 1 });
    setPlayerModalOpen(true);
  };

  const handleEditPlayer = (player: Player) => {
    setEditPlayer(player);
    playerForm.setFieldsValue({
      firstName: player.firstName,
      lastName: player.lastName,
      middleName: player.middleName,
      ampluaCode: player.ampluaCode,
      statusCode: player.statusCode,
      jerseyNumber: player.jerseyNumber,
      teamId: player.teamId,
      birthDate: player.birthDate ? dayjs(player.birthDate) : undefined,
    });
    setPlayerModalOpen(true);
  };

  const handleDeletePlayer = async (id: number) => {
    try {
      await playersApi.delete(id);
      message.success('Игрок удалён');
      loadPlayers();
    } catch {
      message.error('Ошибка удаления');
    }
  };

  const handleSavePlayer = async () => {
    let values: Record<string, unknown>;
    try {
      values = await playerForm.validateFields();
    } catch { return; }
    if (values.birthDate) {
      values.birthDate = (values.birthDate as dayjs.Dayjs).format('YYYY-MM-DD');
    }
    setSavingPlayer(true);
    try {
      if (editPlayer) {
        await playersApi.update(editPlayer.id, values);
        message.success('Игрок обновлён');
      } else {
        await playersApi.create(values);
        message.success('Игрок добавлен');
      }
      setPlayerModalOpen(false);
      loadPlayers();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSavingPlayer(false);
    }
  };

  // перенос игрока в другую команду (drag & drop в дереве)
  const handleTreeDrop = async (info: {
    node: DataNode;
    dragNode: DataNode;
    dropPosition: number;
  }) => {
    const dragKey = String(info.dragNode.key);
    const dropKey = String(info.node.key);
    if (!dragKey.startsWith('player-')) return;

    const playerId = Number(dragKey.replace('player-', ''));
    let targetTeamId: number | null = null;

    if (dropKey.startsWith('team-')) {
      targetTeamId = Number(dropKey.replace('team-', ''));
    } else if (dropKey.startsWith('player-')) {
      const targetPlayerId = Number(dropKey.replace('player-', ''));
      const targetPlayer = players.find(p => p.id === targetPlayerId);
      if (targetPlayer) targetTeamId = targetPlayer.teamId;
    }

    if (!targetTeamId) return;
    const player = players.find(p => p.id === playerId);
    if (!player || player.teamId === targetTeamId) return;

    try {
      await playersApi.update(playerId, { ...player, teamId: targetTeamId });
      message.success('Игрок переведён в другую команду');
      loadPlayers();
    } catch {
      message.error('Ошибка перевода игрока');
    }
  };

  // построение данных для дерева
  const treeData: DataNode[] = useMemo(() => {
    return filteredTeams.map(team => {
      const teamPlayers = players.filter(p => p.teamId === team.id);
      return {
        key: `team-${team.id}`,
        title: (
          <Space>
            {team.logoUrl ? <Avatar src={team.logoUrl} size={22} /> : <TeamOutlined />}
            <Text strong>{team.name}</Text>
            <Tag style={{ fontSize: 10 }}>{team.regionName ?? team.regionOktmo}</Tag>
            <Text type="secondary" style={{ fontSize: 11 }}>{teamPlayers.length} игроков</Text>
            {can('managePlayers') && (
              <Button
                size="small"
                type="dashed"
                icon={<PlusOutlined />}
                onClick={(e) => { e.stopPropagation(); handleCreatePlayer(team.id); }}
                style={{ height: 22, fontSize: 11 }}
              >
                Добавить игрока
              </Button>
            )}
            {can('manageTeams') && (
              <Button
                size="small"
                type="text"
                icon={<EditOutlined />}
                onClick={(e) => { e.stopPropagation(); handleEditTeam(team); }}
              />
            )}
            {can('manageTeams') && (
              <Popconfirm title="Удалить команду?" onConfirm={() => handleDeleteTeam(team.id)} okText="Удалить" okButtonProps={{ danger: true }}>
                <Button
                  size="small"
                  type="text"
                  danger
                  icon={<DeleteOutlined />}
                  onClick={(e) => e.stopPropagation()}
                />
              </Popconfirm>
            )}
          </Space>
        ),
        icon: <TeamOutlined style={{ color: '#1677ff' }} />,
        children: teamPlayers.map(player => ({
          key: `player-${player.id}`,
          isLeaf: true,
          icon: <UserOutlined style={{ color: '#52c41a' }} />,
          title: (
            <Space>
              <Text>{`#${player.jerseyNumber ?? '—'} ${player.fullName ?? player.lastName}`}</Text>
              {player.ampluaName && <Tag color="cyan" style={{ fontSize: 10 }}>{player.ampluaName}</Tag>}
              {player.statusName && (
                <Tag color={player.statusCode === 1 ? 'green' : player.statusCode === 3 ? 'red' : 'orange'} style={{ fontSize: 10 }}>
                  {player.statusName}
                </Tag>
              )}
              {can('managePlayers') && (
                <Button
                  size="small"
                  type="text"
                  icon={<EditOutlined />}
                  onClick={(e) => { e.stopPropagation(); handleEditPlayer(player); }}
                />
              )}
              {can('managePlayers') && (
                <Popconfirm title="Удалить игрока?" onConfirm={() => handleDeletePlayer(player.id)} okText="Удалить" okButtonProps={{ danger: true }}>
                  <Button size="small" type="text" danger icon={<DeleteOutlined />} onClick={(e) => e.stopPropagation()} />
                </Popconfirm>
              )}
            </Space>
          ),
        })),
      };
    });
  }, [filteredTeams, players, can]);

  // колонки таблицы команд
  const columns: ColumnsType<Team> = [
    {
      title: 'Логотип',
      dataIndex: 'logoUrl',
      key: 'logoUrl',
      width: 70,
      render: (url: string) =>
        url ? <Avatar src={url} size={36} /> : <Avatar icon={<UserOutlined />} size={36} />,
    },
    {
      title: 'Название',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
    },
    {
      title: 'Регион',
      dataIndex: 'regionName',
      key: 'regionName',
      render: (name: string, record: Team) => name ?? record.regionOktmo,
    },
    {
      title: 'Тренер',
      dataIndex: 'headCoachFullName',
      key: 'headCoachFullName',
      render: (name: string) => name ?? '—',
    },
    {
      title: 'Площадка',
      dataIndex: 'homeVenueName',
      key: 'homeVenueName',
      render: (name: string) => name ?? '—',
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 120,
      fixed: 'right',
      render: (_: unknown, record: Team) => (
        <Space>
          {can('manageTeams') && (
            <Tooltip title="Редактировать">
              <Button type="text" icon={<EditOutlined />} onClick={() => handleEditTeam(record)} size="small" />
            </Tooltip>
          )}
          {can('manageTeams') && (
            <Popconfirm
              title="Удалить команду?"
              description="Это действие нельзя отменить."
              onConfirm={() => handleDeleteTeam(record.id)}
              okText="Удалить"
              cancelText="Отмена"
              okButtonProps={{ danger: true }}
            >
              <Button type="text" icon={<DeleteOutlined />} danger size="small" />
            </Popconfirm>
          )}
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16, flexWrap: 'wrap', gap: 8 }}>
        <Title level={3} style={{ margin: 0 }}>Команды</Title>
        <Space>
          <Segmented
            options={[
              { value: 'table', icon: <TableOutlined />, label: 'Таблица' },
              { value: 'tree', icon: <ApartmentOutlined />, label: 'Дерево' },
            ]}
            value={viewMode}
            onChange={setViewMode}
          />
          {can('manageTeams') && (
            <Button type="primary" icon={<PlusOutlined />} onClick={handleCreateTeam}>
              Добавить
            </Button>
          )}
        </Space>
      </div>

      {/* фильтры */}
      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
          <Search
            placeholder="Поиск по названию"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
        <Col xs={24} sm={12} md={8}>
          <Select
            placeholder="Фильтр по региону"
            allowClear
            style={{ width: '100%' }}
            onChange={setFilterRegion}
            options={regions.map((r) => ({ value: r.id, label: r.name }))}
          />
        </Col>
      </Row>

      {viewMode === 'table' ? (
        <Table
          rowKey="id"
          dataSource={filteredTeams}
          columns={columns}
          loading={loading}
          scroll={{ x: 'max-content' }}
          pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (total) => `Всего: ${total}` }}
        />
      ) : (
        <div style={{ background: '#fff', borderRadius: 8, padding: '16px', boxShadow: '0 1px 4px rgba(0,0,0,0.1)' }}>
          <Text type="secondary" style={{ display: 'block', marginBottom: 12, fontSize: 12 }}>
            Перетащите игрока на другую команду для перевода. Список содержит все команды и их игроков.
          </Text>
          {loading ? (
            <div style={{ textAlign: 'center', padding: 40 }}>Загрузка...</div>
          ) : (
            <Tree
              showIcon
              draggable
              blockNode
              defaultExpandAll
              treeData={treeData}
              onDrop={handleTreeDrop}
              style={{ fontSize: 13 }}
            />
          )}
        </div>
      )}

      {/* модальное окно команды */}
      <Modal
        title={editRecord ? 'Редактировать команду' : 'Новая команда'}
        open={teamModalOpen}
        onCancel={() => setTeamModalOpen(false)}
        onOk={handleSaveTeam}
        confirmLoading={savingTeam}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={600}
        destroyOnHidden
      >
        <Form form={teamForm} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={16}>
            <Col xs={24} sm={16}>
              <Form.Item name="name" label="Название команды" rules={[{ required: true, message: 'Введите название' }]}>
                <Input placeholder="ВК Динамо Москва" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="logoUrl" label="URL логотипа">
                <Input placeholder="https://..." />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={8}>
              <Form.Item name="regionOktmo" label="Регион" rules={[{ required: true, message: 'Выберите регион' }]}>
                <Select
                  placeholder="Выберите регион"
                  options={regions.map((r) => ({ value: r.id, label: r.name }))}
                  showSearch
                  filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="headCoachId" label="Главный тренер">
                <Select
                  placeholder="Выберите тренера"
                  allowClear
                  options={coaches.map((c) => ({ value: Number(c.id), label: c.name }))}
                  showSearch
                  filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="homeVenueId" label="Домашняя площадка">
                <Select
                  placeholder="Выберите площадку"
                  allowClear
                  options={venues.map((v) => ({ value: Number(v.id), label: v.name }))}
                  showSearch
                  filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* модальное окно игрока */}
      <Modal
        title={editPlayer ? 'Редактировать игрока' : 'Добавить игрока'}
        open={playerModalOpen}
        onCancel={() => setPlayerModalOpen(false)}
        onOk={handleSavePlayer}
        confirmLoading={savingPlayer}
        okText={editPlayer ? 'Сохранить' : 'Добавить'}
        cancelText="Отмена"
        width={560}
        destroyOnHidden
      >
        <Form form={playerForm} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item name="teamId" hidden><Input /></Form.Item>
          <Row gutter={12}>
            <Col xs={24} sm={8}>
              <Form.Item name="lastName" label="Фамилия" rules={[{ required: true }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="firstName" label="Имя" rules={[{ required: true }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="middleName" label="Отчество">
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col xs={12} sm={6}>
              <Form.Item name="jerseyNumber" label="Номер">
                <Select
                  placeholder="№"
                  options={Array.from({ length: 99 }, (_, i) => ({ value: i + 1, label: String(i + 1) }))}
                  showSearch
                />
              </Form.Item>
            </Col>
            <Col xs={12} sm={6}>
              <Form.Item name="ampluaCode" label="Амплуа">
                <Select
                  placeholder="Выберите"
                  options={ampluaLookup.map(a => ({ value: a.code, label: a.name }))}
                />
              </Form.Item>
            </Col>
            <Col xs={12} sm={6}>
              <Form.Item name="statusCode" label="Статус">
                <Select
                  options={playerStatusLookup.map(s => ({ value: s.code, label: s.name }))}
                />
              </Form.Item>
            </Col>
            <Col xs={12} sm={6}>
              <Form.Item name="birthDate" label="Дата рождения" rules={[{ required: true, message: 'Обязательное' }]}>
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default TeamsPage;
