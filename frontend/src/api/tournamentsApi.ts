import type { Tournament, CreateTournamentRequest, UpdateTournamentRequest } from '../types/tournament';

const BASE = '/api/tournaments';

export async function fetchTournaments(): Promise<Tournament[]> {
  const r = await fetch(BASE);
  if (!r.ok) throw new Error('Ошибка при загрузке турниров');
  return r.json() as Promise<Tournament[]>;
}

export async function createTournament(data: CreateTournamentRequest): Promise<Tournament> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании турнира');
  }
  return r.json() as Promise<Tournament>;
}

export async function updateTournament(id: number, data: UpdateTournamentRequest): Promise<Tournament> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении турнира');
  }
  return r.json() as Promise<Tournament>;
}

export async function deleteTournament(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении турнира id=${id}`);
}
