import React, { useEffect, useState } from 'react';
import {
  Table, Button, Modal, Form, Select, App, Typography, Tag,
  Space, Popconfirm, Row, Col, InputNumber, Checkbox, Descriptions, Radio,
} from 'antd';
import {
  PlusOutlined, DeleteOutlined, CheckOutlined, CloseOutlined,
  UserAddOutlined, UserDeleteOutlined, UnorderedListOutlined, ApartmentOutlined,
} from '@ant-design/icons';
import ApplicationKanban from '../components/ApplicationKanban';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { applicationsApi } from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type { Application, ApplicationPlayer } from '../types/index';
import type { LookupDto, LookupItemDto } from '../types/index';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;

const STATUS_COLORS: Record<string, string> = {
  'На рассмотрении': 'blue',
  'Принята': 'green',
  'Отклонена': 'red',
  'Отозвана': 'default',
};

// страница управления заявками на турниры
const ApplicationsPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  // данные
  const [applications, setApplications] = useState<Application[]>([]);
  const [loading, setLoading] = useState(false);
  const [tournaments, setTournaments] = useState<LookupItemDto[]>([]);
  const [teams, setTeams] = useState<LookupItemDto[]>([]);
  const [appStatuses, setAppStatuses] = useState<LookupDto[]>([]);
  const [amplua, setAmplua] = useState<LookupDto[]>([]);

  // режим отображения
  const [viewMode, setViewMode] = useState<'list' | 'kanban'>('list');

  // фильтры
  const [filterTournament, setFilterTournament] = useState<number | undefined>(undefined);
  const [filterTeam, setFilterTeam] = useState<number | undefined>(undefined);

  // модальное окно создания заявки
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [saving, setSaving] = useState(false);
  const [createForm] = Form.useForm();

  // модальное окно просмотра/управления составом
  const [detailModalOpen, setDetailModalOpen] = useState(false);
  const [selectedApp, setSelectedApp] = useState<Application | null>(null);
  const [teamPlayers, setTeamPlayers] = useState<LookupItemDto[]>([]);
  const [playerForm] = Form.useForm();
  const [addingPlayer, setAddingPlayer] = useState(false);
  const [addPlayerOpen, setAddPlayerOpen] = useState(false);

  const loadData = async () => {
    setLoading(true);
    try {
      const [appsData, tourData, teamsData, statusesData, ampluaData] = await Promise.all([
        applicationsApi.getAll({ tournamentId: filterTournament, teamId: filterTeam }),
        lookupsApi.getTournaments(),
        lookupsApi.getTeams(),
        lookupsApi.getApplicationStatuses(),
        lookupsApi.getAmplua(),
      ]);
      setApplications(appsData);
      setTournaments(tourData);
      setTeams(teamsData);
      setAppStatuses(statusesData);
      setAmplua(ampluaData);
    } catch {
      message.error('Ошибка загрузки данных');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { loadData(); }, [filterTournament, filterTeam]);

  // обновление статуса заявки
  const updateStatus = async (app: Application, statusCode: number) => {
    try {
      await applicationsApi.update(app.id, { statusCode });
      message.success('Статус обновлён');
      loadData();
    } catch {
      message.error('Ошибка обновления статуса');
    }
  };

  // смена статуса из kanban (async для ApplicationKanban)
  const handleKanbanStatusChange = async (appId: number, statusCode: number) => {
    await applicationsApi.update(appId, { statusCode });
    await loadData();
  };

  // удаление заявки
  const handleDelete = async (id: number) => {
    try {
      await applicationsApi.delete(id);
      message.success('Заявка удалена');
      loadData();
    } catch {
      message.error('Ошибка удаления');
    }
  };

  // создание заявки
  const handleCreate = async () => {
    let values: Record<string, unknown>;
    try {
      values = await createForm.validateFields();
    } catch { return; }
    setSaving(true);
    try {
      await applicationsApi.create({ ...values, players: [] });
      message.success('Заявка создана');
      setCreateModalOpen(false);
      createForm.resetFields();
      loadData();
    } catch {
      message.error('Ошибка создания заявки');
    } finally {
      setSaving(false);
    }
  };

  // открыть детали / состав
  const openDetail = async (app: Application) => {
    setSelectedApp(app);
    setDetailModalOpen(true);
    // загружаем игроков команды
    if (app.teamId) {
      const players = await lookupsApi.getPlayers(app.teamId);
      setTeamPlayers(players);
    }
  };

  // добавить игрока в состав
  const addPlayerToApp = async () => {
    if (!selectedApp) return;
    let values: Record<string, unknown>;
    try {
      values = await playerForm.validateFields();
    } catch { return; }
    setAddingPlayer(true);
    try {
      const updated = await applicationsApi.addPlayer(selectedApp.id, {
        playerId: values.playerId as number,
        shirtNumber: values.shirtNumber as number | undefined,
        ampluaCode: values.ampluaCode as number | undefined,
        isLibero: !!(values.isLibero),
      });
      setSelectedApp(updated);
      playerForm.resetFields();
      setAddPlayerOpen(false);
      message.success('Игрок добавлен');
    } catch {
      message.error('Ошибка добавления игрока');
    } finally {
      setAddingPlayer(false);
    }
  };

  // удалить игрока из состава
  const removePlayerFromApp = async (playerId: number) => {
    if (!selectedApp) return;
    try {
      await applicationsApi.removePlayer(selectedApp.id, playerId);
      setSelectedApp(prev => prev ? { ...prev, players: prev.players.filter(p => p.playerId !== playerId) } : null);
      message.success('Игрок удалён');
    } catch {
      message.error('Ошибка удаления игрока');
    }
  };

  // колонки таблицы заявок
  const columns: ColumnsType<Application> = [
    {
      title: 'Турнир',
      dataIndex: 'tournamentName',
      key: 'tournamentName',
      render: (v: string) => v ?? '—',
      sorter: (a, b) => (a.tournamentName ?? '').localeCompare(b.tournamentName ?? ''),
    },
    {
      title: 'Команда',
      dataIndex: 'teamName',
      key: 'teamName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Статус',
      dataIndex: 'statusName',
      key: 'statusName',
      render: (v: string) => <Tag color={STATUS_COLORS[v] ?? 'default'}>{v ?? '—'}</Tag>,
      filters: appStatuses.map(s => ({ text: s.name, value: s.name })),
      onFilter: (val, rec) => rec.statusName === val,
    },
    {
      title: 'Игроков',
      key: 'playersCount',
      render: (_: unknown, rec: Application) => rec.players?.length ?? 0,
    },
    {
      title: 'Подана',
      dataIndex: 'submittedAt',
      key: 'submittedAt',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
      sorter: (a, b) => (a.submittedAt ?? '').localeCompare(b.submittedAt ?? ''),
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 220,
      fixed: 'right',
      render: (_: unknown, rec: Application) => (
        <Space size={4}>
          <Button size="small" onClick={() => openDetail(rec)}>Состав</Button>
          {can('reviewApplications') && rec.statusCode !== 2 && (
            <Popconfirm title="Принять заявку?" onConfirm={() => updateStatus(rec, 2)} okText="Да">
              <Button size="small" type="primary" icon={<CheckOutlined />} ghost>
                Принять
              </Button>
            </Popconfirm>
          )}
          {can('reviewApplications') && rec.statusCode !== 3 && (
            <Popconfirm title="Отклонить заявку?" onConfirm={() => updateStatus(rec, 3)} okText="Да" okButtonProps={{ danger: true }}>
              <Button size="small" danger icon={<CloseOutlined />} ghost>
                Отклонить
              </Button>
            </Popconfirm>
          )}
          {can('manageApplications') && (
            <Popconfirm title="Удалить заявку?" onConfirm={() => handleDelete(rec.id)} okText="Удалить" okButtonProps={{ danger: true }}>
              <Button size="small" type="text" danger icon={<DeleteOutlined />} />
            </Popconfirm>
          )}
        </Space>
      ),
    },
  ];

  // колонки игроков в составе
  const playerColumns: ColumnsType<ApplicationPlayer> = [
    {
      title: 'Игрок',
      dataIndex: 'playerName',
      key: 'playerName',
      render: (v: string, rec: ApplicationPlayer) => v ?? `ID ${rec.playerId}`,
    },
    {
      title: '№',
      dataIndex: 'shirtNumber',
      key: 'shirtNumber',
      width: 60,
      render: (v: number) => v ?? '—',
    },
    {
      title: 'Амплуа',
      dataIndex: 'ampluaName',
      key: 'ampluaName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Либеро',
      dataIndex: 'isLibero',
      key: 'isLibero',
      render: (v: boolean) => v ? <Tag color="purple">Либеро</Tag> : '—',
    },
    {
      title: '',
      key: 'del',
      width: 50,
      render: (_: unknown, rec: ApplicationPlayer) =>
        can('manageApplications') && selectedApp?.statusCode === 1 ? (
          <Popconfirm title="Удалить игрока?" onConfirm={() => removePlayerFromApp(rec.playerId)} okText="Да">
            <Button type="text" danger size="small" icon={<UserDeleteOutlined />} />
          </Popconfirm>
        ) : null,
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16, flexWrap: 'wrap', gap: 8 }}>
        <Title level={3} style={{ margin: 0 }}>Заявки на турниры</Title>
        <Space wrap>
          <Radio.Group value={viewMode} onChange={e => setViewMode(e.target.value)} buttonStyle="solid" size="small">
            <Radio.Button value="list"><UnorderedListOutlined /> Список</Radio.Button>
            <Radio.Button value="kanban"><ApartmentOutlined /> Kanban</Radio.Button>
          </Radio.Group>
          {can('manageApplications') && (
            <Button type="primary" icon={<PlusOutlined />} onClick={() => { createForm.resetFields(); setCreateModalOpen(true); }}>
              Подать заявку
            </Button>
          )}
        </Space>
      </div>

      {viewMode === 'list' && (
        <>
          {/* фильтры */}
          <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
            <Col xs={24} sm={12} md={8}>
              <Select
                placeholder="Фильтр по турниру"
                allowClear
                style={{ width: '100%' }}
                options={tournaments.map(t => ({ value: Number(t.id), label: t.name }))}
                onChange={setFilterTournament}
                showSearch
                filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
              />
            </Col>
            <Col xs={24} sm={12} md={8}>
              <Select
                placeholder="Фильтр по команде"
                allowClear
                style={{ width: '100%' }}
                options={teams.map(t => ({ value: Number(t.id), label: t.name }))}
                onChange={setFilterTeam}
                showSearch
                filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
              />
            </Col>
          </Row>

          <Table
            rowKey="id"
            dataSource={applications}
            columns={columns}
            loading={loading}
            scroll={{ x: 'max-content' }}
            pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (total) => `Всего: ${total}` }}
          />
        </>
      )}

      {viewMode === 'kanban' && (
        <ApplicationKanban
          applications={applications}
          statuses={appStatuses}
          onStatusChange={handleKanbanStatusChange}
          canReview={!!can('reviewApplications')}
        />
      )}

      {/* модальное окно создания заявки */}
      <Modal
        title="Подать заявку на турнир"
        open={createModalOpen}
        onCancel={() => setCreateModalOpen(false)}
        onOk={handleCreate}
        confirmLoading={saving}
        okText="Подать"
        cancelText="Отмена"
        destroyOnClose
      >
        <Form form={createForm} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item name="tournamentId" label="Турнир" rules={[{ required: true, message: 'Выберите турнир' }]}>
            <Select
              placeholder="Выберите турнир"
              options={tournaments.map(t => ({ value: Number(t.id), label: t.name }))}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
            />
          </Form.Item>
          <Form.Item name="teamId" label="Команда" rules={[{ required: true, message: 'Выберите команду' }]}>
            <Select
              placeholder="Выберите команду"
              options={teams.map(t => ({ value: Number(t.id), label: t.name }))}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
            />
          </Form.Item>
          <Form.Item name="statusCode" label="Статус" initialValue={1}>
            <Select
              options={appStatuses.map(s => ({ value: s.code, label: s.name }))}
            />
          </Form.Item>
        </Form>
      </Modal>

      {/* модальное окно состава */}
      <Modal
        title={selectedApp ? `Состав заявки: ${selectedApp.teamName} → ${selectedApp.tournamentName}` : 'Состав'}
        open={detailModalOpen}
        onCancel={() => { setDetailModalOpen(false); setAddPlayerOpen(false); }}
        footer={null}
        width={700}
        destroyOnClose
      >
        {selectedApp && (
          <>
            <Descriptions size="small" style={{ marginBottom: 16 }} column={2}>
              <Descriptions.Item label="Статус">
                <Tag color={STATUS_COLORS[selectedApp.statusName ?? ''] ?? 'default'}>{selectedApp.statusName}</Tag>
              </Descriptions.Item>
              <Descriptions.Item label="Игроков">{selectedApp.players?.length ?? 0}</Descriptions.Item>
            </Descriptions>

            {can('manageApplications') && selectedApp.statusCode === 1 && (
              <Button
                type="dashed"
                icon={<UserAddOutlined />}
                style={{ marginBottom: 12 }}
                onClick={() => setAddPlayerOpen(!addPlayerOpen)}
              >
                Добавить игрока
              </Button>
            )}

            {addPlayerOpen && (
              <Form form={playerForm} layout="inline" style={{ marginBottom: 12, flexWrap: 'wrap', gap: 8 }}>
                <Form.Item name="playerId" rules={[{ required: true }]}>
                  <Select
                    placeholder="Игрок"
                    style={{ width: 180 }}
                    options={teamPlayers
                      .filter(p => !selectedApp.players?.some(ap => ap.playerId === Number(p.id)))
                      .map(p => ({ value: Number(p.id), label: p.name }))
                    }
                    showSearch
                    filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                  />
                </Form.Item>
                <Form.Item name="shirtNumber">
                  <InputNumber placeholder="Номер" style={{ width: 90 }} min={1} max={99} />
                </Form.Item>
                <Form.Item name="ampluaCode">
                  <Select
                    placeholder="Амплуа"
                    style={{ width: 140 }}
                    allowClear
                    options={amplua.map(a => ({ value: a.code, label: a.name }))}
                  />
                </Form.Item>
                <Form.Item name="isLibero" valuePropName="checked">
                  <Checkbox>Либеро</Checkbox>
                </Form.Item>
                <Form.Item>
                  <Button type="primary" loading={addingPlayer} onClick={addPlayerToApp}>
                    Добавить
                  </Button>
                </Form.Item>
              </Form>
            )}

            <Table
              rowKey="playerId"
              dataSource={selectedApp.players ?? []}
              columns={playerColumns}
              pagination={false}
              size="small"
              locale={{ emptyText: 'Состав не заполнен' }}
            />
          </>
        )}
      </Modal>
    </div>
  );
};

export default ApplicationsPage;
