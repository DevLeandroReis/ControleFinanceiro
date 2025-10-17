import type { FC } from 'react';
import './AccountsPage.css';

export const AccountsPage: FC = () => {
  return (
    <div className="accounts-page">
      <header className="page-header">
        <h1>🏦 Contas</h1>
        <p>Gerencie suas contas bancárias e carteiras</p>
      </header>

      <div className="page-actions">
        <button className="btn btn--primary">
          ➕ Nova Conta
        </button>
      </div>

      <div className="accounts-grid">
        <p className="empty-state">Nenhuma conta cadastrada ainda.</p>
      </div>
    </div>
  );
};
