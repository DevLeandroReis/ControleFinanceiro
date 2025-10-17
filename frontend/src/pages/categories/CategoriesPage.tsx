import type { FC } from 'react';
import './CategoriesPage.css';

export const CategoriesPage: FC = () => {
  return (
    <div className="categories-page">
      <header className="page-header">
        <h1>📁 Categorias</h1>
        <p>Organize seus gastos por categoria</p>
      </header>

      <div className="page-actions">
        <button className="btn btn--primary">
          ➕ Nova Categoria
        </button>
      </div>

      <div className="categories-grid">
        <p className="empty-state">Nenhuma categoria cadastrada ainda.</p>
      </div>
    </div>
  );
};
