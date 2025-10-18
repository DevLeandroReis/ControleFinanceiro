import type { FC } from 'react';
import { NavLink } from 'react-router-dom';
import { useUserStore } from '../../entities/user';
import type { User } from '../../entities/user';
import { DollarSign, BarChart3, TrendingDown, Building2, LogOut } from 'lucide-react';
import './Sidebar.css';

interface UserState {
  user: User | null;
  logout: () => Promise<void>;
}

export const Sidebar: FC = () => {
  const user = useUserStore((state: UserState) => state.user);
  const logout = useUserStore((state: UserState) => state.logout);

  const handleLogout = () => {
    logout();
  };

  return (
    <aside className="sidebar">
      <div className="sidebar-header">
        <div className="sidebar-logo">
          <span className="logo-icon"><DollarSign size={24} /></span>
          <h2 className="logo-text">Controle Financeiro</h2>
        </div>
      </div>

      <nav className="sidebar-nav">
        <ul className="nav-list">
          <li>
            <NavLink 
              to="/dashboard" 
              className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}
            >
              <span className="nav-icon"><BarChart3 size={20} /></span>
              <span className="nav-text">Dashboard</span>
            </NavLink>
          </li>
          <li>
            <NavLink 
              to="/lancamentos" 
              className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}
            >
              <span className="nav-icon"><TrendingDown size={20} /></span>
              <span className="nav-text">Lançamentos</span>
            </NavLink>
          </li>
          <li>
            <NavLink 
              to="/contas" 
              className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}
            >
              <span className="nav-icon"><Building2 size={20} /></span>
              <span className="nav-text">Contas</span>
            </NavLink>
          </li>
        </ul>
      </nav>

      <div className="sidebar-footer">
        <div className="user-info">
          <div className="user-avatar">
            {user?.nome?.charAt(0).toUpperCase() || 'U'}
          </div>
          <div className="user-details">
            <div className="user-name">{user?.nome || 'Usuário'}</div>
            <div className="user-email">{user?.email || 'email@example.com'}</div>
          </div>
        </div>
        <button onClick={handleLogout} className="logout-button">
          <span className="logout-icon"><LogOut size={20} /></span>
          <span className="logout-text">Sair</span>
        </button>
      </div>
    </aside>
  );
};
