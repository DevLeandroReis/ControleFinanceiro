import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { accountApi, type CreateAccountDTO } from './api';
import type { Account } from './types';

interface AccountState {
  accounts: Account[];
  isLoading: boolean;
  error: string | null;

  // Actions com API
  fetchAccounts: () => Promise<void>;
  addAccount: (data: CreateAccountDTO) => Promise<void>;
  updateAccount: (id: string, data: Partial<CreateAccountDTO>) => Promise<void>;
  deleteAccount: (id: string) => Promise<void>;
  
  // Helpers
  setAccounts: (accounts: Account[]) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearAccounts: () => void;
  getTotalBalance: () => number;
}

export const useAccountStore = create<AccountState>()(
  devtools(
    persist(
      (set, get) => ({
        accounts: [],
        isLoading: false,
        error: null,

        fetchAccounts: async () => {
          set({ isLoading: true, error: null });
          try {
            const accounts = await accountApi.getAll();
            set({ accounts, isLoading: false });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar contas';
            set({ error: errorMessage, isLoading: false });
          }
        },

        addAccount: async (data) => {
          set({ isLoading: true, error: null });
          try {
            const newAccount = await accountApi.create(data);
            set((state) => ({
              accounts: [...state.accounts, newAccount],
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao adicionar conta';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        updateAccount: async (id, data) => {
          set({ isLoading: true, error: null });
          try {
            const updatedAccount = await accountApi.update(id, data);
            set((state) => ({
              accounts: state.accounts.map((a) =>
                a.id === id ? updatedAccount : a
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao atualizar conta';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        deleteAccount: async (id) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.delete(id);
            set((state) => ({
              accounts: state.accounts.filter((a) => a.id !== id),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao deletar conta';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        setAccounts: (accounts) => set({ accounts, error: null }),
        setLoading: (isLoading) => set({ isLoading }),
        setError: (error) => set({ error, isLoading: false }),
        clearAccounts: () => set({ accounts: [], error: null }),
        
        getTotalBalance: () => {
          const { accounts } = get();
          return accounts.reduce((total, account) => total + account.saldoAtual, 0);
        },
      }),
      {
        name: 'accounts-storage',
      }
    ),
    { name: 'AccountStore' }
  )
);
