import React, { useState } from 'react';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import {
  Layout,
  Menu,
  Button,
  Avatar,
  Typography,
  Space,
  Dropdown,
  theme,
} from 'antd';
import {
  DashboardOutlined,
  TeamOutlined,
  UserOutlined,
  HomeOutlined,
  SolutionOutlined,
  AuditOutlined,
  ClusterOutlined,
  EnvironmentOutlined,
  CalendarOutlined,
  TrophyOutlined,
  ThunderboltOutlined,
  GiftOutlined,
  FileTextOutlined,
  OrderedListOutlined,
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  LogoutOutlined,
  DownOutlined,
} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { useAuth } from '../context/AuthContext';
import LiveTicker from '../components/LiveTicker';

const { Sider, Header, Content } = Layout;
const { Text } = Typography;

// пункты навигационного меню
const menuItems: MenuProps['items'] = [
  {
    key: '/',
    icon: <DashboardOutlined />,
    label: 'Главная',
  },
  {
    key: '/teams',
    icon: <TeamOutlined />,
    label: 'Команды',
  },
  {
    key: '/players',
    icon: <UserOutlined />,
    label: 'Игроки',
  },
  {
    key: '/venues',
    icon: <HomeOutlined />,
    label: 'Площадки',
  },
  {
    key: '/coaches',
    icon: <SolutionOutlined />,
    label: 'Тренеры',
  },
  {
    key: '/referees',
    icon: <AuditOutlined />,
    label: 'Судьи',
  },
  {
    key: '/organizers',
    icon: <ClusterOutlined />,
    label: 'Организаторы',
  },
  {
    key: '/regions',
    icon: <EnvironmentOutlined />,
    label: 'Регионы',
  },
  {
    key: '/seasons',
    icon: <CalendarOutlined />,
    label: 'Сезоны',
  },
  {
    key: '/tournaments',
    icon: <TrophyOutlined />,
    label: 'Турниры',
  },
  {
    key: '/matches',
    icon: <ThunderboltOutlined />,
    label: 'Матчи',
  },
  {
    key: '/awards',
    icon: <GiftOutlined />,
    label: 'Награды',
  },
  {
    key: '/applications',
    icon: <FileTextOutlined />,
    label: 'Заявки',
  },
  {
    key: '/standings',
    icon: <OrderedListOutlined />,
    label: 'Таблица',
  },
];

// основной макет приложения
const MainLayout: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const { user, logout } = useAuth();
  const { token: designToken } = theme.useToken();

  // определение активного пункта меню
  const selectedKey = '/' + location.pathname.split('/')[1];

  const handleMenuClick: MenuProps['onClick'] = ({ key }) => {
    navigate(key);
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  // меню пользователя в заголовке
  const userMenuItems: MenuProps['items'] = [
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Выйти',
      danger: true,
    },
  ];

  const handleUserMenuClick: MenuProps['onClick'] = ({ key }) => {
    if (key === 'logout') {
      handleLogout();
    }
  };

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider
        breakpoint="lg"
        collapsedWidth={0}
        collapsed={collapsed}
        onCollapse={(val) => setCollapsed(val)}
        onBreakpoint={(broken) => {
          if (broken) {
            setCollapsed(true);
          }
        }}
        style={{
          overflow: 'auto',
          height: '100vh',
          position: 'fixed',
          left: 0,
          top: 0,
          bottom: 0,
          zIndex: 100,
          boxShadow: '2px 0 8px rgba(0,0,0,0.15)',
        }}
        theme="dark"
        width={220}
      >
        {/* логотип */}
        <div
          style={{
            height: 64,
            display: 'flex',
            alignItems: 'center',
            justifyContent: collapsed ? 'center' : 'flex-start',
            padding: collapsed ? '0' : '0 16px',
            borderBottom: '1px solid rgba(255,255,255,0.1)',
            overflow: 'hidden',
          }}
        >
          <TrophyOutlined style={{ fontSize: 22, color: '#faad14', flexShrink: 0 }} />
          {!collapsed && (
            <span
              style={{
                color: '#fff',
                fontSize: 16,
                fontWeight: 700,
                marginLeft: 10,
                whiteSpace: 'nowrap',
              }}
            >
              Волейбол ИС
            </span>
          )}
        </div>

        {/* навигация */}
        <Menu
          theme="dark"
          mode="inline"
          selectedKeys={[selectedKey]}
          items={menuItems}
          onClick={handleMenuClick}
          style={{ borderRight: 0 }}
        />
      </Sider>

      {/* основное содержимое   */}
      <Layout
        style={{
          marginLeft: collapsed ? 0 : 220,
          transition: 'margin-left 0.2s',
        }}
      >
        <LiveTicker />

              {/* заголовок*/}

        <Header
          style={{
            position: 'sticky',
            top: 0,
            zIndex: 99,
            padding: '0 16px',
            background: designToken.colorBgContainer,
            boxShadow: '0 1px 4px rgba(0,0,0,0.1)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            gap: 8,
          }}
        >
                  {/* кнопка сворачивания сайдбара */}

          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{ fontSize: 16, width: 40, height: 40 }}
          />

                  {/* правая часть заголовка */}

          <Space>
            <Dropdown
              menu={{ items: userMenuItems, onClick: handleUserMenuClick }}
              placement="bottomRight"
              arrow
            >
              <Space style={{ cursor: 'pointer' }}>
                <Avatar
                  style={{ backgroundColor: designToken.colorPrimary }}
                  icon={<UserOutlined />}
                  size="small"
                />
                <Text style={{ maxWidth: 140, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                  {user?.fullName ?? user?.email ?? 'Пользователь'}
                </Text>
                <DownOutlined style={{ fontSize: 10 }} />
              </Space>
            </Dropdown>
          </Space>
        </Header>

              {/* страница*/}

        <Content
          style={{
            padding: '24px 16px',
            minHeight: 'calc(100vh - 64px)',
            background: designToken.colorBgLayout,
          }}
        >
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
};

export default MainLayout;
