// User entity types based on backend DTOs

/**
 * User entity - matches UsuarioDto from backend
 */
export interface User {
  id: string;
  nome: string;
  email: string;
  emailConfirmado: boolean;
  ultimoLogin: string | null;
  ativo: boolean;
  createdAt: string;
  updatedAt: string;
}

/**
 * Auth result - matches AuthResultDto from backend
 */
export interface AuthResult {
  token: string;
  usuario: User;
  expiresAt: string;
}

/**
 * User state interface (deprecated - use useUserStore instead)
 */
export interface UserState {
  user: User | null;
  isLoading: boolean;
  error: string | null;
  setUser: (user: User | null) => void;
  clearUser: () => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
}
