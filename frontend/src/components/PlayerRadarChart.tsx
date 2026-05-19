import React from 'react';
import type { PlayerStats } from '../types/index';

interface PlayerRadarChartProps {
  stats: PlayerStats;
  compareStats?: PlayerStats;
  size?: number;
}

const AXES = [
  { key: 'servesTotal',     label: 'Подача', max: 100 },
  { key: 'attackPoints',    label: 'Атака',  max: 50  },
  { key: 'blocks',          label: 'Блок',   max: 30  },
  { key: 'receptionsTotal', label: 'Приём',  max: 80  },
  { key: 'aces',            label: 'Эйсы',   max: 20  },
  { key: 'totalPoints',     label: 'Очки',   max: 60  },
];

const N = AXES.length;

// радар-карта статистики игрока (чистый SVG)
const PlayerRadarChart: React.FC<PlayerRadarChartProps> = ({
  stats,
  compareStats,
  size = 220,
}) => {
  const cx = size / 2;
  const cy = size / 2;
  const radius = (size / 2) - 32;

  const angle = (i: number) => (Math.PI / 2) + (2 * Math.PI * i / N);
  const point = (i: number, r: number) => ({
    x: cx + r * Math.cos(angle(i)),
    y: cy - r * Math.sin(angle(i)),
  });

  // сетка: 4 уровня
  const gridLevels = [0.25, 0.5, 0.75, 1];
  const gridPolygon = (factor: number) =>
    Array.from({ length: N }, (_, i) => point(i, radius * factor))
      .map(p => `${p.x},${p.y}`)
      .join(' ');

  // данные игрока
  const statsPolygon = (s: PlayerStats) =>
    AXES.map((ax, i) => {
      const val = (s as unknown as Record<string, number>)[ax.key] ?? 0;
      const r = Math.min(val / ax.max, 1) * radius;
      return point(i, r);
    }).map(p => `${p.x},${p.y}`).join(' ');

  return (
    <svg width={size} height={size} style={{ display: 'block' }}>
      {/* сетка */}
      {gridLevels.map(f => (
        <polygon
          key={f}
          points={gridPolygon(f)}
          fill="none"
          stroke="#d9d9d9"
          strokeWidth={0.8}
        />
      ))}

      {/* оси */}
      {AXES.map((_, i) => {
        const outer = point(i, radius);
        return (
          <line
            key={i}
            x1={cx} y1={cy}
            x2={outer.x} y2={outer.y}
            stroke="#e8e8e8"
            strokeWidth={1}
          />
        );
      })}

      {/* подписи осей */}
      {AXES.map((ax, i) => {
        const labelR = radius + 18;
        const p = point(i, labelR);
        return (
          <text
            key={i}
            x={p.x}
            y={p.y}
            textAnchor="middle"
            dominantBaseline="middle"
            fontSize={10}
            fill="#666"
          >
            {ax.label}
          </text>
        );
      })}

      {/* полигон сравнения (позади) */}
      {compareStats && (
        <polygon
          points={statsPolygon(compareStats)}
          fill="rgba(186,117,23,0.2)"
          stroke="#BA7517"
          strokeWidth={1}
          strokeDasharray="4 3"
        />
      )}

      {/* полигон игрока */}
      <polygon
        points={statsPolygon(stats)}
        fill="rgba(22,119,255,0.25)"
        stroke="#1677ff"
        strokeWidth={1.5}
      />

      {/* точки и значения игрока */}
      {AXES.map((ax, i) => {
        const val = (stats as unknown as Record<string, number>)[ax.key] ?? 0;
        const r = Math.min(val / ax.max, 1) * radius;
        const p = point(i, r);
        const labelR = radius + 18;
        const lp = point(i, labelR);
        const offsetX = lp.x > cx + 2 ? 5 : lp.x < cx - 2 ? -5 : 0;
        const offsetY = lp.y > cy + 2 ? 10 : lp.y < cy - 2 ? -4 : 4;
        return (
          <g key={i}>
            <circle cx={p.x} cy={p.y} r={3} fill="#1677ff" />
            <text
              x={lp.x + offsetX}
              y={lp.y + offsetY}
              textAnchor="middle"
              dominantBaseline="middle"
              fontSize={9}
              fontWeight={700}
              fill="#1677ff"
            >
              {val}
            </text>
          </g>
        );
      })}
    </svg>
  );
};

export default PlayerRadarChart;
