import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { userApi, type LoginDTO, type RegisterDTO, type UpdateUserDTO } from './api';
import type { User } from './types';

interface UserState {
  user: User | null;
  isLoading: boolean;
  error: string | null;
  isAuthenticated: boolean;

  // Actions com API
  login: (data: LoginDTO) => Promise<void>;
  register: (data: RegisterDTO) => Promise<void>;
  logout: () => Promise<void>;
  fetchProfile: () => Promise<void>;
  updateProfile: (data: UpdateUserDTO) => Promise<void>;
  
  // Helpers
  setUser: (user: User | null) => void;
  clearUser: () => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
}

export const useUserStore = create<UserState>()(
  devtools(
    persist(
      (set) => ({
        user: null,
        isLoading: false,
        error: null,
        isAuthenticated: false,

        login: async (data) => {
          set({ isLoading: true, error: null });
          try {
            const response = await userApi.login(data);
            
            // Salvar token no localStorage
            localStorage.setItem('auth_token', response.token);
            
            set({ 
              user: response.usuario, 
              isAuthenticated: true,
              isLoading: false 
            });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao fazer login';
            set({ error: errorMessage, isLoading: false, isAuthenticated: false });
            throw error;
          }
        },

        register: async (data) => {
          set({ isLoading: true, error: null });
          try {
            const response = await userApi.register(data);
            
            // Salvar token no localStorage
            localStorage.setItem('auth_token', response.token);
            
            set({ 
              user: response.usuario, 
              isAuthenticated: true,
              isLoading: false 
            });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao registrar usuário';
            set({ error: errorMessage, isLoading: false, isAuthenticated: false });
            throw error;
          }
        },

        logout: async () => {
          set({ isLoading: true, error: null });
          try {
            await userApi.logout();
            set({ 
              user: null, 
              isAuthenticated: false,
              isLoading: false 
            });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao fazer logout';
            set({ error: errorMessage, isLoading: false });
          }
        },

        fetchProfile: async () => {
          set({ isLoading: true, error: null });
          try {
            const user = await userApi.getProfile();
            set({ 
              user, 
              isAuthenticated: true,
              isLoading: false 
            });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar perfil';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        updateProfile: async (data) => {
          set({ isLoading: true, error: null });
          try {
            const updatedUser = await userApi.updateProfile(data);
            set({ 
              user: updatedUser, 
              isLoading: false 
            });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao atualizar perfil';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        setUser: (user) => set({ 
          user, 
          isAuthenticated: user !== null,
          error: null 
        }),
        
        clearUser: () => set({ 
          user: null, 
          isAuthenticated: false,
          error: null 
        }),
        
        setLoading: (isLoading) => set({ isLoading }),
        setError: (error) => set({ error, isLoading: false }),
      }),
      {
        name: 'user-storage',
      }
    ),
    { name: 'UserStore' }
  )
);
