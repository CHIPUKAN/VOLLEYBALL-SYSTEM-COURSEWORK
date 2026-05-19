
// аутентификация
export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  role: string;
  fullName?: string;
  linkedCoachId?: number;
  linkedPlayerId?: number;
  linkedRefereeId?: number;
  linkedOrganizerId?: number;
}

export interface UserDto {
  id: number;
  email: string;
  role: string;
  fullName?: string;
  linkedCoachId?: number;
  linkedPlayerId?: number;
  linkedRefereeId?: number;
  linkedOrganizerId?: number;
  createdAt: string;
}

export interface AuthResponse {
  token: string;
  user: UserDto;
}

// справочники (lookups)
export interface LookupDto {
  code: number;
  name: string;
}

export interface LookupItemDto {
  id: string;
  name: string;
}

// регионы
export interface Region {
  id: string;
  name: string;
}

// команды
export interface Team {
  id: number;
  name: string;
  logoUrl?: string;
  regionOktmo: string;
  regionName?: string;
  headCoachId?: number;
  headCoachName?: string;
  homeVenueId?: number;
  homeVenueName?: string;
  foundedYear?: number;
  contactEmail?: string;
  contactPhone?: string;
}

// площадки
export interface Venue {
  id: number;
  name: string;
  city: string;
  address?: string;
  capacity?: number;
  regionOktmo?: string;
  regionName?: string;
  contactEmail?: string;
  contactPhone?: string;
}

// тренеры
export interface Coach {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  birthDate?: string;
  category?: string;
  licenseNumber?: string;
  licenseExpiry?: string;
  email?: string;
  phone?: string;
}

// игроки
export interface Player {
  id: number;
  teamId: number;
  teamName?: string;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  birthDate?: string;
  ampluaCode: number;
  ampluaName?: string;
  statusCode: number;
  statusName?: string;
  shirtNumber?: number;
  height?: number;
  weight?: number;
  licenseNumber?: string;
  licenseExpiry?: string;
}

// сезоны
export interface Season {
  id: number;
  name: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
}

// организаторы
export interface Organizer {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  email?: string;
  phone?: string;
  organization?: string;
}

// судьи
export interface Referee {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  category?: string;
  licenseNumber?: string;
  licenseExpiry?: string;
  email?: string;
  phone?: string;
}

// турниры
export interface Tournament {
  id: number;
  seasonId: number;
  seasonName?: string;
  organizerId: number;
  organizerName?: string;
  name: string;
  startDate: string;
  endDate: string;
  city?: string;
  venueId?: number;
  venueName?: string;
  formatCode: number;
  formatName?: string;
  scoringSystemCode: number;
  scoringSystemName?: string;
  maxTeams?: number;
  description?: string;
}

// группы
export interface Group {
  id: number;
  tournamentId: number;
  tournamentName?: string;
  name: string;
  description?: string;
}

// матчи
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

// заявки
export interface Application {
  id: number;
  tournamentId: number;
  tournamentName?: string;
  teamId: number;
  teamName?: string;
  statusCode: number;
  statusName?: string;
  submittedAt?: string;
  reviewedAt?: string;
  comment?: string;
  players: ApplicationPlayer[];
}

export interface ApplicationPlayer {
  playerId: number;
  playerName?: string;
  shirtNumber?: number;
  ampluaCode?: number;
  ampluaName?: string;
  isLibero: boolean;
}

// назначения судей
export interface RefereeAssignment {
  id: number;
  matchId: number;
  matchDescription?: string;
  refereeId: number;
  refereeName?: string;
  roleCode: number;
  roleName?: string;
}

// протоколы
export interface Protocol {
  id: number;
  matchId: number;
  matchDescription?: string;
  statusCode: number;
  statusName?: string;
  homeScore: number;
  guestScore: number;
  createdAt: string;
  approvedAt?: string;
  approvedByUserId?: number;
}

// партии
export interface SetDto {
  matchId: number;
  setNumber: number;
  homeScore: number;
  guestScore: number;
  durationMin?: number;
}

// статистика игроков
export interface PlayerStats {
  matchId: number;
  playerId: number;
  playerName?: string;
  teamId: number;
  teamName?: string;
  servesTotal: number;
  aces: number;
  serveErrors: number;
  receptionsTotal: number;
  positiveReceptions: number;
  receptionErrors: number;
  attacksTotal: number;
  attackPoints: number;
  attackErrors: number;
  blocks: number;
  totalPoints: number;
}

// расстановки
export interface StartingLineup {
  matchId: number;
  teamId: number;
  setNumber: number;
  positionNo: number;
  playerId: number;
  playerName?: string;
  shirtNumber?: number;
}

// события матча
export interface MatchEvent {
  id: number;
  matchId: number;
  setNumber: number;
  eventTypeCode: number;
  eventTypeName?: string;
  teamId?: number;
  teamName?: string;
  playerId?: number;
  playerName?: string;
  homeScoreAtMoment: number;
  guestScoreAtMoment: number;
  minuteMark?: number;
  seqInMatch: number;
  isTeamEvent: boolean;
  substitution?: SubstitutionDetail;
  timeout?: TimeoutDetail;
}

export interface SubstitutionDetail {
  subOutPlayerId: number;
  subOutPlayerName?: string;
  subInPlayerId: number;
  subInPlayerName?: string;
  subTypeCode: number;
  subTypeName?: string;
  isLiberoSwap: boolean;
}

export interface TimeoutDetail {
  timeoutTypeCode: number;
  timeoutTypeName?: string;
}

// санкции
export interface Sanction {
  id: number;
  matchId: number;
  teamId: number;
  teamName?: string;
  playerId?: number;
  playerName?: string;
  recipientTypeCode: number;
  recipientTypeName?: string;
  sanctionTypeCode: number;
  sanctionTypeName?: string;
  sanctionKindCode: number;
  sanctionKindName?: string;
  setNumber?: number;
  minuteMark?: number;
}

// награды
export interface Award {
  id: number;
  tournamentId: number;
  tournamentName?: string;
  teamId?: number;
  teamName?: string;
  playerId?: number;
  playerName?: string;
  awardTypeCode: number;
  awardTypeName?: string;
  title: string;
  description?: string;
}

// капитаны
export interface MatchCaptain {
  matchId: number;
  teamId: number;
  teamName?: string;
  playerId: number;
  playerName?: string;
}

// делегации
export interface Delegation {
  id: number;
  matchId: number;
  teamId: number;
  teamName?: string;
  personName: string;
  role: string;
  isCoach: boolean;
}

// live-матч: состояние
export interface LiveSetState {
  setNumber: number;
  homeScore: number;
  guestScore: number;
  servingTeamId: number;
  isFinished: boolean;
}

export interface LiveMatchState {
  currentSetNumber: number;
  sets: LiveSetState[];
  homeMatchScore: number;
  guestMatchScore: number;
}

// для ActionPicker
export interface ActionCategory {
  code: string;
  label: string;
}

export interface ActionResult {
  code: string;
  label: string;
  icon: string;
  scoringEffect: 'acting' | 'opposing' | 'neutral';
  eventTypeName: string;
}

// жеребьёвка
export interface CoinTossResult {
  servingTeamId: number;
  homeTeamSide: 'top' | 'bottom';
}
