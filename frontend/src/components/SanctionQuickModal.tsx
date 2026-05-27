import React, { useState } from 'react';
import { Modal, Form, Select, InputNumber, Row, Col, Button, Typography } from 'antd';
import type { Sanction, LookupDto, LookupItemDto } from '../types/index';

// коды видов санкций: 2 = задержка игры (требует delayViolationCode)
// коды получателей: 1 = игрок, иное = официальный представитель

const { Text } = Typography;

interface SanctionQuickModalProps {
  open: boolean;
  matchId: number;
  setNumber: number;
  homeTeamId: number;
  homeTeamName: string;
  guestTeamId: number;
  guestTeamName: string;
  homePlayers: LookupItemDto[];
  guestPlayers: LookupItemDto[];
  sanctionTypes: LookupDto[];
  sanctionKinds: LookupDto[];
  recipientTypes: LookupDto[];
  homeScoreAtMoment: number;
  guestScoreAtMoment: number;
  nextMemberSeq: number;
  onConfirm: (data: Partial<Sanction>) => void;
  onCancel: () => void;
}

// быстрая форма санкции с фильтрацией игроков по выбранной команде
const SanctionQuickModal: React.FC<SanctionQuickModalProps> = ({
  open,
  matchId,
  setNumber,
  homeTeamId,
  homeTeamName,
  guestTeamId,
  guestTeamName,
  homePlayers,
  guestPlayers,
  sanctionTypes,
  sanctionKinds,
  recipientTypes,
  homeScoreAtMoment,
  guestScoreAtMoment,
  nextMemberSeq,
  onConfirm,
  onCancel,
}) => {
  const [form] = Form.useForm();
  const [selectedTeamId, setSelectedTeamId] = useState<number | undefined>();

  const recipientTypeCode = Form.useWatch('recipientTypeCode', form) as number | undefined;
  const sanctionKindCode  = Form.useWatch('sanctionKindCode', form) as number | undefined;

  const isPlayerRecipient = recipientTypeCode === 1;

  const teamPlayers = selectedTeamId === homeTeamId
    ? homePlayers
    : selectedTeamId === guestTeamId
    ? guestPlayers
    : [];

  const handleOk = async () => {
    let values: Record<string, unknown>;
    try { values = await form.validateFields(); } catch { return; }
    onConfirm({
      matchId,
      setNumber,
      homeScoreAtMoment,
      guestScoreAtMoment,
      memberSeqInMatch: nextMemberSeq,
      ...values,
    });
    form.resetFields();
    setSelectedTeamId(undefined);
  };

  const handleCancel = () => {
    form.resetFields();
    setSelectedTeamId(undefined);
    onCancel();
  };

  const handleTeamChange = (teamId: number) => {
    setSelectedTeamId(teamId);
    form.setFieldValue('playerId', undefined);
  };

  return (
    <Modal
      open={open}
      title="Санкция"
      onCancel={handleCancel}
      footer={null}
      destroyOnHidden
      afterClose={() => { form.resetFields(); setSelectedTeamId(undefined); }}
      width={500}
    >
      <Text type="secondary" style={{ display: 'block', marginBottom: 12 }}>
        Партия: {setNumber} (предзаполнено)
      </Text>
      <Form form={form} layout="vertical">
        <Row gutter={12}>
          <Col xs={24} sm={12}>
            <Form.Item name="teamId" label="Команда" rules={[{ required: true, message: 'Выберите команду' }]}>
              <Select
                options={[
                  { value: homeTeamId, label: homeTeamName },
                  { value: guestTeamId, label: guestTeamName },
                ]}
                onChange={handleTeamChange}
              />
            </Form.Item>
          </Col>
          <Col xs={24} sm={12}>
            {/* игрок — только для получателей типа «игрок» (code=1) */}
            {(recipientTypeCode === undefined || isPlayerRecipient) && (
              <Form.Item
                name="playerId"
                label="Игрок"
                rules={isPlayerRecipient ? [{ required: true, message: 'Выберите игрока' }] : []}
              >
                <Select
                  allowClear
                  disabled={!selectedTeamId}
                  placeholder={selectedTeamId ? 'Не указан' : 'Сначала выберите команду'}
                  options={teamPlayers.map(p => ({ value: Number(p.id), label: p.name }))}
                  showSearch
                  filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
                />
              </Form.Item>
            )}
            {/* представитель делегации — для получателей не-игроков */}
            {recipientTypeCode !== undefined && !isPlayerRecipient && (
              <Form.Item
                name="delegationMemberId"
                label="Представитель делегации (ID)"
                rules={[{ required: true, message: 'Укажите ID представителя' }]}
              >
                <InputNumber min={1} style={{ width: '100%' }} placeholder="ID участника делегации" />
              </Form.Item>
            )}
          </Col>
        </Row>
        <Row gutter={12}>
          <Col xs={24} sm={8}>
            <Form.Item name="recipientTypeCode" label="Получатель" rules={[{ required: true, message: 'Выберите получателя' }]}>
              <Select options={recipientTypes.map(t => ({ value: t.code, label: t.name }))} />
            </Form.Item>
          </Col>
          <Col xs={24} sm={8}>
            <Form.Item name="sanctionTypeCode" label="Тип санкции" rules={[{ required: true, message: 'Выберите тип' }]}>
              <Select options={sanctionTypes.map(t => ({ value: t.code, label: t.name }))} />
            </Form.Item>
          </Col>
          <Col xs={24} sm={8}>
            <Form.Item name="sanctionKindCode" label="Вид" rules={[{ required: true, message: 'Выберите вид' }]}>
              <Select options={sanctionKinds.map(k => ({ value: k.code, label: k.name }))} />
            </Form.Item>
          </Col>
        </Row>
        <Form.Item name="minuteMark" label="Минута">
          <InputNumber min={0} style={{ width: '100%' }} />
        </Form.Item>

        {/* код нарушения задержки — обязателен при виде санкции «задержка игры» (code=2) */}
        {sanctionKindCode === 2 && (
          <Form.Item
            name="delayViolationCode"
            label="Код нарушения задержки"
            rules={[{ required: true, message: 'Укажите код нарушения' }]}
          >
            <InputNumber min={1} style={{ width: '100%' }} placeholder="Код нарушения задержки игры" />
          </Form.Item>
        )}

        <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8 }}>
          <Button onClick={handleCancel}>Отмена</Button>
          <Button type="primary" danger onClick={handleOk}>
            Применить санкцию
          </Button>
        </div>
      </Form>
    </Modal>
  );
};

export default SanctionQuickModal;
