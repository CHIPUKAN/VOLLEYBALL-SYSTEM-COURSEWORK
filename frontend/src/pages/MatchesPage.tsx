import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col, DatePicker, TimePicker, Tag, Switch,
  InputNumber, Radio,
} from 'antd';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, EyeOutlined,
  UnorderedListOutlined, AppstoreOutlined, CalendarOutlined,
} from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { useNavigate } from 'react-router-dom';
import { matchesApi } from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type { Match, LookupDto, LookupItemDto } from '../types/index';
import MatchKanban from '../components/MatchKanban';
import MatchCalendar from '../components/MatchCalendar';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;
const { RangePicker } = DatePicker;

// цвета статусов матча
const STATUS_COLORS: Record<string, string> = {
  Запланирован: 'blue',
  'В процессе': 'orange',
  Завершён: 'green',
  Отменён: 'red',
  Перенесён: 'gold',
  'Техническое поражение': 'volcano',
};

// страница управления матчами
const MatchesPage: React.FC = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();
  const { can } = useAuth();

  const [matches, setMatches] = useState<Match[]>([]);
  const [stages, setStages] = useState<LookupDto[]>([]);
  const [statuses, setStatuses] = useState<LookupDto[]>([]);
  const [tournaments, setTournaments] = useState<LookupItemDto[]>([]);
  const [teams, setTeams] = useState<LookupItemDto[]>([]);
  const [venues, setVenues] = useState<LookupItemDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [viewMode, setViewMode] = useState<'table' | 'kanban' | 'calendar'>('table');

  // фильтры
  const [searchText, setSearchText] = useState('');
  const [filterTournament, setFilterTournament] = useState<number | undefined>(undefined);
  const [filterStatus, setFilterStatus] = useState<number | undefined>(undefined);
  const [filterDateRange, setFilterDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);

  // модальное окно
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Match | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      const [matchesData, stagesData, statusesData, tournamentsData, teamsData, venuesData] = await Promise.all([
        matchesApi.getAll(),
        lookupsApi.getTournamentStages(),
        lookupsApi.getMatchStatuses(),
        lookupsApi.getTournaments(),
        lookupsApi.getTeams(),
        lookupsApi.getVenues(),
      ]);
      setMatches(matchesData);
      setStages(stagesData);
      setStatuses(statusesData);
      setTournaments(tournamentsData);
      setTeams(teamsData);
      setVenues(venuesData);
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
  const filteredMatches = useMemo(() => {
    return matches.filter((m) => {
      // поиск по командам
      const matchesSearch =
        !searchText ||
        (m.homeTeamName ?? '').toLowerCase().includes(searchText.toLowerCase()) ||
        (m.guestTeamName ?? '').toLowerCase().includes(searchText.toLowerCase());
      const matchesTournament = !filterTournament || m.tournamentId === filterTournament;
      const matchesStatus = !filterStatus || m.statusCode === filterStatus;
      let matchesDate = true;
      if (filterDateRange) {
        const mDate = dayjs(m.matchDate);
        matchesDate = mDate.isAfter(filterDateRange[0].subtract(1, 'day')) &&
          mDate.isBefore(filterDateRange[1].add(1, 'day'));
      }
      return matchesSearch && matchesTournament && matchesStatus && matchesDate;
    });
  }, [matches, searchText, filterTournament, filterStatus, filterDateRange]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Match) => {
    setEditRecord(record);
    form.setFieldsValue({
      tournamentId: record.tournamentId,
      homeTeamId: record.homeTeamId,
      guestTeamId: record.guestTeamId,
      matchDate: dayjs(record.matchDate),
      startTime: record.startTime ? dayjs(`2000-01-01T${record.startTime}`) : undefined,
      venueId: record.venueId,
      stageCode: record.stageCode,
      statusCode: record.statusCode,
      hasVideoChallenge: record.hasVideoChallenge,
      netHeight: record.netHeight,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await matchesApi.delete(id);
      message.success('Матч удалён');
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
    const matchDate = (values.matchDate as dayjs.Dayjs).format('YYYY-MM-DD');
    const startTime = values.startTime
      ? (values.startTime as dayjs.Dayjs).format('HH:mm:ss')
      : undefined;
    const payload: Partial<Match> = {
      tournamentId: values.tournamentId as number,
      homeTeamId: values.homeTeamId as number,
      guestTeamId: values.guestTeamId as number,
      matchDate,
      startTime: startTime ?? '',
      venueId: values.venueId as number,
      stageCode: values.stageCode as number,
      statusCode: values.statusCode as number,
      hasVideoChallenge: values.hasVideoChallenge as boolean ?? false,
      netHeight: values.netHeight as number,
    };
    setSaving(true);
    try {
      let savedId: number | undefined;
      if (editRecord) {
        await matchesApi.update(editRecord.id, payload);
        message.success('Матч обновлён');
        savedId = editRecord.id;
      } else {
        const created = await matchesApi.create(payload);
        message.success('Матч создан');
        savedId = created.id;
      }
      setModalOpen(false);
      loadData();

      // предложить открыть ведение матча если статус "В процессе"
      const inProgressStatus = statuses.find(s => s.name === 'В процессе');
      if (inProgressStatus && payload.statusCode === inProgressStatus.code && savedId) {
        Modal.confirm({
          title: 'Матч начат!',
          content: 'Открыть интерфейс ведения матча сейчас?',
          okText: 'Открыть',
          cancelText: 'Позже',
          onOk: () => {
            navigate(`/matches/${savedId}`);
          },
        });
      }
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const handleStatusChange = async (matchId: number, newStatusCode: number) => {
    await matchesApi.update(matchId, { statusCode: newStatusCode });
    loadData();
  };

  const handleDateDrop = async (matchId: number, newDate: string) => {
    const postponedStatus = statuses.find(s => s.name === 'Перенесён');
    await matchesApi.update(matchId, {
      matchDate: newDate,
      ...(postponedStatus ? { statusCode: postponedStatus.code } : {}),
    });
    loadData();
  };

  const columns: ColumnsType<Match> = [
    {
      title: 'Турнир',
      dataIndex: 'tournamentName',
      key: 'tournamentName',
      render: (v: string, r: Match) => v ?? r.tournamentId,
      width: 180,
    },
    {
      title: 'Хозяева',
      dataIndex: 'homeTeamName',
      key: 'homeTeamName',
      render: (v: string, r: Match) => v ?? r.homeTeamId,
    },
    {
      title: 'Гости',
      dataIndex: 'guestTeamName',
      key: 'guestTeamName',
      render: (v: string, r: Match) => v ?? r.guestTeamId,
    },
    {
      title: 'Дата',
      dataIndex: 'matchDate',
      key: 'matchDate',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
      sorter: (a, b) => a.matchDate.localeCompare(b.matchDate),
    },
    {
      title: 'Время',
      dataIndex: 'startTime',
      key: 'startTime',
      render: (v: string) => v ? v.slice(0, 5) : '—',
      width: 80,
    },
    {
      title: 'Площадка',
      dataIndex: 'venueName',
      key: 'venueName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Этап',
      dataIndex: 'stageName',
      key: 'stageName',
      render: (v: string, r: Match) => v ?? r.stageCode,
    },
    {
      title: 'Статус',
      dataIndex: 'statusName',
      key: 'statusName',
      render: (v: string) => (
        <Tag color={STATUS_COLORS[v] ?? 'default'}>{v ?? '—'}</Tag>
      ),
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 130,
      fixed: 'right',
      render: (_: unknown, record: Match) => (
        <Space>
          <Button
            type="text"
            icon={<EyeOutlined />}
            onClick={() => navigate(`/matches/${record.id}`)}
            size="small"
            title="Подробнее"
          />
          <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          <Popconfirm
            title="Удалить матч?"
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
        <Title level={3} style={{ margin: 0 }}>Матчи</Title>
        <Space wrap>
          <Radio.Group value={viewMode} onChange={e => setViewMode(e.target.value)} buttonStyle="solid" size="small">
            <Radio.Button value="table"><UnorderedListOutlined /> Список</Radio.Button>
            <Radio.Button value="kanban"><AppstoreOutlined /> Kanban</Radio.Button>
            <Radio.Button value="calendar"><CalendarOutlined /> Календарь</Radio.Button>
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
            placeholder="Поиск по команде"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Select
            placeholder="Фильтр по турниру"
            allowClear
            style={{ width: '100%' }}
            onChange={setFilterTournament}
            options={tournaments.map((t) => ({ value: Number(t.id), label: t.name }))}
            showSearch
            filterOption={(input, opt) =>
              (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
            }
          />
        </Col>
        <Col xs={24} sm={12} md={4}>
          <Select
            placeholder="Статус"
            allowClear
            style={{ width: '100%' }}
            onChange={setFilterStatus}
            options={statuses.map((s) => ({ value: s.code, label: s.name }))}
          />
        </Col>
        <Col xs={24} sm={12} md={8}>
          <RangePicker
            style={{ width: '100%' }}
            format="DD.MM.YYYY"
            onChange={(dates) =>
              setFilterDateRange(dates ? [dates[0]!, dates[1]!] : null)
            }
          />
        </Col>
      </Row>

      {viewMode === 'table' && (
        <Table
          rowKey="id"
          dataSource={filteredMatches}
          columns={columns}
          loading={loading}
          scroll={{ x: 'max-content' }}
          pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
        />
      )}
      {viewMode === 'kanban' && (
        <MatchKanban
          matches={filteredMatches}
          statuses={statuses}
          onStatusChange={handleStatusChange}
          onMatchClick={id => navigate(`/matches/${id}`)}
          canManage={can('manageMatches')}
          onEdit={record => handleEdit(record)}
          onDelete={id => handleDelete(id)}
        />
      )}
      {viewMode === 'calendar' && (
        <MatchCalendar
          matches={filteredMatches}
          onMatchClick={id => navigate(`/matches/${id}`)}
          onDateDrop={handleDateDrop}
          canManage={can('manageMatches')}
        />
      )}

      {/* модальное окно */}
      <Modal
        title={editRecord ? 'Редактировать матч' : 'Новый матч'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={760}
        destroyOnClose
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="tournamentId" label="Турнир" rules={[{ required: true, message: 'Выберите турнир' }]}>
                <Select
                  placeholder="Выберите турнир"
                  options={tournaments.map((t) => ({ value: Number(t.id), label: t.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="matchDate" label="Дата матча" rules={[{ required: true, message: 'Выберите дату' }]}>
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="startTime" label="Время начала" rules={[{ required: true, message: 'Укажите время' }]}>
                <TimePicker style={{ width: '100%' }} format="HH:mm" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="homeTeamId" label="Команда-хозяин" rules={[{ required: true, message: 'Выберите команду' }]}>
                <Select
                  placeholder="Хозяева"
                  options={teams.map((t) => ({ value: Number(t.id), label: t.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="guestTeamId" label="Команда-гость" rules={[{ required: true, message: 'Выберите команду' }]}>
                <Select
                  placeholder="Гости"
                  options={teams.map((t) => ({ value: Number(t.id), label: t.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={8}>
              <Form.Item name="venueId" label="Площадка" rules={[{ required: true, message: 'Выберите площадку' }]}>
                <Select
                  placeholder="Площадка"
                  options={venues.map((v) => ({ value: Number(v.id), label: v.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="stageCode" label="Этап" rules={[{ required: true, message: 'Выберите этап' }]}>
                <Select
                  placeholder="Этап турнира"
                  options={stages.map((s) => ({ value: s.code, label: s.name }))}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="statusCode" label="Статус" rules={[{ required: true, message: 'Выберите статус' }]}>
                <Select
                  placeholder="Статус матча"
                  options={statuses.map((s) => ({ value: s.code, label: s.name }))}
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={8}>
              <Form.Item name="netHeight" label="Высота сетки (м)">
                <InputNumber min={2.20} max={2.50} step={0.01} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="hasVideoChallenge" label="Видеочеллендж" valuePropName="checked" initialValue={false}>
                <Switch />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default MatchesPage;
