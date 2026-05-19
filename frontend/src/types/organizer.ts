export interface Organizer {
  id: number;
  lastName: string;
  firstName: string;
  middleName?: string;
  email?: string;
  phone?: string;
  fullName: string;
}

export interface CreateOrganizerRequest {
  lastName: string;
  firstName: string;
  middleName?: string;
  email?: string;
  phone?: string;
}

export type UpdateOrganizerRequest = CreateOrganizerRequest;
