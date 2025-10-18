import type { FC } from 'react';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { createTransactionSchema, type CreateTransactionInput } from '@/entities/transaction/model';
import { useCategoryStore, type Category } from '@/entities/category';
import { useAccountStore, type Account } from '@/entities/account';
import { useAddTransaction } from '../model';
import { Plus } from 'lucide-react';
import './AddTransactionForm.css';

interface AddTransactionFormProps {
  onSuccess?: () => void;
  onCancel?: () => void;
}

export const AddTransactionForm: FC<AddTransactionFormProps> = ({ onSuccess, onCancel }) => {
  const { addTransaction, isLoading, error } = useAddTransaction();
  const { categories, fetchCategories } = useCategoryStore();
  const { accounts, fetchAccounts } = useAccountStore();

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

  useEffect(() => {
    fetchCategories();
    fetchAccounts();
  }, [fetchCategories, fetchAccounts]);

  const onSubmit = async (data: CreateTransactionInput) => {
    const result = await addTransaction(data);
    
    if (result.success) {
      reset();
      onSuccess?.();
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="add-transaction-form">
      <h3><Plus size={20} style={{ display: 'inline', marginRight: '8px', verticalAlign: 'middle' }} /> Nova Transação</h3>

      {error && (
        <div className="form-error" role="alert">
          {error}
        </div>
      )}

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="descricao">Descrição *</label>
          <input
            id="descricao"
            type="text"
            {...register('descricao')}
            placeholder="Ex: Salário, Aluguel..."
          />
          {errors.descricao && (
            <span className="error-message">{errors.descricao.message}</span>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="valor">Valor *</label>
          <input
            id="valor"
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
          <label htmlFor="data">Data *</label>
          <input
            id="data"
            type="date"
            {...register('data')}
          />
          {errors.data && (
            <span className="error-message">{errors.data.message}</span>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="tipo">Tipo *</label>
          <select id="tipo" {...register('tipo')}>
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
          <label htmlFor="categoriaId">Categoria *</label>
          <select id="categoriaId" {...register('categoriaId')}>
            <option value="">Selecione uma categoria</option>
            {categories.map((category: Category) => (
              <option key={category.id} value={category.id}>
                {category.icone} {category.nome}
              </option>
            ))}
          </select>
          {errors.categoriaId && (
            <span className="error-message">{errors.categoriaId.message}</span>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="contaId">Conta *</label>
          <select id="contaId" {...register('contaId')}>
            <option value="">Selecione uma conta</option>
            {accounts.map((account: Account) => (
              <option key={account.id} value={account.id}>
                {account.nome}
              </option>
            ))}
          </select>
          {errors.contaId && (
            <span className="error-message">{errors.contaId.message}</span>
          )}
        </div>
      </div>

      <div className="form-actions">
        <button
          type="button"
          className="btn btn--secondary"
          onClick={onCancel}
          disabled={isSubmitting || isLoading}
        >
          Cancelar
        </button>
        <button
          type="submit"
          className="btn btn--primary"
          disabled={isSubmitting || isLoading}
        >
          {isSubmitting || isLoading ? '⏳ Salvando...' : '✓ Adicionar'}
        </button>
      </div>
    </form>
  );
};
