import type { FC } from 'react';
import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useCategoryStore, type Category, type CategoryType } from '../../entities/category';
import { createCategorySchema, type CreateCategoryInput } from '../../entities/category/model';
import { FolderOpen, Plus, X, Zap, Loader2, TrendingDown, DollarSign, Trash2 } from 'lucide-react';
import './CategoriesPage.css';

export const CategoriesPage: FC = () => {
  const {
    categories,
    isLoading,
    error,
    fetchCategories,
    addCategory,
    deleteCategory,
    createDefaultCategories,
  } = useCategoryStore();

  const [showForm, setShowForm] = useState(false);
  const [filterType, setFilterType] = useState<CategoryType | ''>('');

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateCategoryInput>({
    resolver: zodResolver(createCategorySchema),
    defaultValues: {
      nome: '',
      tipo: 'expense',
      cor: '#ef4444',
      icone: '�',
    },
  });

  useEffect(() => {
    fetchCategories();
  }, [fetchCategories]);

  const onSubmit = async (data: CreateCategoryInput) => {
    try {
      await addCategory(data);
      reset();
      setShowForm(false);
    } catch (error) {
      console.error('Erro ao adicionar categoria:', error);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Deseja realmente excluir esta categoria?')) return;
    
    try {
      await deleteCategory(id);
    } catch (error) {
      console.error('Erro ao deletar categoria:', error);
    }
  };

  const handleCreateDefaults = async () => {
    if (!confirm('Deseja criar as categorias padrão do sistema?')) return;
    
    try {
      await createDefaultCategories();
    } catch (error) {
      console.error('Erro ao criar categorias padrão:', error);
    }
  };

  const filteredCategories = filterType
    ? categories.filter((c) => c.tipo === filterType)
    : categories;

  const incomeCategories = filteredCategories.filter((c) => c.tipo === 'income');
  const expenseCategories = filteredCategories.filter((c) => c.tipo === 'expense');

  if (isLoading) {
    return (
      <div className="categories-page">
        <div className="loading"><Loader2 size={20} style={{ display: 'inline', marginRight: '8px' }} className="spinning" /> Carregando categorias...</div>
      </div>
    );
  }

  return (
    <div className="categories-page">
      <header className="page-header">
        <h1><FolderOpen size={28} style={{ display: 'inline', marginRight: '10px', verticalAlign: 'middle' }} /> Categorias</h1>
        <p>Organize seus gastos por categoria</p>
      </header>

      <div className="page-actions">
        <div>
          <button className="btn btn--primary" onClick={() => setShowForm(!showForm)}>
            {showForm ? <><X size={18} style={{ display: 'inline', marginRight: '6px' }} /> Cancelar</> : <><Plus size={18} style={{ display: 'inline', marginRight: '6px' }} /> Nova Categoria</>}
          </button>
          <button className="btn btn--secondary" onClick={handleCreateDefaults}>
            <Zap size={18} style={{ display: 'inline', marginRight: '6px' }} /> Criar Categorias Padrão
          </button>
        </div>
        <select
          className="filter-select"
          value={filterType}
          onChange={(e) => setFilterType(e.target.value as CategoryType | '')}
        >
          <option value="">Todos os tipos</option>
          <option value="income">Receitas</option>
          <option value="expense">Despesas</option>
        </select>
      </div>

      {showForm && (
        <form className="category-form" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-row">
            <div className="form-group">
              <label>Nome da Categoria *</label>
              <input
                type="text"
                {...register('nome')}
                placeholder="Ex: Alimentação, Salário..."
              />
              {errors.nome && (
                <span className="error-message">{errors.nome.message}</span>
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
              <label>Cor</label>
              <input type="color" {...register('cor')} />
            </div>
            <div className="form-group">
              <label>Ícone</label>
              <input
                type="text"
                {...register('icone')}
                placeholder="Ex: 🍔, 💰, 🏠"
                maxLength={2}
              />
              {errors.icone && (
                <span className="error-message">{errors.icone.message}</span>
              )}
            </div>
          </div>
          {error && (
            <div className="form-error" role="alert">
              {error}
            </div>
          )}
          <button type="submit" className="btn btn--primary" disabled={isSubmitting}>
            {isSubmitting ? '⏳ Salvando...' : '✓ Adicionar Categoria'}
          </button>
        </form>
      )}

      <div className="categories-container">
        {categories.length === 0 ? (
          <p className="empty-state">Nenhuma categoria cadastrada ainda.</p>
        ) : (
          <>
            {expenseCategories.length > 0 && (
              <div className="category-section">
                <h2><TrendingDown size={22} style={{ display: 'inline', marginRight: '8px', verticalAlign: 'middle' }} /> Despesas ({expenseCategories.length})</h2>
                <div className="categories-grid">
                  {expenseCategories.map((category: Category) => (
                    <div
                      key={category.id}
                      className="category-card category-card--expense"
                      style={{ borderLeftColor: category.cor }}
                    >
                      <div className="category-header">
                        <div className="category-info">
                          <span className="category-icon">{category.icone}</span>
                          <h3>{category.nome}</h3>
                        </div>
                        <button
                          className="btn-delete"
                          onClick={() => handleDelete(category.id)}
                          title="Excluir categoria"
                        >
                          <Trash2 size={18} />
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {incomeCategories.length > 0 && (
              <div className="category-section">
                <h2><DollarSign size={22} style={{ display: 'inline', marginRight: '8px', verticalAlign: 'middle' }} /> Receitas ({incomeCategories.length})</h2>
                <div className="categories-grid">
                  {incomeCategories.map((category: Category) => (
                    <div
                      key={category.id}
                      className="category-card category-card--income"
                      style={{ borderLeftColor: category.cor }}
                    >
                      <div className="category-header">
                        <div className="category-info">
                          <span className="category-icon">{category.icone}</span>
                          <h3>{category.nome}</h3>
                        </div>
                        <button
                          className="btn-delete"
                          onClick={() => handleDelete(category.id)}
                          title="Excluir categoria"
                        >
                          <Trash2 size={18} />
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
};
