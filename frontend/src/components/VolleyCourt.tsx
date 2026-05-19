import React, { useState, useEffect, useCallback } from 'react';
import { Select, Spin, App, Typography, Tooltip, Tag } from 'antd';
import { CloseCircleOutlined, UserOutlined } from '@ant-design/icons';
import { lineupsApi, playersApi } from '../api/index';
import type { StartingLineup, Player } from '../types/index';

const { Text } = Typography;

interface VolleyCourtProps {
  matchId: number;
  homeTeamId: number;
  homeTeamName: string;
  guestTeamId: number;
  guestTeamName: string;
  setNumber: number;
  readonly?: boolean;
}

// стандартная нумерация позиций волейбола:
// 1 - правый задний (подача), 2 - правый передний, 3 - центр передний,
// 4 - левый передний, 5 - левый задний, 6 - центр задний

const POSITION_LABELS: Record<number, string> = {
  1: '1', 2: '2', 3: '3', 4: '4', 5: '5', 6: '6',
};
const POSITION_NAMES: Record<number, string> = {
  1: 'Правый задний / Подача',
  2: 'Правый передний',
  3: 'Центр передний',
  4: 'Левый передний',
  5: 'Левый задний',
  6: 'Центр задний',
};

// макет поля: передняя линия [4,3,2], задняя [5,6,1]
const FRONT_POSITIONS = [4, 3, 2];
const BACK_POSITIONS = [5, 6, 1];

// компонент волейбольного поля
const VolleyCourt: React.FC<VolleyCourtProps> = ({
  matchId, homeTeamId, homeTeamName, guestTeamId, guestTeamName, setNumber, readonly = false,
}) => {
  const { message } = App.useApp();

  const [lineups, setLineups] = useState<StartingLineup[]>([]);
  const [homePlayers, setHomePlayers] = useState<Player[]>([]);
  const [guestPlayers, setGuestPlayers] = useState<Player[]>([]);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState<Record<string, boolean>>({});
  const [draggedItem, setDraggedItem] = useState<{ playerId: number; teamId: number } | null>(null);

  const loadData = useCallback(async () => {
    setLoading(true);
    try {
      const [lineupData, homeData, guestData] = await Promise.all([
        lineupsApi.getAll(matchId, { setNumber }),
        playersApi.getAll(homeTeamId),
        playersApi.getAll(guestTeamId),
      ]);
      setLineups(lineupData);
      setHomePlayers(homeData);
      setGuestPlayers(guestData);
    } catch {
      message.error('Ошибка загрузки расстановки');
    } finally {
      setLoading(false);
    }
  }, [matchId, setNumber, homeTeamId, guestTeamId]);

  useEffect(() => { loadData(); }, [loadData]);

  const getLineup = (teamId: number, pos: number) =>
    lineups.find(l => l.teamId === teamId && l.positionNo === pos);

  const assignPlayer = async (teamId: number, positionNo: number, playerId: number) => {
    const key = `${teamId}-${positionNo}`;
    setSaving(prev => ({ ...prev, [key]: true }));
    try {
      await lineupsApi.upsert(matchId, { matchId, teamId, setNumber, positionNo, playerId });
      const allPlayers = teamId === homeTeamId ? homePlayers : guestPlayers;
      const player = allPlayers.find(p => p.id === playerId);
      setLineups(prev => {
        const rest = prev.filter(l => !(l.teamId === teamId && l.positionNo === positionNo));
        return [...rest, {
          matchId, teamId, setNumber, positionNo, playerId,
          playerFullName: player?.fullName ?? `#${playerId}`,
          shirtNumber: player?.jerseyNumber,
        }];
      });
    } catch {
      message.error('Ошибка сохранения позиции');
    } finally {
      setSaving(prev => ({ ...prev, [key]: false }));
    }
  };

  const removePlayer = async (teamId: number, positionNo: number) => {
    const key = `${teamId}-${positionNo}`;
    setSaving(prev => ({ ...prev, [key]: true }));
    try {
      await lineupsApi.delete(matchId, teamId, setNumber, positionNo);
      setLineups(prev => prev.filter(l => !(l.teamId === teamId && l.positionNo === positionNo)));
    } catch {
      message.error('Ошибка удаления позиции');
    } finally {
      setSaving(prev => ({ ...prev, [key]: false }));
    }
  };

  // рендер одной ячейки позиции
  const renderSlot = (teamId: number, pos: number, _mirror = false) => {
    const lineup = getLineup(teamId, pos);
    const key = `${teamId}-${pos}`;
    const isLoading = !!saving[key];
    const allPlayers = teamId === homeTeamId ? homePlayers : guestPlayers;
    const usedPlayerIds = new Set(lineups.filter(l => l.teamId === teamId).map(l => l.playerId));
    const available = allPlayers.filter(p => !usedPlayerIds.has(p.id) || p.id === lineup?.playerId);

    return (
      <Tooltip title={POSITION_NAMES[pos]} placement="top" key={key}>
        <div
          draggable={!readonly && !!lineup}
          onDragStart={() => lineup && setDraggedItem({ playerId: lineup.playerId, teamId })}
          onDragEnd={() => setDraggedItem(null)}
          onDragOver={(e) => { e.preventDefault(); (e.currentTarget as HTMLDivElement).style.outline = '2px solid #1677ff'; }}
          onDragLeave={(e) => { (e.currentTarget as HTMLDivElement).style.outline = ''; }}
          onDrop={(e) => {
            (e.currentTarget as HTMLDivElement).style.outline = '';
            if (draggedItem && draggedItem.teamId === teamId) {
              assignPlayer(teamId, pos, draggedItem.playerId);
            }
          }}
          style={{
            width: 100,
            minHeight: 70,
            border: lineup ? '2px solid #1677ff' : '2px dashed #8c8c8c',
            borderRadius: 8,
            background: lineup ? 'rgba(22,119,255,0.15)' : 'rgba(0,0,0,0.18)',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            padding: '6px 4px',
            position: 'relative',
            cursor: (!readonly && lineup) ? 'grab' : 'default',
            transition: 'border-color 0.2s, background 0.2s',
          }}
        >
          <Text style={{ fontSize: 10, color: '#d9d9d9', marginBottom: 2 }}>
            {POSITION_LABELS[pos]} ({pos})
          </Text>

          {isLoading ? (
            <Spin size="small" />
          ) : lineup ? (
            <>
              <UserOutlined style={{ fontSize: 18, color: '#fff', marginBottom: 2 }} />
              <Text style={{ fontSize: 10, color: '#fff', textAlign: 'center', lineHeight: 1.2, fontWeight: 600 }}>
                {lineup.shirtNumber ? `#${lineup.shirtNumber}` : ''}
              </Text>
              <Text style={{ fontSize: 10, color: '#ffffffcc', textAlign: 'center', lineHeight: 1.2 }}>
                {lineup.playerFullName ?? `Игрок ${lineup.playerId}`}
              </Text>
              {!readonly && (
                <CloseCircleOutlined
                  onClick={(e) => { e.stopPropagation(); removePlayer(teamId, pos); }}
                  style={{ position: 'absolute', top: 3, right: 3, color: '#ff7875', fontSize: 13, cursor: 'pointer' }}
                />
              )}
            </>
          ) : !readonly ? (
            <Select
              size="small"
              placeholder="Игрок"
              style={{ width: 90, fontSize: 10 }}
              options={available.map(p => ({
                value: p.id,
                label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
              }))}
              onChange={(val) => assignPlayer(teamId, pos, val)}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
              popupMatchSelectWidth={false}
            />
          ) : (
            <Text style={{ color: '#8c8c8c', fontSize: 11 }}>—</Text>
          )}
        </div>
      </Tooltip>
    );
  };

  const rowStyle: React.CSSProperties = { display: 'flex', gap: 10, justifyContent: 'center' };
  const halfStyle: React.CSSProperties = { display: 'flex', flexDirection: 'column', gap: 10, padding: '12px 16px' };

  return (
    <Spin spinning={loading}>
      <div style={{
        background: 'linear-gradient(180deg, #2d5a3d 0%, #3a7a52 100%)',
        borderRadius: 14,
        padding: '16px 12px',
        boxShadow: '0 4px 20px rgba(0,0,0,0.3)',
        maxWidth: 480,
        margin: '0 auto',
      }}>
        {/* хозяева — верхняя половина поля */}
        <div style={{ textAlign: 'center', marginBottom: 8 }}>
          <Tag color="blue" style={{ fontSize: 12, fontWeight: 600 }}>{homeTeamName}</Tag>
          <Text style={{ color: '#ffffffaa', fontSize: 11, marginLeft: 6 }}>хозяева</Text>
        </div>
        <div style={{ background: 'rgba(255,255,255,0.07)', borderRadius: 10 }}>
          <div style={halfStyle}>
            <div style={rowStyle}>
              {BACK_POSITIONS.map(pos => renderSlot(homeTeamId, pos))}
            </div>
            <div style={rowStyle}>
              {FRONT_POSITIONS.map(pos => renderSlot(homeTeamId, pos))}
            </div>
          </div>
        </div>

        {/* сетка */}
        <div style={{ display: 'flex', alignItems: 'center', gap: 8, margin: '10px 0' }}>
          <div style={{ flex: 1, height: 4, background: '#fff', borderRadius: 2, opacity: 0.9 }} />
          <Text style={{ color: '#fff', fontWeight: 700, fontSize: 11, letterSpacing: 2, flexShrink: 0 }}>СЕТКА</Text>
          <div style={{ flex: 1, height: 4, background: '#fff', borderRadius: 2, opacity: 0.9 }} />
        </div>

        {/* гости — нижняя половина поля (зеркально) */}
        <div style={{ background: 'rgba(255,255,255,0.07)', borderRadius: 10 }}>
          <div style={halfStyle}>
            <div style={rowStyle}>
              {[1, 6, 5].map(pos => renderSlot(guestTeamId, pos))}
            </div>
            <div style={rowStyle}>
              {[2, 3, 4].map(pos => renderSlot(guestTeamId, pos))}
            </div>
          </div>
        </div>
        <div style={{ textAlign: 'center', marginTop: 8 }}>
          <Tag color="orange" style={{ fontSize: 12, fontWeight: 600 }}>{guestTeamName}</Tag>
          <Text style={{ color: '#ffffffaa', fontSize: 11, marginLeft: 6 }}>гости</Text>
        </div>
      </div>

      {/* легенда */}
      <div style={{ marginTop: 10, display: 'flex', gap: 8, flexWrap: 'wrap', justifyContent: 'center' }}>
        {Object.entries(POSITION_NAMES).map(([code, name]) => (
          <Tooltip title={name} key={code}>
            <Tag style={{ cursor: 'help', fontSize: 10 }}>{POSITION_LABELS[Number(code)]} ({code})</Tag>
          </Tooltip>
        ))}
      </div>
    </Spin>
  );
};

export default VolleyCourt;
