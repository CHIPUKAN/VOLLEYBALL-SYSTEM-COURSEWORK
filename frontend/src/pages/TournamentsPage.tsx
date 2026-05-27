import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col, DatePicker, InputNumber, Radio, Layout, Card,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, UnorderedListOutlined, ApartmentOutlined, EyeOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { useNavigate } from 'react-router-dom';
import { tournamentsApi } from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type { Tournament, LookupDto, LookupItemDto } from '../types/index';
import TournamentTree from '../components/TournamentTree';
import { useAuth } from '../context/AuthContext';
import { getApiError } from '../utils/apiError';

const { Title } = Typography;
const { Search } = Input;
const { RangePicker } = DatePicker;
const { TextArea } = Input;


// страница управления турнирами
const TournamentsPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();
  const navigate = useNavigate();

  const [viewMode, setViewMode] = useState<'table' | 'tree'>('table');
  const [selectedMatchId, setSelectedMatchId] = useState<number | null>(null);
  const [tournaments, setTournaments] = useState<Tournament[]>([]);
  const [formats, setFormats] = useState<LookupDto[]>([]);
  const [scoringSystems, setScoringSystems] = useState<LookupDto[]>([]);
  const [seasons, setSeasons] = useState<LookupItemDto[]>([]);
  const [organizers, setOrganizers] = useState<LookupItemDto[]>([]);
  const [loading, setLoading] = useState(false);

  // фильтры
  const [searchText, setSearchText] = useState('');
  const [filterSeason, setFilterSeason] = useState<number | undefined>(undefined);
  const [filterFormat, setFilterFormat] = useState<number | undefined>(undefined);

  // модальное окно
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Tournament | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      const [tourData, formatsData, scoringData, seasonsData, organizersData] = await Promise.all([
        tournamentsApi.getAll(),
        lookupsApi.getTournamentFormats(),
        lookupsApi.getScoringSystems(),
        lookupsApi.getSeasons(),
        lookupsApi.getOrganizers(),
      ]);
      setTournaments(tourData);
      setFormats(formatsData);
      setScoringSystems(scoringData);
      setSeasons(seasonsData);
      setOrganizers(organizersData);
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
  const filteredTournaments = useMemo(() => {
    return tournaments.filter((t) => {
      const matchesSearch = !searchText || t.name.toLowerCase().includes(searchText.toLowerCase());
      const matchesSeason = !filterSeason || t.seasonId === filterSeason;
      const matchesFormat = !filterFormat || t.formatCode === filterFormat;
      return matchesSearch && matchesSeason && matchesFormat;
    });
  }, [tournaments, searchText, filterSeason, filterFormat]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Tournament) => {
    setEditRecord(record);
    form.setFieldsValue({
      name: record.name,
      city: record.city,
      dates: [dayjs(record.startDate), dayjs(record.endDate)],
      seasonId: record.seasonId,
      organizerId: record.organizerId,
      formatCode: record.formatCode,
      scoringSystemCode: record.scoringSystemCode,
      maxTeams: record.maxTeams,
      description: record.description,
      gender: record.gender,
      level: record.level,
      maxPlayersPerApp: record.maxPlayersPerApp,
      ageCategory: record.ageCategory,
      applicationDeadline: record.applicationDeadline ? dayjs(record.applicationDeadline) : undefined,
      setsToWin: record.setsToWin,
      tiebreakScoreTarget: record.tiebreakScoreTarget,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await tournamentsApi.delete(id);
      message.success('Турнир удалён');
      loadData();
    } catch (err) {
      message.error(getApiError(err, 'Ошибка удаления турнира'));
    }
  };

  const handleSave = async () => {
    let values: Record<string, unknown>;
    try {
      values = await form.validateFields();
    } catch {
      return;
    }
    const dates = values.dates as [dayjs.Dayjs, dayjs.Dayjs];
    const payload: Partial<Tournament> = {
      name: values.name as string,
      city: values.city as string,
      startDate: dates[0].format('YYYY-MM-DD'),
      endDate: dates[1].format('YYYY-MM-DD'),
      seasonId: values.seasonId as number,
      organizerId: values.organizerId as number,
      formatCode: values.formatCode as number,
      scoringSystemCode: values.scoringSystemCode as number,
      setsToWin: (values.setsToWin as number) ?? 3,
      tiebreakScoreTarget: (values.tiebreakScoreTarget as number) ?? 15,
      maxTeams: values.maxTeams as number,
      description: values.description as string,
      gender: values.gender as string,
      level: values.level as string,
      maxPlayersPerApp: (values.maxPlayersPerApp as number) ?? 12,
      ageCategory: values.ageCategory as string | undefined,
      applicationDeadline: values.applicationDeadline
        ? (values.applicationDeadline as dayjs.Dayjs).format('YYYY-MM-DD')
        : undefined,
    };
    setSaving(true);
    try {
      if (editRecord) {
        await tournamentsApi.update(editRecord.id, payload);
        message.success('Турнир обновлён');
      } else {
        await tournamentsApi.create(payload);
        message.success('Турнир создан');
      }
      setModalOpen(false);
      loadData();
    } catch (err) {
      message.error(getApiError(err, 'Ошибка сохранения турнира'));
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<Tournament> = [
    {
      title: 'Название',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
      width: 220,
    },
    {
      title: 'Сезон',
      dataIndex: 'seasonName',
      key: 'seasonName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Город',
      dataIndex: 'city',
      key: 'city',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Даты',
      key: 'dates',
      render: (_: unknown, r: Tournament) =>
        `${dayjs(r.startDate).format('DD.MM.YYYY')} — ${dayjs(r.endDate).format('DD.MM.YYYY')}`,
    },
    {
      title: 'Формат',
      dataIndex: 'formatName',
      key: 'formatName',
      render: (v: string, r: Tournament) => v ?? r.formatCode,
    },
    {
      title: 'Матч до',
      key: 'setsToWin',
      width: 100,
      render: (_: unknown, r: Tournament) =>
        `до ${r.setsToWin} ${r.setsToWin === 1 ? 'партии' : 'побед'}`,
    },
    {
      title: 'Организатор',
      dataIndex: 'organizerFullName',
      key: 'organizerFullName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: Tournament) => (
        <Space>
          {can('manageTournaments') && (
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          )}
          {can('manageTournaments') && (
            <Popconfirm
              title="Удалить турнир?"
              onConfirm={() => handleDelete(record.id)}
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
        <Title level={3} style={{ margin: 0 }}>Турниры</Title>
        <Space wrap>
          <Radio.Group value={viewMode} onChange={e => setViewMode(e.target.value)} buttonStyle="solid" size="small">
            <Radio.Button value="table"><UnorderedListOutlined /> Таблица</Radio.Button>
            <Radio.Button value="tree"><ApartmentOutlined /> Дерево</Radio.Button>
          </Radio.Group>
          {can('manageTournaments') && (
            <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
              Добавить
            </Button>
          )}
        </Space>
      </div>

      {viewMode === 'table' && (
        <>
          {/* фильтры */}
          <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
            <Col xs={24} sm={12} md={6}>
              <Search
                placeholder="Поиск по названию"
                onSearch={setSearchText}
                onChange={(e) => { if (!e.target.value) setSearchText(''); }}
                allowClear
              />
            </Col>
            <Col xs={24} sm={12} md={6}>
              <Select
                placeholder="Фильтр по сезону"
                allowClear
                style={{ width: '100%' }}
                onChange={setFilterSeason}
                options={seasons.map((s) => ({ value: Number(s.id), label: s.name }))}
              />
            </Col>
            <Col xs={24} sm={12} md={6}>
              <Select
                placeholder="Фильтр по формату"
                allowClear
                style={{ width: '100%' }}
                onChange={setFilterFormat}
                options={formats.map((f) => ({ value: f.code, label: f.name }))}
              />
            </Col>
          </Row>

          <Table
            rowKey="id"
            dataSource={filteredTournaments}
            columns={columns}
            loading={loading}
            scroll={{ x: 'max-content' }}
            pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
          />
        </>
      )}

      {viewMode === 'tree' && (
        <Layout style={{ background: 'transparent' }}>
          <Layout.Sider
            width={280}
            style={{ background: '#fff', borderRight: '1px solid #f0f0f0', borderRadius: 8, overflow: 'auto', maxHeight: 'calc(100vh - 180px)' }}
          >
            <TournamentTree
              onMatchSelect={(id) => setSelectedMatchId(id)}
              onTournamentSelect={() => {}}
            />
          </Layout.Sider>
          <Layout.Content style={{ padding: '0 16px', background: 'transparent' }}>
            {selectedMatchId ? (
              <Card>
                <Button type="primary" icon={<EyeOutlined />} onClick={() => navigate(`/matches/${selectedMatchId}`)}>
                  Открыть матч #{selectedMatchId}
                </Button>
              </Card>
            ) : (
              <Card style={{ color: '#999', textAlign: 'center' }}>
                Выберите матч в дереве слева
              </Card>
            )}
          </Layout.Content>
        </Layout>
      )}

      {/* модальное окно */}
      <Modal
        title={editRecord ? 'Редактировать турнир' : 'Новый турнир'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={760}
        destroyOnHidden
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={16}>
            <Col xs={24} sm={16}>
              <Form.Item name="name" label="Название" rules={[{ required: true, message: 'Введите название' }]}>
                <Input placeholder="Чемпионат России 2025" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="city" label="Город" rules={[{ required: true, message: 'Введите город' }]}>
                <Input placeholder="Москва" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="dates" label="Даты проведения" rules={[{ required: true, message: 'Выберите даты' }]}>
                <RangePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="seasonId" label="Сезон" rules={[{ required: true, message: 'Выберите сезон' }]}>
                <Select
                  placeholder="Сезон"
                  options={seasons.map((s) => ({ value: Number(s.id), label: s.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={6}>
              <Form.Item name="organizerId" label="Организатор" rules={[{ required: true, message: 'Выберите организатора' }]}>
                <Select
                  placeholder="Организатор"
                  options={organizers.map((o) => ({ value: Number(o.id), label: o.name }))}
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
              <Form.Item name="formatCode" label="Формат" rules={[{ required: true, message: 'Выберите формат' }]}>
                <Select
                  placeholder="Формат"
                  options={formats.map((f) => ({ value: f.code, label: f.name }))}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="scoringSystemCode" label="Система очков" rules={[{ required: true, message: 'Выберите систему' }]}>
                <Select
                  placeholder="Система очков"
                  options={scoringSystems.map((s) => ({ value: s.code, label: s.name }))}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="maxTeams" label="Макс. команд">
                <InputNumber min={2} max={256} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={8}>
              <Form.Item name="gender" label="Пол участников" rules={[{ required: true, message: 'Выберите пол' }]} initialValue="мужской">
                <Select
                  options={[
                    { value: 'мужской', label: 'Мужской' },
                    { value: 'женский', label: 'Женский' },
                    { value: 'смешанный', label: 'Смешанный' },
                  ]}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="level" label="Уровень турнира" rules={[{ required: true, message: 'Выберите уровень' }]} initialValue="региональный">
                <Select
                  options={[
                    { value: 'ФИВБ', label: 'ФИВБ (международный)' },
                    { value: 'национальный', label: 'Национальный' },
                    { value: 'региональный', label: 'Региональный' },
                    { value: 'местный', label: 'Местный' },
                  ]}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="maxPlayersPerApp" label="Игроков в заявке" initialValue={12}>
                <Select
                  options={[
                    { value: 12, label: '12' },
                    { value: 14, label: '14' },
                  ]}
                />
              </Form.Item>
            </Col>
          </Row>
          <Form.Item name="description" label="Описание">
            <TextArea rows={3} />
          </Form.Item>
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="ageCategory" label="Возрастная категория">
                <Input placeholder="U18 / U21 / open" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item
                name="applicationDeadline"
                label="Дедлайн заявок"
                dependencies={['dates']}
                rules={[
                  ({ getFieldValue }) => ({
                    validator(_, value) {
                      if (!value) return Promise.resolve();
                      const dates = getFieldValue('dates') as [import('dayjs').Dayjs, import('dayjs').Dayjs] | undefined;
                      if (!dates?.[0]) return Promise.resolve();
                      if ((value as import('dayjs').Dayjs).isAfter(dates[0])) {
                        return Promise.reject(new Error('Дедлайн должен быть не позже даты начала турнира'));
                      }
                      return Promise.resolve();
                    },
                  }),
                ]}
              >
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="setsToWin" label="Побед для победы в матче" initialValue={3}>
                <Select
                  options={[
                    { value: 1, label: '1 (одна партия)' },
                    { value: 2, label: '2 (best-of-3)' },
                    { value: 3, label: '3 (best-of-5)' },
                  ]}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="tiebreakScoreTarget" label="Очков в решающей партии" initialValue={15}>
                <Select
                  options={[
                    { value: 15, label: '15 (стандарт)' },
                    { value: 25, label: '25 (как обычная партия)' },
                  ]}
                />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default TournamentsPage;
