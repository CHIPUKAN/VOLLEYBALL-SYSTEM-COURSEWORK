export interface Referee {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  category?: string;
  licenseNumber?: string;
  email?: string;
  phone?: string;
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

export interface UpdateRefereeRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  category?: string;
  licenseNumber?: string;
  email?: string;
  phone?: string;
}
