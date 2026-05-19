import type { Match, CreateMatchRequest, UpdateMatchRequest } from '../types/match';

const BASE = '/api/matches';

export async function fetchMatches(tournamentId?: number): Promise<Match[]> {
  const url = tournamentId ? `${BASE}?tournamentId=${tournamentId}` : BASE;
  const r = await fetch(url);
  if (!r.ok) throw new Error('Ошибка при загрузке матчей');
  return r.json() as Promise<Match[]>;
}

export async function createMatch(data: CreateMatchRequest): Promise<Match> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании матча');
  }
  return r.json() as Promise<Match>;
}

export async function updateMatch(id: number, data: UpdateMatchRequest): Promise<Match> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении матча');
  }
  return r.json() as Promise<Match>;
}

export async function deleteMatch(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении матча id=${id}`);
}
