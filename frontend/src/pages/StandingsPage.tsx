import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Table, Select, Typography, Space, Tag, Spin, Empty, App,
} from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { OrderedListOutlined } from '@ant-design/icons';
import { standingsApi, tournamentsApi, groupsApi } from '../api';
import { lookupsApi } from '../api/lookupsApi';
import type { StandingRow, Tournament, Group } from '../types';

const { Title, Text } = Typography;

// цветовые зоны таблицы: первые N мест
const ZONE_ADVANCE_BG = '#e6f7ff';   // выход в следующий этап
const ZONE_ADVANCE_BORDER = '#91caff';
const ZONE_RELEGATION_BG = '#fff1f0'; // зона выбывания
const ZONE_RELEGATION_BORDER = '#ffa39e';

function zoneStyle(rank: number, total: number): React.CSSProperties {
  if (total < 3) return {};
  const advanceCount = Math.floor(total / 2);
  const relegationCount = Math.ceil(total / 4);
  if (rank <= advanceCount) {
    return { background: ZONE_ADVANCE_BG, borderLeft: `3px solid ${ZONE_ADVANCE_BORDER}` };
  }
  if (rank > total - relegationCount) {
    return { background: ZONE_RELEGATION_BG, borderLeft: `3px solid ${ZONE_RELEGATION_BORDER}` };
  }
  return {};
}

// страница турнирной таблицы
const StandingsPage: React.FC = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  const [tournaments, setTournaments] = useState<Tournament[]>([]);
  const [groups, setGroups] = useState<Group[]>([]);
  const [rows, setRows] = useState<StandingRow[]>([]);

  const [selectedTournament, setSelectedTournament] = useState<number | undefined>();
  const [selectedStage, setSelectedStage] = useState<number | undefined>();
  const [selectedGroup, setSelectedGroup] = useState<number | undefined>();

  const [stages, setStages] = useState<import('../types/index').LookupDto[]>([]);
  const [loadingTournaments, setLoadingTournaments] = useState(false);
  const [loadingTable, setLoadingTable] = useState(false);

  // загрузка списка турниров и этапов
  useEffect(() => {
    setLoadingTournaments(true);
    Promise.all([
      tournamentsApi.getAll(),
      lookupsApi.getTournamentStages(),
    ])
      .then(([tourData, stagesData]) => { setTournaments(tourData); setStages(stagesData); })
      .catch(() => message.error('Не удалось загрузить данные'))
      .finally(() => setLoadingTournaments(false));
  }, []);

  // загрузка групп при смене турнира
  useEffect(() => {
    setSelectedGroup(undefined);
    setGroups([]);
    if (!selectedTournament) return;
    groupsApi.getAll(selectedTournament)
      .then(setGroups)
      .catch(() => {});
  }, [selectedTournament]);

  // загрузка таблицы
  useEffect(() => {
    if (!selectedTournament) { setRows([]); return; }
    setLoadingTable(true);
    standingsApi.get(selectedTournament, selectedStage, selectedGroup)
      .then(setRows)
      .catch(() => message.error('Не удалось загрузить турнирную таблицу'))
      .finally(() => setLoadingTable(false));
  }, [selectedTournament, selectedStage, selectedGroup]);

  const stageOptions = stages.map(s => ({ value: s.code, label: s.name }));

  const total = rows.length;

  const columns: ColumnsType<StandingRow> = [
    {
      title: '#',
      dataIndex: 'rank',
      width: 52,
      align: 'center',
      render: (rank: number) => {
        const style = zoneStyle(rank, total);
        const color = style.borderLeft ? (rank <= Math.floor(total / 2) ? 'blue' : 'red') : undefined;
        return (
          <Text strong style={{ color: color === 'blue' ? '#1677ff' : color === 'red' ? '#f5222d' : undefined }}>
            {rank}
          </Text>
        );
      },
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'Команда',
      dataIndex: 'teamName',
      render: (name: string, record) => (
        <Text
          strong
          style={{ cursor: 'pointer', color: '#1677ff' }}
          onClick={() => navigate(`/teams?id=${record.teamId}`)}
        >
          {name}
        </Text>
      ),
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'И',
      dataIndex: 'gamesPlayed',
      width: 52,
      align: 'center',
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'В',
      dataIndex: 'wins',
      width: 52,
      align: 'center',
      render: (v: number) => <Text style={{ color: '#3B6D11' }}>{v}</Text>,
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'П',
      dataIndex: 'losses',
      width: 52,
      align: 'center',
      render: (v: number) => <Text style={{ color: '#A32D2D' }}>{v}</Text>,
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'Партии',
      key: 'sets',
      width: 80,
      align: 'center',
      render: (_: unknown, record) => (
        <Text>{record.setsWon}:{record.setsLost}</Text>
      ),
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'Мячи',
      key: 'pts',
      width: 90,
      align: 'center',
      render: (_: unknown, record) => (
        <Text>{record.pointsWon}:{record.pointsLost}</Text>
      ),
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
    {
      title: 'Очки',
      dataIndex: 'points',
      width: 70,
      align: 'center',
      render: (v: number) => (
        <Tag color="blue" style={{ fontWeight: 700, fontSize: 13, minWidth: 36, textAlign: 'center' }}>
          {v}
        </Tag>
      ),
      onCell: (record) => ({ style: zoneStyle(record.rank, total) }),
    },
  ];

  return (
    <Space direction="vertical" size={16} style={{ width: '100%' }}>
      <Space align="center">
        <OrderedListOutlined style={{ fontSize: 22, color: '#1677ff' }} />
        <Title level={4} style={{ margin: 0 }}>Турнирная таблица</Title>
      </Space>

      {/* фильтры */}
      <Space wrap>
        <Select
          placeholder="Выберите турнир"
          style={{ minWidth: 260 }}
          loading={loadingTournaments}
          value={selectedTournament}
          onChange={(v) => { setSelectedTournament(v); setSelectedStage(undefined); }}
          allowClear
          showSearch
          optionFilterProp="label"
          options={tournaments.map(t => ({ value: t.id, label: t.name }))}
        />
        <Select
          placeholder="Этап"
          style={{ minWidth: 200 }}
          value={selectedStage}
          onChange={setSelectedStage}
          allowClear
          disabled={!selectedTournament}
          options={stageOptions}
        />
        {groups.length > 0 && (
          <Select
            placeholder="Группа"
            style={{ minWidth: 160 }}
            value={selectedGroup}
            onChange={setSelectedGroup}
            allowClear
            options={groups.map(g => ({ value: g.id, label: g.name }))}
          />
        )}
      </Space>

      {/* легенда */}
      {rows.length > 0 && (
        <Space size={16} wrap>
          <Space size={4}>
            <div style={{ width: 12, height: 12, background: ZONE_ADVANCE_BG, border: `2px solid ${ZONE_ADVANCE_BORDER}`, borderRadius: 2 }} />
            <Text type="secondary" style={{ fontSize: 12 }}>Выход в следующий этап</Text>
          </Space>
          <Space size={4}>
            <div style={{ width: 12, height: 12, background: ZONE_RELEGATION_BG, border: `2px solid ${ZONE_RELEGATION_BORDER}`, borderRadius: 2 }} />
            <Text type="secondary" style={{ fontSize: 12 }}>Зона выбывания</Text>
          </Space>
        </Space>
      )}

      {/* таблица */}
      {!selectedTournament ? (
        <Empty description="Выберите турнир для отображения таблицы" image={Empty.PRESENTED_IMAGE_SIMPLE} />
      ) : loadingTable ? (
        <div style={{ textAlign: 'center', padding: 40 }}><Spin size="large" /></div>
      ) : rows.length === 0 ? (
        <Empty description="Нет завершённых матчей с данными о партиях" image={Empty.PRESENTED_IMAGE_SIMPLE} />
      ) : (
        <Table<StandingRow>
          dataSource={rows}
          columns={columns}
          rowKey="teamId"
          pagination={false}
          size="middle"
          bordered
          style={{ borderRadius: 8, overflow: 'hidden' }}
          onRow={(record) => ({
            style: { cursor: 'pointer', ...zoneStyle(record.rank, total) },
            onClick: () => navigate(`/teams?id=${record.teamId}`),
          })}
        />
      )}
    </Space>
  );
};

export default StandingsPage;
