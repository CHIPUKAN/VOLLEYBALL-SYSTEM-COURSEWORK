import React, { useState } from 'react';
import { Modal, Form, Select, Checkbox, Button, Radio, Typography, Space, Alert } from 'antd';
import type { StartingLineup, Player, LookupDto } from '../types/index';

const { Text } = Typography;

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
  onConfirm,
  onCancel,
}) => {
  const [form] = Form.useForm();
  const [selectedTeamId, setSelectedTeamId] = useState<number>(homeTeamId);
  const [selectedSubOutId, setSelectedSubOutId] = useState<number | undefined>();

  const currentLineup = selectedTeamId === homeTeamId ? homeLineup : guestLineup;
  const allPlayers = selectedTeamId === homeTeamId ? allHomePlayers : allGuestPlayers;
  const hasLineup = currentLineup.length > 0;

  const lineupPlayerIds = new Set(currentLineup.map(l => l.playerId));

  // если расстановка настроена — уходит только тот, кто в расстановке
  // если нет — уходит любой игрок команды
  const outgoingOptions = hasLineup
    ? currentLineup.map(l => ({
        value: l.playerId,
        label: `${l.shirtNumber ? '#' + l.shirtNumber + ' ' : ''}${l.playerFullName ?? `Игрок ${l.playerId}`}`,
      }))
    : allPlayers.map(p => ({
        value: p.id,
        label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
      }));

  // приходит тот, кого нет в расстановке (или не совпадает с выбранным уходящим)
  const incomingOptions = hasLineup
    ? allPlayers
        .filter(p => !lineupPlayerIds.has(p.id))
        .map(p => ({
          value: p.id,
          label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
        }))
    : allPlayers
        .filter(p => p.id !== selectedSubOutId)
        .map(p => ({
          value: p.id,
          label: `${p.jerseyNumber ? '#' + p.jerseyNumber + ' ' : ''}${p.fullName ?? p.lastName}`,
        }));

  const handleOk = async () => {
    let values: Record<string, unknown>;
    try { values = await form.validateFields(); } catch { return; }
    onConfirm({
      teamId: selectedTeamId,
      subOutPlayerId: values.subOutPlayerId as number,
      subInPlayerId: values.subInPlayerId as number,
      subTypeCode: values.subTypeCode as number,
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
      destroyOnClose
      afterClose={() => { form.resetFields(); setSelectedSubOutId(undefined); }}
      width={480}
    >
      <Form form={form} layout="vertical" style={{ marginTop: 12 }}>
        {/* выбор команды */}
        <Form.Item label="Команда">
          <Radio.Group
            value={selectedTeamId}
            onChange={e => handleTeamChange(e.target.value)}
            style={{ width: '100%' }}
          >
            <Space direction="horizontal" style={{ width: '100%', justifyContent: 'space-between' }}>
              <Radio.Button value={homeTeamId} style={{ width: '48%', textAlign: 'center' }}>
                {homeTeamName}
              </Radio.Button>
              <Radio.Button value={guestTeamId} style={{ width: '48%', textAlign: 'center' }}>
                {guestTeamName}
              </Radio.Button>
            </Space>
          </Radio.Group>
        </Form.Item>

        {!hasLineup && (
          <Alert
            message="Расстановка не настроена"
            description="Выберите любого игрока команды как уходящего и входящего."
            type="warning"
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
              placeholder={hasLineup ? 'Игрок из расстановки' : 'Любой игрок команды'}
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
        <Form.Item name="subInPlayerId" label="Входящий (со скамейки)" rules={[{ required: true, message: 'Выберите игрока' }]}>
          {incomingOptions.length === 0 ? (
            <Text type="secondary">
              {selectedSubOutId ? 'Нет доступных игроков' : 'Сначала выберите уходящего игрока'}
            </Text>
          ) : (
            <Select
              placeholder={hasLineup ? 'Игрок со скамейки' : 'Любой другой игрок команды'}
              options={incomingOptions}
              showSearch
              filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
            />
          )}
        </Form.Item>

        {/* тип замены */}
        <Form.Item name="subTypeCode" label="Тип замены" rules={[{ required: true, message: 'Выберите тип' }]}>
          <Select
            placeholder="Тип замены"
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
