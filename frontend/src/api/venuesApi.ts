import type { Venue, CreateVenueRequest, UpdateVenueRequest } from '../types/venue';

const BASE_URL = '/api/venues';

// получить все площадки
export async function fetchVenues(): Promise<Venue[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке площадок');
  return response.json() as Promise<Venue[]>;
}

// получить площадку по id
export async function fetchVenueById(id: number): Promise<Venue> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Площадка с id=${id} не найдена`);
  return response.json() as Promise<Venue>;
}

// создать площадку
export async function createVenue(data: CreateVenueRequest): Promise<Venue> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании площадки');
  }
  return response.json() as Promise<Venue>;
}

// обновить площадку
export async function updateVenue(id: number, data: UpdateVenueRequest): Promise<Venue> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении площадки');
  }
  return response.json() as Promise<Venue>;
}

// удалить площадку
export async function deleteVenue(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении площадки id=${id}`);
}
