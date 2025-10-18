import { type FC, useState } from 'react';
import { MoreVertical, Edit, Trash2, Check, X, Clock, AlertCircle, Repeat, Calendar, DollarSign, FolderOpen, Building2 } from 'lucide-react';
import { TipoLancamento, StatusLancamento, TipoRecorrencia } from '../../../entities/transaction/model/types';
import type { Transaction } from '../../../entities/transaction/model/types';
import './TransactionCard.css';

interface TransactionCardProps {
  transaction: Transaction;
  onEdit: (transaction: Transaction) => void;
  onDelete: (id: string) => void;
  onMarcarPago: (id: string) => void;
  onMarcarPendente: (id: string) => void;
  onCancelar: (id: string) => void;
}

export const TransactionCard: FC<TransactionCardProps> = ({
  transaction,
  onEdit,
  onDelete,
  onMarcarPago,
  onMarcarPendente,
  onCancelar,
}) => {
  const [showMenu, setShowMenu] = useState(false);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  };

  const getStatusBadge = (status: number) => {
    const badges = {
      [StatusLancamento.Pendente]: {
        label: 'Pendente',
        className: 'badge-pendente',
        icon: Clock,
      },
      [StatusLancamento.Pago]: {
        label: 'Pago',
        className: 'badge-pago',
        icon: Check,
      },
      [StatusLancamento.Cancelado]: {
        label: 'Cancelado',
        className: 'badge-cancelado',
        icon: X,
      },
      [StatusLancamento.Vencido]: {
        label: 'Vencido',
        className: 'badge-vencido',
        icon: AlertCircle,
      },
    };

    const badge = badges[status];
    if (!badge) return null;

    const Icon = badge.icon;
    return (
      <span className={`status-badge ${badge.className}`}>
        <Icon size={14} />
        {badge.label}
      </span>
    );
  };

  const getTipoRecorrenciaLabel = (tipo: number) => {
    const labels = {
      [TipoRecorrencia.Nenhuma]: '',
      [TipoRecorrencia.Diaria]: 'Diária',
      [TipoRecorrencia.Semanal]: 'Semanal',
      [TipoRecorrencia.Mensal]: 'Mensal',
      [TipoRecorrencia.Anual]: 'Anual',
    };
    return labels[tipo] || '';
  };

  const tipoClass = transaction.tipo === TipoLancamento.Receita ? 'receita' : 'despesa';
  const isVencida = transaction.status === StatusLancamento.Vencido;
  const isPaga = transaction.status === StatusLancamento.Pago;
  const isCancelada = transaction.status === StatusLancamento.Cancelado;

  return (
    <div className={`transaction-card ${tipoClass} ${isVencida ? 'vencida' : ''}`}>
      <div className="card-header">
        <div className="card-title-section">
          <h3 className="card-title">{transaction.descricao}</h3>
          {transaction.ehRecorrente && (
            <span className="badge-recorrente" title={`Recorrência ${getTipoRecorrenciaLabel(transaction.tipoRecorrencia)}`}>
              <Repeat size={14} />
              {getTipoRecorrenciaLabel(transaction.tipoRecorrencia)}
            </span>
          )}
        </div>
        <div className="card-actions">
          <div className="valor-principal">
            {formatCurrency(transaction.valor)}
          </div>
          <div className="menu-container">
            <button
              className="btn-menu"
              onClick={() => setShowMenu(!showMenu)}
              aria-label="Menu de ações"
            >
              <MoreVertical size={20} />
            </button>
            {showMenu && (
              <div className="action-menu">
                {!isPaga && !isCancelada && (
                  <button
                    className="menu-item menu-item-success"
                    onClick={() => {
                      onMarcarPago(transaction.id);
                      setShowMenu(false);
                    }}
                  >
                    <Check size={16} />
                    Marcar como Pago
                  </button>
                )}
                {isPaga && (
                  <button
                    className="menu-item"
                    onClick={() => {
                      onMarcarPendente(transaction.id);
                      setShowMenu(false);
                    }}
                  >
                    <Clock size={16} />
                    Marcar como Pendente
                  </button>
                )}
                {!isCancelada && (
                  <>
                    <button
                      className="menu-item"
                      onClick={() => {
                        onEdit(transaction);
                        setShowMenu(false);
                      }}
                    >
                      <Edit size={16} />
                      Editar
                    </button>
                    <button
                      className="menu-item menu-item-warning"
                      onClick={() => {
                        onCancelar(transaction.id);
                        setShowMenu(false);
                      }}
                    >
                      <X size={16} />
                      Cancelar
                    </button>
                  </>
                )}
                <div className="menu-divider"></div>
                <button
                  className="menu-item menu-item-danger"
                  onClick={() => {
                    onDelete(transaction.id);
                    setShowMenu(false);
                  }}
                >
                  <Trash2 size={16} />
                  Excluir
                </button>
              </div>
            )}
          </div>
        </div>
      </div>

      <div className="card-body">
        <div className="card-info-grid">
          <div className="info-item">
            <Calendar size={16} />
            <span className="info-label">Vencimento:</span>
            <span className="info-value">{formatDate(transaction.dataVencimento)}</span>
          </div>
          
          {transaction.dataPagamento && (
            <div className="info-item">
              <Check size={16} />
              <span className="info-label">Pagamento:</span>
              <span className="info-value">{formatDate(transaction.dataPagamento)}</span>
            </div>
          )}

          {transaction.categoriaNome && (
            <div className="info-item">
              <FolderOpen size={16} />
              <span className="info-label">Categoria:</span>
              <span className="info-value">{transaction.categoriaNome}</span>
            </div>
          )}

          {transaction.contaNome && (
            <div className="info-item">
              <Building2 size={16} />
              <span className="info-label">Conta:</span>
              <span className="info-value">{transaction.contaNome}</span>
            </div>
          )}

          {transaction.quantidadeParcelas && transaction.parcelaAtual && (
            <div className="info-item">
              <DollarSign size={16} />
              <span className="info-label">Parcela:</span>
              <span className="info-value">
                {transaction.parcelaAtual} de {transaction.quantidadeParcelas}
              </span>
            </div>
          )}
        </div>

        {transaction.observacoes && (
          <div className="card-observacoes">
            <p>{transaction.observacoes}</p>
          </div>
        )}

        <div className="card-footer">
          {getStatusBadge(transaction.status)}
          <span className={`tipo-badge ${tipoClass}`}>
            {transaction.tipo === TipoLancamento.Receita ? 'Receita' : 'Despesa'}
          </span>
        </div>
      </div>
    </div>
  );
};
