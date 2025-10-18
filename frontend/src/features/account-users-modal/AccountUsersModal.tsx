import { type FC, useState, useEffect } from 'react';
import { X, Users, Shield, ShieldOff, Trash2, Mail, Calendar, Loader2 } from 'lucide-react';
import type { Account, UserAccount } from '../../entities/account/model/types';
import { useAccountStore } from '../../entities/account';
import './AccountUsersModal.css';

interface AccountUsersModalProps {
  account: Account;
  isOpen: boolean;
  onClose: () => void;
}

export const AccountUsersModal: FC<AccountUsersModalProps> = ({
  account,
  isOpen,
  onClose,
}) => {
  const {
    getAccountUsers,
    removeUserFromAccount,
    grantPermission,
    revokePermission,
    isLoading,
  } = useAccountStore();

  const [users, setUsers] = useState<UserAccount[]>([]);
  const [loadingUsers, setLoadingUsers] = useState(false);

  const loadUsers = async () => {
    setLoadingUsers(true);
    try {
      const accountUsers = await getAccountUsers(account.id);
      setUsers(accountUsers);
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    } finally {
      setLoadingUsers(false);
    }
  };

  useEffect(() => {
    if (isOpen && account) {
      loadUsers();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpen, account?.id]);

  const handleRemoveUser = async (usuarioId: string) => {
    if (!confirm('Deseja realmente remover este usuário da conta?')) return;

    try {
      await removeUserFromAccount(account.id, usuarioId);
      setUsers(users.filter((u) => u.usuarioId !== usuarioId));
    } catch (error) {
      console.error('Erro ao remover usuário:', error);
    }
  };

  const handleTogglePermission = async (user: UserAccount) => {
    try {
      if (user.podeAdicionarUsuarios) {
        await revokePermission(account.id, user.usuarioId);
      } else {
        await grantPermission(account.id, user.usuarioId);
      }
      // Update local state
      setUsers(
        users.map((u) =>
          u.usuarioId === user.usuarioId
            ? { ...u, podeAdicionarUsuarios: !u.podeAdicionarUsuarios }
            : u
        )
      );
    } catch (error) {
      console.error('Erro ao alterar permissão:', error);
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    });
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content modal-content--large" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-header__title-section">
            <Users size={24} />
            <div>
              <h2 className="modal-title">Gerenciar Usuários</h2>
              <p className="modal-subtitle">{account.nome}</p>
            </div>
          </div>
          <button className="modal-close" onClick={onClose} title="Fechar">
            <X size={24} />
          </button>
        </div>

        <div className="modal-body">
          {loadingUsers ? (
            <div className="modal-loading">
              <Loader2 size={32} className="spinning" />
              <p>Carregando usuários...</p>
            </div>
          ) : users.length === 0 ? (
            <div className="modal-empty">
              <Users size={48} />
              <p>Nenhum usuário nesta conta ainda.</p>
            </div>
          ) : (
            <div className="users-list">
              {users.map((user) => {
                const isOwner = user.usuarioId === account.proprietarioId;
                
                return (
                  <div key={user.id} className="user-card">
                    <div className="user-card__header">
                      <div className="user-card__avatar">
                        {user.usuarioNome.charAt(0).toUpperCase()}
                      </div>
                      <div className="user-card__info">
                        <div className="user-card__name">
                          {user.usuarioNome}
                          {isOwner && (
                            <span className="user-card__badge user-card__badge--owner">
                              Proprietário
                            </span>
                          )}
                          {user.podeAdicionarUsuarios && !isOwner && (
                            <span className="user-card__badge user-card__badge--admin">
                              Admin
                            </span>
                          )}
                        </div>
                        <div className="user-card__email">
                          <Mail size={14} />
                          {user.usuarioEmail}
                        </div>
                        <div className="user-card__date">
                          <Calendar size={14} />
                          Membro desde {formatDate(user.dataAdesao)}
                        </div>
                      </div>
                    </div>

                    {!isOwner && (
                      <div className="user-card__actions">
                        <button
                          className={`user-card__btn ${
                            user.podeAdicionarUsuarios
                              ? 'user-card__btn--warning'
                              : 'user-card__btn--primary'
                          }`}
                          onClick={() => handleTogglePermission(user)}
                          disabled={isLoading}
                          title={
                            user.podeAdicionarUsuarios
                              ? 'Remover permissão de admin'
                              : 'Conceder permissão de admin'
                          }
                        >
                          {user.podeAdicionarUsuarios ? (
                            <>
                              <ShieldOff size={18} />
                              Remover Admin
                            </>
                          ) : (
                            <>
                              <Shield size={18} />
                              Tornar Admin
                            </>
                          )}
                        </button>
                        <button
                          className="user-card__btn user-card__btn--danger"
                          onClick={() => handleRemoveUser(user.usuarioId)}
                          disabled={isLoading}
                          title="Remover usuário"
                        >
                          <Trash2 size={18} />
                          Remover
                        </button>
                      </div>
                    )}
                  </div>
                );
              })}
            </div>
          )}
        </div>

        <div className="modal-footer">
          <button className="btn btn--secondary" onClick={onClose}>
            Fechar
          </button>
        </div>
      </div>
    </div>
  );
};
