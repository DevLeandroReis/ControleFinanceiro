import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { accountApi } from './api';
import type { AccountState } from './types';

export const useAccountStore = create<AccountState>()(
  devtools(
    persist(
      (set, get) => ({
        accounts: [],
        receivedRequests: [],
        sentRequests: [],
        isLoading: false,
        error: null,

        // ========== Account CRUD Operations ==========
        
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

        activateAccount: async (id) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.activate(id);
            set((state) => ({
              accounts: state.accounts.map((a) =>
                a.id === id ? { ...a, ativa: true } : a
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao ativar conta';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        deactivateAccount: async (id) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.deactivate(id);
            set((state) => ({
              accounts: state.accounts.map((a) =>
                a.id === id ? { ...a, ativa: false } : a
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao desativar conta';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        // ========== Access Request Operations ==========
        
        requestAccess: async (contaId, mensagem) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.requestAccess({ contaId, mensagem });
            // Reload sent requests after creating a new one
            await get().fetchSentRequests();
            set({ isLoading: false });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao solicitar acesso';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        approveRequest: async (solicitacaoId) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.approveRequest(solicitacaoId);
            // Remove from received requests and reload accounts
            set((state) => ({
              receivedRequests: state.receivedRequests.filter((r) => r.id !== solicitacaoId),
              isLoading: false,
            }));
            await get().fetchAccounts();
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao aprovar solicitação';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        rejectRequest: async (solicitacaoId) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.rejectRequest(solicitacaoId);
            // Remove from received requests
            set((state) => ({
              receivedRequests: state.receivedRequests.filter((r) => r.id !== solicitacaoId),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao rejeitar solicitação';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        cancelRequest: async (solicitacaoId) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.cancelRequest(solicitacaoId);
            // Remove from sent requests
            set((state) => ({
              sentRequests: state.sentRequests.filter((r) => r.id !== solicitacaoId),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao cancelar solicitação';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        fetchReceivedRequests: async () => {
          set({ isLoading: true, error: null });
          try {
            const requests = await accountApi.getReceivedRequests();
            set({ receivedRequests: requests, isLoading: false });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar solicitações recebidas';
            set({ error: errorMessage, isLoading: false });
          }
        },

        fetchSentRequests: async () => {
          set({ isLoading: true, error: null });
          try {
            const requests = await accountApi.getSentRequests();
            set({ sentRequests: requests, isLoading: false });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar solicitações enviadas';
            set({ error: errorMessage, isLoading: false });
          }
        },

        // ========== User Management Operations ==========
        
        getAccountUsers: async (contaId) => {
          set({ isLoading: true, error: null });
          try {
            const users = await accountApi.getAccountUsers(contaId);
            set({ isLoading: false });
            return users;
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar usuários da conta';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        removeUserFromAccount: async (contaId, usuarioId) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.removeUser(contaId, usuarioId);
            // Update the account's users list
            set((state) => ({
              accounts: state.accounts.map((a) =>
                a.id === contaId
                  ? { ...a, usuarios: a.usuarios.filter((u) => u.usuarioId !== usuarioId) }
                  : a
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao remover usuário';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        grantPermission: async (contaId, usuarioId) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.grantPermission(contaId, usuarioId);
            // Update the user's permission in the account
            set((state) => ({
              accounts: state.accounts.map((a) =>
                a.id === contaId
                  ? {
                      ...a,
                      usuarios: a.usuarios.map((u) =>
                        u.usuarioId === usuarioId ? { ...u, podeAdicionarUsuarios: true } : u
                      ),
                    }
                  : a
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao conceder permissão';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        revokePermission: async (contaId, usuarioId) => {
          set({ isLoading: true, error: null });
          try {
            await accountApi.revokePermission(contaId, usuarioId);
            // Update the user's permission in the account
            set((state) => ({
              accounts: state.accounts.map((a) =>
                a.id === contaId
                  ? {
                      ...a,
                      usuarios: a.usuarios.map((u) =>
                        u.usuarioId === usuarioId ? { ...u, podeAdicionarUsuarios: false } : u
                      ),
                    }
                  : a
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao remover permissão';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        // ========== State Management ==========
        
        setAccounts: (accounts) => set({ accounts, error: null }),
        setLoading: (isLoading) => set({ isLoading }),
        setError: (error) => set({ error, isLoading: false }),
        clearAccounts: () => set({ accounts: [], receivedRequests: [], sentRequests: [], error: null }),
        
        getTotalBalance: () => {
          const { accounts } = get();
          return accounts.reduce((total, account) => total + (account.saldoAtual || 0), 0);
        },
      }),
      {
        name: 'accounts-storage',
        partialize: (state) => ({
          accounts: state.accounts,
          // Don't persist requests as they should be fetched fresh
        }),
      }
    ),
    { name: 'AccountStore' }
  )
);
