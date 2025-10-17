import { type FC, type ReactNode, useEffect } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useUserStore } from '../../entities/user';

interface ProtectedRouteProps {
  children: ReactNode;
}

/**
 * ProtectedRoute component - Protects routes that require authentication
 * Redirects to login page if user is not authenticated
 */
export const ProtectedRoute: FC<ProtectedRouteProps> = ({ children }) => {
  const { isAuthenticated, token } = useUserStore();
  const location = useLocation();

  useEffect(() => {
    // Check if token exists in localStorage
    const storedToken = localStorage.getItem('auth_token');
    if (!token && !storedToken) {
      console.log('User not authenticated, redirecting to login');
    }
  }, [token]);

  // If not authenticated, redirect to login with return URL
  if (!isAuthenticated && !token) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <>{children}</>;
};
