import { type FC, useState, useEffect, useMemo } from 'react';
import { Plus, DollarSign, TrendingUp, TrendingDown, Wallet, Loader2, AlertCircle } from 'lucide-react';
import { useTransactionStore } from '../../entities/transaction';
import { useCategoryStore } from '../../entities/category';
import { useAccountStore } from '../../entities/account';
import { TransactionCard } from '../../features/transaction-card';
import { TransactionFilters, type TransactionFiltersState } from '../../features/transaction-filters';
import { TransactionFormModal } from '../../features/transaction-form-modal';
import type { Transaction, CreateTransactionInput, UpdateTransactionInput } from '../../entities/transaction/model/types';
import { StatusLancamento, TipoLancamento } from '../../entities/transaction/model/types';
import './TransactionsPage.css';

export const TransactionsPage: FC = () => {
  const {
    transactions,
    isLoading,
    error,
    fetchTransactions,
    addTransaction,
    updateTransaction,
    deleteTransaction,
    marcarComoPago,
    marcarComoPendente,
    cancelar,
  } = useTransactionStore();

  const { categories, fetchCategories } = useCategoryStore();
  const { accounts, fetchAccounts } = useAccountStore();

  const [filters, setFilters] = useState<TransactionFiltersState>({});
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingTransaction, setEditingTransaction] = useState<Transaction | null>(null);
  const [sortBy, setSortBy] = useState<'date' | 'value' | 'status'>('date');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');

  useEffect(() => {
    fetchTransactions();
    fetchCategories();
    fetchAccounts();
  }, [fetchTransactions, fetchCategories, fetchAccounts]);

  const filteredAndSortedTransactions = useMemo(() => {
    let filtered = [...transactions];

    if (filters.tipo !== undefined) {
      filtered = filtered.filter((t) => t.tipo === filters.tipo);
    }

    if (filters.status !== undefined) {
      filtered = filtered.filter((t) => t.status === filters.status);
    }

    if (filters.categoriaId) {
      filtered = filtered.filter((t) => t.categoriaId === filters.categoriaId);
    }

    if (filters.contaId) {
      filtered = filtered.filter((t) => t.contaId === filters.contaId);
    }

    if (filters.dataInicio) {
      filtered = filtered.filter((t) => t.dataVencimento >= filters.dataInicio!);
    }

    if (filters.dataFim) {
      filtered = filtered.filter((t) => t.dataVencimento <= filters.dataFim!);
    }

    if (filters.mostrarVencidos) {
      filtered = filtered.filter((t) => t.status === StatusLancamento.Vencido);
    }

    if (filters.mostrarRecorrentes) {
      filtered = filtered.filter((t) => t.ehRecorrente);
    }

    filtered.sort((a, b) => {
      let comparison = 0;

      switch (sortBy) {
        case 'date':
          comparison = new Date(a.dataVencimento).getTime() - new Date(b.dataVencimento).getTime();
          break;
        case 'value':
          comparison = a.valor - b.valor;
          break;
        case 'status':
          comparison = a.status - b.status;
          break;
      }

      return sortOrder === 'asc' ? comparison : -comparison;
    });

    return filtered;
  }, [transactions, filters, sortBy, sortOrder]);

  const statistics = useMemo(() => {
    const receitas = filteredAndSortedTransactions
      .filter((t) => t.tipo === TipoLancamento.Receita && t.status === StatusLancamento.Pago)
      .reduce((sum, t) => sum + t.valor, 0);

    const despesas = filteredAndSortedTransactions
      .filter((t) => t.tipo === TipoLancamento.Despesa && t.status === StatusLancamento.Pago)
      .reduce((sum, t) => sum + t.valor, 0);

    const saldo = receitas - despesas;
    const pendentes = filteredAndSortedTransactions.filter((t) => t.status === StatusLancamento.Pendente).length;
    const vencidos = filteredAndSortedTransactions.filter((t) => t.status === StatusLancamento.Vencido).length;

    return { receitas, despesas, saldo, pendentes, vencidos };
  }, [filteredAndSortedTransactions]);

  const handleFormSubmit = async (data: CreateTransactionInput | UpdateTransactionInput) => {
    if (editingTransaction) {
      await updateTransaction(editingTransaction.id, data as UpdateTransactionInput);
      setEditingTransaction(null);
    } else {
      await addTransaction(data as CreateTransactionInput);
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
  };

  if (isLoading && transactions.length === 0) {
    return (
      <div className="transactions-page">
        <div className="loading-container">
          <Loader2 size={40} className="spinning" />
          <p>Carregando lançamentos...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="transactions-page">
      <header className="page-header">
        <div className="header-content">
          <div className="header-title">
            <DollarSign size={32} />
            <div>
              <h1>Lançamentos</h1>
              <p>Gerencie suas receitas e despesas</p>
            </div>
          </div>
          <button className="btn btn-primary" onClick={() => { setEditingTransaction(null); setIsModalOpen(true); }}>
            <Plus size={20} />
            Novo Lançamento
          </button>
        </div>
      </header>

      <div className="statistics-grid">
        <div className="stat-card stat-receita">
          <div className="stat-icon"><TrendingUp size={24} /></div>
          <div className="stat-content">
            <span className="stat-label">Receitas</span>
            <span className="stat-value">{formatCurrency(statistics.receitas)}</span>
          </div>
        </div>
        <div className="stat-card stat-despesa">
          <div className="stat-icon"><TrendingDown size={24} /></div>
          <div className="stat-content">
            <span className="stat-label">Despesas</span>
            <span className="stat-value">{formatCurrency(statistics.despesas)}</span>
          </div>
        </div>
        <div className={`stat-card stat-saldo ${statistics.saldo >= 0 ? 'positive' : 'negative'}`}>
          <div className="stat-icon"><Wallet size={24} /></div>
          <div className="stat-content">
            <span className="stat-label">Saldo</span>
            <span className="stat-value">{formatCurrency(statistics.saldo)}</span>
          </div>
        </div>
        {statistics.vencidos > 0 && (
          <div className="stat-card stat-alert">
            <div className="stat-icon"><AlertCircle size={24} /></div>
            <div className="stat-content">
              <span className="stat-label">Vencidos</span>
              <span className="stat-value">{statistics.vencidos}</span>
            </div>
          </div>
        )}
      </div>

      <div className="page-controls">
        <TransactionFilters filters={filters} onFiltersChange={setFilters} categories={categories} accounts={accounts} />
        <div className="sort-controls">
          <select value={sortBy} onChange={(e) => setSortBy(e.target.value as 'date' | 'value' | 'status')} className="sort-select">
            <option value="date">Data</option>
            <option value="value">Valor</option>
            <option value="status">Status</option>
          </select>
          <button className="sort-order-btn" onClick={() => setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc')}>
            {sortOrder === 'asc' ? '↑' : '↓'}
          </button>
        </div>
      </div>

      {error && (<div className="error-banner"><AlertCircle size={20} /><span>{error}</span></div>)}

      <div className="transactions-list">
        {filteredAndSortedTransactions.length === 0 ? (
          <div className="empty-state">
            <DollarSign size={48} />
            <h3>Nenhum lançamento encontrado</h3>
            <p>{Object.keys(filters).length > 0 ? 'Tente ajustar os filtros' : 'Adicione seu primeiro lançamento'}</p>
          </div>
        ) : (
          filteredAndSortedTransactions.map((transaction) => (
            <TransactionCard
              key={transaction.id}
              transaction={transaction}
              onEdit={(t) => { setEditingTransaction(t); setIsModalOpen(true); }}
              onDelete={(id) => confirm('Deseja excluir?') && deleteTransaction(id)}
              onMarcarPago={(id) => marcarComoPago(id, new Date().toISOString().split('T')[0])}
              onMarcarPendente={marcarComoPendente}
              onCancelar={(id) => confirm('Deseja cancelar?') && cancelar(id)}
            />
          ))
        )}
      </div>

      <TransactionFormModal
        isOpen={isModalOpen}
        onClose={() => { setIsModalOpen(false); setEditingTransaction(null); }}
        onSubmit={handleFormSubmit}
        transaction={editingTransaction}
        categories={categories}
        accounts={accounts}
      />
    </div>
  );
};