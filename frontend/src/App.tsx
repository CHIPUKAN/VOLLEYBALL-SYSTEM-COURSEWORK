import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import MainLayout from './layouts/MainLayout';

// страницы
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import TeamsPage from './pages/TeamsPage';
import PlayersPage from './pages/PlayersPage';
import VenuesPage from './pages/VenuesPage';
import CoachesPage from './pages/CoachesPage';
import RefereesPage from './pages/RefereesPage';
import OrganizersPage from './pages/OrganizersPage';
import RegionsPage from './pages/RegionsPage';
import SeasonsPage from './pages/SeasonsPage';
import TournamentsPage from './pages/TournamentsPage';
import MatchesPage from './pages/MatchesPage';
import MatchDetailPage from './pages/MatchDetailPage';
import AwardsPage from './pages/AwardsPage';
import ApplicationsPage from './pages/ApplicationsPage';
import StandingsPage from './pages/StandingsPage';
import NotFoundPage from './pages/NotFoundPage';

// компонент защищённого маршрута
const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  return <>{children}</>;
};

// корневой компонент маршрутизации
const App: React.FC = () => {
  return (
    <Routes>
      {/* публичные маршруты */}
      <Route path="/login" element={<LoginPage />} />

      {/* защищённые маршруты */}
      <Route
        path="/"
        element={
          <PrivateRoute>
            <MainLayout />
          </PrivateRoute>
        }
      >
        <Route index element={<DashboardPage />} />
        <Route path="teams" element={<TeamsPage />} />
        <Route path="players" element={<PlayersPage />} />
        <Route path="venues" element={<VenuesPage />} />
        <Route path="coaches" element={<CoachesPage />} />
        <Route path="referees" element={<RefereesPage />} />
        <Route path="organizers" element={<OrganizersPage />} />
        <Route path="regions" element={<RegionsPage />} />
        <Route path="seasons" element={<SeasonsPage />} />
        <Route path="tournaments" element={<TournamentsPage />} />
        <Route path="matches" element={<MatchesPage />} />
        <Route path="matches/:id" element={<MatchDetailPage />} />
        <Route path="awards" element={<AwardsPage />} />
        <Route path="applications" element={<ApplicationsPage />} />
        <Route path="standings" element={<StandingsPage />} />
      </Route>

      {/* страница 404 */}
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
};

export default App;
