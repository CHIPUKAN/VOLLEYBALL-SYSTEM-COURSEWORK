import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { refereesApi } from '../api/index';
import type { Referee } from '../types/index';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;

// категории судей
const REFEREE_CATEGORIES = [
  { value: 'FIVB', label: 'FIVB' },
  { value: 'национальная', label: 'Национальная' },
  { value: 'региональная', label: 'Региональная' },
  { value: 'местная', label: 'Местная' },
];

// страница управления судьями
const RefereesPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  const [referees, setReferees] = useState<Referee[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Referee | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      setReferees(await refereesApi.getAll());
    } catch {
      message.error('Ошибка загрузки судей');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredReferees = useMemo(() => {
    if (!searchText) return referees;
    const lower = searchText.toLowerCase();
    return referees.filter((r) => {
      const name = r.fullName ?? `${r.lastName} ${r.firstName}`;
      return name.toLowerCase().includes(lower);
    });
  }, [referees, searchText]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Referee) => {
    setEditRecord(record);
    form.setFieldsValue({
      lastName: record.lastName,
      firstName: record.firstName,
      middleName: record.middleName,
      category: record.category,
      licenseNumber: record.licenseNumber,
      email: record.email,
      phone: record.phone,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await refereesApi.delete(id);
      message.success('Судья удалён');
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
        await refereesApi.update(editRecord.id, values);
        message.success('Судья обновлён');
      } else {
        await refereesApi.create(values);
        message.success('Судья добавлен');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<Referee> = [
    {
      title: 'ФИО',
      key: 'fullName',
      render: (_: unknown, r: Referee) => r.fullName ?? `${r.lastName} ${r.firstName} ${r.middleName ?? ''}`.trim(),
      sorter: (a, b) => a.lastName.localeCompare(b.lastName),
    },
    {
      title: 'Категория',
      dataIndex: 'category',
      key: 'category',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Номер лицензии',
      dataIndex: 'licenseNumber',
      key: 'licenseNumber',
      render: (v: string) => v ?? '—',
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
      render: (_: unknown, record: Referee) => (
        <Space>
          {can('manageReferees') && (
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          )}
          {can('manageReferees') && (
            <Popconfirm
              title="Удалить судью?"
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
        <Title level={3} style={{ margin: 0 }}>Судьи</Title>
        {can('manageReferees') && (
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
        dataSource={filteredReferees}
        columns={columns}
        loading={loading}
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
      />

      <Modal
        title={editRecord ? 'Редактировать судью' : 'Новый судья'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={600}
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
            <Col xs={24} sm={8}>
              <Form.Item name="category" label="Категория">
                <Select placeholder="Выберите категорию" allowClear options={REFEREE_CATEGORIES} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="licenseNumber" label="Номер лицензии">
                <Input />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="phone" label="Телефон">
                <Input placeholder="+7 (999) 000-00-00" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col xs={24}>
              <Form.Item name="email" label="Email" rules={[{ type: 'email', message: 'Некорректный email' }]}>
                <Input />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default RefereesPage;
