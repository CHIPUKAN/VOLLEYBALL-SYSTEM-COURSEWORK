import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col, Tag, DatePicker,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { seasonsApi } from '../api/index';
import type { Season } from '../types/index';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;
const { RangePicker } = DatePicker;

// варианты статуса сезона
const SEASON_STATUSES = [
  { value: 'active', label: 'Активный' },
  { value: 'finished', label: 'Завершённый' },
];

const STATUS_COLORS: Record<string, string> = {
  active: 'green',
  finished: 'default',
};

const STATUS_LABELS: Record<string, string> = {
  active: 'Активный',
  finished: 'Завершённый',
};

// страница управления сезонами
const SeasonsPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  const [seasons, setSeasons] = useState<Season[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Season | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      setSeasons(await seasonsApi.getAll());
    } catch {
      message.error('Ошибка загрузки сезонов');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredSeasons = useMemo(() => {
    if (!searchText) return seasons;
    const lower = searchText.toLowerCase();
    return seasons.filter((s) => s.name.toLowerCase().includes(lower));
  }, [seasons, searchText]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Season) => {
    setEditRecord(record);
    const statusKey = record.status === 'активен' ? 'active' : 'finished';
    form.setFieldsValue({
      name: record.name,
      dates: [dayjs(record.startDate), dayjs(record.endDate)],
      statusKey,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await seasonsApi.delete(id);
      message.success('Сезон удалён');
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
    const dates = values.dates as [dayjs.Dayjs, dayjs.Dayjs];
    const statusKeyMap: Record<string, string> = { active: 'активен', finished: 'завершён' };
    const payload: Partial<Season> = {
      name: values.name as string,
      startDate: dates[0].format('YYYY-MM-DD'),
      endDate: dates[1].format('YYYY-MM-DD'),
      status: statusKeyMap[values.statusKey as string] ?? 'завершён',
    };
    setSaving(true);
    try {
      if (editRecord) {
        await seasonsApi.update(editRecord.id, payload);
        message.success('Сезон обновлён');
      } else {
        await seasonsApi.create(payload);
        message.success('Сезон создан');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<Season> = [
    {
      title: 'Название',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
    },
    {
      title: 'Начало',
      dataIndex: 'startDate',
      key: 'startDate',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
      sorter: (a, b) => a.startDate.localeCompare(b.startDate),
    },
    {
      title: 'Окончание',
      dataIndex: 'endDate',
      key: 'endDate',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
    },
    {
      title: 'Статус',
      dataIndex: 'status',
      key: 'status',
      render: (status: string) => {
        const key = status === 'активен' ? 'active' : 'finished';
        return <Tag color={STATUS_COLORS[key]}>{STATUS_LABELS[key]}</Tag>;
      },
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: Season) => (
        <Space>
          {can('manageSeasons') && (
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          )}
          {can('manageSeasons') && (
            <Popconfirm
              title="Удалить сезон?"
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
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Сезоны</Title>
        {can('manageSeasons') && (
          <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
            Добавить
          </Button>
        )}
      </div>

      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
          <Search
            placeholder="Поиск по названию"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
      </Row>

      <Table
        rowKey="id"
        dataSource={filteredSeasons}
        columns={columns}
        loading={loading}
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
      />

      <Modal
        title={editRecord ? 'Редактировать сезон' : 'Новый сезон'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={520}
        destroyOnHidden
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item name="name" label="Название" rules={[{ required: true, message: 'Введите название сезона' }]}>
            <Input placeholder="Сезон 2024/2025" />
          </Form.Item>
          <Form.Item
            name="dates"
            label="Период сезона"
            rules={[{ required: true, message: 'Выберите даты' }]}
          >
            <RangePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
          </Form.Item>
          <Form.Item name="statusKey" label="Статус" initialValue="active">
            <Select options={SEASON_STATUSES} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default SeasonsPage;
