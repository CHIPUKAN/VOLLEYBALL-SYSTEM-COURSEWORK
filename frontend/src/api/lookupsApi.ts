import client from './client';
import type { LookupDto, LookupItemDto } from '../types';

export const lookupsApi = {
  getAmplua: () => client.get<LookupDto[]>('/lookups/amplua').then(r => r.data),
  getPlayerStatuses: () => client.get<LookupDto[]>('/lookups/player-statuses').then(r => r.data),
  getApplicationStatuses: () => client.get<LookupDto[]>('/lookups/application-statuses').then(r => r.data),
  getTournamentFormats: () => client.get<LookupDto[]>('/lookups/tournament-formats').then(r => r.data),
  getTournamentStages: () => client.get<LookupDto[]>('/lookups/tournament-stages').then(r => r.data),
  getMatchStatuses: () => client.get<LookupDto[]>('/lookups/match-statuses').then(r => r.data),
  getProtocolStatuses: () => client.get<LookupDto[]>('/lookups/protocol-statuses').then(r => r.data),
  getRefereeRoles: () => client.get<LookupDto[]>('/lookups/referee-roles').then(r => r.data),
  getEventTypes: () => client.get<LookupDto[]>('/lookups/event-types').then(r => r.data),
  getSubstitutionTypes: () => client.get<LookupDto[]>('/lookups/substitution-types').then(r => r.data),
  getTimeoutTypes: () => client.get<LookupDto[]>('/lookups/timeout-types').then(r => r.data),
  getSanctionTypes: () => client.get<LookupDto[]>('/lookups/sanction-types').then(r => r.data),
  getDelayViolations: () => client.get<LookupDto[]>('/lookups/delay-violations').then(r => r.data),
  getAwardTypes: () => client.get<LookupDto[]>('/lookups/award-types').then(r => r.data),
  getSanctionKinds: () => client.get<LookupDto[]>('/lookups/sanction-kinds').then(r => r.data),
  getRecipientTypes: () => client.get<LookupDto[]>('/lookups/recipient-types').then(r => r.data),
  getCoinTossOptions: () => client.get<LookupDto[]>('/lookups/coin-toss-options').then(r => r.data),
  getScoringSystems: () => client.get<LookupDto[]>('/lookups/scoring-systems').then(r => r.data),
  getRegions: () => client.get<LookupItemDto[]>('/lookups/regions').then(r => r.data),
  getCoaches: () => client.get<LookupItemDto[]>('/lookups/coaches').then(r => r.data),
  getVenues: () => client.get<LookupItemDto[]>('/lookups/venues').then(r => r.data),
  getTeams: () => client.get<LookupItemDto[]>('/lookups/teams').then(r => r.data),
  getSeasons: () => client.get<LookupItemDto[]>('/lookups/seasons').then(r => r.data),
  getOrganizers: () => client.get<LookupItemDto[]>('/lookups/organizers').then(r => r.data),
  getReferees: () => client.get<LookupItemDto[]>('/lookups/referees').then(r => r.data),
  getTournaments: () => client.get<LookupItemDto[]>('/lookups/tournaments').then(r => r.data),
  getPlayers: (teamId?: number) =>
    client.get<LookupItemDto[]>('/lookups/players', { params: { teamId } }).then(r => r.data),
  getGroups: (tournamentId?: number) =>
    client.get<LookupItemDto[]>('/lookups/groups', { params: { tournamentId } }).then(r => r.data),
  getMatches: (tournamentId?: number) =>
    client.get<LookupItemDto[]>('/lookups/matches', { params: { tournamentId } }).then(r => r.data),
};
