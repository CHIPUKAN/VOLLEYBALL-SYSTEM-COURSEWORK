import React, { useEffect, useState } from 'react';
import { Modal, Button, Typography, Space } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import QRCode from 'qrcode';

const { Text, Title } = Typography;

interface QrModalProps {
  open: boolean;
  matchId: number;
  homeTeamName: string;
  guestTeamName: string;
  homeScore: number;
  guestScore: number;
  onClose: () => void;
}

// модал с QR-кодом результата матча
const QrModal: React.FC<QrModalProps> = ({
  open,
  matchId,
  homeTeamName,
  guestTeamName,
  homeScore,
  guestScore,
  onClose,
}) => {
  const [dataUrl, setDataUrl] = useState<string>('');

  useEffect(() => {
    if (!open) return;
    const url = `${window.location.origin}/matches/${matchId}`;
    QRCode.toDataURL(url, { width: 220, margin: 2 })
      .then(setDataUrl)
      .catch(() => setDataUrl(''));
  }, [open, matchId]);

  const handleDownload = () => {
    if (!dataUrl) return;
    const a = document.createElement('a');
    a.href = dataUrl;
    a.download = `result_${matchId}.png`;
    a.click();
  };

  return (
    <Modal
      open={open}
      title="QR-код результата"
      onCancel={onClose}
      footer={null}
      width={320}
      destroyOnClose
    >
      <Space direction="vertical" align="center" style={{ width: '100%', padding: '12px 0' }}>
        {dataUrl ? (
          <img src={dataUrl} alt="QR-код" style={{ borderRadius: 8, boxShadow: '0 2px 8px rgba(0,0,0,0.1)' }} />
        ) : (
          <div style={{ width: 220, height: 220, background: '#f5f5f5', borderRadius: 8, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <Text type="secondary">Генерация...</Text>
          </div>
        )}

        <Title level={4} style={{ margin: '8px 0 4px', textAlign: 'center' }}>
          {homeTeamName} {homeScore}:{guestScore} {guestTeamName}
        </Title>

        <Button
          icon={<DownloadOutlined />}
          onClick={handleDownload}
          disabled={!dataUrl}
        >
          Скачать
        </Button>
      </Space>
    </Modal>
  );
};

export default QrModal;
