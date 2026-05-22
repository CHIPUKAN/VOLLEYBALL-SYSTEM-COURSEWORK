import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Space, Popconfirm,
  App, Typography, Row, Col, InputNumber,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { venuesApi } from '../api/index';
import type { Venue } from '../types/index';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;

// страница управления площадками
const VenuesPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  const [venues, setVenues] = useState<Venue[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Venue | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      setVenues(await venuesApi.getAll());
    } catch {
      message.error('Ошибка загрузки площадок');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredVenues = useMemo(() => {
    if (!searchText) return venues;
    const lower = searchText.toLowerCase();
    return venues.filter(
      (v) =>
        v.name.toLowerCase().includes(lower) ||
        v.city.toLowerCase().includes(lower),
    );
  }, [venues, searchText]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Venue) => {
    setEditRecord(record);
    form.setFieldsValue({
      name: record.name,
      city: record.city,
      address: record.address,
      capacity: record.capacity,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await venuesApi.delete(id);
      message.success('Площадка удалена');
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
    setSaving(true);
    try {
      if (editRecord) {
        await venuesApi.update(editRecord.id, values);
        message.success('Площадка обновлена');
      } else {
        await venuesApi.create(values);
        message.success('Площадка создана');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<Venue> = [
    {
      title: 'Название',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
    },
    {
      title: 'Город',
      dataIndex: 'city',
      key: 'city',
      sorter: (a, b) => a.city.localeCompare(b.city),
    },
    {
      title: 'Адрес',
      dataIndex: 'address',
      key: 'address',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Вместимость',
      dataIndex: 'capacity',
      key: 'capacity',
      render: (v: number) => (v ? v.toLocaleString('ru-RU') : '—'),
      sorter: (a, b) => (a.capacity ?? 0) - (b.capacity ?? 0),
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: Venue) => (
        <Space>
          {can('manageVenues') && (
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          )}
          {can('manageVenues') && (
            <Popconfirm
              title="Удалить площадку?"
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
        <Title level={3} style={{ margin: 0 }}>Площадки</Title>
        {can('manageVenues') && (
          <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
            Добавить
          </Button>
        )}
      </div>

      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
          <Search
            placeholder="Поиск по названию или городу"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
      </Row>

      <Table
        rowKey="id"
        dataSource={filteredVenues}
        columns={columns}
        loading={loading}
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
      />

      <Modal
        title={editRecord ? 'Редактировать площадку' : 'Новая площадка'}
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
          <Row gutter={16}>
            <Col xs={24} sm={16}>
              <Form.Item name="name" label="Название" rules={[{ required: true, message: 'Введите название' }]}>
                <Input placeholder="Дворец спорта" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="city" label="Город" rules={[{ required: true, message: 'Введите город' }]}>
                <Input placeholder="Москва" />
              </Form.Item>
            </Col>
          </Row>
          <Form.Item name="address" label="Адрес">
            <Input placeholder="ул. Ленина, д. 1" />
          </Form.Item>
          <Form.Item name="capacity" label="Вместимость (зрителей)">
            <InputNumber min={1} max={100000} style={{ width: '100%' }} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default VenuesPage;
