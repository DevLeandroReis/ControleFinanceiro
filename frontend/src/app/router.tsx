import { createBrowserRouter, Navigate } from 'react-router-dom';
import { AuthenticatedLayout } from './layouts';
import { ProtectedRoute } from '../features';
import { 
  LoginPage,
  RegisterPage,
  ForgotPasswordPage,
  ResetPasswordPage,
  DashboardPage,
  TransactionsPage,
  AccountsPage,
  NotFoundPage 
} from '../pages';

export const router = createBrowserRouter([
  // Redirect root to login
  {
    path: '/',
    element: <Navigate to="/login" replace />,
  },
  // Authenticated routes with sidebar
  {
    path: '/',
    element: (
      <ProtectedRoute>
        <AuthenticatedLayout />
      </ProtectedRoute>
    ),
    errorElement: <NotFoundPage />,
    children: [
      {
        path: 'dashboard',
        element: <DashboardPage />,
      },
      {
        path: 'lancamentos',
        element: <TransactionsPage />,
      },
      {
        path: 'transacoes',
        element: <TransactionsPage />,
      },
      {
        path: 'contas',
        element: <AccountsPage />,
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
