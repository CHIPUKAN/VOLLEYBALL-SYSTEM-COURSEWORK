import React from 'react';
import { Calendar, Badge, Modal, Typography } from 'antd';
import type { Dayjs } from 'dayjs';
import dayjs from 'dayjs';
import type { Match } from '../types/index';

const { Text } = Typography;

interface MatchCalendarProps {
  matches: Match[];
  onMatchClick: (matchId: number) => void;
  onDateDrop: (matchId: number, newDate: string) => Promise<void>;
  canManage: boolean;
}

const STATUS_COLORS: Record<string, string> = {
  Запланирован: 'blue',
  'В процессе': 'orange',
  Завершён: 'green',
  Отменён: 'red',
  Перенесён: 'gold',
};

// календарный вид матчей
const MatchCalendar: React.FC<MatchCalendarProps> = ({
  matches,
  onMatchClick,
  onDateDrop,
  canManage,
}) => {
  const dateCellRender = (date: Dayjs) => {
    const dayMatches = matches.filter(m => dayjs(m.matchDate).isSame(date, 'day'));
    if (dayMatches.length === 0) return null;

    const visible = dayMatches.slice(0, 2);
    const rest = dayMatches.length - 2;

    return (
      <ul style={{ listStyle: 'none', padding: 0, margin: 0 }}>
        {visible.map(m => (
          <li
            key={m.id}
            draggable={canManage}
            onDragStart={e => e.dataTransfer.setData('matchId', String(m.id))}
            onClick={e => { e.stopPropagation(); onMatchClick(m.id); }}
            style={{ cursor: 'pointer', marginBottom: 2 }}
          >
            <Badge
              color={STATUS_COLORS[m.statusName ?? ''] ?? 'default'}
              text={
                <Text style={{ fontSize: 11 }} ellipsis>
                  {m.homeTeamName?.split(' ')[0]} — {m.guestTeamName?.split(' ')[0]}
                </Text>
              }
            />
          </li>
        ))}
        {rest > 0 && (
          <li>
            <Text style={{ fontSize: 10, color: '#aaa' }}>+{rest} ещё</Text>
          </li>
        )}
      </ul>
    );
  };

  const handleDrop = (e: React.DragEvent<HTMLDivElement>, date: Dayjs) => {
    e.preventDefault();
    const matchId = Number(e.dataTransfer.getData('matchId'));
    if (!matchId || !canManage) return;

    const newDate = date.format('YYYY-MM-DD');
    const match = matches.find(m => m.id === matchId);
    if (match && dayjs(match.matchDate).isSame(date, 'day')) return;

    Modal.confirm({
      title: 'Перенести матч?',
      content: `Перенести матч на ${date.format('DD.MM.YYYY')}?`,
      okText: 'Перенести',
      cancelText: 'Отмена',
      onOk: () => onDateDrop(matchId, newDate),
    });
  };

  // обёртка с drag-over на ячейки
  const cellRender = (date: Dayjs) => {
    return (
      <div
        onDragOver={e => e.preventDefault()}
        onDrop={e => handleDrop(e, date)}
        style={{ minHeight: 60 }}
      >
        {dateCellRender(date)}
      </div>
    );
  };

  return (
    <Calendar
      cellRender={(date, info) => {
        if (info.type === 'date') return cellRender(date);
        return info.originNode;
      }}
    />
  );
};

export default MatchCalendar;
