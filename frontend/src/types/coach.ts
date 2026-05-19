export interface Coach {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  email?: string;
  phone?: string;
  category?: string;
  fullName: string;
}

export interface CreateCoachRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  email?: string;
  phone?: string;
  category?: string;
}

export type UpdateCoachRequest = CreateCoachRequest;
