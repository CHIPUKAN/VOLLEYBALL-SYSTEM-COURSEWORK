import React, { useState, useEffect } from 'react';
import { Modal, Form, Select, Checkbox, Button, Typography, Alert } from 'antd';
import type { StartingLineup, Player, LookupDto } from '../types/index';

const { Text } = Typography;

export interface SubPair {
  out: number;
  in: number;
}

interface SubstitutionModalProps {
  open: boolean;
  matchId: number;
  setNumber: number;
  homeTeamId: number;
  homeTeamName: string;
  guestTeamId: number;
  guestTeamName: string;
  homeLineup: StartingLineup[];
  guestLineup: StartingLineup[];
  allHomePlayers: Player[];
  allGuestPlayers: Player[];
  substitutionTypes: LookupDto[];
  defaultOutPlayerId?: number;
  defaultTeamId?: number;
  subPairsCurrentSet?: SubPair[];
  onConfirm: (data: {
    teamId: number;
    subOutPlayerId: number;
    subInPlayerId: number;
    subTypeCode: number;
    isLiberoSwap: boolean;
  }) => void;
  onCancel: () => void;
}

// модал замены игрока
const SubstitutionModal: React.FC<SubstitutionModalProps> = ({
  open,
  homeTeamId,
  homeTeamName,
  guestTeamId,
  guestTeamName,
  homeLineup,
  guestLineup,
  allHomePlayers,
  allGuestPlayers,
  substitutionTypes,
  defaultOutPlayerId,
  defaultTeamId,
  subPairsCurrentSet = [],
  onConfirm,
  onCancel,
}) => {
  const [form] = Form.useForm();
  const [selectedTeamId, setSelectedTeamId] = useState<number>(defaultTeamId ?? homeTeamId);
  const [selectedSubOutId, setSelectedSubOutId] = useState<number | undefined>(defaultOutPlayerId);

  // при каждом открытии — инициализация значений по умолчанию
  useEffect(() => {
    if (!open) return;
    const teamId = defaultTeamId ?? homeTeamId;
    setSelectedTeamId(teamId);
    const outId = defaultOutPlayerId;
    setSelectedSubOutId(outId);
    // сбросить поля формы, затем предзаполнить
    form.resetFields();
    if (outId) {
      // задержка нужна, так как destroyOnHidden пересоздаёт форму
      setTimeout(() => {
        form.setFieldValue('subOutPlayerId', outId);
      }, 0);
    }
    if (substitutionTypes.length > 0) {
      setTimeout(() => {
        form.setFieldValue('subTypeCode', substitutionTypes[0].code);
      }, 0);
    }
  }, [open]);

  const currentLineup = selectedTeamId === homeTeamId ? homeLineup : guestLineup;
  const allPlayers = selectedTeamId === homeTeamId ? allHomePlayers : allGuestPlayers;
  const hasLineup = currentLineup.length > 0;

  const lineupPlayerIds = new Set(currentLineup.map(l => l.playerId));

  // проверка: может ли игрок войти на площадку в этой партии
  // игрок, которого заменили, может вернуться только на место своего заменителя
  const canEnter = (playerId: number): boolean => {
    if (subPairsCurrentSet.length === 0) return true;
    const wasSubbedOut = subPairsCurrentSet.some(p => p.out === playerId);
    if (!wasSubbedOut) return true;
    // был заменён ранее — разрешён возврат только если выходит тот, кто пришёл вместо него
    const pair = subPairsCurrentSet.find(p => p.out === playerId);
    return pair ? selectedSubOutId === pair.in : true;
  };

  // уходящий: игроки текущей расстановки
  const outgoingOptions = hasLineup
    ? currentLineup.map(l => ({
        value: l.playerId,
        label: `${l.shirtNumber ? '#' + l.shirtNumber + ' ' : ''}${l.playerFullName ?? `Игрок ${l.playerId}`}`,
      }))
    : allPlayers.map(p => ({
        value: p.id,
        label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
      }));

  // входящий: не в расстановке + может войти по правилам партии
  const incomingOptions = hasLineup
    ? allPlayers
        .filter(p => !lineupPlayerIds.has(p.id) && canEnter(p.id))
        .map(p => ({
          value: p.id,
          label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
        }))
    : allPlayers
        .filter(p => p.id !== selectedSubOutId && canEnter(p.id))
        .map(p => ({
          value: p.id,
          label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
        }));

  // количество игроков, заблокированных правилом повторного выхода
  const blockedCount = hasLineup
    ? allPlayers.filter(p => !lineupPlayerIds.has(p.id) && !canEnter(p.id)).length
    : 0;

  const handleOk = async () => {
    let values: Record<string, unknown>;
    try {
      values = await form.validateFields();
    } catch {
      return;
    }
    onConfirm({
      teamId: selectedTeamId,
      subOutPlayerId: values.subOutPlayerId as number,
      subInPlayerId: values.subInPlayerId as number,
      subTypeCode: (values.subTypeCode as number) ?? substitutionTypes[0]?.code ?? 1,
      isLiberoSwap: (values.isLiberoSwap as boolean) ?? false,
    });
    form.resetFields();
    setSelectedSubOutId(undefined);
  };

  const handleCancel = () => {
    form.resetFields();
    setSelectedSubOutId(undefined);
    onCancel();
  };

  const handleTeamChange = (teamId: number) => {
    setSelectedTeamId(teamId);
    setSelectedSubOutId(undefined);
    form.resetFields(['subOutPlayerId', 'subInPlayerId']);
  };

  const canSubmit = outgoingOptions.length > 0 && (incomingOptions.length > 0 || !hasLineup);

  return (
    <Modal
      open={open}
      title="Замена игрока"
      onCancel={handleCancel}
      footer={null}
      destroyOnHidden
      width={480}
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        {/* выбор команды */}
        <Form.Item label="Команда" style={{ marginBottom: 12 }}>
          <div style={{ display: 'flex', border: '1px solid #d9d9d9', borderRadius: 6, overflow: 'hidden' }}>
            {[
              { id: homeTeamId, name: homeTeamName },
              { id: guestTeamId, name: guestTeamName },
            ].map((t, idx) => (
              <button
                key={t.id}
                type="button"
                onClick={() => handleTeamChange(t.id)}
                title={t.name}
                style={{
                  flex: 1,
                  border: 'none',
                  borderRight: idx === 0 ? '1px solid #d9d9d9' : undefined,
                  background: selectedTeamId === t.id ? '#e6f4ff' : '#fff',
                  color: selectedTeamId === t.id ? '#1677ff' : '#595959',
                  fontWeight: selectedTeamId === t.id ? 700 : 400,
                  cursor: 'pointer',
                  padding: '6px 10px',
                  whiteSpace: 'nowrap',
                  overflow: 'hidden',
                  textOverflow: 'ellipsis',
                  fontSize: 13,
                  transition: 'background 0.15s',
                  maxWidth: '50%',
                }}
              >
                {t.name}
              </button>
            ))}
          </div>
        </Form.Item>

        {!hasLineup && (
          <Alert
            message="Расстановка не настроена"
            description="Выберите любого игрока команды."
            type="warning"
            showIcon
            style={{ marginBottom: 12 }}
          />
        )}

        {blockedCount > 0 && (
          <Alert
            message={`${blockedCount} игрок(а) недоступны`}
            description="Уже выходили на площадку в этой партии. Выберите выходящего игрока, чтобы увидеть доступных для замены."
            type="info"
            showIcon
            style={{ marginBottom: 12 }}
          />
        )}

        {/* выходящий игрок */}
        <Form.Item name="subOutPlayerId" label="Выходящий (на замену)" rules={[{ required: true, message: 'Выберите игрока' }]}>
          {outgoingOptions.length === 0 ? (
            <Text type="secondary">Нет игроков в команде</Text>
          ) : (
            <Select
              placeholder="Игрок из расстановки"
              options={outgoingOptions}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
              onChange={(v: number) => {
                setSelectedSubOutId(v);
                form.setFieldValue('subInPlayerId', undefined);
              }}
            />
          )}
        </Form.Item>

        {/* входящий игрок */}
        <Form.Item
          name="subInPlayerId"
          label="Входящий (со скамейки)"
          dependencies={['subOutPlayerId']}
          rules={[
            { required: true, message: 'Выберите игрока' },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || value !== getFieldValue('subOutPlayerId')) return Promise.resolve();
                return Promise.reject(new Error('Входящий игрок не может совпадать с выходящим'));
              },
            }),
          ]}
        >
          {incomingOptions.length === 0 ? (
            <Text type="secondary" style={{ fontSize: 12 }}>
              {!selectedSubOutId
                ? 'Сначала выберите уходящего игрока'
                : 'Нет доступных игроков (все кандидаты уже выходили в этой партии)'}
            </Text>
          ) : (
            <Select
              placeholder="Игрок со скамейки"
              options={incomingOptions}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
            />
          )}
        </Form.Item>

        {/* тип замены */}
        <Form.Item name="subTypeCode" label="Тип замены">
          <Select
            placeholder="Тип замены (необязательно)"
            allowClear
            options={substitutionTypes.map(t => ({ value: t.code, label: t.name }))}
          />
        </Form.Item>

        {/* либеро */}
        <Form.Item name="isLiberoSwap" valuePropName="checked" initialValue={false}>
          <Checkbox>Замена либеро</Checkbox>
        </Form.Item>

        <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, marginTop: 8 }}>
          <Button onClick={handleCancel}>Отмена</Button>
          <Button type="primary" onClick={handleOk} disabled={!canSubmit}>
            Подтвердить замену
          </Button>
        </div>
      </Form>
    </Modal>
  );
};

export default SubstitutionModal;
