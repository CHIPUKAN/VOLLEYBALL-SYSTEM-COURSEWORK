import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Form,
  Input,
  Button,
  Card,
  Typography,
  Space,
  App,
  Tabs,
} from 'antd';
import { UserOutlined, LockOutlined, TrophyOutlined, MailOutlined } from '@ant-design/icons';
import { authApi } from '../api/authApi';
import { useAuth } from '../context/AuthContext';

const { Title, Text } = Typography;

interface LoginFormValues {
  email: string;
  password: string;
}

interface RegisterFormValues {
  email: string;
  fullName: string;
  password: string;
  confirmPassword: string;
}

// страница входа и регистрации
const LoginPage: React.FC = () => {
  const [loginLoading, setLoginLoading] = useState(false);
  const [registerLoading, setRegisterLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();
  const { message } = App.useApp();
  const [loginForm] = Form.useForm<LoginFormValues>();
  const [registerForm] = Form.useForm<RegisterFormValues>();

  const handleLogin = async (values: LoginFormValues) => {
    setLoginLoading(true);
    try {
      const response = await authApi.login({ email: values.email, password: values.password });
      login(response);
      message.success('Добро пожаловать!');
      navigate('/');
    } catch (error: unknown) {
      const err = error as { response?: { data?: { message?: string } } };
      const msg = err?.response?.data?.message ?? 'Неверный email или пароль';
      message.error(msg);
    } finally {
      setLoginLoading(false);
    }
  };

  const handleRegister = async (values: RegisterFormValues) => {
    setRegisterLoading(true);
    try {
      const response = await authApi.register({
        email: values.email,
        password: values.password,
        fullName: values.fullName,
      });
      login(response);
      message.success('Аккаунт создан! Добро пожаловать!');
      navigate('/');
    } catch (error: unknown) {
      const err = error as { response?: { data?: { message?: string } } };
      const msg = err?.response?.data?.message ?? 'Ошибка при регистрации';
      message.error(msg);
    } finally {
      setRegisterLoading(false);
    }
  };

  return (
    <div
      style={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(135deg, #1677ff 0%, #0050b3 100%)',
        padding: 24,
      }}
    >
      <Card
        style={{
          width: '100%',
          maxWidth: 420,
          boxShadow: '0 8px 40px rgba(0,0,0,0.2)',
          borderRadius: 12,
        }}
        styles={{ body: { padding: '32px 36px' } }}
      >
        <Space
          direction="vertical"
          align="center"
          style={{ width: '100%', marginBottom: 24 }}
        >
          <div
            style={{
              width: 64,
              height: 64,
              borderRadius: '50%',
              background: 'linear-gradient(135deg, #1677ff, #0050b3)',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
            }}
          >
            <TrophyOutlined style={{ fontSize: 30, color: '#fff' }} />
          </div>
          <Title level={3} style={{ margin: 0, textAlign: 'center' }}>
            Волейбол ИС
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', fontSize: 13 }}>
            Система учёта волейбольных турниров
          </Text>
        </Space>

        <Tabs
          defaultActiveKey="login"
          centered
          items={[
            {
              key: 'login',
              label: 'Вход',
              children: (
                <Form
                  form={loginForm}
                  layout="vertical"
                  onFinish={handleLogin}
                  autoComplete="off"
                  size="large"
                >
                  <Form.Item
                    name="email"
                    label="Email"
                    rules={[
                      { required: true, message: 'Введите email' },
                      { type: 'email', message: 'Некорректный email' },
                    ]}
                  >
                    <Input
                      prefix={<UserOutlined style={{ color: '#bfbfbf' }} />}
                      placeholder="admin@example.com"
                    />
                  </Form.Item>

                  <Form.Item
                    name="password"
                    label="Пароль"
                    rules={[{ required: true, message: 'Введите пароль' }]}
                  >
                    <Input.Password
                      prefix={<LockOutlined style={{ color: '#bfbfbf' }} />}
                      placeholder="••••••••"
                    />
                  </Form.Item>

                  <Form.Item style={{ marginBottom: 0, marginTop: 8 }}>
                    <Button
                      type="primary"
                      htmlType="submit"
                      loading={loginLoading}
                      block
                      style={{ height: 44, fontWeight: 600 }}
                    >
                      Войти
                    </Button>
                  </Form.Item>
                </Form>
              ),
            },
            {
              key: 'register',
              label: 'Регистрация',
              children: (
                <Form
                  form={registerForm}
                  layout="vertical"
                  onFinish={handleRegister}
                  autoComplete="off"
                  size="large"
                >
                  <Form.Item
                    name="fullName"
                    label="Имя"
                    rules={[{ required: true, message: 'Введите ваше имя' }]}
                  >
                    <Input
                      prefix={<UserOutlined style={{ color: '#bfbfbf' }} />}
                      placeholder="Иван Иванов"
                    />
                  </Form.Item>

                  <Form.Item
                    name="email"
                    label="Email"
                    rules={[
                      { required: true, message: 'Введите email' },
                      { type: 'email', message: 'Некорректный email' },
                    ]}
                  >
                    <Input
                      prefix={<MailOutlined style={{ color: '#bfbfbf' }} />}
                      placeholder="user@example.com"
                    />
                  </Form.Item>

                  <Form.Item
                    name="password"
                    label="Пароль"
                    rules={[
                      { required: true, message: 'Введите пароль' },
                      { min: 6, message: 'Минимум 6 символов' },
                    ]}
                  >
                    <Input.Password
                      prefix={<LockOutlined style={{ color: '#bfbfbf' }} />}
                      placeholder="Минимум 6 символов"
                    />
                  </Form.Item>

                  <Form.Item
                    name="confirmPassword"
                    label="Подтверждение пароля"
                    dependencies={['password']}
                    rules={[
                      { required: true, message: 'Подтвердите пароль' },
                      ({ getFieldValue }) => ({
                        validator(_, value) {
                          if (!value || getFieldValue('password') === value) {
                            return Promise.resolve();
                          }
                          return Promise.reject(new Error('Пароли не совпадают'));
                        },
                      }),
                    ]}
                  >
                    <Input.Password
                      prefix={<LockOutlined style={{ color: '#bfbfbf' }} />}
                      placeholder="Повторите пароль"
                    />
                  </Form.Item>

                  <Text type="secondary" style={{ fontSize: 12, display: 'block', marginBottom: 12 }}>
                    Новый аккаунт получает роль «Зритель». Расширенный доступ выдаёт администратор.
                  </Text>

                  <Form.Item style={{ marginBottom: 0 }}>
                    <Button
                      type="primary"
                      htmlType="submit"
                      loading={registerLoading}
                      block
                      style={{ height: 44, fontWeight: 600 }}
                    >
                      Создать аккаунт
                    </Button>
                  </Form.Item>
                </Form>
              ),
            },
          ]}
        />
      </Card>
    </div>
  );
};

export default LoginPage;
