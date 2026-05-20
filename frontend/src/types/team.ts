export interface Team {
  id: number;
  name: string;
  logoUrl?: string;
  regionOktmo: string;
  regionName?: string;
  headCoachId?: number;
  headCoachFullName?: string;
  homeVenueId?: number;
  homeVenueName?: string;
  homeVenueCity?: string;
}

export interface CreateTeamRequest {
  name: string;
  regionOktmo: string;
  logoUrl?: string;
  headCoachId?: number;
  homeVenueId?: number;
}

export interface UpdateTeamRequest {
  name: string;
  regionOktmo: string;
  logoUrl?: string;
  headCoachId?: number;
  homeVenueId?: number;
}
