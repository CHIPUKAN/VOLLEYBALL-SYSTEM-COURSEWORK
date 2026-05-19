import React, { useEffect, useState } from 'react';
import { Row, Col, Card, Statistic, Typography, Spin, Space, Button } from 'antd';
import {
  TeamOutlined,
  TrophyOutlined,
  ThunderboltOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { teamsApi, tournamentsApi, matchesApi, playersApi } from '../api/index';
import { useAuth } from '../context/AuthContext';
import type { Match } from '../types/index';

const { Title, Text } = Typography;

interface Stats {
  teams: number;
  tournaments: number;
  matches: number;
  players: number;
}

// главная страница — дашборд
const DashboardPage: React.FC = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [stats, setStats] = useState<Stats>({ teams: 0, tournaments: 0, matches: 0, players: 0 });
  const [loading, setLoading] = useState(true);
  const [liveMatches, setLiveMatches] = useState<Match[]>([]);

  useEffect(() => {
    const loadStats = async () => {
      try {
        const [teams, tournaments, matches, players] = await Promise.all([
          teamsApi.getAll(),
          tournamentsApi.getAll(),
          matchesApi.getAll(),
          playersApi.getAll(),
        ]);
        setStats({
          teams: teams.length,
          tournaments: tournaments.length,
          matches: matches.length,
          players: players.length,
        });
        setLiveMatches(matches.filter(m => m.statusName === 'В процессе'));
      } catch {
        // ошибки загрузки статистики не критичны
      } finally {
        setLoading(false);
      }
    };
    loadStats();
  }, []);

  const statCards = [
    {
      title: 'Команды',
      value: stats.teams,
      icon: <TeamOutlined style={{ fontSize: 32, color: '#1677ff' }} />,
      color: '#e6f4ff',
      borderColor: '#91caff',
    },
    {
      title: 'Турниры',
      value: stats.tournaments,
      icon: <TrophyOutlined style={{ fontSize: 32, color: '#faad14' }} />,
      color: '#fffbe6',
      borderColor: '#ffe58f',
    },
    {
      title: 'Матчи',
      value: stats.matches,
      icon: <ThunderboltOutlined style={{ fontSize: 32, color: '#52c41a' }} />,
      color: '#f6ffed',
      borderColor: '#b7eb8f',
    },
    {
      title: 'Игроки',
      value: stats.players,
      icon: <UserOutlined style={{ fontSize: 32, color: '#722ed1' }} />,
      color: '#f9f0ff',
      borderColor: '#d3adf7',
    },
  ];

  return (
    <div>
      {/* приветствие */}
      <div style={{ marginBottom: 32 }}>
        <Title level={2} style={{ margin: 0 }}>
          Добро пожаловать{user?.fullName ? `, ${user.fullName}` : ''}!
        </Title>
        <Text type="secondary">
          Система учёта волейбольных турниров — обзор данных
        </Text>
      </div>

      {/* статистические карточки */}
      <Spin spinning={loading}>
        <Row gutter={[16, 16]}>
          {statCards.map((card) => (
            <Col key={card.title} xs={24} sm={12} lg={6}>
              <Card
                style={{
                  background: card.color,
                  border: `1px solid ${card.borderColor}`,
                  borderRadius: 10,
                }}
                styles={{ body: { padding: '20px 24px' } }}
              >
                <Space align="center" style={{ width: '100%', justifyContent: 'space-between' }}>
                  <Statistic
                    title={<Text style={{ fontSize: 14 }}>{card.title}</Text>}
                    value={card.value}
                    valueStyle={{ fontSize: 32, fontWeight: 700 }}
                  />
                  {card.icon}
                </Space>
              </Card>
            </Col>
          ))}
        </Row>
      </Spin>

      {/* матчи в прямом эфире */}
      {liveMatches.length > 0 && (
        <Card title="🏐 Матчи идут сейчас" style={{ marginBottom: 16, marginTop: 24, borderColor: '#ff7a00' }}>
          {liveMatches.map(m => (
            <div key={m.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '8px 0', borderBottom: '1px solid #f0f0f0' }}>
              <Text>{m.homeTeamName ?? `Команда ${m.homeTeamId}`} vs {m.guestTeamName ?? `Команда ${m.guestTeamId}`}</Text>
              <Button size="small" type="primary" onClick={() => navigate(`/matches/${m.id}`)}>
                Открыть
              </Button>
            </div>
          ))}
        </Card>
      )}

      {/* информационный блок */}
      <Row gutter={[16, 16]} style={{ marginTop: 32 }}>
        <Col xs={24} md={12}>
          <Card title="О системе" bordered={false} style={{ borderRadius: 10 }}>
            <Space direction="vertical">
              <Text>
                Информационная система для учёта волейбольных турниров позволяет
                управлять командами, игроками, судьями и результатами матчей.
              </Text>
              <Text type="secondary">
                Используйте боковое меню для навигации между разделами.
              </Text>
            </Space>
          </Card>
        </Col>
        <Col xs={24} md={12}>
          <Card title="Текущий пользователь" bordered={false} style={{ borderRadius: 10 }}>
            <Space direction="vertical">
              <Text><strong>Email:</strong> {user?.email}</Text>
              {user?.fullName && <Text><strong>Имя:</strong> {user.fullName}</Text>}
              <Text><strong>Роль:</strong> {user?.role}</Text>
            </Space>
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default DashboardPage;
