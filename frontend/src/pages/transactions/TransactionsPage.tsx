import type { FC } from 'react';
import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTransactionStore, type Transaction, type TransactionType } from '../../entities/transaction';
import { createTransactionSchema, type CreateTransactionInput } from '../../entities/transaction/model';
import { useCategoryStore, type Category } from '../../entities/category';
import { useAccountStore, type Account } from '../../entities/account';
import { DollarSign, Plus, X, Check, Loader2, Trash2 } from 'lucide-react';
import './TransactionsPage.css';

export const TransactionsPage: FC = () => {
  const {
    transactions,
    isLoading,
    error,
    fetchTransactions,
    addTransaction,
    deleteTransaction,
  } = useTransactionStore();

  const { categories, fetchCategories } = useCategoryStore();
  const { accounts, fetchAccounts } = useAccountStore();

  const [showForm, setShowForm] = useState(false);
  const [filterType, setFilterType] = useState<TransactionType | ''>('');

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateTransactionInput>({
    resolver: zodResolver(createTransactionSchema),
    defaultValues: {
      descricao: '',
      valor: 0,
      data: new Date().toISOString().split('T')[0],
      tipo: 'expense',
      categoriaId: '',
      contaId: '',
    },
  });

  // Carregar dados ao montar
  useEffect(() => {
    fetchTransactions();
    fetchCategories();
    fetchAccounts();
  }, [fetchTransactions, fetchCategories, fetchAccounts]);

  const onSubmit = async (data: CreateTransactionInput) => {
    try {
      await addTransaction(data);
      reset();
      setShowForm(false);
    } catch (error) {
      console.error('Erro ao adicionar transação:', error);
    }
  };

  const filteredTransactions = filterType
    ? transactions.filter((t: Transaction) => t.tipo === filterType)
    : transactions;

  const sortedTransactions = [...filteredTransactions].sort(
    (a: Transaction, b: Transaction) => new Date(b.data).getTime() - new Date(a.data).getTime()
  );

  const handleDelete = async (id: string) => {
    if (!confirm('Deseja realmente excluir esta transação?')) return;
    
    try {
      await deleteTransaction(id);
    } catch (error) {
      console.error('Erro ao deletar transação:', error);
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  if (isLoading) {
    return (
      <div className="transactions-page">
        <div className="loading"><Loader2 size={20} style={{ display: 'inline', marginRight: '8px', verticalAlign: 'middle' }} className="spinning" /> Carregando transações...</div>
      </div>
    );
  }

  return (
    <div className="transactions-page">
      <header className="page-header">
        <h1><DollarSign size={28} style={{ display: 'inline', marginRight: '10px', verticalAlign: 'middle' }} /> Lançamentos</h1>
        <p>Gerencie suas receitas e despesas</p>
      </header>

      <div className="page-actions">
        <button className="btn btn--primary" onClick={() => setShowForm(!showForm)}>
          {showForm ? <><X size={18} style={{ display: 'inline', marginRight: '6px' }} /> Cancelar</> : <><Plus size={18} style={{ display: 'inline', marginRight: '6px' }} /> Nova Transação</>}
        </button>
        <div className="filters">
          <select
            className="filter-select"
            value={filterType}
            onChange={(e) => setFilterType(e.target.value as TransactionType | '')}
          >
            <option value="">Todos os tipos</option>
            <option value="income">Receitas</option>
            <option value="expense">Despesas</option>
          </select>
        </div>
      </div>

      {showForm && (
        <form className="transaction-form" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-row">
            <div className="form-group">
              <label>Descrição *</label>
              <input
                type="text"
                {...register('descricao')}
                placeholder="Ex: Salário, Aluguel..."
              />
              {errors.descricao && (
                <span className="error-message">{errors.descricao.message}</span>
              )}
            </div>
            <div className="form-group">
              <label>Valor *</label>
              <input
                type="number"
                step="0.01"
                {...register('valor', { valueAsNumber: true })}
                placeholder="0.00"
              />
              {errors.valor && (
                <span className="error-message">{errors.valor.message}</span>
              )}
            </div>
          </div>
          <div className="form-row">
            <div className="form-group">
              <label>Data *</label>
              <input type="date" {...register('data')} />
              {errors.data && (
                <span className="error-message">{errors.data.message}</span>
              )}
            </div>
            <div className="form-group">
              <label>Tipo *</label>
              <select {...register('tipo')}>
                <option value="expense">Despesa</option>
                <option value="income">Receita</option>
              </select>
              {errors.tipo && (
                <span className="error-message">{errors.tipo.message}</span>
              )}
            </div>
          </div>
          <div className="form-row">
            <div className="form-group">
              <label>Categoria *</label>
              <select {...register('categoriaId')}>
                <option value="">Selecione uma categoria</option>
                {categories.map((cat: Category) => (
                  <option key={cat.id} value={cat.id}>
                    {cat.nome}
                  </option>
                ))}
              </select>
              {errors.categoriaId && (
                <span className="error-message">{errors.categoriaId.message}</span>
              )}
            </div>
            <div className="form-group">
              <label>Conta *</label>
              <select {...register('contaId')}>
                <option value="">Selecione uma conta</option>
                {accounts.map((acc: Account) => (
                  <option key={acc.id} value={acc.id}>
                    {acc.nome}
                  </option>
                ))}
              </select>
              {errors.contaId && (
                <span className="error-message">{errors.contaId.message}</span>
              )}
            </div>
          </div>
          {error && (
            <div className="form-error" role="alert">
              {error}
            </div>
          )}
          <button type="submit" className="btn btn--primary" disabled={isSubmitting}>
            {isSubmitting ? <><Loader2 size={18} style={{ display: 'inline', marginRight: '6px' }} className="spinning" /> Salvando...</> : <><Check size={18} style={{ display: 'inline', marginRight: '6px' }} /> Adicionar Transação</>}
          </button>
        </form>
      )}

      <div className="transactions-list">
        {sortedTransactions.length === 0 ? (
          <p className="empty-state">Nenhuma transação encontrada.</p>
        ) : (
          sortedTransactions.map((transaction: Transaction) => (
            <div
              key={transaction.id}
              className={`transaction-card transaction-card--${transaction.tipo}`}
            >
              <div className="transaction-header">
                <h3>{transaction.descricao}</h3>
                <button
                  className="btn-delete"
                  onClick={() => handleDelete(transaction.id)}
                  title="Excluir transação"
                >
                  <Trash2 size={18} />
                </button>
              </div>
              <div className="transaction-details">
                <span className={`transaction-amount transaction-amount--${transaction.tipo}`}>
                  {transaction.tipo === 'income' ? '+' : '-'} {formatCurrency(Math.abs(transaction.valor))}
                </span>
                <span className="transaction-date">
                  {new Date(transaction.data).toLocaleDateString('pt-BR')}
                </span>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};
