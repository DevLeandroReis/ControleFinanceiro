import { type FC, useState } from 'react';
import { X, Key, Send, Loader2 } from 'lucide-react';
import { useAccountStore } from '../../entities/account';
import './RequestAccessModal.css';

interface RequestAccessModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess?: () => void;
}

export const RequestAccessModal: FC<RequestAccessModalProps> = ({
  isOpen,
  onClose,
  onSuccess,
}) => {
  const { requestAccess, isLoading } = useAccountStore();
  const [accountId, setAccountId] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!accountId.trim()) {
      setError('Por favor, insira o ID da conta');
      return;
    }

    try {
      await requestAccess(accountId.trim(), message.trim() || undefined);
      setSuccess(true);
      setTimeout(() => {
        onSuccess?.();
        handleClose();
      }, 2000);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao solicitar acesso');
    }
  };

  const handleClose = () => {
    setAccountId('');
    setMessage('');
    setError(null);
    setSuccess(false);
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={handleClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <div className="modal-header__title-section">
            <Key size={24} />
            <div>
              <h2 className="modal-title">Solicitar Acesso</h2>
              <p className="modal-subtitle">Insira o ID da conta que deseja acessar</p>
            </div>
          </div>
          <button className="modal-close" onClick={handleClose} title="Fechar">
            <X size={24} />
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="modal-body">
            {success ? (
              <div className="request-access-success">
                <div className="request-access-success__icon">
                  <Send size={48} />
                </div>
                <h3>Solicitação Enviada!</h3>
                <p>Aguarde a aprovação do proprietário da conta.</p>
              </div>
            ) : (
              <>
                <div className="form-group">
                  <label htmlFor="accountId">ID da Conta *</label>
                  <input
                    id="accountId"
                    type="text"
                    className="form-input"
                    placeholder="Ex: 123e4567-e89b-12d3-a456-426614174000"
                    value={accountId}
                    onChange={(e) => setAccountId(e.target.value)}
                    disabled={isLoading}
                    required
                  />
                  <p className="form-hint">
                    Cole aqui o ID da conta compartilhado pelo proprietário
                  </p>
                </div>

                <div className="form-group">
                  <label htmlFor="message">Mensagem (opcional)</label>
                  <textarea
                    id="message"
                    className="form-textarea"
                    placeholder="Adicione uma mensagem explicando por que deseja acessar esta conta..."
                    rows={4}
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    disabled={isLoading}
                  />
                </div>

                {error && (
                  <div className="form-error" role="alert">
                    {error}
                  </div>
                )}
              </>
            )}
          </div>

          {!success && (
            <div className="modal-footer">
              <button
                type="button"
                className="btn btn--secondary"
                onClick={handleClose}
                disabled={isLoading}
              >
                Cancelar
              </button>
              <button
                type="submit"
                className="btn btn--primary"
                disabled={isLoading || !accountId.trim()}
              >
                {isLoading ? (
                  <>
                    <Loader2 size={18} className="spinning" />
                    Enviando...
                  </>
                ) : (
                  <>
                    <Send size={18} />
                    Enviar Solicitação
                  </>
                )}
              </button>
            </div>
          )}
        </form>
      </div>
    </div>
  );
};
