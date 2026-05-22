import React, { useState, useEffect, useCallback, useRef } from 'react';
import {
  Modal, Row, Col, Card, Typography, Button, Tag, App, List, Popconfirm, Space, Badge,
} from 'antd';
import { QuestionOutlined, WifiOutlined } from '@ant-design/icons';
import type {
  Match, StartingLineup, MatchEvent, Player, LookupDto, LookupItemDto,
  CoinTossResult, LiveMatchState, ActionResult,
} from '../types/index';
import {
  eventsApi, lineupsApi, playersApi, setsApi, matchesApi,
} from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import LiveCourtView from './LiveCourtView';
import ActionPickerModal from './ActionPickerModal';
import SubstitutionModal from './SubstitutionModal';
import type { SubPair } from './SubstitutionModal';
import SanctionQuickModal from './SanctionQuickModal';
import RotationIndicator from './RotationIndicator';
import { sanctionsApi } from '../api/index';
import { getApiError } from '../utils/apiError';

// dev-only логгер (только в режиме разработки, не попадает в prod-сборку)
const log = import.meta.env.DEV
  ? (...args: unknown[]) => console.log('[LiveMatch]', ...args)
  : () => {};

// событие в офлайн-буфере
interface PendingEvent {
  id: string;
  actingTeamId: number;
  player: StartingLineup | null;
  actionResult: ActionResult;
  setNumber: number;
  homeScore: number;
  guestScore: number;
  seqInMatch: number;
}

const { Title, Text } = Typography;

interface LiveMatchPanelProps {
  open: boolean;
  match: Match;
  initialState?: CoinTossResult;
  onClose: () => void;
}

const SET_WIN_SCORE = 25;
const TIE_BREAK_SCORE = 15;

function isSetOver(homeScore: number, guestScore: number, setNumber: number): boolean {
  const target = setNumber >= 5 ? TIE_BREAK_SCORE : SET_WIN_SCORE;
  return (homeScore >= target || guestScore >= target) && Math.abs(homeScore - guestScore) >= 2;
}

// главный live-экран протокола матча
const LiveMatchPanel: React.FC<LiveMatchPanelProps> = ({
  open,
  match,
  initialState,
  onClose,
}) => {
  const { message, modal } = App.useApp();

  // live-стейт
  const [liveState, setLiveState] = useState<LiveMatchState>({
    currentSetNumber: 1,
    sets: [{ setNumber: 1, homeScore: 0, guestScore: 0, servingTeamId: initialState?.servingTeamId ?? match.homeTeamId, isFinished: false }],
    homeMatchScore: 0,
    guestMatchScore: 0,
  });
  const [currentSetLineups, setCurrentSetLineups] = useState<StartingLineup[]>([]);
  const [allHomePlayersFull, setAllHomePlayersFull] = useState<Player[]>([]);
  const [allGuestPlayersFull, setAllGuestPlayersFull] = useState<Player[]>([]);
  const [recentEvents, setRecentEvents] = useState<MatchEvent[]>([]);
  const [eventTypes, setEventTypes] = useState<LookupDto[]>([]);
  const [substitutionTypes, setSubstitutionTypes] = useState<LookupDto[]>([]);
  const [sanctionTypes, setSanctionTypes] = useState<LookupDto[]>([]);
  const [sanctionKinds, setSanctionKinds] = useState<LookupDto[]>([]);
  const [recipientTypes, setRecipientTypes] = useState<LookupDto[]>([]);
  const [allPlayersLookup, setAllPlayersLookup] = useState<LookupItemDto[]>([]);
  const [homePlayersLookup, setHomePlayersLookup] = useState<LookupItemDto[]>([]);
  const [guestPlayersLookup, setGuestPlayersLookup] = useState<LookupItemDto[]>([]);

  // UI стейт
  const [selectedPlayer, setSelectedPlayer] = useState<StartingLineup | null>(null);
  const [subModalOpen, setSubModalOpen] = useState(false);
  const [sanctionModalOpen, setSanctionModalOpen] = useState(false);
  const [actionPickerOpen, setActionPickerOpen] = useState(false);
  const [saving, setSaving] = useState(false);

  // ротация
  const [rotationTeam, setRotationTeam] = useState<number | null>(null);

  // дефолтный игрок для модала замены (при открытии через правый клик)
  const [subDefaultPlayerId, setSubDefaultPlayerId] = useState<number | undefined>();
  const [subDefaultTeamId, setSubDefaultTeamId] = useState<number | undefined>();

  // история замен: setNumber → [{out, in}] — для контроля повторных выходов
  const [subPairsPerSet, setSubPairsPerSet] = useState<Map<number, { out: number; in: number }[]>>(new Map());

  // подсказки горячих клавиш
  const [shortcutsOpen, setShortcutsOpen] = useState(false);

  // контекстное меню 
  const [contextMenu, setContextMenu] = useState<{ x: number; y: number; player: StartingLineup } | null>(null);

  // офлайн-буфер
  const [isOnline, setIsOnline] = useState(navigator.onLine);
  const [pendingQueue, setPendingQueue] = useState<PendingEvent[]>([]);

  const timeoutTypesRef = useRef<LookupDto[]>([]);
  const nextSetServingRef = useRef<number | undefined>(undefined);

  // офлайн/онлайн
  useEffect(() => {
    const goOnline = () => setIsOnline(true);
    const goOffline = () => setIsOnline(false);
    window.addEventListener('online', goOnline);
    window.addEventListener('offline', goOffline);
    return () => {
      window.removeEventListener('online', goOnline);
      window.removeEventListener('offline', goOffline);
    };
  }, []);

  // сброс офлайн-очереди при восстановлении связи
  const flushQueueRef = useRef<(() => void) | null>(null);
  useEffect(() => {
    flushQueueRef.current = async () => {
      if (pendingQueue.length === 0) return;
      const queue = [...pendingQueue];
      setPendingQueue([]);
      for (const ev of queue) {
        let dbName: string | null = null;
        if (ev.actionResult.scoringEffect === 'acting') {
          dbName = ev.actingTeamId === match.homeTeamId ? 'Очко хозяев' : 'Очко гостей';
        } else if (ev.actionResult.scoringEffect === 'opposing') {
          dbName = ev.actingTeamId === match.homeTeamId ? 'Очко гостей' : 'Очко хозяев';
        }
        const evType = dbName ? eventTypes.find(et => et.name === dbName) : null;
        if (!evType) continue; // нейтральные события не пишем
        try {
          await eventsApi.create(match.id, {
            setNumber: ev.setNumber,
            eventTypeCode: evType.code,
            teamId: ev.actingTeamId,
            homeScoreAtMoment: ev.homeScore,
            guestScoreAtMoment: ev.guestScore,
            globalSeqInSet: ev.seqInMatch,
          });
        } catch { /* некритично */ }
      }
      message.success(`Синхронизировано ${queue.length} событий`);
    };
  });

  useEffect(() => {
    if (isOnline && pendingQueue.length > 0) {
      flushQueueRef.current?.();
    }
  }, [isOnline]);

  // горячие клавиши
  useEffect(() => {
    if (!open) return;
    const handleKey = (e: KeyboardEvent) => {
      if (e.target instanceof HTMLInputElement || e.target instanceof HTMLTextAreaElement) return;
      if (e.key === 'Escape') {
        setShortcutsOpen(false);
        setContextMenu(null);
        return;
      }
      if (e.key === '?') { setShortcutsOpen(s => !s); return; }
      if ((e.ctrlKey || e.metaKey) && e.key === 'z') { undoLastEvent(); return; }
      if (e.key === 'T' && e.shiftKey) { recordTimeout(match.guestTeamId); return; }
      if (e.key === 't') { recordTimeout(match.homeTeamId); return; }
      if (e.key === 's') { setSubDefaultPlayerId(undefined); setSubDefaultTeamId(undefined); setSubModalOpen(true); return; }
      if (e.key === 'r') { setSanctionModalOpen(true); return; }
      if ((e.key === ' ' || e.key === 'Enter') && selectedPlayer) {
        setActionPickerOpen(true);
      }
    };
    window.addEventListener('keydown', handleKey);
    return () => window.removeEventListener('keydown', handleKey);
  }, [open, selectedPlayer]);

  const currentSet = liveState.sets[liveState.currentSetNumber - 1] ?? liveState.sets[0];
  const servingTeamId = currentSet?.servingTeamId ?? match.homeTeamId;

  // инициализация
  const loadLineups = useCallback(async (setNumber: number) => {
    try {
      const lineups = await lineupsApi.getAll(match.id, { setNumber });
      setCurrentSetLineups(lineups);
    } catch { /* некритично */ }
  }, [match.id]);

  useEffect(() => {
    if (!open) return;

    const init = async () => {
      try {
        const [eventsData, homeData, guestData, evTypesData, subTypesData, sanTypesData, sanKindsData, recTypesData, homeLookup, guestLookup] = await Promise.all([
          eventsApi.getAll(match.id),
          playersApi.getAll(match.homeTeamId),
          playersApi.getAll(match.guestTeamId),
          lookupsApi.getEventTypes(),
          lookupsApi.getSubstitutionTypes(),
          lookupsApi.getSanctionTypes(),
          lookupsApi.getSanctionKinds(),
          lookupsApi.getRecipientTypes(),
          lookupsApi.getPlayers(match.homeTeamId),
          lookupsApi.getPlayers(match.guestTeamId),
        ]);

        setAllHomePlayersFull(homeData);
        setAllGuestPlayersFull(guestData);
        setEventTypes(evTypesData);
        setSubstitutionTypes(subTypesData);
        setSanctionTypes(sanTypesData);
        setSanctionKinds(sanKindsData);
        setRecipientTypes(recTypesData);
        setHomePlayersLookup(homeLookup);
        setGuestPlayersLookup(guestLookup);
        setAllPlayersLookup([...homeLookup, ...guestLookup]);

        // вычислить текущий счёт из событий
        if (eventsData.length > 0) {
          const lastEvent = eventsData[eventsData.length - 1];
          const maxSet = Math.max(...eventsData.map(e => e.setNumber));

          const setsData = await setsApi.getAll(match.id).catch(() => []);
          const homeWins = setsData.filter(s => s.homeScore > s.guestScore).length;
          const guestWins = setsData.filter(s => s.guestScore > s.homeScore).length;

          const setEvents = eventsData.filter(e => e.setNumber === maxSet);
          const lastSetEvent = setEvents[setEvents.length - 1];
          const homeScore = lastSetEvent?.homeScoreAtMoment ?? 0;
          const guestScore = lastSetEvent?.guestScoreAtMoment ?? 0;

          setLiveState({
            currentSetNumber: maxSet,
            sets: setsData.length > 0
              ? setsData.map(s => ({
                  setNumber: s.setNumber,
                  homeScore: s.homeScore,
                  guestScore: s.guestScore,
                  servingTeamId: initialState?.servingTeamId ?? match.homeTeamId,
                  isFinished: true,
                })).concat(isSetOver(homeScore, guestScore, maxSet) ? [] : [{
                  setNumber: maxSet,
                  homeScore,
                  guestScore,
                  servingTeamId: lastEvent.teamId ?? (initialState?.servingTeamId ?? match.homeTeamId),
                  isFinished: false,
                }])
              : [{ setNumber: maxSet, homeScore, guestScore, servingTeamId: lastEvent.teamId ?? (initialState?.servingTeamId ?? match.homeTeamId), isFinished: false }],
            homeMatchScore: homeWins,
            guestMatchScore: guestWins,
          });

          setRecentEvents(eventsData.slice(-20).reverse());
          await loadLineups(maxSet);
        } else {
          await loadLineups(1);
        }
      } catch {
        message.error('Ошибка инициализации live-режима');
      }
    };

    init();
  }, [open, match.id, match.homeTeamId, match.guestTeamId]);

  // запись события
  const recordEvent = async (actingTeamId: number, player: StartingLineup | null, actionResult: ActionResult) => {
    log('recordEvent →', { team: actingTeamId, player: player?.playerId, pos: player?.positionNo, action: actionResult.code });
    setSaving(true);
    try {
      const cs = liveState.sets[liveState.currentSetNumber - 1] ?? { homeScore: 0, guestScore: 0 };
      let newHomeScore = cs.homeScore;
      let newGuestScore = cs.guestScore;

      if (actionResult.scoringEffect === 'acting') {
        if (actingTeamId === match.homeTeamId) newHomeScore++;
        else newGuestScore++;
      } else if (actionResult.scoringEffect === 'opposing') {
        if (actingTeamId === match.homeTeamId) newGuestScore++;
        else newHomeScore++;
      }

      // определяем тип события по DB-именам (только 'Очко хозяев' / 'Очко гостей' имеют смысл)
      let dbEventTypeName: string | null = null;
      if (actionResult.scoringEffect === 'acting') {
        dbEventTypeName = actingTeamId === match.homeTeamId ? 'Очко хозяев' : 'Очко гостей';
      } else if (actionResult.scoringEffect === 'opposing') {
        dbEventTypeName = actingTeamId === match.homeTeamId ? 'Очко гостей' : 'Очко хозяев';
      }
      const evType = dbEventTypeName
        ? eventTypes.find(et => et.name === dbEventTypeName)
        : null;
      // нейтральные события (нет очков) не пишутся в БД
      const eventTypeCode = evType?.code ?? null;

      // порядковый номер = max по текущей партии + 1
      const maxSeqInSet = Math.max(0, ...recentEvents
        .filter(e => e.setNumber === liveState.currentSetNumber)
        .map(e => e.globalSeqInSet ?? 0));
      const nextGlobalSeq = maxSeqInSet + 1;

      const newServingTeamId = actionResult.scoringEffect === 'neutral'
        ? servingTeamId
        : (actionResult.scoringEffect === 'acting' ? actingTeamId : (actingTeamId === match.homeTeamId ? match.guestTeamId : match.homeTeamId));

      // офлайн: добавить в очередь
      if (!isOnline) {
        const pending: PendingEvent = {
          id: `${Date.now()}-${Math.random()}`,
          actingTeamId,
          player,
          actionResult,
          setNumber: liveState.currentSetNumber,
          homeScore: newHomeScore,
          guestScore: newGuestScore,
          seqInMatch: recentEvents.length + 1,
        };
        setPendingQueue(q => [...q, pending]);
        message.warning('Офлайн: событие добавлено в очередь');
      } else if (eventTypeCode != null) {
        log('POST event:', { eventTypeCode, team: actingTeamId, score: `${newHomeScore}:${newGuestScore}` });
        const newEvent = await eventsApi.create(match.id, {
          setNumber: liveState.currentSetNumber,
          eventTypeCode,
          teamId: actingTeamId,
          homeScoreAtMoment: newHomeScore,
          guestScoreAtMoment: newGuestScore,
          globalSeqInSet: nextGlobalSeq,
        });
        log('Event created:', newEvent);
        setRecentEvents(prev => [newEvent, ...prev].slice(0, 50));
      }

      setLiveState(prev => {
        const sets = [...prev.sets];
        const idx = sets.findIndex(s => s.setNumber === prev.currentSetNumber);
        if (idx >= 0) {
          sets[idx] = { ...sets[idx], homeScore: newHomeScore, guestScore: newGuestScore, servingTeamId: newServingTeamId };
        }
        return { ...prev, sets };
      });

      // показать ротацию при смене подачи
      if (actionResult.scoringEffect !== 'neutral' && newServingTeamId !== servingTeamId) {
        setRotationTeam(newServingTeamId);
      }

      // проверка окончания партии
      if (isSetOver(newHomeScore, newGuestScore, liveState.currentSetNumber)) {
        modal.confirm({
          title: `Партия ${liveState.currentSetNumber} завершена!`,
          content: `Счёт: ${newHomeScore}:${newGuestScore}. Завершить партию и перейти к следующей?`,
          okText: 'Завершить партию',
          cancelText: 'Продолжить',
          onOk: () => finishCurrentSet(newHomeScore, newGuestScore),
        });
      }
    } catch (err: unknown) {
      const axErr = err as { response?: { data?: { message?: string }; status?: number } };
      const detail = axErr.response?.data?.message ?? (err instanceof Error ? err.message : '');
      const status = axErr.response?.status;
      if (status === 401 || status === 403) {
        message.error('Нет прав для записи события (требуется роль СекретарьМатча)');
      } else {
        message.error(detail ? `Ошибка: ${detail}` : 'Ошибка записи события');
      }
    } finally {
      setSaving(false);
    }
  };

  // быстрое очко без выбора игрока
  const quickScore = (teamId: number) => {
    const result: ActionResult = { code: 'point_us', label: 'Очко', icon: '⊕', scoringEffect: 'acting', eventTypeName: 'Техническое очко' };
    recordEvent(teamId, null, result);
  };

  // завершение партии
  const finishCurrentSet = async (homeScore: number, guestScore: number) => {
    const setNumber = liveState.currentSetNumber;
    try {
      await setsApi.upsert(match.id, { matchId: match.id, setNumber, homeScore, guestScore });

      const homeWon = homeScore > guestScore;
      const newHomeMatchScore = liveState.homeMatchScore + (homeWon ? 1 : 0);
      const newGuestMatchScore = liveState.guestMatchScore + (homeWon ? 0 : 1);

      if (newHomeMatchScore >= 3 || newGuestMatchScore >= 3) {
        try {
          // statusCode 3 = Завершён (MatchStatusCodes.Completed)
          await matchesApi.update(match.id, { ...match, statusCode: 3 });
        } catch { /* не блокирует завершение */ }
        message.success('Матч завершён! Статус обновлён.');
        setLiveState(prev => ({
          ...prev,
          homeMatchScore: newHomeMatchScore,
          guestMatchScore: newGuestMatchScore,
        }));
        return;
      }

      const nextSetNumber = setNumber + 1;
      // по умолчанию подаёт команда, выбравшая подачу на жеребьёвке
      const defaultNextServing = initialState?.servingTeamId ?? match.homeTeamId;
      nextSetServingRef.current = defaultNextServing;

      await new Promise<void>(resolve => {
        modal.confirm({
          title: `Кто подаёт первым в партии ${nextSetNumber}?`,
          content: (
            <div style={{ marginTop: 12 }}>
              <div style={{ display: 'flex', gap: 8 }}>
                {[
                  { id: match.homeTeamId, name: match.homeTeamName ?? 'Хозяева' },
                  { id: match.guestTeamId, name: match.guestTeamName ?? 'Гости' },
                ].map(t => (
                  <button
                    key={t.id}
                    type="button"
                    id={`next-serve-${t.id}`}
                    onClick={() => {
                      nextSetServingRef.current = t.id;
                      document.querySelectorAll('[id^="next-serve-"]').forEach(el => {
                        (el as HTMLButtonElement).style.background = '#fff';
                        (el as HTMLButtonElement).style.color = '#595959';
                        (el as HTMLButtonElement).style.fontWeight = '400';
                      });
                      const btn = document.getElementById(`next-serve-${t.id}`) as HTMLButtonElement;
                      btn.style.background = '#e6f4ff';
                      btn.style.color = '#1677ff';
                      btn.style.fontWeight = '700';
                    }}
                    style={{
                      flex: 1,
                      border: '1px solid #d9d9d9',
                      borderRadius: 6,
                      padding: '8px 12px',
                      cursor: 'pointer',
                      fontSize: 13,
                      background: t.id === defaultNextServing ? '#e6f4ff' : '#fff',
                      color: t.id === defaultNextServing ? '#1677ff' : '#595959',
                      fontWeight: t.id === defaultNextServing ? 700 : 400,
                    }}
                  >
                    {t.name}
                  </button>
                ))}
              </div>
              <div style={{ marginTop: 8, fontSize: 11, color: '#8c8c8c' }}>
                По умолчанию: команда с правом первой подачи по жеребьёвке
              </div>
            </div>
          ),
          okText: 'Начать партию',
          cancelText: 'Отмена',
          onOk: resolve,
          onCancel: resolve,
        });
      });

      const nextServingTeamId = nextSetServingRef.current ?? defaultNextServing;

      setLiveState(prev => {
        const sets = [...prev.sets.map(s =>
          s.setNumber === setNumber ? { ...s, isFinished: true } : s
        )];
        sets.push({
          setNumber: nextSetNumber,
          homeScore: 0,
          guestScore: 0,
          servingTeamId: nextServingTeamId,
          isFinished: false,
        });
        return {
          ...prev,
          currentSetNumber: nextSetNumber,
          sets,
          homeMatchScore: newHomeMatchScore,
          guestMatchScore: newGuestMatchScore,
        };
      });

      await loadLineups(nextSetNumber);
      message.success(`Начинается партия ${nextSetNumber}`);
    } catch (err) {
      message.error(getApiError(err, 'Ошибка сохранения партии'));
    }
  };

  // замена
  const recordSubstitution = async (data: {
    teamId: number;
    subOutPlayerId: number;
    subInPlayerId: number;
    subTypeCode: number;
    isLiberoSwap: boolean;
  }) => {
    log('recordSubstitution →', data);
    setSaving(true);
    try {
      const cs = liveState.sets[liveState.currentSetNumber - 1] ?? { homeScore: 0, guestScore: 0 };
      const evType = eventTypes.find(et => et.name.toLowerCase().includes('замена'));
      log('eventType for sub:', evType, '| fallback code: 10');
      const evTypeCode = evType?.code ?? eventTypes[0]?.code ?? 10;
      const payload = {
        setNumber: liveState.currentSetNumber,
        eventTypeCode: evTypeCode,
        teamId: data.teamId,
        homeScoreAtMoment: cs.homeScore,
        guestScoreAtMoment: cs.guestScore,
        globalSeqInSet: recentEvents.length + 1,
        substitution: {
          subOutPlayerId: data.subOutPlayerId,
          subInPlayerId: data.subInPlayerId,
          subTypeCode: data.subTypeCode ?? null,
          isLiberoSwap: data.isLiberoSwap,
        },
      };
      log('POST /events payload:', payload);
      const created = await eventsApi.create(match.id, payload);
      log('Substitution event created:', created);

      // обновить расстановку локально
      const allTeamPlayers = data.teamId === match.homeTeamId ? allHomePlayersFull : allGuestPlayersFull;
      const incomingPlayer = allTeamPlayers.find(p => p.id === data.subInPlayerId);
      setCurrentSetLineups(prev =>
        prev.map(l =>
          l.teamId === data.teamId && l.playerId === data.subOutPlayerId
            ? {
                ...l,
                playerId: data.subInPlayerId,
                playerFullName: incomingPlayer?.fullName ?? incomingPlayer?.lastName ?? `Игрок ${data.subInPlayerId}`,
                shirtNumber: incomingPlayer?.jerseyNumber,
              }
            : l
        )
      );

      // запомнить пару замены для контроля повторного выхода в этой партии
      const setNum = liveState.currentSetNumber;
      setSubPairsPerSet(prev => {
        const next = new Map(prev);
        const pairs: SubPair[] = [...(next.get(setNum) ?? []), { out: data.subOutPlayerId, in: data.subInPlayerId }];
        next.set(setNum, pairs);
        return next;
      });

      // добавить в ленту событий
      if (created) setRecentEvents(prev => [created as unknown as MatchEvent, ...prev].slice(0, 50));

      message.success('Замена выполнена');
      setSubModalOpen(false);
      setSubDefaultPlayerId(undefined);
      setSubDefaultTeamId(undefined);
    } catch (err: unknown) {
      const axErr = err as { response?: { data?: { message?: string; errors?: Record<string, string[]> }; status?: number } };
      const status = axErr.response?.status;
      const detail = axErr.response?.data?.message ?? (err instanceof Error ? err.message : '');
      const validationErrors = axErr.response?.data?.errors
        ? Object.values(axErr.response.data.errors).flat().join('; ')
        : '';
      if (status === 401 || status === 403) {
        message.error('Нет прав для записи замены (требуется роль СекретарьМатча)');
      } else if (status === 400 && validationErrors) {
        message.error(`Ошибка данных замены: ${validationErrors}`);
      } else {
        message.error(detail ? `Ошибка замены: ${detail}` : 'Ошибка записи замены');
      }
    } finally {
      setSaving(false);
    }
  };

  // тайм-аут
  const recordTimeout = async (teamId: number) => {
    if (timeoutTypesRef.current.length === 0) {
      try {
        timeoutTypesRef.current = await lookupsApi.getTimeoutTypes();
      } catch { /* некритично */ }
    }
    const cs = liveState.sets[liveState.currentSetNumber - 1] ?? { homeScore: 0, guestScore: 0 };
    const evType = eventTypes.find(et => et.name.toLowerCase().includes('тайм'));
    try {
      await eventsApi.create(match.id, {
        setNumber: liveState.currentSetNumber,
        eventTypeCode: evType?.code ?? 11,
        teamId,
        homeScoreAtMoment: cs.homeScore,
        guestScoreAtMoment: cs.guestScore,
        globalSeqInSet: recentEvents.length + 1,
        timeout: { timeoutTypeCode: timeoutTypesRef.current[0]?.code ?? 1 },
      });
      message.success('Тайм-аут зафиксирован');
    } catch (err) {
      message.error(getApiError(err, 'Ошибка записи тайм-аута'));
    }
  };

  // отмена последнего события
  const undoLastEvent = async () => {
    if (recentEvents.length === 0) return;
    const lastEvent = recentEvents[0];
    try {
      await eventsApi.delete(match.id, lastEvent.id);
      setRecentEvents(prev => prev.slice(1));
      // пересчитать счёт из оставшихся событий
      const newLast = recentEvents[1];
      if (newLast) {
        setLiveState(prev => {
          const sets = [...prev.sets];
          const idx = sets.findIndex(s => s.setNumber === prev.currentSetNumber);
          if (idx >= 0) {
            sets[idx] = { ...sets[idx], homeScore: newLast.homeScoreAtMoment, guestScore: newLast.guestScoreAtMoment };
          }
          return { ...prev, sets };
        });
      } else {
        setLiveState(prev => {
          const sets = [...prev.sets];
          const idx = sets.findIndex(s => s.setNumber === prev.currentSetNumber);
          if (idx >= 0) {
            sets[idx] = { ...sets[idx], homeScore: 0, guestScore: 0 };
          }
          return { ...prev, sets };
        });
      }
      message.success('Событие отменено');
    } catch {
      message.error('Ошибка отмены');
    }
  };

  // обработчик клика по игроку
  const handlePlayerClick = (player: StartingLineup) => {
    setSelectedPlayer(player);
    setActionPickerOpen(true);
  };

  const handleActionConfirm = (category: string, result: ActionResult) => {
    if (!selectedPlayer) return;
    log('handleActionConfirm:', category, result.code, '| player pos:', selectedPlayer.positionNo, '| serving:', servingTeamId);
    setActionPickerOpen(false);
    const teamId = selectedPlayer.teamId;
    recordEvent(teamId, selectedPlayer, result);
    setSelectedPlayer(null);
  };

  // контекстное меню по игроку
  const handlePlayerContextMenu = (player: StartingLineup, pos: { x: number; y: number }) => {
    setContextMenu({ x: pos.x, y: pos.y, player });
  };

  const homeLineup = currentSetLineups.filter(l => l.teamId === match.homeTeamId);
  const guestLineup = currentSetLineups.filter(l => l.teamId === match.guestTeamId);

  return (
    <>
      <Modal
        open={open}
        onCancel={onClose}
        footer={null}
        width="100%"
        style={{ top: 0, padding: 0, maxWidth: '100vw' }}
        styles={{ body: { padding: 0, height: '100vh', overflow: 'hidden', display: 'flex', flexDirection: 'column', position: 'relative' } }}
        destroyOnHidden
      >
        {/* шапка */}
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          padding: '8px 16px',
          background: '#1a1a2e',
          color: '#fff',
          flexShrink: 0,
        }}>
          <Space>
            <Button type="text" onClick={onClose} style={{ color: '#fff' }}>← Свернуть</Button>
            {!isOnline && (
              <Tag color="red" icon={<WifiOutlined />}>Офлайн{pendingQueue.length > 0 ? ` (${pendingQueue.length})` : ''}</Tag>
            )}
          </Space>
          <Text style={{ color: '#fff', fontWeight: 600, fontSize: 15 }}>
            {match.homeTeamName ?? 'Хозяева'} vs {match.guestTeamName ?? 'Гости'}
          </Text>
          <Space>
            <Tag color="orange" style={{ fontSize: 14 }}>Партия {liveState.currentSetNumber}</Tag>
            <Button
              type="text"
              icon={<QuestionOutlined />}
              onClick={() => setShortcutsOpen(s => !s)}
              style={{ color: '#ffffffaa', fontSize: 12 }}
              size="small"
            />
          </Space>
        </div>

        {/* основной контент */}
        <div style={{ flex: 1, overflow: 'hidden', padding: '8px 12px' }}>
          <Row gutter={12} style={{ height: '100%' }}>
            {/* левая колонка — площадка */}
            <Col xs={24} md={14} style={{ display: 'flex', flexDirection: 'column' }}>
              {currentSetLineups.length === 0 && (
                <div style={{
                  background: '#fffbe6',
                  border: '1px solid #ffe58f',
                  borderRadius: 8,
                  padding: '6px 12px',
                  marginBottom: 8,
                  fontSize: 12,
                  color: '#ad6800',
                }}>
                  ⚠ Расстановка не задана — нажмите кнопки «+1» для записи очков, или сначала заполните расстановку во вкладке «Расстановка»
                </div>
              )}
              <LiveCourtView
                homeTeamId={match.homeTeamId}
                homeTeamName={match.homeTeamName ?? 'Хозяева'}
                guestTeamId={match.guestTeamId}
                guestTeamName={match.guestTeamName ?? 'Гости'}
                lineups={currentSetLineups}
                servingTeamId={servingTeamId}
                onPlayerClick={handlePlayerClick}
                onPlayerContextMenu={handlePlayerContextMenu}
                disabled={saving}
              />
            </Col>

            {/* правая колонка — счёт и события */}
            <Col xs={24} md={10} style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
              {/* счёт */}
              <Card size="small" styles={{ body: { padding: '12px 16px' } }}>
                <div style={{ textAlign: 'center' }}>
                  <Text style={{ fontSize: 13, color: '#888' }}>
                    {match.homeTeamName ?? 'Хозяева'} 🏐 {match.guestTeamName ?? 'Гости'}
                  </Text>
                  <div style={{ fontSize: 48, fontWeight: 700, lineHeight: 1.1, color: '#1a1a2e' }}>
                    {currentSet?.homeScore ?? 0}
                    <span style={{ color: '#ccc', fontSize: 32, margin: '0 8px' }}>:</span>
                    {currentSet?.guestScore ?? 0}
                  </div>
                  <Text type="secondary" style={{ fontSize: 13 }}>
                    Матч: {liveState.homeMatchScore} : {liveState.guestMatchScore}
                  </Text>
                </div>
              </Card>

              {/* последние события */}
              <Card
                size="small"
                title="Последние события"
                styles={{ body: { padding: '4px 8px', overflowY: 'auto', maxHeight: 'calc(100vh - 340px)' } }}
              >
                {recentEvents.length === 0 ? (
                  <Text type="secondary" style={{ fontSize: 12 }}>Событий нет</Text>
                ) : (
                  <List
                    size="small"
                    dataSource={recentEvents.slice(0, 20)}
                    renderItem={(ev) => (
                      <List.Item style={{ padding: '3px 0', borderBottom: '1px solid #f0f0f0' }}>
                        <Text style={{ fontSize: 12 }}>
                          {ev.playerFullName ? `${ev.playerFullName}` : (ev.teamName ?? '—')}
                          {' — '}
                          {ev.eventTypeName ?? `тип ${ev.eventTypeCode}`}
                          {'  '}
                          <Text type="secondary" style={{ fontSize: 11 }}>
                            {ev.homeScoreAtMoment}:{ev.guestScoreAtMoment}
                          </Text>
                        </Text>
                      </List.Item>
                    )}
                  />
                )}
              </Card>
            </Col>
          </Row>
        </div>

        {/* нижняя панель кнопок */}
        <div style={{
          padding: '8px 12px',
          background: '#f5f5f5',
          borderTop: '1px solid #e0e0e0',
          flexShrink: 0,
          display: 'flex',
          flexWrap: 'wrap',
          gap: 8,
          justifyContent: 'space-between',
        }}>
          <Space wrap>
            {/* быстрое очко — не требует расстановки */}
            <Button
              size="small"
              type="primary"
              style={{ background: '#1677ff', borderColor: '#1677ff', fontWeight: 700 }}
              onClick={() => quickScore(match.homeTeamId)}
              disabled={saving}
            >
              +1 {match.homeTeamName?.split(' ')[0] ?? 'Хозяева'}
            </Button>
            <Button
              size="small"
              type="primary"
              style={{ background: '#fa8c16', borderColor: '#fa8c16', fontWeight: 700 }}
              onClick={() => quickScore(match.guestTeamId)}
              disabled={saving}
            >
              +1 {match.guestTeamName?.split(' ')[0] ?? 'Гости'}
            </Button>
            <Button
              size="small"
              icon={<span>⏰</span>}
              onClick={() => recordTimeout(match.homeTeamId)}
              disabled={saving}
            >
              Тайм-аут {match.homeTeamName?.split(' ')[0] ?? 'А'}
            </Button>
            <Button
              size="small"
              icon={<span>⏰</span>}
              onClick={() => recordTimeout(match.guestTeamId)}
              disabled={saving}
            >
              Тайм-аут {match.guestTeamName?.split(' ')[0] ?? 'Б'}
            </Button>
            <Button
              size="small"
              icon={<span>🔄</span>}
              onClick={() => { setSubDefaultPlayerId(undefined); setSubDefaultTeamId(undefined); setSubModalOpen(true); }}
              disabled={saving}
            >
              Замена
            </Button>
            <Button
              size="small"
              icon={<span>🚨</span>}
              danger
              onClick={() => setSanctionModalOpen(true)}
              disabled={saving}
            >
              Санкция
            </Button>
            {/* индикатор ротации */}
            {rotationTeam !== null && (
              <RotationIndicator
                teamName={rotationTeam === match.homeTeamId ? (match.homeTeamName ?? 'Хозяева') : (match.guestTeamName ?? 'Гости')}
                animate={true}
                onAnimationEnd={() => setRotationTeam(null)}
              />
            )}
          </Space>
          <Space>
            <Popconfirm
              title="Отменить последнее событие?"
              onConfirm={undoLastEvent}
              okText="Отменить"
              cancelText="Нет"
              disabled={recentEvents.length === 0}
            >
              <Button size="small" icon={<span>↩</span>} disabled={recentEvents.length === 0}>
                Отменить ход
              </Button>
            </Popconfirm>
            <Popconfirm
              title={`Завершить партию ${liveState.currentSetNumber} вручную?`}
              onConfirm={() => finishCurrentSet(currentSet?.homeScore ?? 0, currentSet?.guestScore ?? 0)}
              okText="Завершить"
              cancelText="Нет"
            >
              <Button size="small" type="default">
                Конец партии вручную
              </Button>
            </Popconfirm>
          </Space>
        </div>

        {/* контекстное меню игрока */}
        {contextMenu && (
          <div
            onClick={() => setContextMenu(null)}
            style={{ position: 'absolute', inset: 0, zIndex: 9999 }}
          >
            <div
              onClick={e => e.stopPropagation()}
              style={{
                position: 'absolute',
                left: contextMenu.x,
                top: contextMenu.y,
                background: '#fff',
                borderRadius: 8,
                boxShadow: '0 4px 16px rgba(0,0,0,0.2)',
                padding: '4px 0',
                minWidth: 160,
                zIndex: 10000,
              }}
            >
              <div style={{ padding: '4px 12px 8px', borderBottom: '1px solid #f0f0f0' }}>
                <Text strong style={{ fontSize: 13 }}>
                  {contextMenu.player.playerFullName ?? `Игрок ${contextMenu.player.playerId}`}
                </Text>
                <Text type="secondary" style={{ fontSize: 11, display: 'block' }}>
                  Позиция {contextMenu.player.positionNo}
                </Text>
              </div>
              {[
                { label: 'Записать действие', action: () => { setSelectedPlayer(contextMenu.player); setActionPickerOpen(true); setContextMenu(null); } },
                { label: 'Замена игрока', action: () => { setSubDefaultPlayerId(contextMenu.player.playerId); setSubDefaultTeamId(contextMenu.player.teamId); setSubModalOpen(true); setContextMenu(null); } },
                { label: 'Санкция', action: () => { setSanctionModalOpen(true); setContextMenu(null); } },
              ].map(item => (
                <div
                  key={item.label}
                  onClick={item.action}
                  style={{
                    padding: '6px 12px',
                    cursor: 'pointer',
                    fontSize: 13,
                    transition: 'background 0.1s',
                  }}
                  onMouseEnter={e => (e.currentTarget.style.background = '#f5f5f5')}
                  onMouseLeave={e => (e.currentTarget.style.background = '')}
                >
                  {item.label}
                </div>
              ))}
            </div>
          </div>
        )}

        {/* подсказки горячих клавиш */}
        {shortcutsOpen && (
          <div
            style={{
              position: 'absolute',
              bottom: 80,
              right: 16,
              background: 'rgba(26,26,46,0.95)',
              color: '#fff',
              borderRadius: 10,
              padding: '12px 16px',
              zIndex: 10001,
              minWidth: 220,
              fontSize: 12,
              boxShadow: '0 4px 20px rgba(0,0,0,0.4)',
            }}
          >
            <div style={{ fontWeight: 700, marginBottom: 8, fontSize: 13 }}>⌨ Горячие клавиши</div>
            {[
              ['Пробел / Enter', 'Действие для выбранного игрока'],
              ['t', 'Тайм-аут хозяев'],
              ['T (Shift+t)', 'Тайм-аут гостей'],
              ['s', 'Открыть замену'],
              ['r', 'Открыть санкцию'],
              ['Ctrl+Z', 'Отменить ход'],
              ['?', 'Показать/скрыть подсказки'],
              ['Esc', 'Закрыть меню'],
            ].map(([key, desc]) => (
              <div key={key} style={{ display: 'flex', gap: 8, marginBottom: 4 }}>
                <code style={{ background: 'rgba(255,255,255,0.12)', borderRadius: 4, padding: '1px 5px', flexShrink: 0, fontFamily: 'monospace' }}>
                  {key}
                </code>
                <span style={{ color: '#ffffffaa' }}>{desc}</span>
              </div>
            ))}
          </div>
        )}
      </Modal>

      {/* модал действия */}
      <ActionPickerModal
        open={actionPickerOpen}
        player={selectedPlayer}
        playerFullName={selectedPlayer?.playerFullName}
        teamName={
          selectedPlayer?.teamId === match.homeTeamId
            ? (match.homeTeamName ?? 'Хозяева')
            : (match.guestTeamName ?? 'Гости')
        }
        servingTeamId={servingTeamId}
        onConfirm={handleActionConfirm}
        onCancel={() => { setActionPickerOpen(false); setSelectedPlayer(null); }}
      />

      {/* модал замены */}
      <SubstitutionModal
        open={subModalOpen}
        matchId={match.id}
        setNumber={liveState.currentSetNumber}
        homeTeamId={match.homeTeamId}
        homeTeamName={match.homeTeamName ?? 'Хозяева'}
        guestTeamId={match.guestTeamId}
        guestTeamName={match.guestTeamName ?? 'Гости'}
        homeLineup={homeLineup}
        guestLineup={guestLineup}
        allHomePlayers={allHomePlayersFull}
        allGuestPlayers={allGuestPlayersFull}
        substitutionTypes={substitutionTypes}
        defaultOutPlayerId={subDefaultPlayerId}
        defaultTeamId={subDefaultTeamId}
        subPairsCurrentSet={subPairsPerSet.get(liveState.currentSetNumber)}
        onConfirm={recordSubstitution}
        onCancel={() => { setSubModalOpen(false); setSubDefaultPlayerId(undefined); setSubDefaultTeamId(undefined); }}
      />

      {/* модал санкции */}
      <SanctionQuickModal
        open={sanctionModalOpen}
        matchId={match.id}
        setNumber={liveState.currentSetNumber}
        homeTeamId={match.homeTeamId}
        homeTeamName={match.homeTeamName ?? 'Хозяева'}
        guestTeamId={match.guestTeamId}
        guestTeamName={match.guestTeamName ?? 'Гости'}
        homePlayers={homePlayersLookup}
        guestPlayers={guestPlayersLookup}
        sanctionTypes={sanctionTypes}
        sanctionKinds={sanctionKinds}
        recipientTypes={recipientTypes}
        onConfirm={async (data) => {
          try {
            await sanctionsApi.create(match.id, data);
            message.success('Санкция применена');
            setSanctionModalOpen(false);
          } catch (err) {
            message.error(getApiError(err, 'Ошибка сохранения санкции'));
          }
        }}
        onCancel={() => setSanctionModalOpen(false)}
      />

    </>
  );
};

export default LiveMatchPanel;
