import { type FC } from 'react';
import { Clock, CheckCircle, XCircle, User, Mail } from 'lucide-react';
import type { AccessRequest } from '../../entities/account/model/types';
import { RequestStatusEnum } from '../../entities/account/model/types';
import './AccessRequestCard.css';

interface AccessRequestCardProps {
  request: AccessRequest;
  type: 'received' | 'sent';
  onApprove?: (id: string) => void;
  onReject?: (id: string) => void;
  onCancel?: (id: string) => void;
}

export const AccessRequestCard: FC<AccessRequestCardProps> = ({
  request,
  type,
  onApprove,
  onReject,
  onCancel,
}) => {
  const getStatusInfo = () => {
    switch (request.status) {
      case RequestStatusEnum.Pendente:
        return { icon: Clock, label: 'Pendente', className: 'pending' };
      case RequestStatusEnum.Aprovada:
        return { icon: CheckCircle, label: 'Aprovada', className: 'approved' };
      case RequestStatusEnum.Rejeitada:
        return { icon: XCircle, label: 'Rejeitada', className: 'rejected' };
      case RequestStatusEnum.Cancelada:
        return { icon: XCircle, label: 'Cancelada', className: 'cancelled' };
      default:
        return { icon: Clock, label: 'Pendente', className: 'pending' };
    }
  };

  const statusInfo = getStatusInfo();
  const StatusIcon = statusInfo.icon;

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const isPending = request.status === RequestStatusEnum.Pendente;

  return (
    <div className={`access-request-card access-request-card--${statusInfo.className}`}>
      <div className="access-request-card__header">
        <div className="access-request-card__status">
          <StatusIcon size={20} />
          <span>{statusInfo.label}</span>
        </div>
        <div className="access-request-card__date">
          {formatDate(request.createdAt)}
        </div>
      </div>

      <div className="access-request-card__content">
        <div className="access-request-card__account">
          <h3 className="access-request-card__account-name">{request.contaNome}</h3>
        </div>

        <div className="access-request-card__user-info">
          {type === 'received' ? (
            <>
              <div className="access-request-card__user-item">
                <User size={16} />
                <span>Solicitante: <strong>{request.solicitanteNome}</strong></span>
              </div>
              <div className="access-request-card__user-item">
                <Mail size={16} />
                <span>{request.solicitanteEmail}</span>
              </div>
            </>
          ) : (
            <>
              <div className="access-request-card__user-item">
                <User size={16} />
                <span>Proprietário: <strong>{request.proprietarioNome}</strong></span>
              </div>
            </>
          )}
        </div>

        {request.mensagem && (
          <div className="access-request-card__message">
            <p><strong>Mensagem:</strong></p>
            <p>{request.mensagem}</p>
          </div>
        )}

        {request.dataResposta && (
          <div className="access-request-card__response-date">
            Respondida em: {formatDate(request.dataResposta)}
          </div>
        )}
      </div>

      {isPending && (
        <div className="access-request-card__actions">
          {type === 'received' ? (
            <>
              {onApprove && (
                <button
                  className="access-request-card__btn access-request-card__btn--approve"
                  onClick={() => onApprove(request.id)}
                >
                  <CheckCircle size={18} />
                  Aprovar
                </button>
              )}
              {onReject && (
                <button
                  className="access-request-card__btn access-request-card__btn--reject"
                  onClick={() => onReject(request.id)}
                >
                  <XCircle size={18} />
                  Rejeitar
                </button>
              )}
            </>
          ) : (
            <>
              {onCancel && (
                <button
                  className="access-request-card__btn access-request-card__btn--cancel"
                  onClick={() => onCancel(request.id)}
                >
                  <XCircle size={18} />
                  Cancelar Solicitação
                </button>
              )}
            </>
          )}
        </div>
      )}
    </div>
  );
};
