import { type FC, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { X, DollarSign, Calendar, FileText, FolderOpen, Building2, Repeat, Loader2, Save } from 'lucide-react';
import { createTransactionSchema, updateTransactionSchema } from '../../../entities/transaction/model/schemas';
import type { CreateTransactionInput, UpdateTransactionInput, Transaction } from '../../../entities/transaction/model/types';
import { TipoLancamento, TipoRecorrencia as TipoRecorrenciaEnum } from '../../../entities/transaction/model/types';
import type { Category } from '../../../entities/category';
import type { Account } from '../../../entities/account';
import './TransactionFormModal.css';

interface TransactionFormModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (data: CreateTransactionInput | UpdateTransactionInput) => Promise<void>;
  transaction?: Transaction | null;
  categories: Category[];
  accounts: Account[];
}

export const TransactionFormModal: FC<TransactionFormModalProps> = ({
  isOpen,
  onClose,
  onSubmit,
  transaction,
  categories,
  accounts,
}) => {
  const isEditMode = !!transaction;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    watch,
  } = useForm<CreateTransactionInput | UpdateTransactionInput>({
    resolver: zodResolver(isEditMode ? updateTransactionSchema : createTransactionSchema),
    defaultValues: transaction
      ? {
          descricao: transaction.descricao,
          valor: transaction.valor,
          dataVencimento: transaction.dataVencimento.split('T')[0],
          tipo: transaction.tipo,
          observacoes: transaction.observacoes || '',
          ehRecorrente: transaction.ehRecorrente,
          tipoRecorrencia: transaction.tipoRecorrencia,
          quantidadeParcelas: transaction.quantidadeParcelas || undefined,
          categoriaId: transaction.categoriaId,
          contaId: transaction.contaId,
        }
      : {
          descricao: '',
          valor: 0,
          dataVencimento: new Date().toISOString().split('T')[0],
          tipo: TipoLancamento.Despesa,
          observacoes: '',
          ehRecorrente: false,
          tipoRecorrencia: TipoRecorrenciaEnum.Nenhuma,
          quantidadeParcelas: undefined,
          categoriaId: '',
          contaId: '',
        },
  });

  const ehRecorrente = watch('ehRecorrente');

  useEffect(() => {
    if (isOpen && transaction) {
      reset({
        descricao: transaction.descricao,
        valor: transaction.valor,
        dataVencimento: transaction.dataVencimento.split('T')[0],
        tipo: transaction.tipo,
        observacoes: transaction.observacoes || '',
        ehRecorrente: transaction.ehRecorrente,
        tipoRecorrencia: transaction.tipoRecorrencia,
        quantidadeParcelas: transaction.quantidadeParcelas || undefined,
        categoriaId: transaction.categoriaId,
        contaId: transaction.contaId,
      });
    } else if (isOpen && !transaction) {
      reset({
        descricao: '',
        valor: 0,
        dataVencimento: new Date().toISOString().split('T')[0],
        tipo: TipoLancamento.Despesa,
        observacoes: '',
        ehRecorrente: false,
        tipoRecorrencia: TipoRecorrenciaEnum.Nenhuma,
        quantidadeParcelas: undefined,
        categoriaId: '',
        contaId: '',
      });
    }
  }, [isOpen, transaction, reset]);

  const handleFormSubmit = async (data: CreateTransactionInput | UpdateTransactionInput) => {
    await onSubmit(data);
    reset();
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>
            <DollarSign size={24} />
            {isEditMode ? 'Editar Lançamento' : 'Novo Lançamento'}
          </h2>
          <button className="btn-close" onClick={onClose} disabled={isSubmitting}>
            <X size={20} />
          </button>
        </div>

        <form onSubmit={handleSubmit(handleFormSubmit)} className="modal-form">
          <div className="form-section">
            <h3>Informações Básicas</h3>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="descricao">
                  <FileText size={16} />
                  Descrição *
                </label>
                <input
                  id="descricao"
                  type="text"
                  {...register('descricao')}
                  placeholder="Ex: Conta de luz, Salário..."
                  className={errors.descricao ? 'error' : ''}
                />
                {errors.descricao && (
                  <span className="error-message">{errors.descricao.message}</span>
                )}
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="valor">
                  <DollarSign size={16} />
                  Valor *
                </label>
                <input
                  id="valor"
                  type="number"
                  step="0.01"
                  {...register('valor', { valueAsNumber: true })}
                  placeholder="0,00"
                  className={errors.valor ? 'error' : ''}
                />
                {errors.valor && (
                  <span className="error-message">{errors.valor.message}</span>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="dataVencimento">
                  <Calendar size={16} />
                  Data de Vencimento *
                </label>
                <input
                  id="dataVencimento"
                  type="date"
                  {...register('dataVencimento')}
                  className={errors.dataVencimento ? 'error' : ''}
                />
                {errors.dataVencimento && (
                  <span className="error-message">{errors.dataVencimento.message}</span>
                )}
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="tipo">
                  <DollarSign size={16} />
                  Tipo *
                </label>
                <select
                  id="tipo"
                  {...register('tipo', { valueAsNumber: true })}
                  className={errors.tipo ? 'error' : ''}
                >
                  <option value={TipoLancamento.Despesa}>Despesa</option>
                  <option value={TipoLancamento.Receita}>Receita</option>
                </select>
                {errors.tipo && (
                  <span className="error-message">{errors.tipo.message}</span>
                )}
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="categoriaId">
                  <FolderOpen size={16} />
                  Categoria *
                </label>
                <select
                  id="categoriaId"
                  {...register('categoriaId')}
                  className={errors.categoriaId ? 'error' : ''}
                >
                  <option value="">Selecione uma categoria</option>
                  {categories.map((category) => (
                    <option key={category.id} value={category.id}>
                      {category.nome}
                    </option>
                  ))}
                </select>
                {errors.categoriaId && (
                  <span className="error-message">{errors.categoriaId.message}</span>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="contaId">
                  <Building2 size={16} />
                  Conta *
                </label>
                <select
                  id="contaId"
                  {...register('contaId')}
                  className={errors.contaId ? 'error' : ''}
                >
                  <option value="">Selecione uma conta</option>
                  {accounts.map((account) => (
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
          </div>

          <div className="form-section">
            <h3>
              <Repeat size={18} />
              Recorrência
            </h3>

            <div className="form-row">
              <div className="form-group form-checkbox">
                <label htmlFor="ehRecorrente" className="checkbox-label">
                  <input
                    id="ehRecorrente"
                    type="checkbox"
                    {...register('ehRecorrente')}
                  />
                  <span>Este lançamento é recorrente</span>
                </label>
              </div>
            </div>

            {ehRecorrente && (
              <>
                <div className="form-row">
                  <div className="form-group">
                    <label htmlFor="tipoRecorrencia">Tipo de Recorrência</label>
                    <select
                      id="tipoRecorrencia"
                      {...register('tipoRecorrencia', { valueAsNumber: true })}
                    >
                      <option value={TipoRecorrenciaEnum.Mensal}>Mensal</option>
                      <option value={TipoRecorrenciaEnum.Semanal}>Semanal</option>
                      <option value={TipoRecorrenciaEnum.Diaria}>Diária</option>
                      <option value={TipoRecorrenciaEnum.Anual}>Anual</option>
                    </select>
                  </div>

                  <div className="form-group">
                    <label htmlFor="quantidadeParcelas">Número de Parcelas</label>
                    <input
                      id="quantidadeParcelas"
                      type="number"
                      min="1"
                      {...register('quantidadeParcelas', { valueAsNumber: true })}
                      placeholder="Ex: 12"
                    />
                    {errors.quantidadeParcelas && (
                      <span className="error-message">{errors.quantidadeParcelas.message}</span>
                    )}
                  </div>
                </div>
              </>
            )}
          </div>

          <div className="form-section">
            <h3>Observações</h3>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="observacoes">Observações (opcional)</label>
                <textarea
                  id="observacoes"
                  {...register('observacoes')}
                  rows={3}
                  placeholder="Adicione informações adicionais sobre este lançamento..."
                  className={errors.observacoes ? 'error' : ''}
                />
                {errors.observacoes && (
                  <span className="error-message">{errors.observacoes.message}</span>
                )}
              </div>
            </div>
          </div>

          <div className="modal-footer">
            <button
              type="button"
              className="btn btn--secondary"
              onClick={onClose}
              disabled={isSubmitting}
            >
              <X size={18} />
              Cancelar
            </button>
            <button
              type="submit"
              className="btn btn--primary"
              disabled={isSubmitting}
            >
              {isSubmitting ? (
                <>
                  <Loader2 size={18} className="spinning" />
                  Salvando...
                </>
              ) : (
                <>
                  <Save size={18} />
                  {isEditMode ? 'Salvar Alterações' : 'Criar Lançamento'}
                </>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
