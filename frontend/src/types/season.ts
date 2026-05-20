export interface Season {
  id: number;
  name: string;
  startDate: string;
  endDate: string;
  status: string;
}

export interface CreateSeasonRequest {
  name: string;
  startDate: string;
  endDate: string;
  status: string;
}

export interface UpdateSeasonRequest {
  name: string;
  startDate: string;
  endDate: string;
  status: string;
}
