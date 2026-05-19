import type { Organizer, CreateOrganizerRequest, UpdateOrganizerRequest } from '../types/organizer';

const BASE = '/api/organizers';

export async function fetchOrganizers(): Promise<Organizer[]> {
  const r = await fetch(BASE);
  if (!r.ok) throw new Error('Ошибка при загрузке организаторов');
  return r.json() as Promise<Organizer[]>;
}

export async function createOrganizer(data: CreateOrganizerRequest): Promise<Organizer> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании организатора');
  }
  return r.json() as Promise<Organizer>;
}

export async function updateOrganizer(id: number, data: UpdateOrganizerRequest): Promise<Organizer> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении организатора');
  }
  return r.json() as Promise<Organizer>;
}

export async function deleteOrganizer(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении организатора id=${id}`);
}
