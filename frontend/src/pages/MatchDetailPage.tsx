import React, { useEffect, useState, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Card, Tabs, Table, Typography, Button, Spin, Tag, Descriptions,
  App, Space, Badge, Modal, Form, Select, InputNumber, Popconfirm, Row, Col, Input, Radio,
} from 'antd';
import {
  ArrowLeftOutlined, PlusOutlined, EditOutlined, DeleteOutlined,
  FileAddOutlined, TeamOutlined, QrcodeOutlined, UnorderedListOutlined, FieldTimeOutlined,
} from '@ant-design/icons';
import MatchTimeline from '../components/MatchTimeline';
import PlayerRadarChart from '../components/PlayerRadarChart';
import SubstitutionGraph from '../components/SubstitutionGraph';
import QrModal from '../components/QrModal';
import MatchMiniCourt from '../components/MatchMiniCourt';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import {
  matchesApi, setsApi, eventsApi, sanctionsApi, protocolsApi,
  refereeAssignmentsApi, delegationsApi, captainsApi, playerStatsApi,
} from '../api/index';
import { lookupsApi } from '../api/lookupsApi';
import type {
  Match, SetDto, MatchEvent, Sanction, Protocol,
  RefereeAssignment, Delegation, MatchCaptain, PlayerStats,
} from '../types/index';
import type { LookupDto, LookupItemDto } from '../types/index';
import { useAuth } from '../context/AuthContext';
import { getApiError } from '../utils/apiError';
import VolleyCourt from '../components/VolleyCourt';
import LiveMatchPanel from '../components/LiveMatchPanel';
import PreMatchWizard from '../components/PreMatchWizard';
import type { CoinTossResult } from '../types/index';

const { Title, Text } = Typography;

const STATUS_COLORS: Record<string, string> = {
  Запланирован: 'blue',
  'В процессе': 'orange',
  Завершён: 'green',
  Отменён: 'red',
  Перенесён: 'gold',
  'Техническое поражение': 'volcano',
};

// детальная страница матча
const MatchDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { message } = App.useApp();
  const { can } = useAuth();
  const matchId = Number(id);

  // основные данные матча
  const [match, setMatch] = useState<Match | null>(null);
  const [loadingMatch, setLoadingMatch] = useState(true);

  // партии
  const [sets, setSets] = useState<SetDto[]>([]);
  const [loadingSets, setLoadingSets] = useState(false);
  const [setModalOpen, setSetModalOpen] = useState(false);
  const [editSet, setEditSet] = useState<SetDto | null>(null);
  const [savingSet, setSavingSet] = useState(false);
  const [setForm] = Form.useForm();

  // расстановка
  const [lineupSetNumber, setLineupSetNumber] = useState(1);

  // статистика
  const [stats, setStats] = useState<PlayerStats[]>([]);
  const [loadingStats, setLoadingStats] = useState(false);
  const [statsModalOpen, setStatsModalOpen] = useState(false);
  const [editStat, setEditStat] = useState<PlayerStats | null>(null);
  const [savingStat, setSavingStat] = useState(false);
  const [statsForm] = Form.useForm();
  const [allPlayers, setAllPlayers] = useState<LookupItemDto[]>([]);

  // события
  const [events, setEvents] = useState<MatchEvent[]>([]);
  const [loadingEvents, setLoadingEvents] = useState(false);

  // санкции
  const [sanctions, setSanctions] = useState<Sanction[]>([]);
  const [loadingSanctions, setLoadingSanctions] = useState(false);
  const [sanctionModalOpen, setSanctionModalOpen] = useState(false);
  const [editSanction, setEditSanction] = useState<Sanction | null>(null);
  const [savingSanction, setSavingSanction] = useState(false);
  const [sanctionForm] = Form.useForm();
  const [sanctionTypes, setSanctionTypes] = useState<LookupDto[]>([]);
  const [sanctionKinds, setSanctionKinds] = useState<LookupDto[]>([]);
  const [recipientTypes, setRecipientTypes] = useState<LookupDto[]>([]);

  // судьи
  const [referees, setReferees] = useState<RefereeAssignment[]>([]);
  const [loadingRefs, setLoadingRefs] = useState(false);
  const [refModalOpen, setRefModalOpen] = useState(false);
  const [savingRef, setSavingRef] = useState(false);
  const [refForm] = Form.useForm();
  const [refereesLookup, setRefereesLookup] = useState<LookupItemDto[]>([]);
  const [refRoles, setRefRoles] = useState<LookupDto[]>([]);

  // делегации
  const [delegations, setDelegations] = useState<Delegation[]>([]);
  const [loadingDel, setLoadingDel] = useState(false);
  const [delModalOpen, setDelModalOpen] = useState(false);
  const [editDelegation, setEditDelegation] = useState<Delegation | null>(null);
  const [savingDel, setSavingDel] = useState(false);
  const [delForm] = Form.useForm();

  // капитаны
  const [captains, setCaptains] = useState<MatchCaptain[]>([]);
  const [loadingCaptains, setLoadingCaptains] = useState(false);
  const [savingCaptain, setSavingCaptain] = useState(false);
  const [homeCaptainId, setHomeCaptainId] = useState<number | undefined>();
  const [guestCaptainId, setGuestCaptainId] = useState<number | undefined>();
  const [homePlayers, setHomePlayers] = useState<LookupItemDto[]>([]);
  const [guestPlayers, setGuestPlayers] = useState<LookupItemDto[]>([]);

  // выбранная команда в форме санкции (для фильтрации игроков)
  const [selectedSanctionTeamId, setSelectedSanctionTeamId] = useState<number | undefined>();

  // live-матч
  const [liveMatchOpen, setLiveMatchOpen] = useState(false);
  const [preWizardOpen, setPreWizardOpen] = useState(false);
  const [coinTossResult, setCoinTossResult] = useState<CoinTossResult | null>(null);
  const [statuses, setStatuses] = useState<LookupDto[]>([]);

  // режим событий
  const [eventsView, setEventsView] = useState<'table' | 'timeline'>('table');

  // сравнение радар
  const [radarCompareOpen, setRadarCompareOpen] = useState(false);
  const [radarPlayerAId, setRadarPlayerAId] = useState<number | undefined>();
  const [radarPlayerBId, setRadarPlayerBId] = useState<number | undefined>();
  const [radarStatsA, setRadarStatsA] = useState<PlayerStats | null>(null);
  const [radarStatsB, setRadarStatsB] = useState<PlayerStats | null>(null);

  // QR-код
  const [qrModalOpen, setQrModalOpen] = useState(false);

  // протокол
  const [protocol, setProtocol] = useState<Protocol | null>(null);
  const [loadingProtocol, setLoadingProtocol] = useState(false);
  const [protocolModalOpen, setProtocolModalOpen] = useState(false);
  const [savingProtocol, setSavingProtocol] = useState(false);
  const [protocolForm] = Form.useForm();
  const [protocolStatuses, setProtocolStatuses] = useState<LookupDto[]>([]);

  // загрузка основного матча
  useEffect(() => {
    if (!matchId) return;
    const load = async () => {
      setLoadingMatch(true);
      try {
        const matchData = await matchesApi.getById(matchId);
        setMatch(matchData);
        // сразу загружаем игроков обеих команд
        const [hp, gp, statusList] = await Promise.all([
          lookupsApi.getPlayers(matchData.homeTeamId),
          lookupsApi.getPlayers(matchData.guestTeamId),
          lookupsApi.getMatchStatuses(),
        ]);
        setHomePlayers(hp);
        setGuestPlayers(gp);
        setAllPlayers([...hp, ...gp]);
        setStatuses(statusList);
      } catch {
        message.error('Не удалось загрузить данные матча');
      } finally {
        setLoadingMatch(false);
      }
    };
    load();
  }, [matchId]);

  // загрузчики вкладок
  const loadSets = useCallback(async () => {
    setLoadingSets(true);
    try { setSets(await setsApi.getAll(matchId)); } catch { message.error('Ошибка загрузки партий'); } finally { setLoadingSets(false); }
  }, [matchId]);

  const loadEvents = useCallback(async () => {
    setLoadingEvents(true);
    try { setEvents(await eventsApi.getAll(matchId)); } catch { message.error('Ошибка загрузки событий'); } finally { setLoadingEvents(false); }
  }, [matchId]);

  const loadSanctions = useCallback(async () => {
    setLoadingSanctions(true);
    try {
      const [data, types, kinds, recTypes] = await Promise.all([
        sanctionsApi.getAll(matchId),
        lookupsApi.getSanctionTypes(),
        lookupsApi.getSanctionKinds(),
        lookupsApi.getRecipientTypes(),
      ]);
      setSanctions(data);
      setSanctionTypes(types);
      setSanctionKinds(kinds);
      setRecipientTypes(recTypes);
    } catch { message.error('Ошибка загрузки санкций'); } finally { setLoadingSanctions(false); }
  }, [matchId]);

  const loadProtocol = useCallback(async () => {
    setLoadingProtocol(true);
    try {
      const [data, statuses] = await Promise.all([
        protocolsApi.getByMatch(matchId).catch((e: unknown) => {
          if (import.meta.env.DEV) console.warn('[Protocol] getByMatch error:', e);
          return null;
        }),
        lookupsApi.getProtocolStatuses(),
      ]);
      if (import.meta.env.DEV) console.log('[Protocol] loaded:', data, '| statuses:', statuses);
      setProtocol(data);
      setProtocolStatuses(statuses);
    } catch { setProtocol(null); } finally { setLoadingProtocol(false); }
  }, [matchId]);

  const loadReferees = useCallback(async () => {
    setLoadingRefs(true);
    try {
      const [data, refList, roles] = await Promise.all([
        refereeAssignmentsApi.getAll(matchId),
        lookupsApi.getReferees().catch((): LookupItemDto[] => []),
        lookupsApi.getRefereeRoles().catch((): LookupDto[] => []),
      ]);
      setReferees(data);
      setRefereesLookup(refList);
      setRefRoles(roles);
    } catch { message.error('Ошибка загрузки судей'); } finally { setLoadingRefs(false); }
  }, [matchId]);

  const loadDelegations = useCallback(async () => {
    setLoadingDel(true);
    try { setDelegations(await delegationsApi.getAll(matchId)); } catch { message.error('Ошибка загрузки делегаций'); } finally { setLoadingDel(false); }
  }, [matchId]);

  const loadCaptains = useCallback(async () => {
    setLoadingCaptains(true);
    try {
      const data = await captainsApi.getAll(matchId);
      setCaptains(data);
      setMatch(prev => {
        if (!prev) return prev;
        const home = data.find(c => c.teamId === prev.homeTeamId);
        const guest = data.find(c => c.teamId === prev.guestTeamId);
        setHomeCaptainId(home?.playerId);
        setGuestCaptainId(guest?.playerId);
        return prev;
      });
    } catch { message.error('Ошибка загрузки капитанов'); } finally { setLoadingCaptains(false); }
  }, [matchId]);

  const loadStats = useCallback(async () => {
    setLoadingStats(true);
    try { setStats(await playerStatsApi.getAll(matchId)); } catch { message.error('Ошибка загрузки статистики'); } finally { setLoadingStats(false); }
  }, [matchId]);

  // запуск матча из визарда
  const handleStartMatch = async (toss: CoinTossResult) => {
    try {
      const statusInProgress = statuses.find(s => s.name === 'В процессе')?.code;
      if (statusInProgress && match) {
        await matchesApi.update(matchId, {
          ...match,
          statusCode: statusInProgress,
          firstServeTeamId: toss.servingTeamId,
          coinTossWinnerTeamId: toss.servingTeamId,
          coinTossChoiceCode: 1,
        });
      }
      setMatch(prev => prev ? {
        ...prev,
        statusCode: statuses.find(s => s.name === 'В процессе')?.code ?? prev.statusCode,
        statusName: 'В процессе',
        firstServeTeamId: toss.servingTeamId,
        coinTossWinnerTeamId: toss.servingTeamId,
        coinTossChoiceCode: 1,
      } : prev);
    } catch { message.error('Не удалось обновить статус матча'); }
    setCoinTossResult(toss);
    setPreWizardOpen(false);
    setLiveMatchOpen(true);
  };

  // загружаем партии и протокол сразу
  useEffect(() => {
    if (matchId) {
      loadSets();
      loadProtocol();
    }
  }, [matchId, loadSets, loadProtocol]);

  const handleTabChange = (key: string) => {
    if (key === 'sets' && sets.length === 0) loadSets();
    if (key === 'events' && events.length === 0) loadEvents();
    if (key === 'substitutions' && events.length === 0) loadEvents();
    if (key === 'sanctions' && sanctions.length === 0) loadSanctions();
    if (key === 'protocol') loadProtocol();
    if (key === 'referees' && referees.length === 0) loadReferees();
    if (key === 'delegations' && delegations.length === 0) loadDelegations();
    if (key === 'captains' && captains.length === 0) loadCaptains();
    if (key === 'stats' && stats.length === 0) loadStats();
  };

  // загрузка статистики для сравнения
  const loadRadarStats = async (playerId: number, which: 'A' | 'B') => {
    const found = stats.find(s => s.playerId === playerId);
    if (which === 'A') setRadarStatsA(found ?? null);
    else setRadarStatsB(found ?? null);
  };

  // операции с партиями
  const handleSaveSet = async () => {
    let values: Record<string, unknown>;
    try { values = await setForm.validateFields(); } catch { return; }
    setSavingSet(true);
    try {
      await setsApi.upsert(matchId, { matchId, ...values });
      message.success('Партия сохранена');
      setSetModalOpen(false);
      loadSets();
    } catch (err) { message.error(getApiError(err, 'Ошибка сохранения партии')); } finally { setSavingSet(false); }
  };

  const handleDeleteSet = async (setNumber: number) => {
    try {
      await setsApi.delete(matchId, setNumber);
      message.success('Партия удалена');
      loadSets();
    } catch { message.error('Ошибка удаления'); }
  };

  const openSetModal = (set?: SetDto) => {
    if (set) {
      setEditSet(set);
      setForm.setFieldsValue(set);
    } else {
      setEditSet(null);
      setForm.resetFields();
      setForm.setFieldsValue({ setNumber: (sets.length + 1) });
    }
    setSetModalOpen(true);
  };

  // операции с санкциями
  const openSanctionModal = (sanction?: Sanction) => {
    if (sanctions.length === 0) loadSanctions();
    if (sanction) {
      setEditSanction(sanction);
      sanctionForm.setFieldsValue(sanction);
      setSelectedSanctionTeamId(sanction.teamId);
    } else {
      setEditSanction(null);
      sanctionForm.resetFields();
      setSelectedSanctionTeamId(undefined);
    }
    setSanctionModalOpen(true);
  };

  const handleSaveSanction = async () => {
    let values: Record<string, unknown>;
    try { values = await sanctionForm.validateFields(); } catch { return; }
    setSavingSanction(true);
    try {
      if (editSanction) {
        await sanctionsApi.update(matchId, editSanction.id, values);
        message.success('Санкция обновлена');
      } else {
        await sanctionsApi.create(matchId, { matchId, ...values });
        message.success('Санкция добавлена');
      }
      setSanctionModalOpen(false);
      loadSanctions();
    } catch (err) { message.error(getApiError(err, 'Ошибка сохранения санкции')); } finally { setSavingSanction(false); }
  };

  // операции с протоколом
  const openProtocolModal = () => {
    protocolForm.resetFields();
    if (protocol) {
      protocolForm.setFieldsValue({ statusCode: protocol.statusCode });
    } else {
      const defaultCode = protocolStatuses.find(s => s.name === 'Черновик')?.code ?? protocolStatuses[0]?.code;
      protocolForm.setFieldsValue({ statusCode: defaultCode });
    }
    setProtocolModalOpen(true);
  };

  const handleSaveProtocol = async () => {
    let values: Record<string, unknown>;
    try {
      values = await protocolForm.validateFields();
    } catch {
      message.warning('Заполните все обязательные поля протокола');
      return;
    }
    if (import.meta.env.DEV) console.log('[Protocol] save values:', values);
    setSavingProtocol(true);
    try {
      if (protocol) {
        await protocolsApi.update(protocol.id, values);
        message.success('Протокол обновлён');
      } else {
        const created = await protocolsApi.create({ matchId, ...values });
        if (import.meta.env.DEV) console.log('[Protocol] created:', created);
        message.success('Протокол создан');
      }
      setProtocolModalOpen(false);
      loadProtocol();
    } catch (err: unknown) {
      const axErr = err as { response?: { data?: { message?: string }; status?: number } };
      const detail = axErr.response?.data?.message ?? (err instanceof Error ? err.message : '');
      message.error(detail ? `Ошибка протокола: ${detail}` : 'Ошибка сохранения протокола');
    } finally {
      setSavingProtocol(false);
    }
  };

  // операции с судьями
  const handleSaveRef = async () => {
    let values: Record<string, unknown>;
    try { values = await refForm.validateFields(); } catch { return; }
    setSavingRef(true);
    try {
      await refereeAssignmentsApi.create({ matchId, ...values });
      message.success('Судья назначен');
      setRefModalOpen(false);
      refForm.resetFields();
      loadReferees();
    } catch (err) { message.error(getApiError(err, 'Ошибка назначения судьи')); } finally { setSavingRef(false); }
  };

  const handleDeleteRef = async (refId: number) => {
    try {
      await refereeAssignmentsApi.delete(refId);
      message.success('Назначение удалено');
      loadReferees();
    } catch { message.error('Ошибка удаления'); }
  };

  // операции с делегациями
  const openDelModal = (del?: Delegation) => {
    if (del) {
      setEditDelegation(del);
      delForm.setFieldsValue({
        teamId: del.teamId,
        lastName: del.lastName,
        firstName: del.firstName,
        middleName: del.middleName,
        roleType: del.roleType,
      });
    } else {
      setEditDelegation(null);
      delForm.resetFields();
    }
    setDelModalOpen(true);
  };

  const handleSaveDel = async () => {
    let values: Record<string, unknown>;
    try { values = await delForm.validateFields(); } catch { return; }
    setSavingDel(true);
    try {
      if (editDelegation) {
        await delegationsApi.update(matchId, editDelegation.id, values);
        message.success('Запись обновлена');
      } else {
        await delegationsApi.create(matchId, { matchId, ...values });
        message.success('Запись добавлена');
      }
      setDelModalOpen(false);
      loadDelegations();
    } catch (err) { message.error(getApiError(err, 'Ошибка сохранения делегации')); } finally { setSavingDel(false); }
  };

  // операции с капитанами
  const saveCaptain = async (teamId: number, playerId: number | undefined) => {
    if (!playerId) return;
    setSavingCaptain(true);
    try {
      await captainsApi.upsert(matchId, { teamId, playerId });
      message.success('Капитан сохранён');
      loadCaptains();
    } catch (err) { message.error(getApiError(err, 'Ошибка сохранения капитана')); } finally { setSavingCaptain(false); }
  };

  const deleteCaptain = async (teamId: number) => {
    try {
      await captainsApi.delete(matchId, teamId);
      if (match?.homeTeamId === teamId) setHomeCaptainId(undefined);
      else setGuestCaptainId(undefined);
      message.success('Капитан удалён');
    } catch { message.error('Ошибка удаления'); }
  };

  // операции со статистикой
  const openStatsModal = (stat?: PlayerStats) => {
    if (stat) {
      setEditStat(stat);
      statsForm.setFieldsValue(stat);
    } else {
      setEditStat(null);
      statsForm.resetFields();
    }
    setStatsModalOpen(true);
  };

  const handleSaveStat = async () => {
    let values: Record<string, unknown>;
    try { values = await statsForm.validateFields(); } catch { return; }
    // totalPoints вводится вручную секретарём или берётся из поля формы
    setSavingStat(true);
    try {
      await playerStatsApi.upsert(matchId, { matchId, ...values });
      message.success('Статистика сохранена');
      setStatsModalOpen(false);
      loadStats();
    } catch (err) { message.error(getApiError(err, 'Ошибка сохранения статистики')); } finally { setSavingStat(false); }
  };

  // вычисления (защита от null, т.к. БД допускает nullable счёт)
  const homeWins = sets.filter((s) => s.homeScore != null && s.guestScore != null && s.homeScore > s.guestScore).length;
  const guestWins = sets.filter((s) => s.homeScore != null && s.guestScore != null && s.guestScore > s.homeScore).length;

  // колонки таблиц
  const setsColumns: ColumnsType<SetDto> = [
    { title: '№', dataIndex: 'setNumber', key: 'setNumber', width: 60 },
    {
      title: 'Счёт хозяев', dataIndex: 'homeScore', key: 'homeScore',
      render: (v: number | null, r: SetDto) => v == null ? '—' : <Text strong style={{ color: v > (r.guestScore ?? 0) ? '#52c41a' : 'inherit' }}>{v}</Text>,
    },
    {
      title: 'Счёт гостей', dataIndex: 'guestScore', key: 'guestScore',
      render: (v: number | null, r: SetDto) => v == null ? '—' : <Text strong style={{ color: v > (r.homeScore ?? 0) ? '#52c41a' : 'inherit' }}>{v}</Text>,
    },
    { title: 'Длит. (мин)', dataIndex: 'durationMin', key: 'durationMin', render: (v: number) => v ?? '—' },
    {
      title: '',
      key: 'actions',
      width: 80,
      render: (_: unknown, rec: SetDto) => can('manageSets') ? (
        <Space>
          <Button type="text" size="small" icon={<EditOutlined />} onClick={() => openSetModal(rec)} />
          <Popconfirm title="Удалить партию?" onConfirm={() => handleDeleteSet(rec.setNumber)} okText="Да" okButtonProps={{ danger: true }}>
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ) : null,
    },
  ];

  const eventsColumns: ColumnsType<MatchEvent> = [
    { title: 'Партия', dataIndex: 'setNumber', key: 'setNumber', width: 70 },
    { title: '№', dataIndex: 'globalSeqInSet', key: 'globalSeqInSet', width: 60 },
    { title: 'Тип', dataIndex: 'eventTypeName', key: 'eventTypeName', render: (v: string, r: MatchEvent) => v ?? r.eventTypeCode },
    { title: 'Команда', dataIndex: 'teamName', key: 'teamName', render: (v: string) => v ?? '—' },
    { title: 'Счёт', key: 'score', render: (_: unknown, r: MatchEvent) => `${r.homeScoreAtMoment}:${r.guestScoreAtMoment}` },
    { title: 'Мин.', dataIndex: 'minuteMark', key: 'minuteMark', width: 60, render: (v: number) => v ?? '—' },
    {
      title: 'Детали', key: 'details', render: (_: unknown, r: MatchEvent) => {
        if (r.substitution) return <Text type="secondary" style={{ fontSize: 11 }}>Замена: {r.substitution.subOutPlayerName ?? '?'} → {r.substitution.subInPlayerName ?? '?'}</Text>;
        if (r.timeout) return <Text type="secondary" style={{ fontSize: 11 }}>Тайм-аут: {r.timeout.timeoutTypeName ?? '—'}</Text>;
        return '—';
      },
    },
  ];

  const sanctionsColumns: ColumnsType<Sanction> = [
    { title: 'Команда', dataIndex: 'teamName', key: 'teamName', render: (v: string) => v ?? '—' },
    { title: 'Игрок', dataIndex: 'playerFullName', key: 'playerFullName', render: (v: string) => v ?? '—' },
    { title: 'Тип', dataIndex: 'sanctionTypeName', key: 'sanctionTypeName', render: (v: string, r: Sanction) => v ?? r.sanctionTypeCode },
    { title: 'Вид', dataIndex: 'sanctionKindName', key: 'sanctionKindName', render: (v: string) => v ?? '—' },
    { title: 'Партия', dataIndex: 'setNumber', key: 'setNumber', render: (v: number) => v ?? '—' },
    { title: 'Мин.', dataIndex: 'minuteMark', key: 'minuteMark', render: (v: number) => v ?? '—' },
    {
      title: '', key: 'actions', width: 80,
      render: (_: unknown, rec: Sanction) => can('manageSanctions') ? (
        <Space>
          <Button type="text" size="small" icon={<EditOutlined />} onClick={() => openSanctionModal(rec)} />
          <Popconfirm title="Удалить санкцию?" onConfirm={async () => { await sanctionsApi.delete(matchId, rec.id); loadSanctions(); }} okText="Да" okButtonProps={{ danger: true }}>
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ) : null,
    },
  ];

  const refColumns: ColumnsType<RefereeAssignment> = [
    { title: 'Судья', dataIndex: 'refereeFullName', key: 'refereeFullName' },
    { title: 'Роль', dataIndex: 'roleName', key: 'roleName' },
    {
      title: '', key: 'del', width: 60,
      render: (_: unknown, rec: RefereeAssignment) => can('manageRefereeAssignment') ? (
        <Popconfirm title="Удалить назначение?" onConfirm={() => handleDeleteRef(rec.id)} okText="Да" okButtonProps={{ danger: true }}>
          <Button type="text" size="small" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      ) : null,
    },
  ];

  const delColumns: ColumnsType<Delegation> = [
    { title: 'Команда', dataIndex: 'teamName', key: 'teamName' },
    { title: 'ФИО', dataIndex: 'fullName', key: 'fullName', render: (v: string) => v ?? '—' },
    { title: 'Роль', dataIndex: 'roleType', key: 'roleType' },
    {
      title: '', key: 'actions', width: 80,
      render: (_: unknown, rec: Delegation) => can('manageDelegation') ? (
        <Space>
          <Button type="text" size="small" icon={<EditOutlined />} onClick={() => openDelModal(rec)} />
          <Popconfirm title="Удалить?" onConfirm={async () => { await delegationsApi.delete(matchId, rec.id); loadDelegations(); }} okText="Да" okButtonProps={{ danger: true }}>
            <Button type="text" size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ) : null,
    },
  ];

  const statsColumns: ColumnsType<PlayerStats> = [
    {
      title: 'Игрок', dataIndex: 'playerFullName', key: 'playerFullName', fixed: 'left', width: 140,
      render: (v: string, r: PlayerStats) => v ?? `Игрок ${r.playerId}`,
    },
    { title: 'Команда', dataIndex: 'teamName', key: 'teamName', width: 130 },
    { title: 'Подачи', dataIndex: 'servesTotal', key: 'servesTotal', width: 80 },
    { title: 'Эйсы', dataIndex: 'aces', key: 'aces', width: 70 },
    { title: 'Ошибок подач', dataIndex: 'serveErrors', key: 'serveErrors', width: 100 },
    { title: 'Приёмы', dataIndex: 'receptionsTotal', key: 'receptionsTotal', width: 90 },
    { title: 'Атаки', dataIndex: 'attacksTotal', key: 'attacksTotal', width: 80 },
    { title: 'Очки атак', dataIndex: 'attackPoints', key: 'attackPoints', width: 90 },
    { title: 'Блоки', dataIndex: 'blocks', key: 'blocks', width: 70 },
    { title: 'Итого', dataIndex: 'totalPoints', key: 'totalPoints', width: 70, render: (v: number) => <Text strong>{v}</Text> },
    {
      title: '', key: 'actions', width: 60, fixed: 'right',
      render: (_: unknown, rec: PlayerStats) => can('editStats') ? (
        <Button type="text" size="small" icon={<EditOutlined />} onClick={() => openStatsModal(rec)} />
      ) : null,
    },
  ];

  // рендер
  if (loadingMatch) {
    return (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 300 }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!match) {
    return (
      <div>
        <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/matches')} style={{ marginBottom: 16 }}>
          Назад к матчам
        </Button>
        <Text type="secondary">Матч не найден</Text>
      </div>
    );
  }

  return (
    <div>
      <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/matches')} style={{ marginBottom: 16 }}>
        Назад к матчам
      </Button>

      {/* шапка матча */}
      <Card
        style={{ marginBottom: 16, borderRadius: 10 }}
        title={
          <Space align="center" style={{ flexWrap: 'wrap', gap: 8 }}>
            <Title level={4} style={{ margin: 0 }}>
              {match.homeTeamName ?? `Команда #${match.homeTeamId}`}
              <span style={{ margin: '0 12px', color: '#d9d9d9' }}>vs</span>
              {match.guestTeamName ?? `Команда #${match.guestTeamId}`}
            </Title>
            {sets.length > 0 && (
              <Badge
                count={`${homeWins}:${guestWins}`}
                style={{ backgroundColor: '#1677ff', fontSize: 16, padding: '0 10px', height: 28, lineHeight: '28px' }}
              />
            )}
          </Space>
        }
        extra={<Tag color={STATUS_COLORS[match.statusName ?? ''] ?? 'default'} style={{ fontSize: 13 }}>{match.statusName ?? '—'}</Tag>}
      >
        {match.statusName === 'В процессе' && can('createProtocol') && (
          <div style={{ marginBottom: 12 }}>
            <Button
              type="primary"
              size="large"
              icon={<span>🏐</span>}
              onClick={() => setLiveMatchOpen(true)}
              style={{
                background: '#52c41a', borderColor: '#52c41a',
                fontSize: 15, height: 44, paddingLeft: 24, paddingRight: 24,
              }}
            >
              Открыть ведение матча
            </Button>
          </div>
        )}
        {match.statusName === 'Запланирован' && can('manageMatches') && (
          <div style={{ marginBottom: 12 }}>
            <Button
              type="default"
              icon={<span>▶</span>}
              onClick={() => setPreWizardOpen(true)}
            >
              Начать матч (подготовка)
            </Button>
          </div>
        )}
        <Descriptions column={{ xs: 1, sm: 2, md: 3 }} size="small">
          <Descriptions.Item label="Турнир">{match.tournamentName ?? '—'}</Descriptions.Item>
          <Descriptions.Item label="Формат матча">до {match.tournamentSetsToWin ?? 3} {(match.tournamentSetsToWin ?? 3) === 1 ? 'партии' : 'побед'}</Descriptions.Item>
          <Descriptions.Item label="Дата">{match.matchDate ? dayjs(match.matchDate).format('DD.MM.YYYY') : '—'}</Descriptions.Item>
          <Descriptions.Item label="Время">{match.startTime ? match.startTime.slice(0, 5) : '—'}</Descriptions.Item>
          <Descriptions.Item label="Площадка">{match.venueName ?? '—'}</Descriptions.Item>
          <Descriptions.Item label="Этап">{match.stageName ?? '—'}</Descriptions.Item>
          {match.groupName && <Descriptions.Item label="Группа">{match.groupName}</Descriptions.Item>}
          <Descriptions.Item label="Видеочеллендж">{match.hasVideoChallenge ? 'Да' : 'Нет'}</Descriptions.Item>
          {match.netHeight && <Descriptions.Item label="Высота сетки">{match.netHeight} м</Descriptions.Item>}
        </Descriptions>
        {events.length > 0 && (
          <div style={{ marginTop: 8 }}>
            <MatchMiniCourt
              events={events}
              homeTeamId={match.homeTeamId}
              guestTeamId={match.guestTeamId}
              homeTeamName={match.homeTeamName ?? 'Хозяева'}
              guestTeamName={match.guestTeamName ?? 'Гости'}
            />
          </div>
        )}
      </Card>

      {/* вкладки */}
      <Card style={{ borderRadius: 10 }}>
        <Tabs
          defaultActiveKey="sets"
          onChange={handleTabChange}
          items={[
            // партии
            {
              key: 'sets',
              label: `Партии ${sets.length > 0 ? `(${homeWins}:${guestWins})` : ''}`,
              children: (
                <Spin spinning={loadingSets}>
                  {can('manageSets') && (
                    <Button icon={<PlusOutlined />} type="dashed" onClick={() => openSetModal()} style={{ marginBottom: 12 }}>
                      Добавить партию
                    </Button>
                  )}
                  <Table
                    rowKey="setNumber" dataSource={sets} columns={setsColumns}
                    pagination={false} size="small" scroll={{ x: 'max-content' }}
                    locale={{ emptyText: 'Партии не зарегистрированы' }}
                    summary={(data) => {
                      if (data.length === 0) return null;
                      const th = data.reduce((a, r) => a + (r.homeScore ?? 0), 0);
                      const tg = data.reduce((a, r) => a + (r.guestScore ?? 0), 0);
                      const td = data.reduce((a, r) => a + (r.durationMin ?? 0), 0);
                      return (
                        <Table.Summary.Row>
                          <Table.Summary.Cell index={0}><Text strong>Итого</Text></Table.Summary.Cell>
                          <Table.Summary.Cell index={1}><Text strong>{th}</Text></Table.Summary.Cell>
                          <Table.Summary.Cell index={2}><Text strong>{tg}</Text></Table.Summary.Cell>
                          <Table.Summary.Cell index={3}><Text strong>{td > 0 ? `${td} мин` : '—'}</Text></Table.Summary.Cell>
                          <Table.Summary.Cell index={4} />
                        </Table.Summary.Row>
                      );
                    }}
                  />
                </Spin>
              ),
            },

            // расстановка
            {
              key: 'lineup',
              label: 'Расстановка',
              children: (
                <div>
                  <Row align="middle" style={{ marginBottom: 16 }}>
                    <Text style={{ marginRight: 8 }}>Партия:</Text>
                    <Select
                      value={lineupSetNumber}
                      onChange={setLineupSetNumber}
                      options={[1, 2, 3, 4, 5].map(n => ({ value: n, label: `Партия ${n}` }))}
                      style={{ width: 130 }}
                    />
                  </Row>
                  <VolleyCourt
                    matchId={matchId}
                    homeTeamId={match.homeTeamId}
                    homeTeamName={match.homeTeamName ?? 'Хозяева'}
                    guestTeamId={match.guestTeamId}
                    guestTeamName={match.guestTeamName ?? 'Гости'}
                    setNumber={lineupSetNumber}
                    readonly={!can('manageLineup')}
                  />
                </div>
              ),
            },

            // статистика
            {
              key: 'stats',
              label: 'Статистика',
              children: (
                <Spin spinning={loadingStats}>
                  <Space style={{ marginBottom: 12 }}>
                    {can('editStats') && (
                      <Button icon={<PlusOutlined />} type="dashed" onClick={() => openStatsModal()}>
                        Добавить статистику
                      </Button>
                    )}
                    {stats.length >= 2 && (
                      <Button onClick={() => { setRadarPlayerAId(undefined); setRadarPlayerBId(undefined); setRadarStatsA(null); setRadarStatsB(null); setRadarCompareOpen(true); }}>
                        Сравнить двух игроков
                      </Button>
                    )}
                  </Space>
                  <Table
                    rowKey={(r) => `${r.matchId}-${r.playerId}`}
                    dataSource={stats}
                    columns={statsColumns}
                    pagination={false}
                    size="small"
                    scroll={{ x: 'max-content' }}
                    locale={{ emptyText: 'Статистика не введена' }}
                    onRow={rec => ({
                      onClick: () => {
                        setRadarStatsA(rec);
                        setRadarStatsB(null);
                        setRadarPlayerAId(rec.playerId);
                        setRadarPlayerBId(undefined);
                        setRadarCompareOpen(true);
                      },
                      style: { cursor: 'pointer' },
                      title: 'Нажмите для просмотра радар-диаграммы',
                    })}
                  />
                </Spin>
              ),
            },

            // события
            {
              key: 'events',
              label: 'События',
              children: (
                <Spin spinning={loadingEvents}>
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12, flexWrap: 'wrap', gap: 8 }}>
                    {can('manageEvents') && match.statusName === 'В процессе' ? (
                      <Button type="dashed" icon={<PlusOutlined />} onClick={() => setLiveMatchOpen(true)}>
                        Добавить событие (открыть ведение матча)
                      </Button>
                    ) : <span />}
                    <Radio.Group value={eventsView} onChange={e => setEventsView(e.target.value)} buttonStyle="solid" size="small">
                      <Radio.Button value="table"><UnorderedListOutlined /> Таблица</Radio.Button>
                      <Radio.Button value="timeline"><FieldTimeOutlined /> Таймлайн</Radio.Button>
                    </Radio.Group>
                  </div>
                  {eventsView === 'table' ? (
                    <Table
                      rowKey="id" dataSource={events} columns={eventsColumns}
                      pagination={{ pageSize: 20, showSizeChanger: true }}
                      size="small" scroll={{ x: 'max-content' }}
                      locale={{ emptyText: 'События не зарегистрированы' }}
                    />
                  ) : (
                    <MatchTimeline
                      matchId={matchId}
                      events={events}
                      sets={sets}
                      homeTeamId={match.homeTeamId}
                      guestTeamId={match.guestTeamId}
                      homeTeamName={match.homeTeamName ?? 'Хозяева'}
                      guestTeamName={match.guestTeamName ?? 'Гости'}
                    />
                  )}
                </Spin>
              ),
            },

            // замены
            {
              key: 'substitutions',
              label: 'Замены',
              children: (
                <Spin spinning={loadingEvents}>
                  <SubstitutionGraph
                    events={events}
                    sets={sets}
                    homeTeamId={match.homeTeamId}
                    homeTeamName={match.homeTeamName ?? 'Хозяева'}
                    guestTeamId={match.guestTeamId}
                    guestTeamName={match.guestTeamName ?? 'Гости'}
                  />
                </Spin>
              ),
            },

            // санкции
            {
              key: 'sanctions',
              label: `Санкции${sanctions.length > 0 ? ` (${sanctions.length})` : ''}`,
              children: (
                <Spin spinning={loadingSanctions}>
                  {can('manageSanctions') && (
                    <Button icon={<PlusOutlined />} type="dashed" onClick={() => openSanctionModal()} style={{ marginBottom: 12 }}>
                      Добавить санкцию
                    </Button>
                  )}
                  <Table
                    rowKey="id" dataSource={sanctions} columns={sanctionsColumns}
                    pagination={false} size="small" scroll={{ x: 'max-content' }}
                    locale={{ emptyText: 'Санкции не применялись' }}
                  />
                </Spin>
              ),
            },

            // судьи
            {
              key: 'referees',
              label: 'Судьи',
              children: (
                <Spin spinning={loadingRefs}>
                  {can('manageRefereeAssignment') && (
                    <Button icon={<PlusOutlined />} type="dashed" onClick={() => { refForm.resetFields(); setRefModalOpen(true); }} style={{ marginBottom: 12 }}>
                      Назначить судью
                    </Button>
                  )}
                  <Table
                    rowKey="id" dataSource={referees} columns={refColumns}
                    pagination={false} size="small"
                    locale={{ emptyText: 'Судьи не назначены' }}
                  />
                </Spin>
              ),
            },

            // длегеации
            {
              key: 'delegations',
              label: 'Делегации',
              children: (
                <Spin spinning={loadingDel}>
                  {can('manageDelegation') && (
                    <Button icon={<PlusOutlined />} type="dashed" onClick={() => openDelModal()} style={{ marginBottom: 12 }}>
                      Добавить участника
                    </Button>
                  )}
                  <Table
                    rowKey="id" dataSource={delegations} columns={delColumns}
                    pagination={false} size="small"
                    locale={{ emptyText: 'Делегации не внесены' }}
                  />
                </Spin>
              ),
            },

            // капитаны
            {
              key: 'captains',
              label: 'Капитаны',
              children: (
                <Spin spinning={loadingCaptains || savingCaptain}>
                  <Row gutter={[24, 16]} style={{ marginTop: 8 }}>
                    <Col xs={24} sm={12}>
                      <Card size="small" title={<><TeamOutlined /> {match.homeTeamName ?? 'Хозяева'}</>}>
                        <Space>
                          <Select
                            placeholder="Выберите капитана"
                            value={homeCaptainId}
                            onChange={setHomeCaptainId}
                            disabled={!can('manageCaptain')}
                            style={{ width: 220 }}
                            allowClear
                            options={homePlayers.map(p => ({ value: Number(p.id), label: p.name }))}
                            showSearch
                            filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                          />
                          {can('manageCaptain') && (
                            <>
                              <Button type="primary" size="small" onClick={() => saveCaptain(match.homeTeamId, homeCaptainId)}>
                                Сохранить
                              </Button>
                              {captains.some(c => c.teamId === match.homeTeamId) && (
                                <Button size="small" danger onClick={() => deleteCaptain(match.homeTeamId)}>
                                  Удалить
                                </Button>
                              )}
                            </>
                          )}
                        </Space>
                      </Card>
                    </Col>
                    <Col xs={24} sm={12}>
                      <Card size="small" title={<><TeamOutlined /> {match.guestTeamName ?? 'Гости'}</>}>
                        <Space>
                          <Select
                            placeholder="Выберите капитана"
                            value={guestCaptainId}
                            onChange={setGuestCaptainId}
                            disabled={!can('manageCaptain')}
                            style={{ width: 220 }}
                            allowClear
                            options={guestPlayers.map(p => ({ value: Number(p.id), label: p.name }))}
                            showSearch
                            filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                          />
                          {can('manageCaptain') && (
                            <>
                              <Button type="primary" size="small" onClick={() => saveCaptain(match.guestTeamId, guestCaptainId)}>
                                Сохранить
                              </Button>
                              {captains.some(c => c.teamId === match.guestTeamId) && (
                                <Button size="small" danger onClick={() => deleteCaptain(match.guestTeamId)}>
                                  Удалить
                                </Button>
                              )}
                            </>
                          )}
                        </Space>
                      </Card>
                    </Col>
                  </Row>
                </Spin>
              ),
            },

            // протокол
            {
              key: 'protocol',
              label: 'Итог матча',
              children: (
                <Spin spinning={loadingProtocol}>
                  <Space style={{ marginBottom: 16 }}>
                    {can('createProtocol') && (
                      <Button
                        icon={protocol ? <EditOutlined /> : <FileAddOutlined />}
                        type={protocol ? 'default' : 'primary'}
                        onClick={openProtocolModal}
                      >
                        {protocol ? 'Редактировать протокол' : 'Создать протокол'}
                      </Button>
                    )}
                    {(match.statusCode === 3 || !!protocol) && (
                      <Button icon={<QrcodeOutlined />} onClick={() => setQrModalOpen(true)}>
                        QR-код
                      </Button>
                    )}
                  </Space>
                  {protocol ? (
                    <Descriptions column={{ xs: 1, sm: 2 }} size="small" bordered>
                      <Descriptions.Item label="Статус">
                        <Tag color="blue">{protocol.statusName ?? '—'}</Tag>
                      </Descriptions.Item>
                      <Descriptions.Item label="Итоговый счёт">
                        <Text strong style={{ fontSize: 20 }}>
                          {homeWins} : {guestWins}
                        </Text>
                      </Descriptions.Item>
                      <Descriptions.Item label="Утверждён">
                        {protocol.approvalDate ? dayjs(protocol.approvalDate).format('DD.MM.YYYY') : 'Не утверждён'}
                      </Descriptions.Item>
                    </Descriptions>
                  ) : (
                    <Text type="secondary">Протокол матча не сформирован</Text>
                  )}
                </Spin>
              ),
            },
          ]}
        />
      </Card>

      {/* модальные окна */}

      {/* партия */}
      <Modal
        title={editSet ? 'Редактировать партию' : 'Добавить партию'}
        open={setModalOpen}
        onCancel={() => setSetModalOpen(false)}
        onOk={handleSaveSet}
        confirmLoading={savingSet}
        okText="Сохранить"
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form form={setForm} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={12}>
            <Col span={8}>
              <Form.Item name="setNumber" label="№ партии" rules={[{ required: true }]}>
                <InputNumber min={1} max={5} style={{ width: '100%' }} disabled={!!editSet} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item name="homeScore" label="Счёт хозяев" rules={[{ required: true }]}>
                <InputNumber min={0} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item name="guestScore" label="Счёт гостей" rules={[{ required: true }]}>
                <InputNumber min={0} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>
          <Form.Item name="durationMin" label="Длительность (мин)">
            <InputNumber min={0} style={{ width: '100%' }} />
          </Form.Item>
        </Form>
      </Modal>

      {/* санкция */}
      <Modal
        title={editSanction ? 'Редактировать санкцию' : 'Добавить санкцию'}
        open={sanctionModalOpen}
        onCancel={() => { setSanctionModalOpen(false); setSelectedSanctionTeamId(undefined); }}
        onOk={handleSaveSanction}
        confirmLoading={savingSanction}
        okText="Сохранить"
        cancelText="Отмена"
        width={560}
        destroyOnHidden
      >
        <Form form={sanctionForm} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={12}>
            <Col xs={24} sm={12}>
              <Form.Item name="teamId" label="Команда" rules={[{ required: true, message: 'Выберите команду' }]}>
                <Select
                  options={[
                    { value: match.homeTeamId, label: match.homeTeamName ?? 'Хозяева' },
                    { value: match.guestTeamId, label: match.guestTeamName ?? 'Гости' },
                  ]}
                  onChange={(v: number) => {
                    setSelectedSanctionTeamId(v);
                    sanctionForm.setFieldValue('playerId', undefined);
                  }}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="playerId" label="Игрок">
                <Select
                  allowClear
                  disabled={!selectedSanctionTeamId}
                  placeholder={selectedSanctionTeamId ? 'Не указан' : 'Сначала выберите команду'}
                  options={(selectedSanctionTeamId === match.homeTeamId ? homePlayers : guestPlayers)
                    .map(p => ({ value: Number(p.id), label: p.name }))}
                  showSearch
                  filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col xs={24} sm={8}>
              <Form.Item name="recipientTypeCode" label="Получатель" rules={[{ required: true }]}>
                <Select options={recipientTypes.map(t => ({ value: t.code, label: t.name }))} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="sanctionTypeCode" label="Тип санкции" rules={[{ required: true }]}>
                <Select options={sanctionTypes.map(t => ({ value: t.code, label: t.name }))} />
              </Form.Item>
            </Col>
            <Col xs={24} sm={8}>
              <Form.Item name="sanctionKindCode" label="Вид" rules={[{ required: true }]}>
                <Select options={sanctionKinds.map(k => ({ value: k.code, label: k.name }))} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col span={8}>
              <Form.Item name="setNumber" label="Партия" rules={[{ required: true }]}>
                <InputNumber min={1} max={5} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item name="memberSeqInMatch" label="№ санкции" initialValue={1}>
                <InputNumber min={0} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item name="minuteMark" label="Минута">
                <InputNumber min={0} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col span={12}>
              <Form.Item name="homeScoreAtMoment" label="Счёт хозяев" initialValue={0}>
                <InputNumber min={0} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="guestScoreAtMoment" label="Счёт гостей" initialValue={0}>
                <InputNumber min={0} style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* протокол */}
      <Modal
        title={protocol ? 'Редактировать протокол' : 'Создать протокол матча'}
        open={protocolModalOpen}
        onCancel={() => setProtocolModalOpen(false)}
        onOk={handleSaveProtocol}
        confirmLoading={savingProtocol}
        okText="Сохранить"
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form form={protocolForm} layout="vertical" style={{ marginTop: 16 }}>
          {sets.length > 0 && (
            <Text type="secondary" style={{ display: 'block', marginBottom: 12, fontSize: 12 }}>
              Текущий счёт по партиям: {homeWins} : {guestWins}
            </Text>
          )}
          <Form.Item name="statusCode" label="Статус протокола" rules={[{ required: true }]}>
            <Select options={protocolStatuses.map(s => ({ value: s.code, label: s.name }))} />
          </Form.Item>
        </Form>
      </Modal>

      {/* назначение судьи */}
      <Modal
        title="Назначить судью"
        open={refModalOpen}
        onCancel={() => setRefModalOpen(false)}
        onOk={handleSaveRef}
        confirmLoading={savingRef}
        okText="Назначить"
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form form={refForm} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item name="refereeId" label="Судья" rules={[{ required: true }]}>
            <Select
              options={refereesLookup.map(r => ({ value: Number(r.id), label: r.name }))}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
              placeholder="Выберите судью"
            />
          </Form.Item>
          <Form.Item name="roleCode" label="Роль" rules={[{ required: true }]}>
            <Select options={refRoles.map(r => ({ value: r.code, label: r.name }))} placeholder="Выберите роль" />
          </Form.Item>
        </Form>
      </Modal>

      {/* делегация */}
      <Modal
        title={editDelegation ? 'Редактировать участника' : 'Добавить участника делегации'}
        open={delModalOpen}
        onCancel={() => setDelModalOpen(false)}
        onOk={handleSaveDel}
        confirmLoading={savingDel}
        okText="Сохранить"
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form form={delForm} layout="vertical" style={{ marginTop: 16 }}>
          <Form.Item name="teamId" label="Команда" rules={[{ required: true }]}>
            <Select
              options={[
                { value: match.homeTeamId, label: match.homeTeamName ?? 'Хозяева' },
                { value: match.guestTeamId, label: match.guestTeamName ?? 'Гости' },
              ]}
            />
          </Form.Item>
          <Form.Item name="lastName" label="Фамилия" rules={[{ required: true }]}>
            <Input placeholder="Иванов" />
          </Form.Item>
          <Form.Item name="firstName" label="Имя" rules={[{ required: true }]}>
            <Input placeholder="Иван" />
          </Form.Item>
          <Form.Item name="middleName" label="Отчество">
            <Input placeholder="Иванович" />
          </Form.Item>
          <Form.Item name="roleType" label="Роль в делегации" rules={[{ required: true }]}>
            <Select
              options={[
                { value: 'помощник тренера', label: 'Помощник тренера' },
                { value: 'массажист', label: 'Массажист' },
                { value: 'врач', label: 'Врач' },
              ]}
              placeholder="Выберите роль"
            />
          </Form.Item>
        </Form>
      </Modal>

      {/* визард подготовки матча */}
      {preWizardOpen && (
        <PreMatchWizard
          open={preWizardOpen}
          match={match}
          onComplete={handleStartMatch}
          onSkip={async () => {
            const inProgressCode = statuses.find(s => s.name === 'В процессе')?.code ?? 2;
            try { await matchesApi.update(matchId, { ...match, statusCode: inProgressCode }); } catch { /* не блокирует */ }
            setMatch(prev => prev ? { ...prev, statusCode: inProgressCode, statusName: 'В процессе' } : prev);
            setPreWizardOpen(false);
            setLiveMatchOpen(true);
          }}
          onCancel={() => setPreWizardOpen(false)}
        />
      )}

      {/* живой протокол матча */}
      {liveMatchOpen && (
        <LiveMatchPanel
          open={liveMatchOpen}
          match={match}
          initialState={coinTossResult ?? undefined}
          completedStatusCode={statuses.find(s => s.name === 'Завершён')?.code ?? 3}
          onClose={() => {
            setLiveMatchOpen(false);
            loadSets();
            loadProtocol();
            loadEvents();
          }}
        />
      )}

      {/* QR-код результата */}
      {qrModalOpen && protocol && (
        <QrModal
          open={qrModalOpen}
          matchId={matchId}
          homeTeamName={match.homeTeamName ?? 'Хозяева'}
          guestTeamName={match.guestTeamName ?? 'Гости'}
          homeScore={homeWins}
          guestScore={guestWins}
          onClose={() => setQrModalOpen(false)}
        />
      )}

      {/* сравнение статистики двух игроков */}
      <Modal
        title={radarPlayerBId !== undefined || (!radarStatsA && !radarPlayerAId) ? 'Сравнение игроков' : (radarStatsA ? `${radarStatsA.playerFullName ?? 'Игрок'} — статистика` : 'Статистика игрока')}
        open={radarCompareOpen}
        onCancel={() => { setRadarCompareOpen(false); setRadarStatsA(null); setRadarStatsB(null); setRadarPlayerAId(undefined); setRadarPlayerBId(undefined); }}
        footer={null}
        width={680}
        destroyOnHidden
      >
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={12}>
            <Select
              placeholder="Игрок A"
              style={{ width: '100%' }}
              value={radarPlayerAId}
              options={allPlayers.map(p => ({ value: Number(p.id), label: p.name }))}
              onChange={(id: number) => { setRadarPlayerAId(id); loadRadarStats(id, 'A'); }}
              allowClear
              onClear={() => { setRadarPlayerAId(undefined); setRadarStatsA(null); }}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
            />
          </Col>
          <Col span={12}>
            <Select
              placeholder="Игрок B (для сравнения)"
              style={{ width: '100%' }}
              value={radarPlayerBId}
              options={allPlayers.map(p => ({ value: Number(p.id), label: p.name }))}
              onChange={(id: number) => { setRadarPlayerBId(id); loadRadarStats(id, 'B'); }}
              allowClear
              onClear={() => { setRadarPlayerBId(undefined); setRadarStatsB(null); }}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
            />
          </Col>
        </Row>
        {radarStatsA ? (
          <div style={{ display: 'flex', justifyContent: 'center' }}>
            <PlayerRadarChart
              stats={radarStatsA}
              compareStats={radarStatsB ?? undefined}
              size={360}
            />
          </div>
        ) : (
          <Text type="secondary">
            {radarPlayerAId ? 'Нет статистики для выбранного игрока' : 'Выберите игрока для просмотра статистики'}
          </Text>
        )}
      </Modal>

      {/* статистика игрока */}
      <Modal
        title={editStat ? 'Редактировать статистику' : 'Добавить статистику игрока'}
        open={statsModalOpen}
        onCancel={() => setStatsModalOpen(false)}
        onOk={handleSaveStat}
        confirmLoading={savingStat}
        okText="Сохранить"
        cancelText="Отмена"
        width={600}
        destroyOnHidden
      >
        <Form form={statsForm} layout="vertical" style={{ marginTop: 16 }}>
          <Row gutter={12}>
            <Col xs={24} sm={12}>
              <Form.Item name="playerId" label="Игрок" rules={[{ required: true }]}>
                <Select
                  disabled={!!editStat}
                  options={allPlayers.map(p => ({ value: Number(p.id), label: p.name }))}
                  showSearch
                  filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                  placeholder="Выберите игрока"
                  onChange={(pid: number) => {
                    if (!editStat) {
                      const isHome = homePlayers.some(p => Number(p.id) === pid);
                      statsForm.setFieldValue('teamId', isHome ? match.homeTeamId : match.guestTeamId);
                    }
                  }}
                />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="teamId" label="Команда" rules={[{ required: true }]}>
                <Select
                  disabled={!!editStat}
                  options={[
                    { value: match.homeTeamId, label: match.homeTeamName ?? 'Хозяева' },
                    { value: match.guestTeamId, label: match.guestTeamName ?? 'Гости' },
                  ]}
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            {[
              { name: 'servesTotal', label: 'Подачи всего' },
              { name: 'aces', label: 'Эйсы' },
              { name: 'serveErrors', label: 'Ошибки подач' },
              { name: 'receptionsTotal', label: 'Приёмы всего' },
              { name: 'positiveReceptions', label: 'Положит. приёмы' },
              { name: 'receptionErrors', label: 'Ошибки приёмов' },
              { name: 'attacksTotal', label: 'Атаки всего' },
              { name: 'attackPoints', label: 'Очки атак' },
              { name: 'attackErrors', label: 'Ошибки атак' },
              { name: 'blocks', label: 'Блоки' },
            ].map(f => (
              <Col xs={12} sm={8} key={f.name}>
                <Form.Item name={f.name} label={f.label} initialValue={0}>
                  <InputNumber min={0} style={{ width: '100%' }} />
                </Form.Item>
              </Col>
            ))}
            <Col xs={12} sm={8}>
              <Form.Item
                label="Очков итого (авто)"
                shouldUpdate={(prev, curr) =>
                  prev.aces !== curr.aces ||
                  prev.attackPoints !== curr.attackPoints ||
                  prev.blocks !== curr.blocks
                }
              >
                {({ getFieldValue }) => {
                  const total =
                    (getFieldValue('aces') || 0) +
                    (getFieldValue('attackPoints') || 0) +
                    (getFieldValue('blocks') || 0);
                  return (
                    <Input
                      value={total}
                      readOnly
                      style={{ width: '100%', background: '#f5f5f5', cursor: 'default' }}
                    />
                  );
                }}
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>
    </div>
  );
};

export default MatchDetailPage;
