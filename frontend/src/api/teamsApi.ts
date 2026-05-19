import type { Team, CreateTeamRequest, UpdateTeamRequest } from '../types/team';

const BASE_URL = '/api/teams';

// Получить все команды
export async function fetchTeams(): Promise<Team[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке команд');
  return response.json() as Promise<Team[]>;
}

// Получить команду по id
export async function fetchTeamById(id: number): Promise<Team> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Команда с id=${id} не найдена`);
  return response.json() as Promise<Team>;
}

// создать команду
export async function createTeam(data: CreateTeamRequest): Promise<Team> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании команды');
  }
  return response.json() as Promise<Team>;
}

// обновить команду
export async function updateTeam(id: number, data: UpdateTeamRequest): Promise<Team> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении команды');
  }
  return response.json() as Promise<Team>;
}

// удалить команду
export async function deleteTeam(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении команды id=${id}`);
}
