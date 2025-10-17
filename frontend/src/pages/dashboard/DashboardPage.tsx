import type { FC } from 'react';
import { useTransactionStore, type Transaction } from '../../entities/transaction';
import { useAccountStore } from '../../entities/account';
import './DashboardPage.css';

export const DashboardPage: FC = () => {
  const transactions = useTransactionStore((state: { transactions: Transaction[] }) => state.transactions);
  const accounts = useAccountStore((state: { accounts: unknown[] }) => state.accounts);
  const getTotalBalance = useAccountStore((state: { getTotalBalance: () => number }) => state.getTotalBalance);

  // Calcular receitas e despesas do mês atual
  const currentMonth = new Date().getMonth();
  const currentYear = new Date().getFullYear();

  const monthlyTransactions = transactions.filter((t: Transaction) => {
    const date = new Date(t.data);
    return date.getMonth() === currentMonth && date.getFullYear() === currentYear;
  });

  const income = monthlyTransactions
    .filter((t: Transaction) => t.tipo === 'income')
    .reduce((sum: number, t: Transaction) => sum + t.valor, 0);

  const expense = monthlyTransactions
    .filter((t: Transaction) => t.tipo === 'expense')
    .reduce((sum: number, t: Transaction) => sum + t.valor, 0);

  const totalBalance = getTotalBalance();

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  // Últimas 5 transações
  const recentTransactions = [...transactions]
    .sort((a, b) => new Date(b.data).getTime() - new Date(a.data).getTime())
    .slice(0, 5);

  return (
    <div className="dashboard-page">
      <header className="page-header">
        <h1>📊 Dashboard</h1>
        <p>Visão geral das suas finanças</p>
      </header>

      <div className="dashboard-grid">
        <div className="dashboard-card dashboard-card--income">
          <h3>Receitas do Mês</h3>
          <p className="amount">{formatCurrency(income)}</p>
        </div>

        <div className="dashboard-card dashboard-card--expense">
          <h3>Despesas do Mês</h3>
          <p className="amount">{formatCurrency(expense)}</p>
        </div>

        <div className="dashboard-card dashboard-card--balance">
          <h3>Saldo Total</h3>
          <p className="amount">{formatCurrency(totalBalance)}</p>
        </div>

        <div className="dashboard-card dashboard-card--accounts">
          <h3>Total de Contas</h3>
          <p className="amount">{accounts.length}</p>
        </div>
      </div>

      <div className="dashboard-section">
        <h2>Atividades Recentes</h2>
        {recentTransactions.length === 0 ? (
          <p className="empty-state">Nenhuma transação registrada ainda.</p>
        ) : (
          <div className="transactions-list">
            {recentTransactions.map((transaction) => (
              <div
                key={transaction.id}
                className={`transaction-item transaction-item--${transaction.tipo}`}
              >
                <div className="transaction-info">
                  <strong>{transaction.descricao}</strong>
                  <small>{new Date(transaction.data).toLocaleDateString('pt-BR')}</small>
                </div>
                <div className={`transaction-amount transaction-amount--${transaction.tipo}`}>
                  {transaction.tipo === 'income' ? '+' : '-'} {formatCurrency(Math.abs(transaction.valor))}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};
