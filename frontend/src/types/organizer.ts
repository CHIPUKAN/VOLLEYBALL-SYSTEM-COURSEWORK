export interface Organizer {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  fullName?: string;
  email?: string;
  phone?: string;
}

export interface CreateOrganizerRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  email?: string;
  phone?: string;
}

export interface UpdateOrganizerRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  email?: string;
  phone?: string;
}
