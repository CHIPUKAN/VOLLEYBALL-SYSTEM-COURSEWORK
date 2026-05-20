export interface Coach {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  category?: string;
  email?: string;
  phone?: string;
}

export interface CreateCoachRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  category?: string;
  email?: string;
  phone?: string;
}

export interface UpdateCoachRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  category?: string;
  email?: string;
  phone?: string;
}
