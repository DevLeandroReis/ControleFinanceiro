import type { FC } from 'react';
import { NavLink } from 'react-router-dom';
import './Navigation.css';

export const Navigation: FC = () => {
  return (
    <nav className="navigation">
      <div className="nav-container">
        <div className="nav-brand">
          <h2>💰 Controle Financeiro</h2>
        </div>

        <ul className="nav-menu">
          <li>
            <NavLink to="/" end className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              🏠 Home
            </NavLink>
          </li>
          <li>
            <NavLink to="/dashboard" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              📊 Dashboard
            </NavLink>
          </li>
          <li>
            <NavLink to="/transacoes" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              💰 Lançamentos
            </NavLink>
          </li>
          <li>
            <NavLink to="/contas" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              🏦 Contas
            </NavLink>
          </li>
          <li>
            <NavLink to="/categorias" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              📁 Categorias
            </NavLink>
          </li>
        </ul>
      </div>
    </nav>
  );
};
