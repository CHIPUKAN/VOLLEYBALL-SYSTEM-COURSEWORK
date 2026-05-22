import React, { useMemo } from 'react';
import { Typography, Tooltip } from 'antd';
import type { MatchEvent, SetDto } from '../types/index';

const { Text } = Typography;

interface SubstitutionGraphProps {
  events: MatchEvent[];
  sets: SetDto[];
  homeTeamId: number;
  homeTeamName: string;
  guestTeamId: number;
  guestTeamName: string;
}

interface PlayerSpan {
  playerId: number;
  playerName: string;
  startSeq: number;
  endSeq?: number;
  isSubOut: boolean;
  isSubIn: boolean;
}

// график замен матча
const SubstitutionGraph: React.FC<SubstitutionGraphProps> = ({
  events,
  sets,
  homeTeamId,
  homeTeamName,
  guestTeamId,
  guestTeamName,
}) => {
  const subEvents = useMemo(
    () => events.filter(e => e.substitution != null),
    [events]
  );

  if (subEvents.length === 0) {
    return <Text type="secondary">Замен не было</Text>;
  }

  const maxSeq = Math.max(...events.map(e => e.globalSeqInSet), 1);

  const buildSpans = (teamId: number): PlayerSpan[] => {
    const spans: PlayerSpan[] = [];
    const teamSubEvents = subEvents.filter(e => e.teamId === teamId);

    teamSubEvents.forEach(ev => {
      const sub = ev.substitution!;
      // линия выходящего игрока — заканчивается здесь
      const existing = spans.find(s => s.playerId === sub.subOutPlayerId && !s.endSeq);
      if (existing) {
        existing.endSeq = ev.globalSeqInSet;
        existing.isSubOut = true;
      } else {
        spans.push({
          playerId: sub.subOutPlayerId,
          playerName: sub.subOutPlayerName ?? `#${sub.subOutPlayerId}`,
          startSeq: 1,
          endSeq: ev.globalSeqInSet,
          isSubOut: true,
          isSubIn: false,
        });
      }
      // линия входящего игрока — начинается здесь
      spans.push({
        playerId: sub.subInPlayerId,
        playerName: sub.subInPlayerName ?? `#${sub.subInPlayerId}`,
        startSeq: ev.globalSeqInSet,
        endSeq: undefined,
        isSubOut: false,
        isSubIn: true,
      });
    });

    return spans;
  };

  const renderTeam = (teamId: number, teamName: string) => {
    const spans = buildSpans(teamId);
    if (spans.length === 0) return null;

    const W = 500;
    const lineH = 24;
    const H = spans.length * lineH + 20;

    return (
      <div style={{ marginBottom: 24 }}>
        <Text strong style={{ fontSize: 13 }}>{teamName}</Text>
        <svg width="100%" viewBox={`0 0 ${W} ${H}`} style={{ display: 'block', maxWidth: W }}>
          {/* фоновые линии партий */}
          {sets.map(s => {
            const x = ((s.setNumber - 1) / sets.length) * W;
            return (
              <line key={s.setNumber} x1={x} y1={0} x2={x} y2={H}
                stroke="#f0f0f0" strokeWidth={1} />
            );
          })}

          {spans.map((span, idx) => {
            const y = idx * lineH + lineH / 2;
            const x1 = (span.startSeq / maxSeq) * W;
            const x2 = span.endSeq ? (span.endSeq / maxSeq) * W : W;

            return (
              <g key={`${span.playerId}-${idx}`}>
                {/* линия жизни */}
                <line
                  x1={x1} y1={y} x2={x2} y2={y}
                  stroke="#1677ff"
                  strokeWidth={2}
                  strokeDasharray={span.isSubIn ? '4 2' : 'none'}
                />

                {/* имя */}
                <text x={x1 + 2} y={y - 4} fontSize={9} fill="#555">
                  {span.playerName}
                </text>

                {/* точка выхода */}
                {span.endSeq && (
                  <Tooltip title={`Вышел: ${span.playerName}`}>
                    <circle
                      cx={x2} cy={y} r={5}
                      fill="#ff4d4f"
                      style={{ cursor: 'default' }}
                    />
                  </Tooltip>
                )}

                {/* точка входа */}
                {span.isSubIn && (
                  <Tooltip title={`Вошёл: ${span.playerName}`}>
                    <circle
                      cx={x1} cy={y} r={5}
                      fill="#52c41a"
                      style={{ cursor: 'default' }}
                    />
                  </Tooltip>
                )}
              </g>
            );
          })}
        </svg>
      </div>
    );
  };

  return (
    <div>
      {renderTeam(homeTeamId, homeTeamName)}
      {renderTeam(guestTeamId, guestTeamName)}
      <div style={{ display: 'flex', gap: 16, marginTop: 8 }}>
        <span style={{ display: 'flex', alignItems: 'center', gap: 4 }}>
          <svg width={12} height={12}><circle cx={6} cy={6} r={5} fill="#ff4d4f" /></svg>
          <Text style={{ fontSize: 11, color: '#666' }}>Вышел</Text>
        </span>
        <span style={{ display: 'flex', alignItems: 'center', gap: 4 }}>
          <svg width={12} height={12}><circle cx={6} cy={6} r={5} fill="#52c41a" /></svg>
          <Text style={{ fontSize: 11, color: '#666' }}>Вошёл</Text>
        </span>
      </div>
    </div>
  );
};

export default SubstitutionGraph;
