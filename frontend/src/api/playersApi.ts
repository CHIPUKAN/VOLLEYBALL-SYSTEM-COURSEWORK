import type { Player, CreatePlayerRequest, UpdatePlayerRequest } from '../types/player';

const BASE_URL = '/api/players';

// получить всех игроков
export async function fetchPlayers(teamId?: number): Promise<Player[]> {
  const url = teamId ? `${BASE_URL}?teamId=${teamId}` : BASE_URL;
  const response = await fetch(url);
  if (!response.ok) throw new Error('Ошибка при загрузке игроков');
  return response.json() as Promise<Player[]>;
}

// получить игрока по id
export async function fetchPlayerById(id: number): Promise<Player> {
  const response = await fetch(`${BASE_URL}/${id}`);
  if (!response.ok) throw new Error(`Игрок с id=${id} не найден`);
  return response.json() as Promise<Player>;
}

// создать игрока
export async function createPlayer(data: CreatePlayerRequest): Promise<Player> {
  const response = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании игрока');
  }
  return response.json() as Promise<Player>;
}

// обновить игрока
export async function updatePlayer(id: number, data: UpdatePlayerRequest): Promise<Player> {
  const response = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!response.ok) {
    const err = await response.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении игрока');
  }
  return response.json() as Promise<Player>;
}

// удалить игрока
export async function deletePlayer(id: number): Promise<void> {
  const response = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  if (!response.ok) throw new Error(`Ошибка при удалении игрока id=${id}`);
}
