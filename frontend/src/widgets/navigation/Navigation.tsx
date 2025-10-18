import type { FC } from 'react';
import { NavLink } from 'react-router-dom';
import { DollarSign, Home, BarChart3, Building2, FolderOpen } from 'lucide-react';
import './Navigation.css';

export const Navigation: FC = () => {
  return (
    <nav className="navigation">
      <div className="nav-container">
        <div className="nav-brand">
          <h2><DollarSign size={24} style={{ display: 'inline', marginRight: '8px' }} /> Controle Financeiro</h2>
        </div>

        <ul className="nav-menu">
          <li>
            <NavLink to="/" end className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <Home size={18} style={{ display: 'inline', marginRight: '6px' }} /> Home
            </NavLink>
          </li>
          <li>
            <NavLink to="/dashboard" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <BarChart3 size={18} style={{ display: 'inline', marginRight: '6px' }} /> Dashboard
            </NavLink>
          </li>
          <li>
            <NavLink to="/transacoes" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <DollarSign size={18} style={{ display: 'inline', marginRight: '6px' }} /> Lançamentos
            </NavLink>
          </li>
          <li>
            <NavLink to="/contas" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <Building2 size={18} style={{ display: 'inline', marginRight: '6px' }} /> Contas
            </NavLink>
          </li>
          <li>
            <NavLink to="/categorias" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <FolderOpen size={18} style={{ display: 'inline', marginRight: '6px' }} /> Categorias
            </NavLink>
          </li>
        </ul>
      </div>
    </nav>
  );
};
