import React from 'react';
import { Typography } from 'antd';
import type { Player } from '../types/index';

const { Text } = Typography;

interface PlayerCardProps {
  player: Player;
  draggable?: boolean;
  onDragStart?: (player: Player) => void;
  onClick?: (player: Player) => void;
}

const AVATAR_COLORS = [
  '#B5D4F4', '#C0DD97', '#F5C4B3', '#F4C0D1',
  '#AFA9EC', '#FAC775', '#9FE1CB', '#D3D1C7',
];

function teamColor(teamId: number): string {
  return AVATAR_COLORS[teamId % AVATAR_COLORS.length];
}

// карточка игрока
const PlayerCard: React.FC<PlayerCardProps> = ({
  player,
  draggable = false,
  onDragStart,
  onClick,
}) => {
  const initials =
    (player.firstName?.[0] ?? '') + (player.lastName?.[0] ?? '');
  const bg = teamColor(player.teamId);

  return (
    <div
      draggable={draggable}
      onDragStart={() => onDragStart?.(player)}
      onClick={() => onClick?.(player)}
      style={{
        minWidth: 160,
        maxWidth: 200,
        padding: '12px 10px',
        borderRadius: 10,
        background: '#fff',
        border: '1px solid #e0e0e0',
        boxShadow: '0 1px 4px rgba(0,0,0,0.07)',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        gap: 6,
        cursor: onClick ? 'pointer' : draggable ? 'grab' : 'default',
        transition: 'transform 0.15s, box-shadow 0.15s, border-color 0.15s',
        userSelect: 'none',
        position: 'relative',
      }}
      onMouseEnter={e => {
        (e.currentTarget as HTMLDivElement).style.transform = 'translateY(-2px)';
        (e.currentTarget as HTMLDivElement).style.boxShadow = '0 4px 12px rgba(0,0,0,0.13)';
        (e.currentTarget as HTMLDivElement).style.borderColor = '#1677ff55';
      }}
      onMouseLeave={e => {
        (e.currentTarget as HTMLDivElement).style.transform = '';
        (e.currentTarget as HTMLDivElement).style.boxShadow = '0 1px 4px rgba(0,0,0,0.07)';
        (e.currentTarget as HTMLDivElement).style.borderColor = '#e0e0e0';
      }}
    >
      {/* номер рубашки */}
      {player.shirtNumber && (
        <div style={{ position: 'absolute', top: 8, right: 10 }}>
          <Text style={{ fontSize: 11, color: '#888', fontWeight: 600 }}>
            #{player.shirtNumber}
          </Text>
        </div>
      )}

      {/* аватар */}
      <div style={{
        width: 56,
        height: 56,
        borderRadius: '50%',
        background: bg,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        fontSize: 18,
        fontWeight: 700,
        color: '#333',
        flexShrink: 0,
      }}>
        {initials || '?'}
      </div>

      {/* ФИО */}
      <div style={{ textAlign: 'center' }}>
        <Text strong style={{ fontSize: 12, display: 'block', lineHeight: 1.3 }}>
          {player.lastName}
        </Text>
        <Text style={{ fontSize: 11, color: '#555', lineHeight: 1.2 }}>
          {player.firstName}
        </Text>
      </div>

      {/* амплуа */}
      {player.ampluaName && (
        <Text style={{ fontSize: 10, color: '#999', textAlign: 'center' }}>
          {player.ampluaName}
        </Text>
      )}
    </div>
  );
};

export default PlayerCard;
