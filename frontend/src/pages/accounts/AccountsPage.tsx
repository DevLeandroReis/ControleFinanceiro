import type { FC } from 'react';
import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useAccountStore, type Account } from '../../entities/account';
import { createAccountSchema, type CreateAccountInput } from '../../entities/account/model';
import { Building2, Plus, X, Loader2, Trash2 } from 'lucide-react';
import './AccountsPage.css';

export const AccountsPage: FC = () => {
  const {
    accounts,
    isLoading,
    error,
    fetchAccounts,
    addAccount,
    deleteAccount,
  } = useAccountStore();

  const [showForm, setShowForm] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateAccountInput>({
    resolver: zodResolver(createAccountSchema),
    defaultValues: {
      nome: '',
      tipo: 'corrente',
      saldoInicial: 0,
      cor: '#646cff',
    },
  });

  useEffect(() => {
    fetchAccounts();
  }, [fetchAccounts]);

  const onSubmit = async (data: CreateAccountInput) => {
    try {
      await addAccount(data);
      reset();
      setShowForm(false);
    } catch (error) {
      console.error('Erro ao adicionar conta:', error);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Deseja realmente excluir esta conta?')) return;
    
    try {
      await deleteAccount(id);
    } catch (error) {
      console.error('Erro ao deletar conta:', error);
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const getTipoLabel = (tipo: string) => {
    const tipos: Record<string, string> = {
      corrente: 'Conta Corrente',
      poupanca: 'Poupança',
      investimento: 'Investimento',
      carteira: 'Carteira',
    };
    return tipos[tipo] || tipo;
  };

  if (isLoading) {
    return (
      <div className="accounts-page">
        <div className="loading"><Loader2 size={20} style={{ display: 'inline', marginRight: '8px' }} className="spinning" /> Carregando contas...</div>
      </div>
    );
  }

  return (
    <div className="accounts-page">
      <header className="page-header">
        <h1><Building2 size={28} style={{ display: 'inline', marginRight: '10px', verticalAlign: 'middle' }} /> Contas</h1>
        <p>Gerencie suas contas bancárias e carteiras</p>
      </header>

      <div className="page-actions">
        <button className="btn btn--primary" onClick={() => setShowForm(!showForm)}>
          {showForm ? <><X size={18} style={{ display: 'inline', marginRight: '6px' }} /> Cancelar</> : <><Plus size={18} style={{ display: 'inline', marginRight: '6px' }} /> Nova Conta</>}
        </button>
      </div>

      {showForm && (
        <form className="account-form" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-row">
            <div className="form-group">
              <label>Nome da Conta *</label>
              <input
                type="text"
                {...register('nome')}
                placeholder="Ex: Banco do Brasil, Nubank..."
              />
              {errors.nome && (
                <span className="error-message">{errors.nome.message}</span>
              )}
            </div>
            <div className="form-group">
              <label>Tipo *</label>
              <select {...register('tipo')}>
                <option value="corrente">Conta Corrente</option>
                <option value="poupanca">Poupança</option>
                <option value="investimento">Investimento</option>
                <option value="carteira">Carteira</option>
              </select>
              {errors.tipo && (
                <span className="error-message">{errors.tipo.message}</span>
              )}
            </div>
          </div>
          <div className="form-row">
            <div className="form-group">
              <label>Saldo Inicial *</label>
              <input
                type="number"
                step="0.01"
                {...register('saldoInicial', { valueAsNumber: true })}
                placeholder="0.00"
              />
              {errors.saldoInicial && (
                <span className="error-message">{errors.saldoInicial.message}</span>
              )}
            </div>
            <div className="form-group">
              <label>Cor</label>
              <input type="color" {...register('cor')} />
            </div>
          </div>
          {error && (
            <div className="form-error" role="alert">
              {error}
            </div>
          )}
          <button type="submit" className="btn btn--primary" disabled={isSubmitting}>
            {isSubmitting ? '⏳ Salvando...' : '✓ Adicionar Conta'}
          </button>
        </form>
      )}

      <div className="accounts-grid">
        {accounts.length === 0 ? (
          <p className="empty-state">Nenhuma conta cadastrada ainda.</p>
        ) : (
          accounts.map((account: Account) => (
            <div
              key={account.id}
              className="account-card"
              style={{ borderLeftColor: account.cor }}
            >
              <div className="account-header">
                <h3>{account.nome}</h3>
                <button
                  className="btn-delete"
                  onClick={() => handleDelete(account.id)}
                  title="Excluir conta"
                >
                  <Trash2 size={18} />
                </button>
              </div>
              <p className="account-type">{getTipoLabel(account.tipo)}</p>
              <p className="account-balance">
                {formatCurrency(account.saldoAtual)}
              </p>
            </div>
          ))
        )}
      </div>
    </div>
  );
};
