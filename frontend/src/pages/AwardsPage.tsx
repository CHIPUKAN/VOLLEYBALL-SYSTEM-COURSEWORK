import React, { useEffect, useState, useMemo } from 'react';
import {
  Table, Button, Modal, Form, Input, Select, Space, Popconfirm,
  App, Typography, Row, Col,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import { awardsApi } from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type { Award, LookupDto, LookupItemDto } from '../types/index';

const { Title } = Typography;

// страница управления наградами
const AwardsPage: React.FC = () => {
  const { message } = App.useApp();

  const [awards, setAwards] = useState<Award[]>([]);
  const [awardTypes, setAwardTypes] = useState<LookupDto[]>([]);
  const [tournaments, setTournaments] = useState<LookupItemDto[]>([]);
  const [players, setPlayers] = useState<LookupItemDto[]>([]);
  const [teams, setTeams] = useState<LookupItemDto[]>([]);
  const [loading, setLoading] = useState(false);

  // фильтры
  const [filterTournament, setFilterTournament] = useState<number | undefined>(undefined);

  // модальное окно
  const [modalOpen, setModalOpen] = useState(false);
  const [editRecord, setEditRecord] = useState<Award | null>(null);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const loadData = async () => {
    setLoading(true);
    try {
      const [awardsData, typesData, tournamentsData, playersData, teamsData] = await Promise.all([
        awardsApi.getAll(),
        lookupsApi.getAwardTypes(),
        lookupsApi.getTournaments(),
        lookupsApi.getPlayers(),
        lookupsApi.getTeams(),
      ]);
      setAwards(awardsData);
      setAwardTypes(typesData);
      setTournaments(tournamentsData);
      setPlayers(playersData);
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
  const filteredAwards = useMemo(() => {
    if (!filterTournament) return awards;
    return awards.filter((a) => a.tournamentId === filterTournament);
  }, [awards, filterTournament]);

  const handleCreate = () => {
    setEditRecord(null);
    form.resetFields();
    setModalOpen(true);
  };

  const handleEdit = (record: Award) => {
    setEditRecord(record);
    form.setFieldsValue({
      tournamentId: record.tournamentId,
      awardTypeCode: record.awardTypeCode,
      name: record.name,
      playerId: record.playerId,
      teamId: record.teamId,
    });
    setModalOpen(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await awardsApi.delete(id);
      message.success('Награда удалена');
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
        await awardsApi.update(editRecord.id, values);
        message.success('Награда обновлена');
      } else {
        await awardsApi.create(values);
        message.success('Награда создана');
      }
      setModalOpen(false);
      loadData();
    } catch {
      message.error('Ошибка сохранения');
    } finally {
      setSaving(false);
    }
  };

  const columns: ColumnsType<Award> = [
    {
      title: 'Турнир',
      dataIndex: 'tournamentName',
      key: 'tournamentName',
      render: (v: string, r: Award) => v ?? r.tournamentId,
    },
    {
      title: 'Тип награды',
      dataIndex: 'awardTypeName',
      key: 'awardTypeName',
      render: (v: string, r: Award) => v ?? r.awardTypeCode,
    },
    {
      title: 'Название',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
    },
    {
      title: 'Игрок',
      dataIndex: 'playerFullName',
      key: 'playerFullName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Команда',
      dataIndex: 'teamName',
      key: 'teamName',
      render: (v: string) => v ?? '—',
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      fixed: 'right',
      render: (_: unknown, record: Award) => (
        <Space>
          <Button type="text" icon={<EditOutlined />} onClick={() => handleEdit(record)} size="small" />
          <Popconfirm
            title="Удалить награду?"
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
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Награды</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleCreate}>
          Добавить
        </Button>
      </div>

      {/* фильтр по турниру */}
      <Row gutter={[12, 12]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
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
      </Row>

      <Table
        rowKey="id"
        dataSource={filteredAwards}
        columns={columns}
        loading={loading}
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 15, showSizeChanger: true, showTotal: (t) => `Всего: ${t}` }}
      />

      {/* модальное окно */}
      <Modal
        title={editRecord ? 'Редактировать награду' : 'Новая награда'}
        open={modalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={handleSave}
        confirmLoading={saving}
        okText={editRecord ? 'Сохранить' : 'Создать'}
        cancelText="Отмена"
        width={600}
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
            <Col xs={24} sm={12}>
              <Form.Item name="awardTypeCode" label="Тип награды" rules={[{ required: true, message: 'Выберите тип' }]}>
                <Select
                  placeholder="Тип награды"
                  options={awardTypes.map((a) => ({ value: a.code, label: a.name }))}
                />
              </Form.Item>
            </Col>
          </Row>
          <Form.Item name="name" label="Название" rules={[{ required: true, message: 'Введите название награды' }]}>
            <Input placeholder="Лучший игрок турнира" />
          </Form.Item>
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="playerId" label="Игрок (если личная)">
                <Select
                  placeholder="Выберите игрока"
                  allowClear
                  options={players.map((p) => ({ value: Number(p.id), label: p.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="teamId" label="Команда (если командная)">
                <Select
                  placeholder="Выберите команду"
                  allowClear
                  options={teams.map((t) => ({ value: Number(t.id), label: t.name }))}
                  showSearch
                  filterOption={(input, opt) =>
                    (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default AwardsPage;
