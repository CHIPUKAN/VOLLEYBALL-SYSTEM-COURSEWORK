import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Space, Popconfirm,
  App, Typography, Row, Col,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { organizersApi } from '../api/index';
import type { Organizer } from '../types/index';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;

// страница управления организаторами
const OrganizersPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  const [organizers, setOrganizers] = useState<Organizer[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Organizer | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      setOrganizers(await organizersApi.getAll());
    } catch {
      message.error('Ошибка загрузки организаторов');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredOrganizers = useMemo(() => {
    if (!searchText) return organizers;
    const lower = searchText.toLowerCase();
    return organizers.filter((o) => {
      const name = o.fullName ?? `${o.lastName} ${o.firstName}`;
      return name.toLowerCase().includes(lower);
    });
  }, [organizers, searchText]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Organizer) => {
    setEditRecord(record);
    form.setFieldsValue({
      lastName: record.lastName,
      firstName: record.firstName,
      middleName: record.middleName,
      email: record.email,
      phone: record.phone,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await organizersApi.delete(id);
      message.success('Организатор удалён');
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
        await organizersApi.update(editRecord.id, values);
        message.success('Организатор обновлён');
      } else {
        await organizersApi.create(values);
        message.success('Организатор добавлен');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<Organizer> = [
    {
      title: 'ФИО',
      key: 'fullName',
      render: (_: unknown, r: Organizer) => r.fullName ?? `${r.lastName} ${r.firstName} ${r.middleName ?? ''}`.trim(),
      sorter: (a, b) => a.lastName.localeCompare(b.lastName),
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Телефон',
      dataIndex: 'phone',
      key: 'phone',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: Organizer) => (
        <Space>
          {can('manageOrganizers') && (
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          )}
          {can('manageOrganizers') && (
            <Popconfirm
              title="Удалить организатора?"
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
        <Title level={3} style={{ margin: 0 }}>Организаторы</Title>
        {can('manageOrganizers') && (
          <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
            Добавить
          </Button>
        )}
      </div>

      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
          <Search
            placeholder="Поиск по ФИО"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
      </Row>

      <Table
        rowKey="id"
        dataSource={filteredOrganizers}
        columns={columns}
        loading={loading}
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
      />

      <Modal
        title={editRecord ? 'Редактировать организатора' : 'Новый организатор'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={560}
        destroyOnHidden
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
            <Col xs={24} sm={12}>
              <Form.Item name="email" label="Email" rules={[{ type: 'email', message: 'Некорректный email' }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="phone" label="Телефон">
                <Input placeholder="+7 (999) 000-00-00" />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default OrganizersPage;
