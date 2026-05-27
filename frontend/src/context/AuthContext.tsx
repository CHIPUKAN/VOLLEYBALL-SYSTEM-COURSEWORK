import React, { createContext, useContext, useState, useCallback, useEffect } from 'react';
import type { UserDto, AuthResponse } from '../types/index';

// матрица прав доступа по ролям (синхронизирована с [Authorize] атрибутами на бэкенде)
const PERMISSIONS: Record<string, string[]> = {
  manageTeams:            ['Суперадминистратор'],
  managePlayers:          ['Суперадминистратор', 'ТренерКоманды'],
  manageCoaches:          ['Суперадминистратор'],
  manageReferees:         ['Суперадминистратор'],
  manageOrganizers:       ['Суперадминистратор'],
  manageVenues:           ['Суперадминистратор'],
  manageRegions:          ['Суперадминистратор'],
  manageSeasons:          ['Суперадминистратор', 'Организатор'],
  manageTournaments:      ['Суперадминистратор', 'Организатор'],
  manageMatches:          ['Суперадминистратор', 'Организатор'],
  manageGroups:           ['Суперадминистратор', 'Организатор'],
  createProtocol:         ['Суперадминистратор', 'СекретарьМатча', 'Организатор'],
  updateProtocol:         ['Суперадминистратор', 'СекретарьМатча', 'Организатор'],
  manageSanctions:        ['Суперадминистратор', 'СекретарьМатча'],
  manageEvents:           ['Суперадминистратор', 'СекретарьМатча'],
  manageSets:             ['Суперадминистратор', 'СекретарьМатча'],
  manageLineup:           ['Суперадминистратор', 'СекретарьМатча', 'ТренерКоманды'],
  editStats:              ['Суперадминистратор', 'СекретарьМатча'],
  manageRefereeAssignment:['Суперадминистратор', 'Организатор'],
  manageDelegation:       ['Суперадминистратор', 'ТренерКоманды', 'ПредставительКоманды'],
  manageCaptain:          ['Суперадминистратор', 'СекретарьМатча', 'ТренерКоманды'],
  manageApplications:     ['Суперадминистратор', 'ТренерКоманды', 'ПредставительКоманды'],
  reviewApplications:     ['Суперадминистратор', 'Организатор'],
  manageAwards:           ['Суперадминистратор', 'Организатор'],
};

// проверяет, не истёк ли JWT-токен по полю exp в payload
const isTokenValid = (t: string | null): boolean => {
  if (!t) return false;
  try {
    const parts = t.split('.');
    if (parts.length !== 3) return false;
    // base64url → base64 + восстановление паддинга (atob требует кратность 4)
    const base64 = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const padded = base64.padEnd(base64.length + (4 - (base64.length % 4)) % 4, '=');
    const payload = JSON.parse(atob(padded)) as Record<string, unknown>;
    const expSec = payload.exp;
    if (typeof expSec !== 'number') return false;
    return expSec * 1000 > Date.now();
  } catch {
    return false;
  }
};

// типы контекста
interface AuthContextValue {
  user: UserDto | null;
  token: string | null;
  role: string | null;
  login: (response: AuthResponse) => void;
  logout: () => void;
  isAuthenticated: boolean;
  isSuperAdmin: boolean;
  can: (action: string) => boolean;
}

// создание контекста
const AuthContext = createContext<AuthContextValue | undefined>(undefined);

// провайдер аутентификации
export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<UserDto | null>(() => {
    try {
      const stored = localStorage.getItem('user');
      return stored ? (JSON.parse(stored) as UserDto) : null;
    } catch {
      return null;
    }
  });

  const [token, setToken] = useState<string | null>(() => {
    const stored = localStorage.getItem('token');
    if (stored && !isTokenValid(stored)) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      return null;
    }
    return stored;
  });

  // очистка данных авторизации
  const logout = useCallback(() => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  }, []);

  // периодическая проверка срока действия токена (раз в минуту)
  useEffect(() => {
    const interval = setInterval(() => {
      if (token && !isTokenValid(token)) {
        logout();
        window.location.href = '/login';
      }
    }, 60_000);
    return () => clearInterval(interval);
  }, [token, logout]);

  // синхронизация состояния при изменении localStorage из других вкладок
  useEffect(() => {
    const handleStorage = (e: StorageEvent) => {
      if (e.key === 'token') {
        const newToken = e.newValue;
        if (!newToken || !isTokenValid(newToken)) {
          setToken(null);
        } else {
          setToken(newToken);
        }
      }
      if (e.key === 'user') {
        try {
          setUser(e.newValue ? (JSON.parse(e.newValue) as UserDto) : null);
        } catch {
          setUser(null);
        }
      }
    };
    window.addEventListener('storage', handleStorage);
    return () => window.removeEventListener('storage', handleStorage);
  }, []);

  // сохранение данных авторизации
  const login = useCallback((response: AuthResponse) => {
    localStorage.setItem('token', response.token);
    localStorage.setItem('user', JSON.stringify(response.user));
    setToken(response.token);
    setUser(response.user);
  }, []);

  const role = user?.role ?? null;
  const isAuthenticated = token !== null && user !== null && isTokenValid(token);
  const isSuperAdmin = role === 'Суперадминистратор';

  // проверка права доступа по действию
  const can = useCallback((action: string): boolean => {
    if (!role) return false;
    if (role === 'Суперадминистратор') return true;
    return (PERMISSIONS[action] ?? []).includes(role);
  }, [role]);

  return (
    <AuthContext.Provider value={{ user, token, role, login, logout, isAuthenticated, isSuperAdmin, can }}>
      {children}
    </AuthContext.Provider>
  );
};

// хук для использования контекста
export const useAuth = (): AuthContextValue => {
  const ctx = useContext(AuthContext);
  if (ctx === undefined) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return ctx;
};
