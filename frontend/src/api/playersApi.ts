import type { Player, CreatePlayerRequest, UpdatePlayerRequest } from '../types/player';

const BASE = '/api/players';

export async function fetchPlayers(teamId?: number): Promise<Player[]> {
  const url = teamId ? `${BASE}?teamId=${teamId}` : BASE;
  const r = await fetch(url);
  if (!r.ok) throw new Error('Ошибка при загрузке игроков');
  return r.json() as Promise<Player[]>;
}

export async function createPlayer(data: CreatePlayerRequest): Promise<Player> {
  const r = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при создании игрока');
  }
  return r.json() as Promise<Player>;
}

export async function updatePlayer(id: number, data: UpdatePlayerRequest): Promise<Player> {
  const r = await fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!r.ok) {
    const err = await r.json() as { message?: string };
    throw new Error(err.message ?? 'Ошибка при обновлении игрока');
  }
  return r.json() as Promise<Player>;
}

export async function deletePlayer(id: number): Promise<void> {
  const r = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
  if (!r.ok) throw new Error(`Ошибка при удалении игрока id=${id}`);
}
