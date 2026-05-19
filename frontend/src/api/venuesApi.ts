import type { Venue, CreateVenueRequest, UpdateVenueRequest } from '../types/venue';

const BASE = '/api/venues';

export async function fetchVenues(): Promise<Venue[]> {
  const r = await fetch(BASE);
  if (!r.ok) throw new Error('Ошибка при загрузке площадок');
  return r.json() as Promise<Venue[]>;
}

export async function createVenue(data: CreateVenueRequest): Promise<Venue> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании площадки');
  }
  return r.json() as Promise<Venue>;
}

export async function updateVenue(id: number, data: UpdateVenueRequest): Promise<Venue> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении площадки');
  }
  return r.json() as Promise<Venue>;
}

export async function deleteVenue(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении площадки id=${id}`);
}
