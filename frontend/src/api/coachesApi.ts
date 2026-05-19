import type { Coach, CreateCoachRequest, UpdateCoachRequest } from '../types/coach';

const BASE = '/api/coaches';

export async function fetchCoaches(): Promise<Coach[]> {
  const r = await fetch(BASE);
  if (!r.ok) throw new Error('Ошибка при загрузке тренеров');
  return r.json() as Promise<Coach[]>;
}

export async function createCoach(data: CreateCoachRequest): Promise<Coach> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании тренера');
  }
  return r.json() as Promise<Coach>;
}

export async function updateCoach(id: number, data: UpdateCoachRequest): Promise<Coach> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении тренера');
  }
  return r.json() as Promise<Coach>;
}

export async function deleteCoach(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении тренера id=${id}`);
}
