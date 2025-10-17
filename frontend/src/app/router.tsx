import { createBrowserRouter } from 'react-router-dom';
import { RootLayout } from './layouts';
import { HomePage } from '../pages/home';
import { DashboardPage } from '../pages/dashboard';
import { TransactionsPage } from '../pages/transactions';
import { AccountsPage } from '../pages/accounts';
import { CategoriesPage } from '../pages/categories';
import { NotFoundPage } from '../pages/not-found';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <RootLayout />,
    errorElement: <NotFoundPage />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: 'dashboard',
        element: <DashboardPage />,
      },
      {
        path: 'transacoes',
        element: <TransactionsPage />,
      },
      {
        path: 'contas',
        element: <AccountsPage />,
      },
      {
        path: 'categorias',
        element: <CategoriesPage />,
      },
    ],
  },
]);
