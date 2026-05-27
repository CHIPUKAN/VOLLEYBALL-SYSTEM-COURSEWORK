import React, { useState, useRef } from 'react';
import type { Tournament } from '../types/index';
import { Tree, Spin, Typography } from 'antd';
import {
  CalendarOutlined, TrophyOutlined, ApartmentOutlined,
  ClockCircleOutlined, PlayCircleOutlined, CheckCircleOutlined,
} from '@ant-design/icons';
import type { DataNode } from 'antd/es/tree';
import { seasonsApi, tournamentsApi, matchesApi } from '../api/index';

const { Text } = Typography;

interface TournamentTreeProps {
  onMatchSelect: (matchId: number) => void;
  onTournamentSelect: (tournamentId: number) => void;
}

const matchIcon = (statusName?: string) => {
  if (statusName === 'В процессе') return <PlayCircleOutlined style={{ color: '#BA7517' }} />;
  if (statusName === 'Завершён') return <CheckCircleOutlined style={{ color: '#3B6D11' }} />;
  return <ClockCircleOutlined style={{ color: '#378ADD' }} />;
};

// иерархическое дерево турниров
const TournamentTree: React.FC<TournamentTreeProps> = ({
  onMatchSelect,
  onTournamentSelect,
}) => {
  const [treeData, setTreeData] = useState<DataNode[]>([]);
  const [loading, setLoading] = useState(true);
  const [expandedKeys, setExpandedKeys] = useState<React.Key[]>([]);
  const tournamentsCache = useRef<Tournament[] | null>(null);

  // начальная загрузка сезонов
  React.useEffect(() => {
    const load = async () => {
      try {
        const seasons = await seasonsApi.getAll();
        setTreeData(seasons.map(s => ({
          key: `season-${s.id}`,
          title: (
            <Text>
              <CalendarOutlined style={{ color: '#534AB7', marginRight: 6 }} />
              {s.name}
            </Text>
          ),
          isLeaf: false,
        })));
      } catch { /* некритично */ } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const loadData = async (node: DataNode): Promise<void> => {
    const key = String(node.key);

    if (key.startsWith('season-')) {
      const seasonId = Number(key.replace('season-', ''));
      if (tournamentsCache.current == null) {
        tournamentsCache.current = await tournamentsApi.getAll();
      }
      const filtered = tournamentsCache.current.filter(t => t.seasonId === seasonId);
      const children: DataNode[] = filtered.map(t => ({
        key: `tournament-${t.id}`,
        title: (
          <Text
            onClick={() => onTournamentSelect(t.id)}
            style={{ cursor: 'pointer' }}
          >
            <TrophyOutlined style={{ color: '#BA7517', marginRight: 6 }} />
            {t.name}
          </Text>
        ),
        isLeaf: false,
      }));
      setTreeData(prev => updateTreeData(prev, key, children));
    }

    if (key.startsWith('tournament-')) {
      const tournamentId = Number(key.replace('tournament-', ''));
      const allMatches = await matchesApi.getAll({ tournamentId });
      const byStage: Record<string, typeof allMatches> = {};
      allMatches.forEach(m => {
        const stage = m.stageName ?? 'Без этапа';
        if (!byStage[stage]) byStage[stage] = [];
        byStage[stage].push(m);
      });

      const children: DataNode[] = Object.entries(byStage).map(([stage, stageMatches]) => ({
        key: `stage-${tournamentId}-${stage}`,
        title: (
          <Text>
            <ApartmentOutlined style={{ color: '#3B6D11', marginRight: 6 }} />
            {stage}
          </Text>
        ),
        isLeaf: false,
        children: stageMatches.map(m => ({
          key: `match-${m.id}`,
          title: (
            <Text
              onClick={() => onMatchSelect(m.id)}
              style={{ cursor: 'pointer' }}
            >
              {matchIcon(m.statusName)}
              {' '}
              {m.homeTeamName ?? '?'} — {m.guestTeamName ?? '?'}
              <Text type="secondary" style={{ fontSize: 11, marginLeft: 6 }}>
                {m.matchDate ? m.matchDate.slice(5, 10) : ''}
                {m.statusName === 'В процессе' ? ' · идёт' : ''}
              </Text>
            </Text>
          ),
          isLeaf: true,
        })),
      }));
      setTreeData(prev => updateTreeData(prev, key, children));
    }
  };

  return (
    <Spin spinning={loading}>
      <Tree
        treeData={treeData}
        loadData={loadData}
        expandedKeys={expandedKeys}
        onExpand={keys => setExpandedKeys(keys as React.Key[])}
        showLine
        blockNode
      />
    </Spin>
  );
};

function updateTreeData(list: DataNode[], key: string, children: DataNode[]): DataNode[] {
  return list.map(node => {
    if (node.key === key) return { ...node, children };
    if (node.children) return { ...node, children: updateTreeData(node.children, key, children) };
    return node;
  });
}

export default TournamentTree;
