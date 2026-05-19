import type { Season, CreateSeasonRequest, UpdateSeasonRequest } from '../types/season';

const BASE = '/api/seasons';

export async function fetchSeasons(): Promise<Season[]> {
  const r = await fetch(BASE);
  if (!r.ok) throw new Error('Ошибка при загрузке сезонов');
  return r.json() as Promise<Season[]>;
}

export async function createSeason(data: CreateSeasonRequest): Promise<Season> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании сезона');
  }
  return r.json() as Promise<Season>;
}

export async function updateSeason(id: number, data: UpdateSeasonRequest): Promise<Season> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении сезона');
  }
  return r.json() as Promise<Season>;
}

export async function deleteSeason(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении сезона id=${id}`);
}
