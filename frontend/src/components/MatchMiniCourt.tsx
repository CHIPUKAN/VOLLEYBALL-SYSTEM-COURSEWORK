import React, { useMemo } from 'react';
import type { MatchEvent } from '../types/index';

interface MatchMiniCourtProps {
  events: MatchEvent[];
  homeTeamId: number;
  guestTeamId: number;
  homeTeamName: string;
  guestTeamName: string;
  size?: number;
}

// мини-площадка SVG в шапке матча
const MatchMiniCourt: React.FC<MatchMiniCourtProps> = ({
  events,
  homeTeamId,
  guestTeamId,
  homeTeamName,
  guestTeamName,
  size = 80,
}) => {
  const h = Math.round(size * 0.625); // ~50 при size=80

  const servingTeamId = useMemo(() => {
    if (events.length === 0) return homeTeamId;
    for (let i = events.length - 1; i >= 0; i--) {
      if (events[i].teamId != null) return events[i].teamId!;
    }
    return homeTeamId;
  }, [events, homeTeamId]);

  const ballX = servingTeamId === homeTeamId
    ? size * 0.25
    : size * 0.75;
  const ballY = h / 2;

  return (
    <svg width={size} height={h} style={{ display: 'block' }}>
      {/* площадка */}
      <rect x={0} y={0} width={size} height={h} rx={3}
        fill="#3a7a52" />
      {/* сетка */}
      <line x1={size / 2} y1={0} x2={size / 2} y2={h}
        stroke="#fff" strokeWidth={2} />
      {/* боковые линии */}
      <rect x={1} y={1} width={size - 2} height={h - 2} rx={2}
        fill="none" stroke="#ffffff66" strokeWidth={1} />
      {/* мяч */}
      <circle cx={ballX} cy={ballY} r={5}
        fill="#fadb14" stroke="#d48806" strokeWidth={1} />
      {/* подписи */}
      <text x={size * 0.25} y={h - 3}
        textAnchor="middle" fontSize={7} fill="#ffffff99">
        {homeTeamName.slice(0, 6)}
      </text>
      <text x={size * 0.75} y={h - 3}
        textAnchor="middle" fontSize={7} fill="#ffffff99">
        {guestTeamName.slice(0, 6)}
      </text>
    </svg>
  );
  void guestTeamId;
};

export default MatchMiniCourt;
