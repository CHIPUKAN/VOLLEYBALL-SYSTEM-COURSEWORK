import React from 'react';
import { Typography, Tooltip } from 'antd';
import type { MatchEvent, SetDto } from '../types/index';

const { Text } = Typography;

interface MatchTimelineProps {
  matchId: number;
  events: MatchEvent[];
  sets: SetDto[];
  homeTeamName: string;
  guestTeamName: string;
}

const EVENT_STYLES: Record<string, { icon: string; color: string; label: string }> = {
  'Эйс':              { icon: '⚡', color: '#185FA5', label: 'Эйс' },
  'Блок-очко':        { icon: '🛡', color: '#3B6D11', label: 'Блок' },
  'Очко атаки':       { icon: '💥', color: '#534AB7', label: 'Атака' },
  'Ошибка подачи':    { icon: '✗',  color: '#A32D2D', label: 'Ошибка' },
  'Ошибка атаки':     { icon: '✗',  color: '#A32D2D', label: 'Ошибка' },
  'Замена':           { icon: '↔',  color: '#BA7517', label: 'Замена' },
  'Тайм-аут':         { icon: 'T',  color: '#5F5E5A', label: 'Тайм-аут' },
};
const DEFAULT_STYLE = { icon: '●', color: '#aaa', label: 'Событие' };

// таймлайн событий матча
const MatchTimeline: React.FC<MatchTimelineProps> = ({
  events,
  sets,
  homeTeamName,
  guestTeamName,
}) => {
  if (events.length === 0) {
    return <Text type="secondary">Нет событий для отображения</Text>;
  }

  const setNumbers = Array.from(new Set(events.map(e => e.setNumber))).sort();

  // типы событий в данных
  const usedTypes = Array.from(new Set(events.map(e => e.eventTypeName ?? '').filter(Boolean)));

  return (
    <div>
      {setNumbers.map(setNum => {
        const setEvents = events.filter(e => e.setNumber === setNum);
        const setData = sets.find(s => s.setNumber === setNum);
        const total = setEvents.length || 1;

        const homeEvents = setEvents.filter(e => e.teamId !== undefined);
        const guestTeamId = setEvents.find(e => e.teamId)?.teamId;

        return (
          <div key={setNum} style={{ marginBottom: 24 }}>
            <div style={{ marginBottom: 6, display: 'flex', alignItems: 'center', gap: 8 }}>
              <Text strong>Партия {setNum}</Text>
              {setData && (
                <Text type="secondary" style={{ fontSize: 12 }}>
                  {setData.homeScore}:{setData.guestScore}
                </Text>
              )}
            </div>

            {/* дорожка хозяев */}
            <div style={{ marginBottom: 4 }}>
              <Text style={{ fontSize: 11, color: '#888', width: 80, display: 'inline-block' }}>
                {homeTeamName}
              </Text>
              <div style={{ position: 'relative', height: 28, background: 'rgba(22,119,255,0.08)', borderRadius: 4, display: 'inline-block', width: 'calc(100% - 84px)' }}>
                {setEvents.filter(e => !e.isTeamEvent || e.teamId).map(ev => {
                  const style = EVENT_STYLES[ev.eventTypeName ?? ''] ?? DEFAULT_STYLE;
                  const pct = ((ev.seqInMatch - setEvents[0].seqInMatch) / total) * 100;
                  const isHome = ev.teamId !== guestTeamId;
                  if (!isHome) return null;
                  return (
                    <Tooltip
                      key={ev.id}
                      title={`${ev.eventTypeName ?? '?'} — ${ev.playerFullName ?? ev.teamName ?? '—'} · ${ev.homeScoreAtMoment}:${ev.guestScoreAtMoment}`}
                    >
                      <span style={{
                        position: 'absolute',
                        left: `${Math.min(pct, 96)}%`,
                        top: '50%',
                        transform: 'translateY(-50%)',
                        fontSize: 14,
                        color: style.color,
                        cursor: 'default',
                        userSelect: 'none',
                      }}>
                        {style.icon}
                      </span>
                    </Tooltip>
                  );
                })}
              </div>
            </div>

            {/* дорожка гостей */}
            <div>
              <Text style={{ fontSize: 11, color: '#888', width: 80, display: 'inline-block' }}>
                {guestTeamName}
              </Text>
              <div style={{ position: 'relative', height: 28, background: 'rgba(250,100,0,0.08)', borderRadius: 4, display: 'inline-block', width: 'calc(100% - 84px)' }}>
                {setEvents.map(ev => {
                  const style = EVENT_STYLES[ev.eventTypeName ?? ''] ?? DEFAULT_STYLE;
                  const pct = ((ev.seqInMatch - setEvents[0].seqInMatch) / total) * 100;
                  const isGuest = ev.teamId === guestTeamId;
                  if (!isGuest) return null;
                  return (
                    <Tooltip
                      key={ev.id}
                      title={`${ev.eventTypeName ?? '?'} — ${ev.playerFullName ?? ev.teamName ?? '—'} · ${ev.homeScoreAtMoment}:${ev.guestScoreAtMoment}`}
                    >
                      <span style={{
                        position: 'absolute',
                        left: `${Math.min(pct, 96)}%`,
                        top: '50%',
                        transform: 'translateY(-50%)',
                        fontSize: 14,
                        color: style.color,
                        cursor: 'default',
                        userSelect: 'none',
                      }}>
                        {style.icon}
                      </span>
                    </Tooltip>
                  );
                })}
              </div>
            </div>

            {/* ось X — счёт */}
            <div style={{ marginLeft: 84, display: 'flex', justifyContent: 'space-between' }}>
              {setEvents.filter((_, i) => i % Math.ceil(total / 10) === 0).map(ev => (
                <Text key={ev.id} style={{ fontSize: 9, color: '#bbb' }}>
                  {ev.homeScoreAtMoment}:{ev.guestScoreAtMoment}
                </Text>
              ))}
            </div>
          </div>
        );
        void homeEvents;
      })}

      {/* легенда */}
      <div style={{ display: 'flex', flexWrap: 'wrap', gap: 12, marginTop: 8 }}>
        {usedTypes.map(t => {
          const s = EVENT_STYLES[t] ?? DEFAULT_STYLE;
          return (
            <span key={t} style={{ display: 'flex', alignItems: 'center', gap: 4, fontSize: 12 }}>
              <span style={{ color: s.color, fontSize: 14 }}>{s.icon}</span>
              <Text style={{ color: '#666', fontSize: 11 }}>{t}</Text>
            </span>
          );
        })}
      </div>
    </div>
  );
};

export default MatchTimeline;
