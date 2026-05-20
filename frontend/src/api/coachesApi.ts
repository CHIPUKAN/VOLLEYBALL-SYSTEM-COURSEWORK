import type { Coach, CreateCoachRequest, UpdateCoachRequest } from '../types/coach';

const BASE_URL = '/api/coaches';

// получить всех тренеров
export async function fetchCoaches(): Promise<Coach[]> {
  const response = await fetch(BASE_URL);
  if (!response.ok) throw new Error('Ошибка при загрузке тренеров');
  return response.json() as Promise<Coach[]>;
}

// получить тренера по id
export async function fetchCoachById(id: number): Promise<Coach> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Тренер с id=${id} не найден`);
  return response.json() as Promise<Coach>;
}

// создать тренера
export async function createCoach(data: CreateCoachRequest): Promise<Coach> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании тренера');
  }
  return response.json() as Promise<Coach>;
}

// обновить тренера
export async function updateCoach(id: number, data: UpdateCoachRequest): Promise<Coach> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении тренера');
  }
  return response.json() as Promise<Coach>;
}

// удалить тренера
export async function deleteCoach(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении тренера id=${id}`);
}
