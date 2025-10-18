import { type FC, useState } from 'react';
import { Wallet, Users, Copy, Check, Edit2, Trash2, Power, Settings, Eye, EyeOff } from 'lucide-react';
import type { Account } from '../../entities/account/model/types';
import './AccountCard.css';

interface AccountCardProps {
  account: Account;
  onEdit?: (account: Account) => void;
  onDelete?: (id: string) => void;
  onToggleActive?: (id: string, isActive: boolean) => void;
  onManageUsers?: (account: Account) => void;
}

export const AccountCard: FC<AccountCardProps> = ({
  account,
  onEdit,
  onDelete,
  onToggleActive,
  onManageUsers,
}) => {
  const [copied, setCopied] = useState(false);
  const [showId, setShowId] = useState(false);

  const isOwner = account.usuarios?.[0]?.usuarioId === account.proprietarioId;

  const handleCopyId = async () => {
    try {
      await navigator.clipboard.writeText(account.id);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (error) {
      console.error('Erro ao copiar ID:', error);
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    });
  };

  return (
    <div className={`account-card ${!account.ativa ? 'account-card--inactive' : ''}`}>
      <div className="account-card__header">
        <div className="account-card__icon">
          <Wallet size={24} />
        </div>
        <div className="account-card__title-section">
          <h3 className="account-card__title">{account.nome}</h3>
          {account.descricao && (
            <p className="account-card__description">{account.descricao}</p>
          )}
        </div>
        {!account.ativa && (
          <span className="account-card__status-badge">Inativa</span>
        )}
      </div>

      <div className="account-card__info">
        <div className="account-card__info-item">
          <span className="account-card__info-label">Proprietário</span>
          <span className="account-card__info-value">{account.proprietarioNome}</span>
        </div>
        
        <div className="account-card__info-item">
          <span className="account-card__info-label">Usuários</span>
          <span className="account-card__info-value">
            <Users size={14} style={{ display: 'inline', marginRight: '4px' }} />
            {account.usuarios?.length || 0}
          </span>
        </div>

        <div className="account-card__info-item">
          <span className="account-card__info-label">Criada em</span>
          <span className="account-card__info-value">{formatDate(account.createdAt)}</span>
        </div>
      </div>

      <div className="account-card__id-section">
        <div className="account-card__id-header">
          <span className="account-card__id-label">ID da Conta</span>
          <button
            className="account-card__id-toggle"
            onClick={() => setShowId(!showId)}
            title={showId ? 'Ocultar ID' : 'Mostrar ID'}
          >
            {showId ? <EyeOff size={16} /> : <Eye size={16} />}
          </button>
        </div>
        {showId && (
          <div className="account-card__id-content">
            <code className="account-card__id">{account.id}</code>
            <button
              className={`account-card__copy-btn ${copied ? 'account-card__copy-btn--copied' : ''}`}
              onClick={handleCopyId}
              title="Copiar ID"
            >
              {copied ? <Check size={16} /> : <Copy size={16} />}
            </button>
          </div>
        )}
        <p className="account-card__id-hint">
          Compartilhe este ID com amigos para que possam solicitar acesso
        </p>
      </div>

      <div className="account-card__actions">
        {onManageUsers && (
          <button
            className="account-card__action-btn account-card__action-btn--primary"
            onClick={() => onManageUsers(account)}
            title="Gerenciar usuários"
          >
            <Settings size={18} />
            Gerenciar Usuários
          </button>
        )}
        
        {isOwner && (
          <>
            {onEdit && (
              <button
                className="account-card__action-btn"
                onClick={() => onEdit(account)}
                title="Editar conta"
              >
                <Edit2 size={18} />
              </button>
            )}
            
            {onToggleActive && (
              <button
                className="account-card__action-btn"
                onClick={() => onToggleActive(account.id, !account.ativa)}
                title={account.ativa ? 'Desativar conta' : 'Ativar conta'}
              >
                <Power size={18} />
              </button>
            )}
            
            {onDelete && (
              <button
                className="account-card__action-btn account-card__action-btn--danger"
                onClick={() => onDelete(account.id)}
                title="Excluir conta"
              >
                <Trash2 size={18} />
              </button>
            )}
          </>
        )}
      </div>
    </div>
  );
};
