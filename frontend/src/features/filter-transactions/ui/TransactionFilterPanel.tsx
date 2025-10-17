import type { FC } from 'react';
import { useState } from 'react';
import type { TransactionType } from '@/entities/transaction';
import { useCategoryStore, type Category } from '@/entities/category';
import { useAccountStore, type Account } from '@/entities/account';
import type { TransactionFilters } from '../model';
import './TransactionFilterPanel.css';

interface TransactionFilterPanelProps {
  filters: TransactionFilters;
  onFiltersChange: (filters: TransactionFilters) => void;
  onClearFilters: () => void;
}

export const TransactionFilterPanel: FC<TransactionFilterPanelProps> = ({
  filters,
  onFiltersChange,
  onClearFilters,
}) => {
  const { categories } = useCategoryStore();
  const { accounts } = useAccountStore();
  const [isExpanded, setIsExpanded] = useState(false);

  const handleFilterChange = (key: keyof TransactionFilters, value: string | number | undefined) => {
    onFiltersChange({
      ...filters,
      [key]: value === '' ? undefined : value,
    });
  };

  const hasActiveFilters = Object.values(filters).some((value) => value !== undefined && value !== '');

  return (
    <div className="transaction-filter-panel">
      <div className="filter-header">
        <h3>🔍 Filtros</h3>
        <div className="filter-actions">
          {hasActiveFilters && (
            <button
              type="button"
              className="btn btn--clear"
              onClick={onClearFilters}
            >
              Limpar
            </button>
          )}
          <button
            type="button"
            className="btn btn--toggle"
            onClick={() => setIsExpanded(!isExpanded)}
          >
            {isExpanded ? '▲ Recolher' : '▼ Expandir'}
          </button>
        </div>
      </div>

      {isExpanded && (
        <div className="filter-content">
          <div className="filter-row">
            <div className="filter-group">
              <label htmlFor="filter-tipo">Tipo</label>
              <select
                id="filter-tipo"
                value={filters.tipo || ''}
                onChange={(e) => handleFilterChange('tipo', e.target.value as TransactionType | '')}
              >
                <option value="">Todos</option>
                <option value="income">Receita</option>
                <option value="expense">Despesa</option>
              </select>
            </div>

            <div className="filter-group">
              <label htmlFor="filter-categoria">Categoria</label>
              <select
                id="filter-categoria"
                value={filters.categoriaId || ''}
                onChange={(e) => handleFilterChange('categoriaId', e.target.value)}
              >
                <option value="">Todas</option>
                {categories.map((category: Category) => (
                  <option key={category.id} value={category.id}>
                    {category.icone} {category.nome}
                  </option>
                ))}
              </select>
            </div>

            <div className="filter-group">
              <label htmlFor="filter-conta">Conta</label>
              <select
                id="filter-conta"
                value={filters.contaId || ''}
                onChange={(e) => handleFilterChange('contaId', e.target.value)}
              >
                <option value="">Todas</option>
                {accounts.map((account: Account) => (
                  <option key={account.id} value={account.id}>
                    {account.nome}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="filter-row">
            <div className="filter-group">
              <label htmlFor="filter-data-inicio">Data Inicial</label>
              <input
                id="filter-data-inicio"
                type="date"
                value={filters.dataInicio || ''}
                onChange={(e) => handleFilterChange('dataInicio', e.target.value)}
              />
            </div>

            <div className="filter-group">
              <label htmlFor="filter-data-fim">Data Final</label>
              <input
                id="filter-data-fim"
                type="date"
                value={filters.dataFim || ''}
                onChange={(e) => handleFilterChange('dataFim', e.target.value)}
              />
            </div>
          </div>

          <div className="filter-row">
            <div className="filter-group">
              <label htmlFor="filter-valor-min">Valor Mínimo</label>
              <input
                id="filter-valor-min"
                type="number"
                step="0.01"
                placeholder="0.00"
                value={filters.valorMin || ''}
                onChange={(e) => handleFilterChange('valorMin', e.target.value ? parseFloat(e.target.value) : undefined)}
              />
            </div>

            <div className="filter-group">
              <label htmlFor="filter-valor-max">Valor Máximo</label>
              <input
                id="filter-valor-max"
                type="number"
                step="0.01"
                placeholder="0.00"
                value={filters.valorMax || ''}
                onChange={(e) => handleFilterChange('valorMax', e.target.value ? parseFloat(e.target.value) : undefined)}
              />
            </div>

            <div className="filter-group">
              <label htmlFor="filter-descricao">Buscar Descrição</label>
              <input
                id="filter-descricao"
                type="text"
                placeholder="Digite para buscar..."
                value={filters.descricao || ''}
                onChange={(e) => handleFilterChange('descricao', e.target.value)}
              />
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
