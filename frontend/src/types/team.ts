// тип данных команды (ответ от API)
export interface Team {
  id: number;
  name: string;
  logoUrl?: string;
  regionOktmo: string;
  regionName?: string;
  headCoachId: number;
  headCoachFullName?: string;
  homeVenueId: number;
  homeVenueName?: string;
  homeVenueCity?: string;
}

// тип для создания новой команды
export interface CreateTeamRequest {
  name: string;
  logoUrl?: string;
  regionOktmo: string;
  headCoachId: number;
  homeVenueId: number;
}

// тип для обновления команды
export interface UpdateTeamRequest {
  name: string;
  logoUrl?: string;
  regionOktmo: string;
  headCoachId: number;
  homeVenueId: number;
}
