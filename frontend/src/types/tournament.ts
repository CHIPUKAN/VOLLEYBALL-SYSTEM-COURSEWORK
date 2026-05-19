export interface Tournament {
  id: number;
  seasonId?: number;
  seasonName?: string;
  organizerId?: number;
  organizerFullName?: string;
  name: string;
  startDate: string;
  endDate: string;
  applicationDeadline?: string;
  city: string;
  description?: string;
  maxTeams?: number;
  gender: string;
  ageCategory?: string;
  level: string;
  maxPlayersPerApp: number;
  formatCode: number;
  formatName?: string;
  scoringSystemCode: number;
  scoringSystemName?: string;
}

export interface CreateTournamentRequest {
  seasonId?: number;
  organizerId?: number;
  name: string;
  startDate: string;
  endDate: string;
  applicationDeadline?: string;
  city: string;
  description?: string;
  maxTeams?: number;
  gender: string;
  ageCategory?: string;
  level: string;
  maxPlayersPerApp: number;
  formatCode: number;
  scoringSystemCode: number;
}

export type UpdateTournamentRequest = CreateTournamentRequest;
