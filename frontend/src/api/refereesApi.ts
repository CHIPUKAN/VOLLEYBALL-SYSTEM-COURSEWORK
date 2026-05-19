import type { Referee, CreateRefereeRequest, UpdateRefereeRequest } from '../types/referee';

const BASE = '/api/referees';

export async function fetchReferees(): Promise<Referee[]> {
  const r = await fetch(BASE);
  if (!r.ok) throw new Error('Ошибка при загрузке судей');
  return r.json() as Promise<Referee[]>;
}

export async function createReferee(data: CreateRefereeRequest): Promise<Referee> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании судьи');
  }
  return r.json() as Promise<Referee>;
}

export async function updateReferee(id: number, data: UpdateRefereeRequest): Promise<Referee> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении судьи');
  }
  return r.json() as Promise<Referee>;
}

export async function deleteReferee(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении судьи id=${id}`);
}
