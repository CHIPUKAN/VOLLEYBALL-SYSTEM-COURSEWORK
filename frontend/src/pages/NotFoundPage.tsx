import React from 'react';
import { Button, Result } from 'antd';
import { useNavigate } from 'react-router-dom';

// страница 404 — маршрут не найден
const NotFoundPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <Result
      status="404"
      title="404"
      subTitle="Страница не найдена"
      extra={
        <Button type="primary" onClick={() => navigate('/')}>
          На главную
        </Button>
      }
    />
  );
};

export default NotFoundPage;
