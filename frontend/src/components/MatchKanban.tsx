import React, { useState } from 'react';
import { Typography, Tag, Button, Dropdown, App } from 'antd';
import { EllipsisOutlined, EditOutlined, EyeOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import type { Match, LookupDto } from '../types/index';

const { Text } = Typography;

const COLUMNS = [
  { statusName: 'Запланирован', color: '#378ADD', bgColor: '#E6F1FB' },
  { statusName: 'В процессе',   color: '#BA7517', bgColor: '#FAEEDA' },
  { statusName: 'Завершён',     color: '#3B6D11', bgColor: '#EAF3DE' },
  { statusName: 'Перенесён',    color: '#534AB7', bgColor: '#EEEDFE' },
  { statusName: 'Отменён',      color: '#A32D2D', bgColor: '#FCEBEB' },
];

interface MatchKanbanProps {
  matches: Match[];
  statuses: LookupDto[];
  onStatusChange: (matchId: number, newStatusCode: number) => Promise<void>;
  onMatchClick: (matchId: number) => void;
  canManage: boolean;
  onEdit?: (match: Match) => void;
  onDelete?: (matchId: number) => void;
}

// kanban-доска матчей
const MatchKanban: React.FC<MatchKanbanProps> = ({
  matches,
  statuses,
  onStatusChange,
  onMatchClick,
  canManage,
  onEdit,
  onDelete,
}) => {
  const navigate = useNavigate();
  const { message } = App.useApp();
  const [optimistic, setOptimistic] = useState<Map<number, string>>(new Map());
  const [dragOverCol, setDragOverCol] = useState<string | null>(null);

  const getStatusName = (match: Match) =>
    optimistic.get(match.id) ?? match.statusName ?? '';

  const handleDrop = async (e: React.DragEvent, targetStatusName: string) => {
    e.preventDefault();
    setDragOverCol(null);
    const matchId = Number(e.dataTransfer.getData('matchId'));
    const match = matches.find(m => m.id === matchId);
    if (!match || getStatusName(match) === targetStatusName) return;

    const targetStatus = statuses.find(s => s.name === targetStatusName);
    if (!targetStatus) return;

    // оптимистичное обновление
    setOptimistic(prev => new Map(prev).set(matchId, targetStatusName));
    try {
      await onStatusChange(matchId, targetStatus.code);
    } catch {
      setOptimistic(prev => {
        const next = new Map(prev);
        next.delete(matchId);
        return next;
      });
      message.error('Ошибка смены статуса');
    }
  };

  return (
    <div style={{ display: 'flex', gap: 12, overflowX: 'auto', paddingBottom: 8 }}>
      {COLUMNS.map(col => {
        const colMatches = matches.filter(m => getStatusName(m) === col.statusName);
        const isDragOver = dragOverCol === col.statusName;

        return (
          <div
            key={col.statusName}
            onDragOver={e => { e.preventDefault(); setDragOverCol(col.statusName); }}
            onDragLeave={() => setDragOverCol(null)}
            onDrop={e => handleDrop(e, col.statusName)}
            style={{
              minWidth: 220,
              width: 220,
              flexShrink: 0,
              background: isDragOver ? col.bgColor + 'dd' : col.bgColor,
              borderRadius: 10,
              border: isDragOver ? `2px solid ${col.color}` : '2px solid transparent',
              transition: 'border 0.15s',
            }}
          >
            {/* заголовок колонки */}
            <div style={{
              padding: '10px 12px 8px',
              borderBottom: `2px solid ${col.color}33`,
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
            }}>
              <Text strong style={{ color: col.color, fontSize: 13 }}>{col.statusName}</Text>
              <Tag style={{ border: `1px solid ${col.color}`, color: col.color, background: 'transparent', fontSize: 11 }}>
                {colMatches.length}
              </Tag>
            </div>

            {/* карточки */}
            <div style={{
              height: 'calc(100vh - 220px)',
              overflowY: 'auto',
              padding: '8px 8px',
              display: 'flex',
              flexDirection: 'column',
              gap: 8,
            }}>
              {colMatches.map(match => (
                <MatchKanbanCard
                  key={match.id}
                  match={match}
                  colColor={col.color}
                  canManage={canManage}
                  onClick={() => onMatchClick(match.id)}
                  onEdit={() => onEdit?.(match)}
                  onDelete={() => onDelete?.(match.id)}
                  navigate={navigate}
                />
              ))}
              {colMatches.length === 0 && (
                <div style={{ textAlign: 'center', padding: '20px 0' }}>
                  <Text type="secondary" style={{ fontSize: 12 }}>Нет матчей</Text>
                </div>
              )}
            </div>
          </div>
        );
      })}
    </div>
  );
};

// карточка матча в Kanban
interface KanbanCardProps {
  match: Match;
  colColor: string;
  canManage: boolean;
  onClick: () => void;
  onEdit: () => void;
  onDelete: () => void;
  navigate: ReturnType<typeof useNavigate>;
}

const MatchKanbanCard: React.FC<KanbanCardProps> = ({
  match,
  colColor,
  canManage,
  onClick,
  onEdit,
  onDelete,
  navigate,
}) => {
  const isLive = match.statusName === 'В процессе';
  const isDone = match.statusName === 'Завершён';

  const menuItems = [
    { key: 'view', icon: <EyeOutlined />, label: 'Детали', onClick },
    ...(canManage ? [
      { key: 'edit', icon: <EditOutlined />, label: 'Редактировать', onClick: onEdit },
      { key: 'delete', icon: <DeleteOutlined />, label: 'Удалить', danger: true, onClick: onDelete },
    ] : []),
  ];

  return (
    <div
      draggable={canManage}
      onDragStart={e => { e.dataTransfer.setData('matchId', String(match.id)); }}
      style={{
        background: '#fff',
        borderRadius: 8,
        padding: '10px 10px 8px',
        boxShadow: '0 1px 4px rgba(0,0,0,0.08)',
        borderLeft: `3px solid ${colColor}`,
        cursor: canManage ? 'grab' : 'pointer',
        position: 'relative',
      }}
    >
      {/* пульсирующий индикатор live */}
      {isLive && (
        <span style={{
          position: 'absolute', top: 8, left: 8,
          width: 8, height: 8, borderRadius: '50%',
          background: '#ff7a00',
          boxShadow: '0 0 0 0 rgba(255,122,0,0.4)',
          animation: 'pulse 1.5s infinite',
          display: 'inline-block',
        }} />
      )}

      {/* заголовок */}
      <Text strong style={{ fontSize: 12, display: 'block', marginBottom: 4, paddingLeft: isLive ? 14 : 0 }}>
        {match.homeTeamName ?? '?'} — {match.guestTeamName ?? '?'}
      </Text>

      <Text style={{ fontSize: 11, color: '#888', display: 'block' }}>
        {match.matchDate ? match.matchDate.slice(0, 10) : '—'}
        {match.startTime ? ` · ${match.startTime.slice(0, 5)}` : ''}
      </Text>

      {match.venueName && (
        <Text style={{ fontSize: 11, color: '#aaa', display: 'block' }}>
          {match.venueName}
        </Text>
      )}

      {/* кнопки для live */}
      {isLive && (
        <Button
          size="small"
          type="primary"
          style={{ marginTop: 6, fontSize: 11, height: 24 }}
          onClick={e => { e.stopPropagation(); navigate(`/matches/${match.id}`); }}
        >
          Открыть
        </Button>
      )}

      {/* меню */}
      <div style={{ position: 'absolute', top: 6, right: 6 }}>
        <Dropdown
          menu={{
            items: menuItems.map(i => ({
              key: i.key,
              icon: i.icon,
              label: i.label,
              danger: (i as { danger?: boolean }).danger,
              onClick: i.onClick,
            })),
          }}
          trigger={['click']}
        >
          <Button
            type="text"
            size="small"
            icon={<EllipsisOutlined />}
            onClick={e => e.stopPropagation()}
            style={{ padding: 0, width: 20, height: 20 }}
          />
        </Dropdown>
      </div>

      {isLive && (
        <style>{`@keyframes pulse{0%,100%{box-shadow:0 0 0 0 rgba(255,122,0,.4)}50%{box-shadow:0 0 0 5px rgba(255,122,0,0)}}`}</style>
      )}
    </div>
  );
};

export default MatchKanban;
