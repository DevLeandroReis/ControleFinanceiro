import type { FC } from 'react';
import './TransactionsPage.css';

export const TransactionsPage: FC = () => {
  return (
    <div className="transactions-page">
      <header className="page-header">
        <h1>💰 Lançamentos</h1>
        <p>Gerencie suas receitas e despesas</p>
      </header>

      <div className="page-actions">
        <button className="btn btn--primary">
          ➕ Nova Transação
        </button>
        <div className="filters">
          <select className="filter-select">
            <option value="">Todas as categorias</option>
          </select>
          <select className="filter-select">
            <option value="">Todos os tipos</option>
            <option value="income">Receitas</option>
            <option value="expense">Despesas</option>
          </select>
        </div>
      </div>

      <div className="transactions-list">
        <p className="empty-state">Nenhuma transação encontrada.</p>
      </div>
    </div>
  );
};
