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
  endTime?: string;
  venueId: number;
  venueName?: string;
  venueCity?: string;
  stageCode: number;
  stageName?: string;
  groupId?: number;
  groupName?: string;
  statusCode: number;
  statusName?: string;
  techDefeatReason?: string;
  coinTossWinnerTeamId?: number;
  coinTossChoiceCode?: number;
  coinTossChoiceName?: string;
  firstServeTeamId?: number;
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
  statusCode?: number;
  groupId?: number;
  hasVideoChallenge?: boolean;
  netHeight?: number;
}

export interface UpdateMatchRequest {
  tournamentId: number;
  homeTeamId: number;
  guestTeamId: number;
  matchDate: string;
  startTime: string;
  venueId: number;
  stageCode: number;
  statusCode?: number;
  groupId?: number;
  hasVideoChallenge?: boolean;
  netHeight?: number;
}
