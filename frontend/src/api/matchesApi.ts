import type { Match, CreateMatchRequest, UpdateMatchRequest } from '../types/match';

const BASE_URL = '/api/matches';

// получить все матчи
export async function fetchMatches(tournamentId?: number): Promise<Match[]> {
  const url = tournamentId ? `${BASE_URL}?tournamentId=${tournamentId}` : BASE_URL;
  const response = await fetch(url);
  if (!response.ok) throw new Error('Ошибка при загрузке матчей');
  return response.json() as Promise<Match[]>;
}

// получить матч по id
export async function fetchMatchById(id: number): Promise<Match> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Матч с id=${id} не найден`);
  return response.json() as Promise<Match>;
}

// создать матч
export async function createMatch(data: CreateMatchRequest): Promise<Match> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании матча');
  }
  return response.json() as Promise<Match>;
}

// обновить матч
export async function updateMatch(id: number, data: UpdateMatchRequest): Promise<Match> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении матча');
  }
  return response.json() as Promise<Match>;
}

// удалить матч
export async function deleteMatch(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении матча id=${id}`);
}
