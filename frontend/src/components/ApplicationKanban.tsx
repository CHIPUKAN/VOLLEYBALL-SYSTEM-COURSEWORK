import React, { useState } from 'react';
import { Typography, Modal, Input, App } from 'antd';
import { useNavigate } from 'react-router-dom';
import type { Application, LookupDto } from '../types/index';

const { Text } = Typography;

interface ApplicationKanbanProps {
  applications: Application[];
  statuses: LookupDto[];
  onStatusChange: (appId: number, statusCode: number, comment?: string) => Promise<void>;
  canReview: boolean;
}

const COLUMNS = [
  { statusCode: 1, statusName: 'На рассмотрении', color: '#378ADD', bgColor: '#E6F1FB' },
  { statusCode: 2, statusName: 'Принята',         color: '#3B6D11', bgColor: '#EAF3DE' },
  { statusCode: 3, statusName: 'Отклонена',       color: '#A32D2D', bgColor: '#FCEBEB' },
  { statusCode: 4, statusName: 'Отозвана',        color: '#888888', bgColor: '#F2F2F2' },
];

// kanban заявок на турниры
const ApplicationKanban: React.FC<ApplicationKanbanProps> = ({
  applications,
  statuses,
  onStatusChange,
  canReview,
}) => {
  const { message } = App.useApp();
  const navigate = useNavigate();
  const [dragOverCol, setDragOverCol] = useState<number | null>(null);
  const [optimistic, setOptimistic] = useState<Map<number, number>>(new Map());
  const [rejectModal, setRejectModal] = useState<{ appId: number; teamName: string; tournamentId: number } | null>(null);
  const [rejectComment, setRejectComment] = useState('');

  const getStatusCode = (app: Application): number =>
    optimistic.get(app.id) ?? app.statusCode;

  const handleDrop = async (e: React.DragEvent, targetStatusCode: number, targetStatusName: string) => {
    e.preventDefault();
    setDragOverCol(null);
    if (!canReview) return;

    const appId = Number(e.dataTransfer.getData('appId'));
    const app = applications.find(a => a.id === appId);
    if (!app || getStatusCode(app) === targetStatusCode) return;

    if (targetStatusCode === 3) {
      setRejectModal({ appId, teamName: app.teamName ?? '?', tournamentId: app.tournamentId });
      setRejectComment('');
      return;
    }

    setOptimistic(prev => new Map(prev).set(appId, targetStatusCode));
    try {
      await onStatusChange(appId, targetStatusCode);

      if (targetStatusCode === 2) {
        Modal.confirm({
          title: 'Заявка одобрена!',
          content: `Назначить матч для команды ${app.teamName ?? '?'} в этом турнире?`,
          okText: 'Назначить матч',
          cancelText: 'Позже',
          onOk: () => navigate(`/matches?tournamentId=${app.tournamentId}&newMatch=true`),
        });
      }
    } catch {
      setOptimistic(prev => { const next = new Map(prev); next.delete(appId); return next; });
      message.error('Ошибка смены статуса');
    }
  };

  const confirmReject = async () => {
    if (!rejectModal) return;
    setOptimistic(prev => new Map(prev).set(rejectModal.appId, 3));
    try {
      await onStatusChange(rejectModal.appId, 3, rejectComment);
    } catch {
      setOptimistic(prev => { const next = new Map(prev); next.delete(rejectModal.appId); return next; });
      message.error('Ошибка отклонения');
    }
    setRejectModal(null);
  };

  return (
    <>
      <div style={{ display: 'flex', gap: 12, overflowX: 'auto', paddingBottom: 8 }}>
        {COLUMNS.map(col => {
          const colApps = applications.filter(a => getStatusCode(a) === col.statusCode);
          const isOver = dragOverCol === col.statusCode;

          return (
            <div
              key={col.statusCode}
              onDragOver={e => { e.preventDefault(); setDragOverCol(col.statusCode); }}
              onDragLeave={() => setDragOverCol(null)}
              onDrop={e => handleDrop(e, col.statusCode, col.statusName)}
              style={{
                minWidth: 240,
                width: 240,
                flexShrink: 0,
                background: isOver ? col.bgColor + 'cc' : col.bgColor,
                borderRadius: 10,
                border: isOver ? `2px solid ${col.color}` : '2px solid transparent',
                transition: 'border 0.15s',
              }}
            >
              {/* заголовок */}
              <div style={{ padding: '10px 12px 8px', borderBottom: `2px solid ${col.color}33`, display: 'flex', justifyContent: 'space-between' }}>
                <Text strong style={{ color: col.color, fontSize: 13 }}>{col.statusName}</Text>
                <Text style={{ color: col.color, fontSize: 12, fontWeight: 600 }}>{colApps.length}</Text>
              </div>

              {/* карточки */}
              <div style={{ height: 'calc(100vh - 220px)', overflowY: 'auto', padding: '8px', display: 'flex', flexDirection: 'column', gap: 8 }}>
                {colApps.map(app => (
                  <div
                    key={app.id}
                    draggable={canReview}
                    onDragStart={e => e.dataTransfer.setData('appId', String(app.id))}
                    style={{
                      background: '#fff',
                      borderRadius: 8,
                      padding: '10px',
                      boxShadow: '0 1px 4px rgba(0,0,0,0.08)',
                      borderLeft: `3px solid ${col.color}`,
                      cursor: canReview ? 'grab' : 'default',
                    }}
                  >
                    <Text strong style={{ fontSize: 13, display: 'block' }}>
                      {app.teamName ?? `Команда ${app.teamId}`}
                    </Text>
                    <Text style={{ fontSize: 11, color: '#888', display: 'block' }}>
                      {app.submittedAt ? app.submittedAt.slice(0, 10) : '—'}
                    </Text>
                    <Text style={{ fontSize: 11, color: '#aaa', display: 'block' }}>
                      {app.players.length} игроков
                    </Text>
                    {app.players.slice(0, 3).map(p => (
                      <Text key={p.playerId} style={{ fontSize: 10, color: '#888', display: 'block' }}>
                        #{p.shirtNumber ?? '?'} {p.playerName ?? `Игрок ${p.playerId}`}
                        {p.ampluaName ? ` (${p.ampluaName})` : ''}
                      </Text>
                    ))}
                    {app.players.length > 3 && (
                      <Text style={{ fontSize: 10, color: '#aaa' }}>+{app.players.length - 3} ещё</Text>
                    )}
                    {app.comment && (
                      <Text style={{ fontSize: 11, color: '#999', fontStyle: 'italic', display: 'block', marginTop: 4 }}>
                        {app.comment}
                      </Text>
                    )}
                  </div>
                ))}
                {colApps.length === 0 && (
                  <div style={{ textAlign: 'center', padding: '20px 0' }}>
                    <Text type="secondary" style={{ fontSize: 12 }}>Нет заявок</Text>
                  </div>
                )}
              </div>
            </div>
          );
        })}
      </div>

      {/* модал отклонения */}
      <Modal
        open={!!rejectModal}
        title={`Отклонить заявку: ${rejectModal?.teamName ?? ''}`}
        onOk={confirmReject}
        onCancel={() => setRejectModal(null)}
        okText="Отклонить"
        okButtonProps={{ danger: true }}
        cancelText="Отмена"
      >
        <Input.TextArea
          placeholder="Причина отклонения (опционально)"
          value={rejectComment}
          onChange={e => setRejectComment(e.target.value)}
          rows={3}
          style={{ marginTop: 12 }}
        />
      </Modal>
    </>
  );
};

export default ApplicationKanban;
