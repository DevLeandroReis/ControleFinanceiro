import type { FC } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { createCategorySchema, type CreateCategoryInput } from '@/entities/category/model';
import type { Category } from '@/entities/category';
import { useEditCategory } from '../model';
import { Edit } from 'lucide-react';
import './EditCategoryForm.css';

interface EditCategoryFormProps {
  category: Category;
  onSuccess?: () => void;
  onCancel?: () => void;
}

export const EditCategoryForm: FC<EditCategoryFormProps> = ({ 
  category, 
  onSuccess, 
  onCancel 
}) => {
  const { editCategory, isLoading, error } = useEditCategory();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<CreateCategoryInput>({
    resolver: zodResolver(createCategorySchema),
    defaultValues: {
      nome: category.nome,
      tipo: category.tipo,
      cor: category.cor || '#3b82f6',
      icone: category.icone || '�',
    },
  });

  const onSubmit = async (data: CreateCategoryInput) => {
    const result = await editCategory(category.id, data);
    
    if (result.success) {
      onSuccess?.();
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="edit-category-form">
      <h3><Edit size={20} style={{ display: 'inline', marginRight: '8px', verticalAlign: 'middle' }} /> Editar Categoria</h3>

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
            placeholder="Ex: Alimentação, Transporte..."
          />
          {errors.nome && (
            <span className="error-message">{errors.nome.message}</span>
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

        <div className="form-group">
          <label htmlFor="icone">Ícone (emoji)</label>
          <input
            id="icone"
            type="text"
            maxLength={2}
            {...register('icone')}
            placeholder="💰"
          />
          {errors.icone && (
            <span className="error-message">{errors.icone.message}</span>
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
