import type { FC } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { createAccountSchema, type CreateAccountInput } from '@/entities/account/model';
import type { Account } from '@/entities/account';
import { useEditAccount } from '../model';
import './EditAccountForm.css';

interface EditAccountFormProps {
  account: Account;
  onSuccess?: () => void;
  onCancel?: () => void;
}

export const EditAccountForm: FC<EditAccountFormProps> = ({ 
  account, 
  onSuccess, 
  onCancel 
}) => {
  const { editAccount, isLoading, error } = useEditAccount();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<CreateAccountInput>({
    resolver: zodResolver(createAccountSchema),
    defaultValues: {
      nome: account.nome,
      tipo: account.tipo,
      saldoInicial: account.saldoInicial,
      cor: account.cor || '#3b82f6',
    },
  });

  const onSubmit = async (data: CreateAccountInput) => {
    const result = await editAccount(account.id, data);
    
    if (result.success) {
      onSuccess?.();
    }
  };

  const accountTypeLabels = {
    corrente: 'Conta Corrente',
    poupanca: 'Poupança',
    investimento: 'Investimento',
    carteira: 'Carteira',
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="edit-account-form">
      <h3>✏️ Editar Conta</h3>

      {error && (
        <div className="form-error" role="alert">
          {error}
        </div>
      )}

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="nome">Nome *</label>
          <input
            id="nome"
            type="text"
            {...register('nome')}
            placeholder="Ex: Banco Inter, Carteira..."
          />
          {errors.nome && (
            <span className="error-message">{errors.nome.message}</span>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="tipo">Tipo *</label>
          <select id="tipo" {...register('tipo')}>
            <option value="">Selecione o tipo</option>
            {Object.entries(accountTypeLabels).map(([value, label]) => (
              <option key={value} value={value}>
                {label}
              </option>
            ))}
          </select>
          {errors.tipo && (
            <span className="error-message">{errors.tipo.message}</span>
          )}
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="saldoInicial">Saldo Inicial *</label>
          <input
            id="saldoInicial"
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
          <label htmlFor="cor">Cor</label>
          <input
            id="cor"
            type="color"
            {...register('cor')}
          />
          {errors.cor && (
            <span className="error-message">{errors.cor.message}</span>
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
          {isSubmitting || isLoading ? '⏳ Salvando...' : '✓ Salvar Alterações'}
        </button>
      </div>
    </form>
  );
};
