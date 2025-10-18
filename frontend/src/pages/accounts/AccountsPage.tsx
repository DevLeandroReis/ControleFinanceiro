import { type FC, useState, useEffect } from 'react';
import {
  Wallet,
  Plus,
  Key,
  Inbox,
  Send,
  Loader2,
  AlertCircle,
  Edit2,
  X,
} from 'lucide-react';
import { useAccountStore } from '../../entities/account';
import { AccountCard } from '../../features/account-card';
import { AccessRequestCard } from '../../features/access-request-card';
import { AccountUsersModal } from '../../features/account-users-modal';
import { RequestAccessModal } from '../../features/request-access-modal';
import type { Account, AccessRequest } from '../../entities/account/model/types';
import './AccountsPage.css';

type TabType = 'accounts' | 'received' | 'sent';

export const AccountsPage: FC = () => {
  const {
    accounts,
    receivedRequests,
    sentRequests,
    isLoading,
    error,
    fetchAccounts,
    fetchReceivedRequests,
    fetchSentRequests,
    addAccount,
    updateAccount,
    deleteAccount,
    activateAccount,
    deactivateAccount,
    approveRequest,
    rejectRequest,
    cancelRequest,
  } = useAccountStore();

  const [activeTab, setActiveTab] = useState<TabType>('accounts');
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [showRequestModal, setShowRequestModal] = useState(false);
  const [showUsersModal, setShowUsersModal] = useState(false);
  const [selectedAccount, setSelectedAccount] = useState<Account | null>(null);
  const [editingAccount, setEditingAccount] = useState<Account | null>(null);

  // Form states
  const [formData, setFormData] = useState({
    nome: '',
    descricao: '',
  });
  const [formError, setFormError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    fetchAccounts();
    fetchReceivedRequests();
    fetchSentRequests();
  }, [fetchAccounts, fetchReceivedRequests, fetchSentRequests]);

  const handleCreateAccount = async (e: React.FormEvent) => {
    e.preventDefault();
    setFormError(null);

    if (!formData.nome.trim()) {
      setFormError('O nome da conta é obrigatório');
      return;
    }

    setIsSubmitting(true);
    try {
      if (editingAccount) {
        await updateAccount(editingAccount.id, {
          nome: formData.nome.trim(),
          descricao: formData.descricao.trim() || undefined,
        });
      } else {
        await addAccount({
          nome: formData.nome.trim(),
          descricao: formData.descricao.trim() || undefined,
        });
      }
      handleCancelForm();
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Erro ao salvar conta');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleEditAccount = (account: Account) => {
    setEditingAccount(account);
    setFormData({
      nome: account.nome,
      descricao: account.descricao || '',
    });
    setShowCreateForm(true);
  };

  const handleCancelForm = () => {
    setShowCreateForm(false);
    setEditingAccount(null);
    setFormData({ nome: '', descricao: '' });
    setFormError(null);
  };

  const handleDeleteAccount = async (id: string) => {
    if (!confirm('Deseja realmente excluir esta conta? Esta ação não pode ser desfeita.')) {
      return;
    }

    try {
      await deleteAccount(id);
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Erro ao excluir conta');
    }
  };

  const handleToggleActive = async (id: string, isActive: boolean) => {
    try {
      if (isActive) {
        await activateAccount(id);
      } else {
        await deactivateAccount(id);
      }
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Erro ao alterar status da conta');
    }
  };

  const handleManageUsers = (account: Account) => {
    setSelectedAccount(account);
    setShowUsersModal(true);
  };

  const handleApproveRequest = async (id: string) => {
    if (!confirm('Deseja aprovar esta solicitação de acesso?')) return;
    
    try {
      await approveRequest(id);
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Erro ao aprovar solicitação');
    }
  };

  const handleRejectRequest = async (id: string) => {
    if (!confirm('Deseja rejeitar esta solicitação de acesso?')) return;
    
    try {
      await rejectRequest(id);
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Erro ao rejeitar solicitação');
    }
  };

  const handleCancelRequest = async (id: string) => {
    if (!confirm('Deseja cancelar esta solicitação?')) return;
    
    try {
      await cancelRequest(id);
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Erro ao cancelar solicitação');
    }
  };

  const pendingReceivedCount = receivedRequests.filter((r: { status: number }) => r.status === 0).length;
  const pendingSentCount = sentRequests.filter((r: { status: number }) => r.status === 0).length;

  return (
    <div className="accounts-page">
      <header className="accounts-page__header">
        <div className="accounts-page__header-content">
          <div className="accounts-page__title-section">
            <Wallet size={32} strokeWidth={2.5} />
            <div>
              <h1 className="accounts-page__title">Contas</h1>
              <p className="accounts-page__subtitle">
                Gerencie suas contas e compartilhe com outras pessoas
              </p>
            </div>
          </div>

          <div className="accounts-page__actions">
            <button
              className="btn btn--secondary"
              onClick={() => setShowRequestModal(true)}
              title="Solicitar acesso a uma conta existente"
            >
              <Key size={18} />
              Solicitar Acesso
            </button>
            <button
              className="btn btn--primary"
              onClick={() => setShowCreateForm(!showCreateForm)}
              title={showCreateForm ? 'Cancelar' : 'Criar uma nova conta'}
            >
              {showCreateForm ? (
                <>
                  <X size={18} />
                  Cancelar
                </>
              ) : (
                <>
                  <Plus size={18} />
                  Nova Conta
                </>
              )}
            </button>
          </div>
        </div>

        {/* Tabs */}
        <nav className="accounts-page__tabs" role="tablist" aria-label="Navegação de contas">
          <button
            className={`accounts-page__tab ${activeTab === 'accounts' ? 'accounts-page__tab--active' : ''}`}
            onClick={() => setActiveTab('accounts')}
            role="tab"
            aria-selected={activeTab === 'accounts'}
            aria-controls="accounts-panel"
          >
            <Wallet size={18} />
            <span>Minhas Contas</span>
            <span className="accounts-page__tab-badge" aria-label={`${accounts.length} contas`}>
              {accounts.length}
            </span>
          </button>
          <button
            className={`accounts-page__tab ${activeTab === 'received' ? 'accounts-page__tab--active' : ''}`}
            onClick={() => setActiveTab('received')}
            role="tab"
            aria-selected={activeTab === 'received'}
            aria-controls="received-panel"
          >
            <Inbox size={18} />
            <span>Recebidas</span>
            {pendingReceivedCount > 0 && (
              <span 
                className="accounts-page__tab-badge accounts-page__tab-badge--alert"
                aria-label={`${pendingReceivedCount} solicitações pendentes`}
              >
                {pendingReceivedCount}
              </span>
            )}
          </button>
          <button
            className={`accounts-page__tab ${activeTab === 'sent' ? 'accounts-page__tab--active' : ''}`}
            onClick={() => setActiveTab('sent')}
            role="tab"
            aria-selected={activeTab === 'sent'}
            aria-controls="sent-panel"
          >
            <Send size={18} />
            <span>Enviadas</span>
            {pendingSentCount > 0 && (
              <span 
                className="accounts-page__tab-badge"
                aria-label={`${pendingSentCount} solicitações pendentes`}
              >
                {pendingSentCount}
              </span>
            )}
          </button>
        </nav>
      </header>

      {/* Create/Edit Form */}
      {showCreateForm && (
        <section className="accounts-page__form-container" aria-labelledby="form-title">
          <form className="account-form" onSubmit={handleCreateAccount}>
            <h3 className="account-form__title" id="form-title">
              {editingAccount ? (
                <>
                  <Edit2 size={20} />
                  Editar Conta
                </>
              ) : (
                <>
                  <Plus size={20} />
                  Nova Conta
                </>
              )}
            </h3>

            <div className="form-grid">
              <div className="form-group">
                <label htmlFor="nome">
                  Nome da Conta <span aria-label="obrigatório">*</span>
                </label>
                <input
                  id="nome"
                  type="text"
                  className="form-input"
                  placeholder="Ex: Conta Pessoal, Conta Conjunta..."
                  value={formData.nome}
                  onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                  disabled={isSubmitting}
                  required
                  aria-required="true"
                  aria-invalid={!!formError}
                />
              </div>

              <div className="form-group">
                <label htmlFor="descricao">Descrição (opcional)</label>
                <textarea
                  id="descricao"
                  className="form-textarea"
                  placeholder="Adicione uma descrição para ajudar a identificar esta conta..."
                  rows={3}
                  value={formData.descricao}
                  onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
                  disabled={isSubmitting}
                />
              </div>
            </div>

            {formError && (
              <div className="form-error" role="alert" aria-live="assertive">
                <AlertCircle size={16} />
                <span>{formError}</span>
              </div>
            )}

            <div className="account-form__actions">
              <button
                type="button"
                className="btn btn--secondary"
                onClick={handleCancelForm}
                disabled={isSubmitting}
              >
                Cancelar
              </button>
              <button
                type="submit"
                className="btn btn--primary"
                disabled={isSubmitting || !formData.nome.trim()}
              >
                {isSubmitting ? (
                  <>
                    <Loader2 size={18} className="spinning" />
                    Salvando...
                  </>
                ) : editingAccount ? (
                  'Atualizar Conta'
                ) : (
                  'Criar Conta'
                )}
              </button>
            </div>
          </form>
        </section>
      )}

      {/* Error Message */}
      {error && (
        <div className="accounts-page__error" role="alert" aria-live="polite">
          <AlertCircle size={20} />
          <span>{error}</span>
        </div>
      )}

      {/* Loading State */}
      {isLoading && !accounts.length && !receivedRequests.length && !sentRequests.length ? (
        <div className="accounts-page__loading" role="status" aria-live="polite">
          <Loader2 size={32} className="spinning" aria-hidden="true" />
          <p>Carregando suas contas...</p>
        </div>
      ) : (
        /* Tab Content */
        <main className="accounts-page__content">
          {/* My Accounts Tab */}
          {activeTab === 'accounts' && (
            <div 
              className="accounts-grid" 
              role="tabpanel" 
              id="accounts-panel"
              aria-labelledby="tab-accounts"
            >
              {accounts.length === 0 ? (
                <div className="accounts-page__empty">
                  <Wallet size={64} strokeWidth={1.5} aria-hidden="true" />
                  <h3>Nenhuma conta ainda</h3>
                  <p>Crie sua primeira conta ou solicite acesso a uma conta existente</p>
                  <div className="accounts-page__empty-actions">
                    <button className="btn btn--primary" onClick={() => setShowCreateForm(true)}>
                      <Plus size={18} />
                      Criar Conta
                    </button>
                    <button
                      className="btn btn--secondary"
                      onClick={() => setShowRequestModal(true)}
                    >
                      <Key size={18} />
                      Solicitar Acesso
                    </button>
                  </div>
                </div>
              ) : (
                accounts.map((account: Account) => (
                  <AccountCard
                    key={account.id}
                    account={account}
                    onEdit={handleEditAccount}
                    onDelete={handleDeleteAccount}
                    onToggleActive={handleToggleActive}
                    onManageUsers={handleManageUsers}
                  />
                ))
              )}
            </div>
          )}

          {/* Received Requests Tab */}
          {activeTab === 'received' && (
            <div 
              className="requests-list"
              role="tabpanel"
              id="received-panel"
              aria-labelledby="tab-received"
            >
              {receivedRequests.length === 0 ? (
                <div className="accounts-page__empty">
                  <Inbox size={64} strokeWidth={1.5} aria-hidden="true" />
                  <h3>Nenhuma solicitação recebida</h3>
                  <p>Quando alguém solicitar acesso às suas contas, aparecerá aqui</p>
                </div>
              ) : (
                receivedRequests.map((request: AccessRequest) => (
                  <AccessRequestCard
                    key={request.id}
                    request={request}
                    type="received"
                    onApprove={handleApproveRequest}
                    onReject={handleRejectRequest}
                  />
                ))
              )}
            </div>
          )}

          {/* Sent Requests Tab */}
          {activeTab === 'sent' && (
            <div 
              className="requests-list"
              role="tabpanel"
              id="sent-panel"
              aria-labelledby="tab-sent"
            >
              {sentRequests.length === 0 ? (
                <div className="accounts-page__empty">
                  <Send size={64} strokeWidth={1.5} aria-hidden="true" />
                  <h3>Nenhuma solicitação enviada</h3>
                  <p>Solicite acesso a uma conta usando o ID compartilhado</p>
                  <button
                    className="btn btn--primary"
                    onClick={() => setShowRequestModal(true)}
                  >
                    <Key size={18} />
                    Solicitar Acesso
                  </button>
                </div>
              ) : (
                sentRequests.map((request: AccessRequest) => (
                  <AccessRequestCard
                    key={request.id}
                    request={request}
                    type="sent"
                    onCancel={handleCancelRequest}
                  />
                ))
              )}
            </div>
          )}
        </main>
      )}

      {/* Modals */}
      <RequestAccessModal
        isOpen={showRequestModal}
        onClose={() => setShowRequestModal(false)}
        onSuccess={() => {
          fetchSentRequests();
        }}
      />

      {selectedAccount && (
        <AccountUsersModal
          account={selectedAccount}
          isOpen={showUsersModal}
          onClose={() => {
            setShowUsersModal(false);
            setSelectedAccount(null);
          }}
        />
      )}
    </div>
  );
};
