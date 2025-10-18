import type { FC } from 'react';
import './DashboardPage.css';

// Dados fictícios para o dashboard
const mockData = {
  summary: {
    totalReceitas: 15750.00,
    totalDespesas: 8420.50,
    saldo: 7329.50,
    totalContas: 3,
  },
  receitasPorCategoria: [
    { categoria: 'Salário', valor: 12000.00, percentual: 76 },
    { categoria: 'Freelance', valor: 2500.00, percentual: 16 },
    { categoria: 'Investimentos', valor: 1250.00, percentual: 8 },
  ],
  despesasPorCategoria: [
    { categoria: 'Moradia', valor: 3200.00, percentual: 38 },
    { categoria: 'Alimentação', valor: 1800.00, percentual: 21 },
    { categoria: 'Transporte', valor: 850.00, percentual: 10 },
    { categoria: 'Lazer', valor: 1200.00, percentual: 14 },
    { categoria: 'Saúde', valor: 670.50, percentual: 8 },
    { categoria: 'Educação', valor: 700.00, percentual: 8 },
  ],
  recentTransactions: [
    { id: 1, descricao: 'Salário Outubro', tipo: 'receita', valor: 12000.00, data: '2025-10-05', categoria: 'Salário' },
    { id: 2, descricao: 'Aluguel', tipo: 'despesa', valor: 2500.00, data: '2025-10-10', categoria: 'Moradia' },
    { id: 3, descricao: 'Freelance - Site WordPress', tipo: 'receita', valor: 2500.00, data: '2025-10-12', categoria: 'Freelance' },
    { id: 4, descricao: 'Supermercado', tipo: 'despesa', valor: 680.00, data: '2025-10-14', categoria: 'Alimentação' },
    { id: 5, descricao: 'Netflix', tipo: 'despesa', valor: 55.90, data: '2025-10-15', categoria: 'Lazer' },
    { id: 6, descricao: 'Uber', tipo: 'despesa', valor: 120.00, data: '2025-10-16', categoria: 'Transporte' },
  ],
  monthlyData: [
    { mes: 'Mai', receitas: 14200, despesas: 7800 },
    { mes: 'Jun', receitas: 13800, despesas: 8200 },
    { mes: 'Jul', receitas: 15000, despesas: 7500 },
    { mes: 'Ago', receitas: 14500, despesas: 8900 },
    { mes: 'Set', receitas: 16200, despesas: 8100 },
    { mes: 'Out', receitas: 15750, despesas: 8420.50 },
  ],
};

export const DashboardPage: FC = () => {
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    });
  };

  return (
    <div className="dashboard-page">
      <header className="dashboard-header">
        <div>
          <h1>📊 Dashboard</h1>
          <p>Visão geral das suas finanças</p>
        </div>
        <div className="dashboard-period">
          <span className="period-label">Período:</span>
          <span className="period-value">Outubro 2025</span>
        </div>
      </header>

      {/* Cards de Resumo */}
      <div className="dashboard-grid">
        <div className="summary-card summary-card--income">
          <div className="card-icon">📈</div>
          <div className="card-content">
            <h3>Receitas do Mês</h3>
            <p className="card-amount">{formatCurrency(mockData.summary.totalReceitas)}</p>
            <span className="card-badge card-badge--positive">+12.5% vs mês anterior</span>
          </div>
        </div>

        <div className="summary-card summary-card--expense">
          <div className="card-icon">📉</div>
          <div className="card-content">
            <h3>Despesas do Mês</h3>
            <p className="card-amount">{formatCurrency(mockData.summary.totalDespesas)}</p>
            <span className="card-badge card-badge--negative">+3.8% vs mês anterior</span>
          </div>
        </div>

        <div className="summary-card summary-card--balance">
          <div className="card-icon">💰</div>
          <div className="card-content">
            <h3>Saldo Total</h3>
            <p className="card-amount">{formatCurrency(mockData.summary.saldo)}</p>
            <span className="card-badge card-badge--positive">Superávit de 46.5%</span>
          </div>
        </div>

        <div className="summary-card summary-card--accounts">
          <div className="card-icon">🏦</div>
          <div className="card-content">
            <h3>Contas Ativas</h3>
            <p className="card-amount">{mockData.summary.totalContas}</p>
            <span className="card-badge">Banco, Carteira, Poupança</span>
          </div>
        </div>
      </div>

      {/* Seção de Gráficos */}
      <div className="dashboard-charts">
        <div className="chart-card chart-card--large">
          <h2>📊 Evolução Mensal</h2>
          <div className="bar-chart">
            {mockData.monthlyData.map((month) => (
              <div key={month.mes} className="bar-group">
                <div className="bar-container">
                  <div 
                    className="bar bar--income" 
                    style={{ height: `${(month.receitas / 20000) * 100}%` }}
                    title={formatCurrency(month.receitas)}
                  >
                    <span className="bar-label">{formatCurrency(month.receitas)}</span>
                  </div>
                  <div 
                    className="bar bar--expense" 
                    style={{ height: `${(month.despesas / 20000) * 100}%` }}
                    title={formatCurrency(month.despesas)}
                  >
                    <span className="bar-label">{formatCurrency(month.despesas)}</span>
                  </div>
                </div>
                <span className="bar-month">{month.mes}</span>
              </div>
            ))}
          </div>
          <div className="chart-legend">
            <div className="legend-item">
              <span className="legend-color legend-color--income"></span>
              <span>Receitas</span>
            </div>
            <div className="legend-item">
              <span className="legend-color legend-color--expense"></span>
              <span>Despesas</span>
            </div>
          </div>
        </div>

        <div className="chart-card">
          <h2>📈 Receitas por Categoria</h2>
          <div className="category-list">
            {mockData.receitasPorCategoria.map((item) => (
              <div key={item.categoria} className="category-item">
                <div className="category-info">
                  <span className="category-name">{item.categoria}</span>
                  <span className="category-value">{formatCurrency(item.valor)}</span>
                </div>
                <div className="progress-bar">
                  <div 
                    className="progress-fill progress-fill--income" 
                    style={{ width: `${item.percentual}%` }}
                  ></div>
                </div>
                <span className="category-percent">{item.percentual}%</span>
              </div>
            ))}
          </div>
        </div>

        <div className="chart-card">
          <h2>📉 Despesas por Categoria</h2>
          <div className="category-list">
            {mockData.despesasPorCategoria.map((item) => (
              <div key={item.categoria} className="category-item">
                <div className="category-info">
                  <span className="category-name">{item.categoria}</span>
                  <span className="category-value">{formatCurrency(item.valor)}</span>
                </div>
                <div className="progress-bar">
                  <div 
                    className="progress-fill progress-fill--expense" 
                    style={{ width: `${item.percentual}%` }}
                  ></div>
                </div>
                <span className="category-percent">{item.percentual}%</span>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Transações Recentes */}
      <div className="recent-transactions">
        <h2>⏱️ Atividades Recentes</h2>
        <div className="transactions-table">
          <div className="table-header">
            <div className="header-cell">Descrição</div>
            <div className="header-cell">Categoria</div>
            <div className="header-cell">Data</div>
            <div className="header-cell">Valor</div>
          </div>
          {mockData.recentTransactions.map((transaction) => (
            <div key={transaction.id} className="table-row">
              <div className="table-cell">
                <span className={`transaction-type-icon transaction-type-icon--${transaction.tipo}`}>
                  {transaction.tipo === 'receita' ? '📥' : '📤'}
                </span>
                <span className="transaction-description">{transaction.descricao}</span>
              </div>
              <div className="table-cell">
                <span className="transaction-category">{transaction.categoria}</span>
              </div>
              <div className="table-cell">
                <span className="transaction-date">{formatDate(transaction.data)}</span>
              </div>
              <div className="table-cell">
                <span className={`transaction-value transaction-value--${transaction.tipo}`}>
                  {transaction.tipo === 'receita' ? '+' : '-'} {formatCurrency(Math.abs(transaction.valor))}
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

