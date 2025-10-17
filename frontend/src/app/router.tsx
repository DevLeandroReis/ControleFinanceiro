import { createBrowserRouter } from 'react-router-dom';
import { RootLayout } from './layouts';
import { ProtectedRoute } from '../features';
import { 
  HomePage, 
  LoginPage,
  RegisterPage,
  ForgotPasswordPage,
  ResetPasswordPage,
  DashboardPage,
  TransactionsPage,
  AccountsPage,
  CategoriesPage,
  NotFoundPage 
} from '../pages';

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
        element: (
          <ProtectedRoute>
            <DashboardPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'transacoes',
        element: (
          <ProtectedRoute>
            <TransactionsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'contas',
        element: (
          <ProtectedRoute>
            <AccountsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'categorias',
        element: (
          <ProtectedRoute>
            <CategoriesPage />
          </ProtectedRoute>
        ),
      },
    ],
  },
  // Auth routes - outside main layout
  {
    path: 'login',
    element: <LoginPage />,
  },
  {
    path: 'register',
    element: <RegisterPage />,
  },
  {
    path: 'forgot-password',
    element: <ForgotPasswordPage />,
  },
  {
    path: 'reset-password',
    element: <ResetPasswordPage />,
  },
]);
