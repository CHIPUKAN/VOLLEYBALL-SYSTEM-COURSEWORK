export interface Player {
  id: number;
  teamId?: number;
  teamName?: string;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  birthDate?: string;
  heightCm?: number;
  weightKg?: number;
  jerseyNumber?: number;
  ampluaCode: number;
  ampluaName?: string;
  sportsRank?: string;
  email?: string;
  phone?: string;
  statusCode: number;
  statusName?: string;
  photoUrl?: string;
}

export interface CreatePlayerRequest {
  teamId?: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  birthDate?: string;
  heightCm?: number;
  weightKg?: number;
  jerseyNumber?: number;
  ampluaCode: number;
  sportsRank?: string;
  email?: string;
  phone?: string;
  statusCode: number;
}

export interface UpdatePlayerRequest {
  teamId?: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  birthDate?: string;
  heightCm?: number;
  weightKg?: number;
  jerseyNumber?: number;
  ampluaCode: number;
  sportsRank?: string;
  email?: string;
  phone?: string;
  statusCode: number;
}
