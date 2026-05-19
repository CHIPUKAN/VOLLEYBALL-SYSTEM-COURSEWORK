import React from 'react';
import { Tag, Typography } from 'antd';
import type { StartingLineup } from '../types/index';

const { Text } = Typography;

interface LiveCourtViewProps {
  homeTeamId: number;
  homeTeamName: string;
  guestTeamId: number;
  guestTeamName: string;
  lineups: StartingLineup[];
  servingTeamId: number;
  onPlayerClick: (player: StartingLineup) => void;
  onPlayerContextMenu?: (player: StartingLineup, position: { x: number; y: number }) => void;
  disabled?: boolean;
}

const FRONT_POSITIONS_HOME = [4, 3, 2];
const BACK_POSITIONS_HOME = [5, 6, 1];

// кликабельная площадка для live-режима
const LiveCourtView: React.FC<LiveCourtViewProps> = ({
  homeTeamId,
  homeTeamName,
  guestTeamId,
  guestTeamName,
  lineups,
  servingTeamId,
  onPlayerClick,
  onPlayerContextMenu,
  disabled = false,
}) => {
  const getLineup = (teamId: number, pos: number): StartingLineup | undefined =>
    lineups.find(l => l.teamId === teamId && l.positionNo === pos);

  const isServingPosition = (teamId: number, pos: number): boolean =>
    teamId === servingTeamId && pos === 1;

  const renderSlot = (teamId: number, pos: number) => {
    const lineup = getLineup(teamId, pos);
    const isServing = isServingPosition(teamId, pos);
    const key = `${teamId}-${pos}`;

    if (!lineup) {
      // пустая ячейка
      return (
        <div
          key={key}
          style={{
            width: 84,
            height: 84,
            borderRadius: 12,
            background: 'rgba(0,0,0,0.22)',
            border: '2px dashed #8c8c8c',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: '#595959',
            fontSize: 20,
            cursor: 'default',
          }}
        >
          +
        </div>
      );
    }

    const lastName = (lineup.playerFullName ?? '').split(' ').slice(0, 2).join(' ');

    return (
      <div key={key} style={{ position: 'relative', display: 'inline-block' }}>
        {isServing && (
          <div style={{ position: 'absolute', top: -14, left: '50%', transform: 'translateX(-50%)', fontSize: 14, zIndex: 1 }}>
            🏐
          </div>
        )}
        <div
          onClick={() => !disabled && onPlayerClick(lineup)}
          onContextMenu={e => {
            if (onPlayerContextMenu) {
              e.preventDefault();
              onPlayerContextMenu(lineup, { x: e.clientX, y: e.clientY });
            }
          }}
          style={{
            width: 84,
            height: 84,
            borderRadius: 12,
            background: isServing
              ? 'rgba(250,200,0,0.35)'
              : 'rgba(22,119,255,0.22)',
            border: isServing
              ? '2px solid #fadb14'
              : '2px solid #1677ff',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            cursor: disabled ? 'default' : 'pointer',
            boxShadow: isServing
              ? '0 0 10px rgba(250,200,0,0.5)'
              : '0 2px 8px rgba(0,0,0,0.25)',
            transition: disabled ? 'none' : 'transform 0.15s, box-shadow 0.15s',
            userSelect: 'none',
          }}
          onMouseEnter={e => {
            if (!disabled) {
              (e.currentTarget as HTMLDivElement).style.transform = 'scale(1.08)';
              (e.currentTarget as HTMLDivElement).style.boxShadow = '0 4px 16px rgba(0,0,0,0.4)';
            }
          }}
          onMouseLeave={e => {
            (e.currentTarget as HTMLDivElement).style.transform = '';
            (e.currentTarget as HTMLDivElement).style.boxShadow = isServing
              ? '0 0 10px rgba(250,200,0,0.5)'
              : '0 2px 8px rgba(0,0,0,0.25)';
          }}
        >
          <Text style={{ fontSize: 18, fontWeight: 700, color: '#fff', lineHeight: 1 }}>
            {lineup.shirtNumber ? `#${lineup.shirtNumber}` : pos}
          </Text>
          <Text style={{ fontSize: 10, color: '#ffffffcc', textAlign: 'center', lineHeight: 1.2, marginTop: 3, maxWidth: 76, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
            {lastName || `Игрок ${lineup.playerId}`}
          </Text>
        </div>
      </div>
    );
  };

  const rowStyle: React.CSSProperties = {
    display: 'flex',
    gap: 10,
    justifyContent: 'center',
  };
  const halfStyle: React.CSSProperties = {
    display: 'flex',
    flexDirection: 'column',
    gap: 10,
    padding: '14px 12px',
  };

  return (
    <div style={{
      background: 'linear-gradient(180deg, #2d5a3d 0%, #3a7a52 100%)',
      borderRadius: 14,
      padding: '16px 12px',
      boxShadow: '0 4px 20px rgba(0,0,0,0.3)',
      maxWidth: 520,
      margin: '0 auto',
    }}>
      {/* хозяева */}
      <div style={{ textAlign: 'center', marginBottom: 8 }}>
        <Tag color="blue" style={{ fontSize: 12, fontWeight: 600 }}>{homeTeamName}</Tag>
        <Text style={{ color: '#ffffffaa', fontSize: 11, marginLeft: 6 }}>хозяева</Text>
        {servingTeamId === homeTeamId && (
          <Tag color="gold" style={{ fontSize: 11, marginLeft: 6 }}>подаёт 🏐</Tag>
        )}
      </div>
      <div style={{ background: 'rgba(255,255,255,0.07)', borderRadius: 10 }}>
        <div style={halfStyle}>
          <div style={rowStyle}>
            {BACK_POSITIONS_HOME.map(pos => renderSlot(homeTeamId, pos))}
          </div>
          <div style={rowStyle}>
            {FRONT_POSITIONS_HOME.map(pos => renderSlot(homeTeamId, pos))}
          </div>
        </div>
      </div>

      {/* сетка */}
      <div style={{ display: 'flex', alignItems: 'center', gap: 8, margin: '10px 0' }}>
        <div style={{ flex: 1, height: 4, background: '#fff', borderRadius: 2, opacity: 0.9 }} />
        <Text style={{ color: '#fff', fontWeight: 700, fontSize: 11, letterSpacing: 2, flexShrink: 0 }}>СЕТКА</Text>
        <div style={{ flex: 1, height: 4, background: '#fff', borderRadius: 2, opacity: 0.9 }} />
      </div>

      {/* гости */}
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
        {servingTeamId === guestTeamId && (
          <Tag color="gold" style={{ fontSize: 11, marginLeft: 6 }}>подаёт 🏐</Tag>
        )}
      </div>
    </div>
  );
};

export default LiveCourtView;
