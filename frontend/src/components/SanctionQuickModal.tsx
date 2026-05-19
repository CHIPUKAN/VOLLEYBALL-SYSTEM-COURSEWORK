import React from 'react';
import { Modal, Form, Select, InputNumber, Row, Col, Button, Typography } from 'antd';
import type { Sanction, LookupDto, LookupItemDto } from '../types/index';

const { Text } = Typography;

interface SanctionQuickModalProps {
  open: boolean;
  matchId: number;
  setNumber: number;
  homeTeamId: number;
  homeTeamName: string;
  guestTeamId: number;
  guestTeamName: string;
  allPlayers: LookupItemDto[];
  sanctionTypes: LookupDto[];
  sanctionKinds: LookupDto[];
  recipientTypes: LookupDto[];
  onConfirm: (data: Partial<Sanction>) => void;
  onCancel: () => void;
}

// быстрая форма санкции (компактный лейаут)
const SanctionQuickModal: React.FC<SanctionQuickModalProps> = ({
  open,
  matchId,
  setNumber,
  homeTeamId,
  homeTeamName,
  guestTeamId,
  guestTeamName,
  allPlayers,
  sanctionTypes,
  sanctionKinds,
  recipientTypes,
  onConfirm,
  onCancel,
}) => {
  const [form] = Form.useForm();

  const handleOk = async () => {
    let values: Record<string, unknown>;
    try { values = await form.validateFields(); } catch { return; }
    onConfirm({ matchId, setNumber, ...values });
    form.resetFields();
  };

  const handleCancel = () => {
    form.resetFields();
    onCancel();
  };

  return (
    <Modal
      open={open}
      title="Санкция"
      onCancel={handleCancel}
      footer={null}
      destroyOnClose
      afterClose={() => form.resetFields()}
      width={500}
    >
      <Text type="secondary" style={{ display: 'block', marginBottom: 12 }}>
        Партия: {setNumber} (предзаполнено)
      </Text>
      <Form form={form} layout="vertical">
        <Row gutter={12}>
          <Col xs={24} sm={12}>
            <Form.Item name="teamId" label="Команда" rules={[{ required: true }]}>
              <Select
                options={[
                  { value: homeTeamId, label: homeTeamName },
                  { value: guestTeamId, label: guestTeamName },
                ]}
              />
            </Form.Item>
          </Col>
          <Col xs={24} sm={12}>
            <Form.Item name="playerId" label="Игрок">
              <Select
                allowClear
                placeholder="Не указан"
                options={allPlayers.map(p => ({ value: Number(p.id), label: p.name }))}
                showSearch
                filterOption={(input, opt) => (opt?.label as string ?? '').toLowerCase().includes(input.toLowerCase())}
              />
            </Form.Item>
          </Col>
        </Row>
        <Row gutter={12}>
          <Col xs={24} sm={8}>
            <Form.Item name="recipientTypeCode" label="Получатель" rules={[{ required: true }]}>
              <Select options={recipientTypes.map(t => ({ value: t.code, label: t.name }))} />
            </Form.Item>
          </Col>
          <Col xs={24} sm={8}>
            <Form.Item name="sanctionTypeCode" label="Тип санкции" rules={[{ required: true }]}>
              <Select options={sanctionTypes.map(t => ({ value: t.code, label: t.name }))} />
            </Form.Item>
          </Col>
          <Col xs={24} sm={8}>
            <Form.Item name="sanctionKindCode" label="Вид" rules={[{ required: true }]}>
              <Select options={sanctionKinds.map(k => ({ value: k.code, label: k.name }))} />
            </Form.Item>
          </Col>
        </Row>
        <Form.Item name="minuteMark" label="Минута">
          <InputNumber min={0} style={{ width: '100%' }} />
        </Form.Item>

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
