import React, { useState } from 'react';
import { Modal, Button, Typography, Space, Alert } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import type { StartingLineup, ActionResult } from '../types/index';

const { Text, Title } = Typography;

type RallyPhase = 'pre_serve' | 'in_play';

interface ActionPickerModalProps {
  open: boolean;
  player: StartingLineup | null;
  playerFullName?: string;
  teamName?: string;
  servingTeamId?: number;
  rallyPhase?: RallyPhase;
  onConfirm: (category: string, result: ActionResult) => void;
  onCancel: () => void;
}

const ACTION_CATEGORIES = [
  { code: 'serve',     label: 'Подача',   icon: '🏐' },
  { code: 'reception', label: 'Приём',    icon: '🤲' },
  { code: 'set',       label: 'Передача', icon: '🙌' },
  { code: 'attack',    label: 'Атака',    icon: '💥' },
  { code: 'block',     label: 'Блок',     icon: '🛡️' },
  { code: 'other',     label: 'Другое',   icon: '⚙️' },
];

const ACTION_RESULTS: Record<string, ActionResult[]> = {
  serve: [
    { code: 'ace',            label: 'Эйс',            icon: '⚡', scoringEffect: 'acting',   eventTypeName: 'Эйс' },
    { code: 'serve_error',    label: 'Ошибка подачи',  icon: '✗',  scoringEffect: 'opposing', eventTypeName: 'Ошибка подачи' },
    { code: 'serve_received', label: 'Принята в игру', icon: '↩',  scoringEffect: 'neutral',  eventTypeName: 'Подача' },
  ],
  reception: [
    { code: 'rec_perfect', label: 'Отличный', icon: '✓✓', scoringEffect: 'neutral',  eventTypeName: 'Приём отличный' },
    { code: 'rec_good',    label: 'Хороший',  icon: '✓',  scoringEffect: 'neutral',  eventTypeName: 'Приём хороший' },
    { code: 'rec_poor',    label: 'Слабый',   icon: '~',  scoringEffect: 'neutral',  eventTypeName: 'Приём слабый' },
    { code: 'rec_error',   label: 'Ошибка',   icon: '✗',  scoringEffect: 'opposing', eventTypeName: 'Ошибка приёма' },
  ],
  set: [
    { code: 'set_good',  label: 'Передача',        icon: '✓', scoringEffect: 'neutral',  eventTypeName: 'Передача' },
    { code: 'set_error', label: 'Ошибка передачи', icon: '✗', scoringEffect: 'opposing', eventTypeName: 'Ошибка передачи' },
  ],
  attack: [
    { code: 'atk_point',   label: 'Очко',          icon: '💥', scoringEffect: 'acting',   eventTypeName: 'Очко атаки' },
    { code: 'atk_blocked', label: 'Заблокирована', icon: '🛡',  scoringEffect: 'neutral',  eventTypeName: 'Атака заблокирована' },
    { code: 'atk_error',   label: 'Ошибка',        icon: '✗',  scoringEffect: 'opposing', eventTypeName: 'Ошибка атаки' },
  ],
  block: [
    { code: 'blk_point', label: 'Очко блока', icon: '🛡',  scoringEffect: 'acting',   eventTypeName: 'Блок-очко' },
    { code: 'blk_touch', label: 'Касание',    icon: '✋', scoringEffect: 'neutral',  eventTypeName: 'Блок-касание' },
    { code: 'blk_error', label: 'Ошибка',     icon: '✗',  scoringEffect: 'opposing', eventTypeName: 'Ошибка блока' },
  ],
  other: [
    { code: 'point_opponent',    label: 'Очко соперника (техн.)',    icon: '⊕', scoringEffect: 'opposing', eventTypeName: 'Техническое очко' },
    { code: 'point_us',          label: 'Очко нашей команды (техн.)',icon: '⊕', scoringEffect: 'acting',   eventTypeName: 'Техническое очко' },
    { code: 'net_touch_opp',     label: 'Касание сетки соперником', icon: '🔴', scoringEffect: 'acting',   eventTypeName: 'Касание сетки' },
    { code: 'foot_fault_opp',    label: 'Заступ соперника',         icon: '🔴', scoringEffect: 'acting',   eventTypeName: 'Заступ' },
    { code: 'rotation_fault_opp',label: 'Ошибка расстановки сопер.',icon: '🔴', scoringEffect: 'acting',   eventTypeName: 'Ошибка расстановки' },
    { code: 'double_hit_opp',    label: 'Двойное касание соперника',icon: '🔴', scoringEffect: 'acting',   eventTypeName: 'Двойное касание' },
    { code: 'four_hits_opp',     label: 'Четыре касания соперника', icon: '🔴', scoringEffect: 'acting',   eventTypeName: 'Четыре касания' },
    { code: 'caught_ball_opp',   label: 'Захват мяча соперником',   icon: '🔴', scoringEffect: 'acting',   eventTypeName: 'Захват мяча' },
    { code: 'disputed_us',       label: 'Спорный мяч (нам)',        icon: '⚖', scoringEffect: 'acting',   eventTypeName: 'Спорный мяч' },
    { code: 'disputed_them',     label: 'Спорный мяч (им)',         icon: '⚖', scoringEffect: 'opposing', eventTypeName: 'Спорный мяч' },
    { code: 'net_touch_self',    label: 'Касание сетки нашим игр.', icon: '🔵', scoringEffect: 'opposing', eventTypeName: 'Касание сетки' },
    { code: 'foot_fault_self',   label: 'Заступ нашего игрока',     icon: '🔵', scoringEffect: 'opposing', eventTypeName: 'Заступ' },
  ],
};

const EFFECT_COLORS: Record<string, string> = {
  acting:   '#52c41a',
  opposing: '#ff4d4f',
  neutral:  '#8c8c8c',
};

const FRONT_ROW = new Set([2, 3, 4]);

// вычислить допустимые категории действий для конкретного игрока
function getAvailableCategories(
  player: StartingLineup | null,
  servingTeamId: number | undefined,
  rallyPhase: RallyPhase = 'in_play',
) {
  if (!player || servingTeamId === undefined) return ACTION_CATEGORIES;

  const isServingTeam = player.teamId === servingTeamId;
  const pos = player.positionNo;

  // до подачи доступны только подача (поз.1 подающей) и другое
  if (rallyPhase === 'pre_serve') {
    return ACTION_CATEGORIES.filter(cat => {
      if (cat.code === 'serve') return isServingTeam && pos === 1;
      if (cat.code === 'other') return true;
      return false;
    });
  }

  return ACTION_CATEGORIES.filter(cat => {
    switch (cat.code) {
      case 'serve':
        // подаёт только игрок № 1 подающей команды
        return isServingTeam && pos === 1;
      case 'reception':
        // принимают только игроки не-подающей команды
        return !isServingTeam;
      case 'set':
        // передача только у принимающей команды
        return !isServingTeam;
      case 'block':
        // блокируют передние игроки (2,3,4) не-подающей команды
        return !isServingTeam && FRONT_ROW.has(pos);
      case 'attack':
        // атакуют передние игроки обеих команд
        return FRONT_ROW.has(pos);
      case 'other':
        return true;
      default:
        return true;
    }
  });
}

// двухшаговый модал выбора действия
const ActionPickerModal: React.FC<ActionPickerModalProps> = ({
  open,
  player,
  playerFullName,
  teamName,
  servingTeamId,
  rallyPhase,
  onConfirm,
  onCancel,
}) => {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);

  const handleCategorySelect = (code: string) => setSelectedCategory(code);

  const handleResultSelect = (result: ActionResult) => {
    if (selectedCategory) {
      onConfirm(selectedCategory, result);
      setSelectedCategory(null);
    }
  };

  const handleCancel = () => { setSelectedCategory(null); onCancel(); };
  const handleBack = () => setSelectedCategory(null);

  const playerLabel = player
    ? `#${player.shirtNumber ?? '?'} ${playerFullName ?? player.playerFullName ?? ''}`
    : '';

  const availableCategories = getAvailableCategories(player, servingTeamId, rallyPhase);
  const hiddenCategories = ACTION_CATEGORIES.filter(c => !availableCategories.some(a => a.code === c.code));

  return (
    <Modal
      open={open}
      onCancel={handleCancel}
      footer={null}
      title={
        <Space>
          {selectedCategory && (
            <Button type="text" icon={<ArrowLeftOutlined />} onClick={handleBack} size="small">
              Назад
            </Button>
          )}
          <Text strong>
            {selectedCategory
              ? `${ACTION_CATEGORIES.find(c => c.code === selectedCategory)?.icon ?? ''} ${ACTION_CATEGORIES.find(c => c.code === selectedCategory)?.label ?? ''}`
              : 'Действие игрока'}
          </Text>
        </Space>
      }
      destroyOnHidden
      afterClose={() => setSelectedCategory(null)}
      width={420}
    >
      <div style={{ marginBottom: 10 }}>
        <Text type="secondary">
          {playerLabel}{teamName ? ` · ${teamName}` : ''}
        </Text>
      </div>

      {/* подсказка о недоступных действиях */}
      {!selectedCategory && hiddenCategories.length > 0 && player && servingTeamId !== undefined && (
        <div style={{ marginBottom: 10, fontSize: 11, color: '#8c8c8c', lineHeight: 1.4 }}>
          {player.teamId === servingTeamId
            ? player.positionNo !== 1
              ? '🏐 Подача — только игрок позиции 1 подающей команды'
              : null
            : '🏐 Подача и блок недоступны для принимающей команды на данных позициях'}
        </div>
      )}

      {/* шаг 1: категории */}
      {!selectedCategory && (
        availableCategories.length === 0 ? (
          <Alert
            message="Нет доступных действий"
            description="Для этого игрока нет допустимых действий по правилам волейбола."
            type="info"
            showIcon
          />
        ) : (
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: 10 }}>
            {availableCategories.map(cat => (
              <Button
                key={cat.code}
                onClick={() => handleCategorySelect(cat.code)}
                style={{ height: 72, fontSize: 13, display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', gap: 4 }}
                block
              >
                <span style={{ fontSize: 22 }}>{cat.icon}</span>
                <span>{cat.label}</span>
              </Button>
            ))}
          </div>
        )
      )}

      {/* шаг 2: результаты */}
      {selectedCategory && (
        <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
          {(ACTION_RESULTS[selectedCategory] ?? []).map(result => (
            <Button
              key={result.code}
              onClick={() => handleResultSelect(result)}
              style={{
                minHeight: 60,
                fontSize: 14,
                display: 'flex',
                alignItems: 'center',
                gap: 12,
                background: EFFECT_COLORS[result.scoringEffect] + '22',
                borderColor: EFFECT_COLORS[result.scoringEffect],
                color: '#000',
              }}
              block
            >
              <span style={{ fontSize: 18, minWidth: 24 }}>{result.icon}</span>
              <span style={{ flex: 1, textAlign: 'left' }}>{result.label}</span>
              <Title level={5} style={{ margin: 0, color: EFFECT_COLORS[result.scoringEffect] }}>
                {result.scoringEffect === 'acting' ? '+1' : result.scoringEffect === 'opposing' ? '−1' : '±0'}
              </Title>
            </Button>
          ))}
        </div>
      )}

      <div style={{ marginTop: 16, textAlign: 'right' }}>
        <Button onClick={handleCancel}>Отмена</Button>
      </div>
    </Modal>
  );
};

export default ActionPickerModal;
