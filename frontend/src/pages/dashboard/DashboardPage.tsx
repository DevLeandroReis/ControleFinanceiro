import type { FC } from 'react';
import './DashboardPage.css';

export const DashboardPage: FC = () => {
  return (
    <div className="dashboard-page">
      <header className="page-header">
        <h1>📊 Dashboard</h1>
        <p>Visão geral das suas finanças</p>
      </header>

      <div className="dashboard-grid">
        <div className="dashboard-card dashboard-card--income">
          <h3>Receitas do Mês</h3>
          <p className="amount">R$ 0,00</p>
        </div>

        <div className="dashboard-card dashboard-card--expense">
          <h3>Despesas do Mês</h3>
          <p className="amount">R$ 0,00</p>
        </div>

        <div className="dashboard-card dashboard-card--balance">
          <h3>Saldo Total</h3>
          <p className="amount">R$ 0,00</p>
        </div>

        <div className="dashboard-card dashboard-card--accounts">
          <h3>Total de Contas</h3>
          <p className="amount">0</p>
        </div>
      </div>

      <div className="dashboard-section">
        <h2>Atividades Recentes</h2>
        <p className="empty-state">Nenhuma transação registrada ainda.</p>
      </div>
    </div>
  );
};
