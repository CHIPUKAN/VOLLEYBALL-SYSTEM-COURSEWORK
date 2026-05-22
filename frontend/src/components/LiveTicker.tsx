import React, { useEffect, useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { matchesApi, setsApi } from '../api/index';
import type { Match, SetDto } from '../types/index';

interface MatchTickerInfo {
  match: Match;
  currentSet: SetDto | null;
}

// живой тикер матчей (липкая полоска)
const LiveTicker: React.FC = () => {
  const [tickerItems, setTickerItems] = useState<MatchTickerInfo[]>([]);
  const navigate = useNavigate();
  const intervalRef = useRef<ReturnType<typeof setInterval> | null>(null);

  const fetchLive = async () => {
    try {
      // statusCode 2 = В процессе — серверный фильтр снижает нагрузку
      const live = await matchesApi.getAll({ statusCode: 2 });
      if (live.length === 0) {
        setTickerItems([]);
        return;
      }
      const setsResults = await Promise.all(
        live.map(m => setsApi.getAll(m.id).catch(() => [] as SetDto[]))
      );
      setTickerItems(
        live.map((m, i) => {
          const sets = setsResults[i];
          const currentSet = sets.length > 0
            ? sets.reduce((a, b) => b.setNumber > a.setNumber ? b : a)
            : null;
          return { match: m, currentSet };
        })
      );
    } catch { /* некритично */ }
  };

  useEffect(() => {
    fetchLive();
    intervalRef.current = setInterval(fetchLive, 15000);
    return () => {
      if (intervalRef.current) clearInterval(intervalRef.current);
    };
  }, []);

  if (tickerItems.length === 0) return null;

  return (
    <>
      <style>{`
        @keyframes ticker {
          0%   { transform: translateX(0); }
          100% { transform: translateX(-50%); }
        }
        .ticker-track {
          display: inline-block;
          white-space: nowrap;
          animation: ticker ${Math.max(20, tickerItems.length * 12)}s linear infinite;
        }
        .ticker-track:hover {
          animation-play-state: paused;
        }
      `}</style>
      <div style={{
        height: 32,
        background: '#1a1a2e',
        overflow: 'hidden',
        display: 'flex',
        alignItems: 'center',
      }}>
        <div style={{ overflow: 'hidden', width: '100%' }}>
          <span className="ticker-track" style={{ color: '#fff', fontSize: 12 }}>
            {[...tickerItems, ...tickerItems].map(({ match: m, currentSet }, idx) => (
              <React.Fragment key={`${m.id}-${idx}`}>
                {idx > 0 && (
                  <span style={{ color: '#ffffff55', padding: '0 20px' }}>·</span>
                )}
                <span
                  onClick={() => navigate(`/matches/${m.id}`)}
                  style={{ cursor: 'pointer' }}
                >
                  🏐 {m.homeTeamName ?? '?'} vs {m.guestTeamName ?? '?'}
                  {currentSet && (
                    <>
                      <span style={{ color: '#ffffff88', margin: '0 6px' }}>·</span>
                      Партия {currentSet.setNumber}
                      <span style={{ color: '#ffffff88', margin: '0 6px' }}>·</span>
                      {currentSet.homeScore}:{currentSet.guestScore}
                    </>
                  )}
                </span>
              </React.Fragment>
            ))}
          </span>
        </div>
      </div>
    </>
  );
};

export default LiveTicker;
