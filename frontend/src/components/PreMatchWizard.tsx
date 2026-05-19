import React, { useState } from 'react';
import { Modal, Steps, Card, Button, Radio, Space, Typography, Row, Col } from 'antd';
import type { Match, Player, CoinTossResult } from '../types/index';
import VolleyCourt from './VolleyCourt';

const { Title, Text } = Typography;

interface PreMatchWizardProps {
  open: boolean;
  match: Match;
  homePlayers: Player[];
  guestPlayers: Player[];
  onComplete: (coinToss: CoinTossResult) => void;
  onSkip: () => void;
  onCancel: () => void;
}

const STEPS = ['Жеребьёвка', 'Расстановка хозяев', 'Расстановка гостей', 'Готовность'];

// визард подготовки матча
const PreMatchWizard: React.FC<PreMatchWizardProps> = ({
  open,
  match,
  onComplete,
  onSkip,
  onCancel,
}) => {
  const [currentStep, setCurrentStep] = useState(0);
  const [servingTeamId, setServingTeamId] = useState<number>(match.homeTeamId);
  const [homeTeamSide, setHomeTeamSide] = useState<'top' | 'bottom'>('top');

  const coinTossResult: CoinTossResult = { servingTeamId, homeTeamSide };

  const next = () => setCurrentStep(s => s + 1);
  const prev = () => setCurrentStep(s => s - 1);
  const skipStep = () => {
    if (currentStep < STEPS.length - 1) {
      setCurrentStep(s => s + 1);
    }
  };

  const handleComplete = () => {
    onComplete(coinTossResult);
  };

  const renderStep = () => {
    switch (currentStep) {
      // шаг 0: жеребьёвка
      case 0:
        return (
          <Card>
            <Text type="secondary" style={{ display: 'block', marginBottom: 20 }}>
              Перед матчем проводится жеребьёвка. Победитель жеребьёвки выбирает подачу или сторону.
            </Text>
            <Space direction="vertical" size="large" style={{ width: '100%' }}>
              <div>
                <Text strong style={{ display: 'block', marginBottom: 8 }}>
                  Кто подаёт первым в 1 партии?
                </Text>
                <Radio.Group
                  value={servingTeamId}
                  onChange={e => setServingTeamId(e.target.value)}
                >
                  <Radio.Button value={match.homeTeamId}>
                    {match.homeTeamName ?? 'Хозяева'}
                  </Radio.Button>
                  <Radio.Button value={match.guestTeamId}>
                    {match.guestTeamName ?? 'Гости'}
                  </Radio.Button>
                </Radio.Group>
              </div>
              <div>
                <Text strong style={{ display: 'block', marginBottom: 8 }}>
                  Хозяева играют:
                </Text>
                <Radio.Group
                  value={homeTeamSide}
                  onChange={e => setHomeTeamSide(e.target.value)}
                >
                  <Radio.Button value="top">Сверху поля (стандарт)</Radio.Button>
                  <Radio.Button value="bottom">Снизу поля</Radio.Button>
                </Radio.Group>
              </div>
            </Space>
          </Card>
        );

      // шаг 1: расстановка хозяев
      case 1:
        return (
          <Card>
            <Text type="secondary" style={{ display: 'block', marginBottom: 12 }}>
              Заполните стартовую расстановку хозяев на Партию 1
            </Text>
            <VolleyCourt
              matchId={match.id}
              homeTeamId={match.homeTeamId}
              homeTeamName={match.homeTeamName ?? 'Хозяева'}
              guestTeamId={match.guestTeamId}
              guestTeamName={match.guestTeamName ?? 'Гости'}
              setNumber={1}
              readonly={false}
            />
          </Card>
        );

      // шаг 2: расстановка гостей
      case 2:
        return (
          <Card>
            <Text type="secondary" style={{ display: 'block', marginBottom: 12 }}>
              Заполните стартовую расстановку гостей на Партию 1
            </Text>
            <VolleyCourt
              matchId={match.id}
              homeTeamId={match.homeTeamId}
              homeTeamName={match.homeTeamName ?? 'Хозяева'}
              guestTeamId={match.guestTeamId}
              guestTeamName={match.guestTeamName ?? 'Гости'}
              setNumber={1}
              readonly={false}
            />
          </Card>
        );

      // шаг 3: готовность
      case 3:
        return (
          <Card>
            <Title level={4} style={{ marginBottom: 16 }}>Матч готов к началу</Title>
            <Row gutter={[16, 12]}>
              <Col span={24}>
                <Text><strong>Хозяева:</strong> {match.homeTeamName ?? '—'}</Text>
              </Col>
              <Col span={24}>
                <Text><strong>Гости:</strong> {match.guestTeamName ?? '—'}</Text>
              </Col>
              <Col span={24}>
                <Text>
                  <strong>Подаёт первым:</strong>{' '}
                  {servingTeamId === match.homeTeamId
                    ? (match.homeTeamName ?? 'Хозяева')
                    : (match.guestTeamName ?? 'Гости')}
                </Text>
              </Col>
              <Col span={24}>
                <Text>
                  <strong>Сторона хозяев:</strong>{' '}
                  {homeTeamSide === 'top' ? 'Сверху' : 'Снизу'}
                </Text>
              </Col>
              <Col span={24}>
                <Text strong style={{ fontSize: 24, color: '#52c41a' }}>
                  Счёт: 0 : 0
                </Text>
              </Col>
            </Row>
          </Card>
        );

      default:
        return null;
    }
  };

  return (
    <Modal
      open={open}
      title="Подготовка к матчу"
      onCancel={onCancel}
      footer={null}
      width={620}
      destroyOnClose
      afterClose={() => setCurrentStep(0)}
    >
      <Steps
        current={currentStep}
        items={STEPS.map(s => ({ title: s }))}
        style={{ marginBottom: 24 }}
        size="small"
      />

      {renderStep()}

      <div style={{ display: 'flex', justifyContent: 'space-between', marginTop: 20 }}>
        <Space>
          {currentStep > 0 && (
            <Button onClick={prev}>← Назад</Button>
          )}
          <Button type="link" onClick={currentStep === STEPS.length - 1 ? onSkip : skipStep}>
            {currentStep === STEPS.length - 1 ? 'Пропустить всё' : 'Пропустить шаг →'}
          </Button>
        </Space>

        <Space>
          <Button onClick={onCancel}>Отмена</Button>
          {currentStep < STEPS.length - 1 ? (
            <Button type="primary" onClick={next}>
              Далее →
            </Button>
          ) : (
            <Button type="primary" onClick={handleComplete} style={{ background: '#52c41a', borderColor: '#52c41a' }}>
              🏐 Начать матч
            </Button>
          )}
        </Space>
      </div>
    </Modal>
  );
};

export default PreMatchWizard;
