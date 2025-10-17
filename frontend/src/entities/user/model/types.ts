// User entity types
export interface User {
  id: string;
  nome: string;
  email: string;
  dataCriacao: string;
}

export interface UserState {
  user: User | null;
  isLoading: boolean;
  error: string | null;
  setUser: (user: User | null) => void;
  clearUser: () => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
}
