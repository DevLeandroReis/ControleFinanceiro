import { type FC, useState } from 'react';
import { Filter, X, Calendar, DollarSign, FolderOpen, Building2, AlertCircle } from 'lucide-react';
import { TipoLancamento, StatusLancamento } from '../../../entities/transaction/model/types';
import type { Category } from '../../../entities/category';
import type { Account } from '../../../entities/account';
import './TransactionFilters.css';

export interface TransactionFiltersState {
  tipo?: number;
  status?: number;
  categoriaId?: string;
  contaId?: string;
  dataInicio?: string;
  dataFim?: string;
  mostrarVencidos?: boolean;
  mostrarRecorrentes?: boolean;
}

interface TransactionFiltersProps {
  filters: TransactionFiltersState;
  onFiltersChange: (filters: TransactionFiltersState) => void;
  categories: Category[];
  accounts: Account[];
}

export const TransactionFilters: FC<TransactionFiltersProps> = ({
  filters,
  onFiltersChange,
  categories,
  accounts,
}) => {
  const [isOpen, setIsOpen] = useState(false);

  const handleFilterChange = (key: keyof TransactionFiltersState, value: string | number | boolean | undefined) => {
    onFiltersChange({ ...filters, [key]: value });
  };

  const clearFilters = () => {
    onFiltersChange({});
  };

  const hasActiveFilters = Object.keys(filters).length > 0;

  return (
    <div className="transaction-filters">
      <button
        className={`filters-toggle ${isOpen ? 'active' : ''} ${hasActiveFilters ? 'has-filters' : ''}`}
        onClick={() => setIsOpen(!isOpen)}
      >
        <Filter size={18} />
        <span>Filtros</span>
        {hasActiveFilters && <span className="filters-badge">{Object.keys(filters).length}</span>}
      </button>

      {isOpen && (
        <div className="filters-panel">
          <div className="filters-header">
            <h3><Filter size={20} /> Filtros Avançados</h3>
            <button className="btn-close" onClick={() => setIsOpen(false)}>
              <X size={18} />
            </button>
          </div>

          <div className="filters-content">
            {/* Tipo de Lançamento */}
            <div className="filter-group">
              <label>
                <DollarSign size={16} />
                Tipo de Lançamento
              </label>
              <select
                value={filters.tipo || ''}
                onChange={(e) => handleFilterChange('tipo', e.target.value ? Number(e.target.value) : undefined)}
              >
                <option value="">Todos</option>
                <option value={TipoLancamento.Receita}>Receitas</option>
                <option value={TipoLancamento.Despesa}>Despesas</option>
              </select>
            </div>

            {/* Status */}
            <div className="filter-group">
              <label>
                <AlertCircle size={16} />
                Status
              </label>
              <select
                value={filters.status || ''}
                onChange={(e) => handleFilterChange('status', e.target.value ? Number(e.target.value) : undefined)}
              >
                <option value="">Todos</option>
                <option value={StatusLancamento.Pendente}>Pendente</option>
                <option value={StatusLancamento.Pago}>Pago</option>
                <option value={StatusLancamento.Cancelado}>Cancelado</option>
                <option value={StatusLancamento.Vencido}>Vencido</option>
              </select>
            </div>

            {/* Categoria */}
            <div className="filter-group">
              <label>
                <FolderOpen size={16} />
                Categoria
              </label>
              <select
                value={filters.categoriaId || ''}
                onChange={(e) => handleFilterChange('categoriaId', e.target.value || undefined)}
              >
                <option value="">Todas</option>
                {categories.map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.nome}
                  </option>
                ))}
              </select>
            </div>

            {/* Conta */}
            <div className="filter-group">
              <label>
                <Building2 size={16} />
                Conta
              </label>
              <select
                value={filters.contaId || ''}
                onChange={(e) => handleFilterChange('contaId', e.target.value || undefined)}
              >
                <option value="">Todas</option>
                {accounts.map((account) => (
                  <option key={account.id} value={account.id}>
                    {account.nome}
                  </option>
                ))}
              </select>
            </div>

            {/* Período */}
            <div className="filter-group">
              <label>
                <Calendar size={16} />
                Período
              </label>
              <div className="date-range">
                <input
                  type="date"
                  value={filters.dataInicio || ''}
                  onChange={(e) => handleFilterChange('dataInicio', e.target.value || undefined)}
                  placeholder="Data Inicial"
                />
                <span>até</span>
                <input
                  type="date"
                  value={filters.dataFim || ''}
                  onChange={(e) => handleFilterChange('dataFim', e.target.value || undefined)}
                  placeholder="Data Final"
                />
              </div>
            </div>

            {/* Filtros Especiais */}
            <div className="filter-group filter-checkboxes">
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  checked={filters.mostrarVencidos || false}
                  onChange={(e) => handleFilterChange('mostrarVencidos', e.target.checked || undefined)}
                />
                <span>Apenas Vencidos</span>
              </label>
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  checked={filters.mostrarRecorrentes || false}
                  onChange={(e) => handleFilterChange('mostrarRecorrentes', e.target.checked || undefined)}
                />
                <span>Apenas Recorrentes</span>
              </label>
            </div>
          </div>

          <div className="filters-footer">
            <button
              className="btn btn--secondary"
              onClick={clearFilters}
              disabled={!hasActiveFilters}
            >
              <X size={16} />
              Limpar Filtros
            </button>
          </div>
        </div>
      )}
    </div>
  );
};
