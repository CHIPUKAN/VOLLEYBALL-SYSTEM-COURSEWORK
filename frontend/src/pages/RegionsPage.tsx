import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Space, Popconfirm,
  App, Typography, Row, Col,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { regionsApi } from '../api/index';
import { useAuth } from '../context/AuthContext';

const { Title } = Typography;
const { Search } = Input;

// тип региона на основе ответа API
interface RegionRow {
  oktmoCode: string;
  name: string;
}

// страница управления регионами
const RegionsPage: React.FC = () => {
  const { message } = App.useApp();
  const { can } = useAuth();

  const [regions, setRegions] = useState<RegionRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<RegionRow | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      setRegions(await regionsApi.getAll());
    } catch {
      message.error('Ошибка загрузки регионов');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredRegions = useMemo(() => {
    if (!searchText) return regions;
    const lower = searchText.toLowerCase();
    return regions.filter(
      (r) =>
        r.name.toLowerCase().includes(lower) ||
        r.oktmoCode.includes(lower),
    );
  }, [regions, searchText]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: RegionRow) => {
    setEditRecord(record);
    form.setFieldsValue({ name: record.name });
    setModalOpen(true);
  };

  const handleDelete = async (oktmoCode: string) => {
    try {
      await regionsApi.delete(oktmoCode);
      message.success('Регион удалён');
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
        await regionsApi.update(editRecord.oktmoCode, { name: values.name as string });
        message.success('Регион обновлён');
      } else {
        await regionsApi.create({
          oktmoCode: values.oktmoCode as string,
          name: values.name as string,
        });
        message.success('Регион создан');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<RegionRow> = [
    {
      title: 'ОКТМО',
      dataIndex: 'oktmoCode',
      key: 'oktmoCode',
      width: 140,
    },
    {
      title: 'Название',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: RegionRow) => (
        <Space>
          {can('manageRegions') && (
            <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          )}
          {can('manageRegions') && (
            <Popconfirm
              title="Удалить регион?"
              onConfirm={() => handleDelete(record.oktmoCode)}
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
        <Title level={3} style={{ margin: 0 }}>Регионы</Title>
        {can('manageRegions') && (
          <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
            Добавить
          </Button>
        )}
      </div>

      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
          <Search
            placeholder="Поиск по названию или ОКТМО"
            onSearch={setSearchText}
            onChange={(e) => { if (!e.target.value) setSearchText(''); }}
            allowClear
          />
        </Col>
      </Row>

      <Table
        rowKey="oktmoCode"
        dataSource={filteredRegions}
        columns={columns}
        loading={loading}
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
      />

      <Modal
        title={editRecord ? 'Редактировать регион' : 'Новый регион'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={480}
        destroyOnHidden
      >
        <Form form={form} layout="vertical" style={{ marginTop: 16 }}>
          {!editRecord && (
            <Form.Item
              name="oktmoCode"
              label="Код ОКТМО"
              rules={[
                { required: true, message: 'Введите код ОКТМО' },
                { pattern: /^\d{11}$/, message: 'Код ОКТМО должен состоять из 11 цифр' },
              ]}
              extra="11 цифр, например: 45000000000"
            >
              <Input maxLength={11} placeholder="45000000000" />
            </Form.Item>
          )}
          <Form.Item name="name" label="Название" rules={[{ required: true, message: 'Введите название' }]}>
            <Input placeholder="Москва" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default RegionsPage;
