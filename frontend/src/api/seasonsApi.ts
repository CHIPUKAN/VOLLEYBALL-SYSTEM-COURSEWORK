import type { Season, CreateSeasonRequest, UpdateSeasonRequest } from '../types/season';

const BASE_URL = '/api/seasons';

// получить все сезоны
export async function fetchSeasons(): Promise<Season[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке сезонов');
  return response.json() as Promise<Season[]>;
}

// получить сезон по id
export async function fetchSeasonById(id: number): Promise<Season> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Сезон с id=${id} не найден`);
  return response.json() as Promise<Season>;
}

// создать сезон
export async function createSeason(data: CreateSeasonRequest): Promise<Season> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании сезона');
  }
  return response.json() as Promise<Season>;
}

// обновить сезон
export async function updateSeason(id: number, data: UpdateSeasonRequest): Promise<Season> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении сезона');
  }
  return response.json() as Promise<Season>;
}

// удалить сезон
export async function deleteSeason(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении сезона id=${id}`);
}
