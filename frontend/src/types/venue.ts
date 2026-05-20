export interface Venue {
  id: number;
  name: string;
  city: string;
  address?: string;
  capacity?: number;
}

export interface CreateVenueRequest {
  name: string;
  city: string;
  address?: string;
  capacity?: number;
}

export interface UpdateVenueRequest {
  name: string;
  city: string;
  address?: string;
  capacity?: number;
}
