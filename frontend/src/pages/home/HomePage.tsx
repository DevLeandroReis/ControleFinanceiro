import type { FC } from 'react';
import { Link } from 'react-router-dom';
import './HomePage.css';

export const HomePage: FC = () => {
  return (
    <div className="home-page">
      <h1>Bem-vindo ao Controle Financeiro</h1>
      <p>Sistema de gerenciamento financeiro pessoal</p>
      
      <div className="features-grid">
        <Link to="/dashboard" className="feature-card">
          <h3>📊 Dashboard</h3>
          <p>Visualize suas finanças de forma clara</p>
        </Link>
        
        <Link to="/transacoes" className="feature-card">
          <h3>💰 Lançamentos</h3>
          <p>Registre receitas e despesas</p>
        </Link>
        
        <Link to="/contas" className="feature-card">
          <h3>🏦 Contas</h3>
          <p>Gerencie suas contas bancárias</p>
        </Link>
        
        <Link to="/categorias" className="feature-card">
          <h3>📁 Categorias</h3>
          <p>Organize seus gastos por categoria</p>
        </Link>
      </div>
    </div>
  );
};
