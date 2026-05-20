import type { Organizer, CreateOrganizerRequest, UpdateOrganizerRequest } from '../types/organizer';

const BASE_URL = '/api/organizers';

// получить всех организаторов
export async function fetchOrganizers(): Promise<Organizer[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке организаторов');
  return response.json() as Promise<Organizer[]>;
}

// получить организатора по id
export async function fetchOrganizerById(id: number): Promise<Organizer> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Организатор с id=${id} не найден`);
  return response.json() as Promise<Organizer>;
}

// создать организатора
export async function createOrganizer(data: CreateOrganizerRequest): Promise<Organizer> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании организатора');
  }
  return response.json() as Promise<Organizer>;
}

// обновить организатора
export async function updateOrganizer(id: number, data: UpdateOrganizerRequest): Promise<Organizer> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении организатора');
  }
  return response.json() as Promise<Organizer>;
}

// удалить организатора
export async function deleteOrganizer(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении организатора id=${id}`);
}
