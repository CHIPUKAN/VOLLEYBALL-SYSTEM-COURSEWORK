import client from './client';
import type {
  Team, Venue, Coach, Player, Season, Organizer, Referee,
  Tournament, Group, Match, Application, RefereeAssignment,
  Protocol, SetDto, PlayerStats, StartingLineup, MatchEvent,
  Sanction, Award, MatchCaptain, Delegation, StandingRow,
} from '../types';

// команды
export const teamsApi = {
  getAll: () => client.get<Team[]>('/teams').then(r => r.data),
  getById: (id: number) => client.get<Team>(`/teams/${id}`).then(r => r.data),
  create: (data: Partial<Team>) => client.post<Team>('/teams', data).then(r => r.data),
  update: (id: number, data: Partial<Team>) => client.put<Team>(`/teams/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/teams/${id}`),
};

// площадки
export const venuesApi = {
  getAll: () => client.get<Venue[]>('/venues').then(r => r.data),
  getById: (id: number) => client.get<Venue>(`/venues/${id}`).then(r => r.data),
  create: (data: Partial<Venue>) => client.post<Venue>('/venues', data).then(r => r.data),
  update: (id: number, data: Partial<Venue>) => client.put<Venue>(`/venues/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/venues/${id}`),
};

// тренеры
export const coachesApi = {
  getAll: () => client.get<Coach[]>('/coaches').then(r => r.data),
  getById: (id: number) => client.get<Coach>(`/coaches/${id}`).then(r => r.data),
  create: (data: Partial<Coach>) => client.post<Coach>('/coaches', data).then(r => r.data),
  update: (id: number, data: Partial<Coach>) => client.put<Coach>(`/coaches/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/coaches/${id}`),
};

// игроки
export const playersApi = {
  getAll: (teamId?: number) =>
    client.get<Player[]>('/players', { params: { teamId } }).then(r => r.data),
  getById: (id: number) => client.get<Player>(`/players/${id}`).then(r => r.data),
  create: (data: Partial<Player>) => client.post<Player>('/players', data).then(r => r.data),
  update: (id: number, data: Partial<Player>) => client.put<Player>(`/players/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/players/${id}`),
};

// сезоны
export const seasonsApi = {
  getAll: () => client.get<Season[]>('/seasons').then(r => r.data),
  getById: (id: number) => client.get<Season>(`/seasons/${id}`).then(r => r.data),
  create: (data: Partial<Season>) => client.post<Season>('/seasons', data).then(r => r.data),
  update: (id: number, data: Partial<Season>) => client.put<Season>(`/seasons/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/seasons/${id}`),
};

// организаторы
export const organizersApi = {
  getAll: () => client.get<Organizer[]>('/organizers').then(r => r.data),
  getById: (id: number) => client.get<Organizer>(`/organizers/${id}`).then(r => r.data),
  create: (data: Partial<Organizer>) => client.post<Organizer>('/organizers', data).then(r => r.data),
  update: (id: number, data: Partial<Organizer>) => client.put<Organizer>(`/organizers/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/organizers/${id}`),
};

// судьи
export const refereesApi = {
  getAll: () => client.get<Referee[]>('/referees').then(r => r.data),
  getById: (id: number) => client.get<Referee>(`/referees/${id}`).then(r => r.data),
  create: (data: Partial<Referee>) => client.post<Referee>('/referees', data).then(r => r.data),
  update: (id: number, data: Partial<Referee>) => client.put<Referee>(`/referees/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/referees/${id}`),
};

// регионы
export const regionsApi = {
  getAll: () => client.get<{ oktmoCode: string; name: string }[]>('/regions').then(r => r.data),
  create: (data: { oktmoCode: string; name: string }) =>
    client.post<{ oktmoCode: string; name: string }>('/regions', data).then(r => r.data),
  update: (oktmoCode: string, data: { name: string }) =>
    client.put<{ oktmoCode: string; name: string }>(`/regions/${oktmoCode}`, data).then(r => r.data),
  delete: (oktmoCode: string) => client.delete(`/regions/${oktmoCode}`),
};

// турниры
export const tournamentsApi = {
  getAll: () => client.get<Tournament[]>('/tournaments').then(r => r.data),
  getById: (id: number) => client.get<Tournament>(`/tournaments/${id}`).then(r => r.data),
  create: (data: Partial<Tournament>) => client.post<Tournament>('/tournaments', data).then(r => r.data),
  update: (id: number, data: Partial<Tournament>) => client.put<Tournament>(`/tournaments/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/tournaments/${id}`),
};

// группы
export const groupsApi = {
  getAll: (tournamentId?: number) =>
    client.get<Group[]>('/groups', { params: { tournamentId } }).then(r => r.data),
  getById: (id: number) => client.get<Group>(`/groups/${id}`).then(r => r.data),
  create: (data: Partial<Group>) => client.post<Group>('/groups', data).then(r => r.data),
  update: (id: number, data: Partial<Group>) => client.put<Group>(`/groups/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/groups/${id}`),
};

// матчи
export const matchesApi = {
  getAll: (params?: { tournamentId?: number; statusCode?: number; teamId?: number }) =>
    client.get<Match[]>('/matches', { params }).then(r => r.data),
  getById: (id: number) => client.get<Match>(`/matches/${id}`).then(r => r.data),
  create: (data: Partial<Match>) => client.post<Match>('/matches', data).then(r => r.data),
  update: (id: number, data: Partial<Match>) => client.put<Match>(`/matches/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/matches/${id}`),
};

// заявки
export const applicationsApi = {
  getAll: (params?: { tournamentId?: number; teamId?: number }) =>
    client.get<Application[]>('/applications', { params }).then(r => r.data),
  getById: (id: number) => client.get<Application>(`/applications/${id}`).then(r => r.data),
  create: (data: Partial<Application>) => client.post<Application>('/applications', data).then(r => r.data),
  update: (id: number, data: Partial<Application>) => client.put<Application>(`/applications/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/applications/${id}`),
  addPlayer: (id: number, data: { playerId: number; shirtNumber: number; isLibero: boolean }) =>
    client.post<Application>(`/applications/${id}/players`, data).then(r => r.data),
  removePlayer: (id: number, playerId: number) =>
    client.delete(`/applications/${id}/players/${playerId}`),
};

// назначения судей
export const refereeAssignmentsApi = {
  getAll: (matchId?: number) =>
    client.get<RefereeAssignment[]>('/referee-assignments', { params: { matchId } }).then(r => r.data),
  create: (data: Partial<RefereeAssignment>) => client.post<RefereeAssignment>('/referee-assignments', data).then(r => r.data),
  update: (id: number, data: Partial<RefereeAssignment>) => client.put<RefereeAssignment>(`/referee-assignments/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/referee-assignments/${id}`),
};

// протоколы
export const protocolsApi = {
  getAll: (matchId?: number) =>
    client.get<Protocol[]>('/protocols', { params: { matchId } }).then(r => r.data),
  getById: (id: number) => client.get<Protocol>(`/protocols/${id}`).then(r => r.data),
  getByMatch: (matchId: number) =>
    client.get<Protocol[]>('/protocols', { params: { matchId } }).then(r => r.data[0] ?? null),
  create: (data: Partial<Protocol>) => client.post<Protocol>('/protocols', data).then(r => r.data),
  update: (id: number, data: Partial<Protocol>) => client.put<Protocol>(`/protocols/${id}`, data).then(r => r.data),
};

// партии
export const setsApi = {
  getAll: (matchId: number) =>
    client.get<SetDto[]>(`/matches/${matchId}/sets`).then(r => r.data),
  upsert: (matchId: number, data: Partial<SetDto>) =>
    client.put<SetDto>(`/matches/${matchId}/sets`, data).then(r => r.data),
  delete: (matchId: number, setNumber: number) =>
    client.delete(`/matches/${matchId}/sets/${setNumber}`),
};

// статистика игроков
export const playerStatsApi = {
  getAll: (matchId: number) =>
    client.get<PlayerStats[]>(`/matches/${matchId}/player-stats`).then(r => r.data),
  upsert: (matchId: number, data: Partial<PlayerStats>) =>
    client.put<PlayerStats>(`/matches/${matchId}/player-stats`, data).then(r => r.data),
};

// расстановки
export const lineupsApi = {
  getAll: (matchId: number, params?: { teamId?: number; setNumber?: number }) =>
    client.get<StartingLineup[]>(`/matches/${matchId}/lineups`, { params }).then(r => r.data),
  upsert: (matchId: number, data: Partial<StartingLineup>) =>
    client.put<StartingLineup>(`/matches/${matchId}/lineups`, data).then(r => r.data),
  delete: (matchId: number, teamId: number, setNumber: number, positionNo: number) =>
    client.delete(`/matches/${matchId}/lineups/${teamId}/${setNumber}/${positionNo}`),
};

// события матча
export const eventsApi = {
  getAll: (matchId: number) =>
    client.get<MatchEvent[]>(`/matches/${matchId}/events`).then(r => r.data),
  create: (matchId: number, data: Partial<MatchEvent> & { substitution?: unknown; timeout?: unknown }) =>
    client.post<MatchEvent>(`/matches/${matchId}/events`, data).then(r => r.data),
  delete: (matchId: number, eventId: number) =>
    client.delete(`/matches/${matchId}/events/${eventId}`),
};

// санкции
export const sanctionsApi = {
  getAll: (matchId: number) =>
    client.get<Sanction[]>(`/matches/${matchId}/sanctions`).then(r => r.data),
  create: (matchId: number, data: Partial<Sanction>) =>
    client.post<Sanction>(`/matches/${matchId}/sanctions`, data).then(r => r.data),
  update: (matchId: number, id: number, data: Partial<Sanction>) =>
    client.put<Sanction>(`/matches/${matchId}/sanctions/${id}`, data).then(r => r.data),
  delete: (matchId: number, id: number) =>
    client.delete(`/matches/${matchId}/sanctions/${id}`),
};

// награды
export const awardsApi = {
  getAll: (tournamentId?: number) =>
    client.get<Award[]>('/awards', { params: { tournamentId } }).then(r => r.data),
  create: (data: Partial<Award>) => client.post<Award>('/awards', data).then(r => r.data),
  update: (id: number, data: Partial<Award>) => client.put<Award>(`/awards/${id}`, data).then(r => r.data),
  delete: (id: number) => client.delete(`/awards/${id}`),
};

// капитаны
export const captainsApi = {
  getAll: (matchId: number) =>
    client.get<MatchCaptain[]>(`/matches/${matchId}/captains`).then(r => r.data),
  upsert: (matchId: number, data: { teamId: number; playerId: number }) =>
    client.put<MatchCaptain>(`/matches/${matchId}/captains`, data).then(r => r.data),
  delete: (matchId: number, teamId: number) =>
    client.delete(`/matches/${matchId}/captains/${teamId}`),
};

// турнирная таблица
export const standingsApi = {
  get: (tournamentId: number, stageCode?: number, groupId?: number) =>
    client.get<StandingRow[]>('/standings', { params: { tournamentId, stageCode, groupId } }).then(r => r.data),
};

// делегации
export const delegationsApi = {
  getAll: (matchId: number) =>
    client.get<Delegation[]>(`/matches/${matchId}/delegations`).then(r => r.data),
  create: (matchId: number, data: Partial<Delegation>) =>
    client.post<Delegation>(`/matches/${matchId}/delegations`, data).then(r => r.data),
  update: (matchId: number, id: number, data: Partial<Delegation>) =>
    client.put<Delegation>(`/matches/${matchId}/delegations/${id}`, data).then(r => r.data),
  delete: (matchId: number, id: number) =>
    client.delete(`/matches/${matchId}/delegations/${id}`),
};
