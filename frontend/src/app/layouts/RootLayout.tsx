import type { FC } from 'react';
import { Outlet } from 'react-router-dom';
import { Navigation } from '../../widgets/navigation';
import './RootLayout.css';

export const RootLayout: FC = () => {
  return (
    <div className="root-layout">
      <Navigation />
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
};
