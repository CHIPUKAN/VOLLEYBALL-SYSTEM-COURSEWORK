export interface Referee {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  category?: string;
  licenseNumber?: string;
  email?: string;
  phone?: string;
  fullName: string;
}

export interface CreateRefereeRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  category?: string;
  licenseNumber?: string;
  email?: string;
  phone?: string;
}

export type UpdateRefereeRequest = CreateRefereeRequest;
