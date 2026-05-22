import client from './client';
import type { AuthResponse, LoginDto, RegisterDto, UserDto } from '../types';

export const authApi = {
  login: (dto: LoginDto) =>
    client.post<AuthResponse>('/auth/login', dto).then(r => r.data),

  register: (dto: Omit<RegisterDto, 'role'> & { role?: string }) =>
    client.post<AuthResponse>('/auth/register', { ...dto, role: 'Зритель' }).then(r => r.data),

  getMe: () =>
    client.get<UserDto>('/auth/me').then(r => r.data),

  getUsers: () =>
    client.get<UserDto[]>('/users').then(r => r.data),

  createUser: (dto: RegisterDto) =>
    client.post<UserDto>('/users', dto).then(r => r.data),

  updateUser: (id: number, dto: RegisterDto) =>
    client.put<UserDto>(`/users/${id}`, dto).then(r => r.data),

  deleteUser: (id: number) =>
    client.delete(`/users/${id}`),
};
