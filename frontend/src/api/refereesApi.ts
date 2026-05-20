import type { Referee, CreateRefereeRequest, UpdateRefereeRequest } from '../types/referee';

const BASE_URL = '/api/referees';

// получить всех судей
export async function fetchReferees(): Promise<Referee[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке судей');
  return response.json() as Promise<Referee[]>;
}

// получить судью по id
export async function fetchRefereeById(id: number): Promise<Referee> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Судья с id=${id} не найден`);
  return response.json() as Promise<Referee>;
}

// создать судью
export async function createReferee(data: CreateRefereeRequest): Promise<Referee> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании судьи');
  }
  return response.json() as Promise<Referee>;
}

// обновить судью
export async function updateReferee(id: number, data: UpdateRefereeRequest): Promise<Referee> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении судьи');
  }
  return response.json() as Promise<Referee>;
}

// удалить судью
export async function deleteReferee(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении судьи id=${id}`);
}
