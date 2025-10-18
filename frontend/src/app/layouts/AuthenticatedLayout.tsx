import type { FC } from 'react';
import { Outlet } from 'react-router-dom';
import { Sidebar } from '../../widgets/sidebar';
import './AuthenticatedLayout.css';

export const AuthenticatedLayout: FC = () => {
  return (
    <div className="authenticated-layout">
      <Sidebar />
      <main className="authenticated-content">
        <Outlet />
      </main>
    </div>
  );
};
