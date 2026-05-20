import type { Tournament, CreateTournamentRequest, UpdateTournamentRequest } from '../types/tournament';

const BASE_URL = '/api/tournaments';

// получить все турниры
export async function fetchTournaments(): Promise<Tournament[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке турниров');
  return response.json() as Promise<Tournament[]>;
}

// получить турнир по id
export async function fetchTournamentById(id: number): Promise<Tournament> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Турнир с id=${id} не найден`);
  return response.json() as Promise<Tournament>;
}

// создать турнир
export async function createTournament(data: CreateTournamentRequest): Promise<Tournament> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании турнира');
  }
  return response.json() as Promise<Tournament>;
}

// обновить турнир
export async function updateTournament(id: number, data: UpdateTournamentRequest): Promise<Tournament> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении турнира');
  }
  return response.json() as Promise<Tournament>;
}

// удалить турнир
export async function deleteTournament(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении турнира id=${id}`);
}
