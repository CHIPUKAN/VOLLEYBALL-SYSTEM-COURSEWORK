import React, { createContext, useContext, useState, useCallback, useEffect } from 'react';
import type { UserDto, AuthResponse } from '../types/index';

// матрица прав доступа по ролям
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
  createProtocol:         ['Суперадминистратор', 'СекретарьМатча', 'СудьяМатча'],
  updateProtocol:         ['Суперадминистратор', 'СекретарьМатча', 'СудьяМатча'],
  manageSanctions:        ['Суперадминистратор', 'СекретарьМатча', 'СудьяМатча'],
  manageEvents:           ['Суперадминистратор', 'СекретарьМатча', 'СудьяМатча'],
  manageSets:             ['Суперадминистратор', 'СекретарьМатча', 'СудьяМатча'],
  manageLineup:           ['Суперадминистратор', 'ТренерКоманды', 'СекретарьМатча', 'СудьяМатча'],
  editStats:              ['Суперадминистратор', 'СекретарьМатча', 'СудьяМатча'],
  manageRefereeAssignment:['Суперадминистратор', 'Организатор'],
  manageDelegation:       ['Суперадминистратор', 'ТренерКоманды', 'ПредставительКоманды'],
  manageCaptain:          ['Суперадминистратор', 'ТренерКоманды'],
  manageApplications:     ['Суперадминистратор', 'ТренерКоманды', 'ПредставительКоманды'],
  reviewApplications:     ['Суперадминистратор', 'Организатор'],
  manageAwards:           ['Суперадминистратор', 'Организатор'],
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
    return localStorage.getItem('token');
  });

  // синхронизация состояния при изменении localStorage из других вкладок
  useEffect(() => {
    const handleStorage = (e: StorageEvent) => {
      if (e.key === 'token') {
        setToken(e.newValue);
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

  // очистка данных авторизации
  const logout = useCallback(() => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  }, []);

  const role = user?.role ?? null;
  const isAuthenticated = token !== null && user !== null;
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
