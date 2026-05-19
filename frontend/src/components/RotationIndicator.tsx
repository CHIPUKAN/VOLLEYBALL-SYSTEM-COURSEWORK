import React, { useEffect, useRef } from 'react';
import { Typography } from 'antd';

const { Text } = Typography;

interface RotationIndicatorProps {
  teamName: string;
  animate: boolean;
  onAnimationEnd: () => void;
}

const POSITIONS = [1, 2, 3, 4, 5, 6];

// индикатор ротации команды
const RotationIndicator: React.FC<RotationIndicatorProps> = ({
  teamName,
  animate,
  onAnimationEnd,
}) => {
  const groupRef = useRef<SVGGElement | null>(null);

  useEffect(() => {
    if (!animate || !groupRef.current) return;
    const el = groupRef.current;
    el.style.transition = 'transform 0.6s ease-in-out';
    el.style.transform = 'rotate(-60deg)';
    const timer = setTimeout(() => {
      el.style.transition = 'none';
      el.style.transform = 'rotate(0deg)';
      onAnimationEnd();
    }, 650);
    return () => clearTimeout(timer);
  }, [animate]);

  const cx = 40;
  const cy = 40;
  const r = 28;
  const circleR = 8;

  // позиции: pos 1 снизу-справа, по часовой стрелке
  const posAngle = (pos: number) => {
    // стандартная нумерация: 1=нижний правый, против часовой
    const angles: Record<number, number> = {
      1: -30, 2: -90, 3: -150, 4: 150, 5: 90, 6: 30,
    };
    return (angles[pos] ?? 0) * (Math.PI / 180);
  };

  return (
    <div style={{ textAlign: 'center', display: 'inline-flex', flexDirection: 'column', alignItems: 'center', gap: 2 }}>
      <svg width={80} height={80}>
        {/* стрелка ротации */}
        <path
          d={`M ${cx + r + 2} ${cy} A ${r + 2} ${r + 2} 0 0 0 ${cx} ${cy - r - 2}`}
          fill="none"
          stroke="#1677ff"
          strokeWidth={1.5}
          markerEnd="url(#arrow)"
          opacity={animate ? 1 : 0.3}
        />
        <defs>
          <marker id="arrow" markerWidth="6" markerHeight="6" refX="3" refY="3" orient="auto">
            <path d="M0,0 L0,6 L6,3 z" fill="#1677ff" />
          </marker>
        </defs>

        <g ref={groupRef} style={{ transformOrigin: `${cx}px ${cy}px` }}>
          {POSITIONS.map(pos => {
            const a = posAngle(pos);
            const px = cx + r * Math.cos(a);
            const py = cy + r * Math.sin(a);
            return (
              <g key={pos}>
                <circle
                  cx={px} cy={py} r={circleR}
                  fill={pos === 1 ? '#fadb14' : '#1677ff22'}
                  stroke={pos === 1 ? '#d48806' : '#1677ff'}
                  strokeWidth={1}
                />
                <text
                  x={px} y={py}
                  textAnchor="middle"
                  dominantBaseline="middle"
                  fontSize={8}
                  fontWeight={700}
                  fill={pos === 1 ? '#333' : '#1677ff'}
                >
                  {pos}
                </text>
              </g>
            );
          })}
        </g>
      </svg>
      <Text style={{ fontSize: 10, color: '#666' }}>Ротация {teamName}</Text>
    </div>
  );
};

export default RotationIndicator;
