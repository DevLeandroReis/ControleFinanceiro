import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { userApi, type LoginDTO, type RegisterDTO, type ForgotPasswordDTO, type ResetPasswordDTO } from './api';
import type { User } from './types';
import { AxiosError } from 'axios';

/**
 * Helper function to extract error message from API responses
 */
const getErrorMessage = (error: unknown): string => {
  if (error instanceof AxiosError) {
    const data = error.response?.data;
    if (typeof data === 'string') return data;
    if (data?.message) return data.message;
    if (data?.errors) {
      // Flatten validation errors
      const messages = Object.values(data.errors).flat();
      return messages.join(', ');
    }
  }
  if (error instanceof Error) return error.message;
  return 'Erro desconhecido';
};

interface UserState {
  user: User | null;
  token: string | null;
  isLoading: boolean;
  error: string | null;
  isAuthenticated: boolean;
  successMessage: string | null;

  // Auth actions
  login: (data: LoginDTO) => Promise<void>;
  register: (data: RegisterDTO) => Promise<void>;
  logout: () => Promise<void>;
  forgotPassword: (data: ForgotPasswordDTO) => Promise<void>;
  resetPassword: (data: ResetPasswordDTO) => Promise<void>;
  
  // User data actions
  fetchUserById: (id: string) => Promise<void>;
  
  // Helpers
  setUser: (user: User | null) => void;
  clearUser: () => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  setSuccessMessage: (message: string | null) => void;
  clearMessages: () => void;
}

export const useUserStore = create<UserState>()(
  devtools(
    persist(
      (set) => ({
        user: null,
        token: null,
        isLoading: false,
        error: null,
        successMessage: null,
        isAuthenticated: false,

        login: async (data) => {
          set({ isLoading: true, error: null, successMessage: null });
          try {
            const response = await userApi.login(data);
            
            // Save token to localStorage
            localStorage.setItem('auth_token', response.token);
            
            set({ 
              user: response.usuario,
              token: response.token,
              isAuthenticated: true,
              isLoading: false,
              error: null,
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false, isAuthenticated: false });
            throw error;
          }
        },

        register: async (data) => {
          set({ isLoading: true, error: null, successMessage: null });
          try {
            const user = await userApi.register(data);
            
            set({ 
              user,
              isLoading: false,
              error: null,
              successMessage: 'Cadastro realizado com sucesso! Verifique seu email para confirmar sua conta.',
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        logout: async () => {
          set({ isLoading: true, error: null, successMessage: null });
          try {
            await userApi.logout();
            set({ 
              user: null,
              token: null,
              isAuthenticated: false,
              isLoading: false,
              error: null,
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false });
          }
        },

        forgotPassword: async (data) => {
          set({ isLoading: true, error: null, successMessage: null });
          try {
            const response = await userApi.forgotPassword(data);
            set({ 
              isLoading: false,
              error: null,
              successMessage: response.message,
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        resetPassword: async (data) => {
          set({ isLoading: true, error: null, successMessage: null });
          try {
            const response = await userApi.resetPassword(data);
            set({ 
              isLoading: false,
              error: null,
              successMessage: response.message,
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        fetchUserById: async (id: string) => {
          set({ isLoading: true, error: null });
          try {
            const user = await userApi.getUserById(id);
            set({ 
              user,
              isAuthenticated: true,
              isLoading: false,
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        setUser: (user) => set({ 
          user, 
          isAuthenticated: user !== null,
          error: null,
        }),
        
        clearUser: () => set({ 
          user: null,
          token: null,
          isAuthenticated: false,
          error: null,
        }),
        
        setLoading: (isLoading) => set({ isLoading }),
        
        setError: (error) => set({ error, isLoading: false }),
        
        setSuccessMessage: (successMessage) => set({ successMessage }),
        
        clearMessages: () => set({ error: null, successMessage: null }),
      }),
      {
        name: 'user-storage',
        partialize: (state) => ({
          user: state.user,
          token: state.token,
          isAuthenticated: state.isAuthenticated,
        }),
      }
    ),
    { name: 'UserStore' }
  )
);
