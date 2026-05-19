export interface Match {
  id: number;
  tournamentId: number;
  tournamentName?: string;
  homeTeamId: number;
  homeTeamName?: string;
  guestTeamId: number;
  guestTeamName?: string;
  matchDate: string;
  startTime: string;
  venueId: number;
  venueName?: string;
  venueCity?: string;
  stageCode: number;
  stageName?: string;
  groupId?: number;
  groupName?: string;
  statusCode: number;
  statusName?: string;
  hasVideoChallenge: boolean;
  netHeight?: number;
}

export interface CreateMatchRequest {
  tournamentId: number;
  homeTeamId: number;
  guestTeamId: number;
  matchDate: string;
  startTime: string;
  venueId: number;
  stageCode: number;
  groupId?: number;
  statusCode: number;
  hasVideoChallenge: boolean;
  netHeight?: number;
}

export type UpdateMatchRequest = CreateMatchRequest;
