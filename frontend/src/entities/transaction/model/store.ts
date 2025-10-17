import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { transactionApi, type CreateTransactionDTO } from './api';
import type { Transaction } from './types';

interface TransactionState {
  transactions: Transaction[];
  isLoading: boolean;
  error: string | null;

  // Actions com API
  fetchTransactions: () => Promise<void>;
  addTransaction: (data: CreateTransactionDTO) => Promise<void>;
  updateTransaction: (id: string, data: Partial<CreateTransactionDTO>) => Promise<void>;
  deleteTransaction: (id: string) => Promise<void>;
  
  // Helpers
  setTransactions: (transactions: Transaction[]) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearTransactions: () => void;
}

export const useTransactionStore = create<TransactionState>()(
  devtools(
    (set) => ({
      transactions: [],
      isLoading: false,
      error: null,

      fetchTransactions: async () => {
        set({ isLoading: true, error: null });
        try {
          const transactions = await transactionApi.getAll();
          set({ transactions, isLoading: false });
        } catch (error) {
          const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar transações';
          set({ error: errorMessage, isLoading: false });
        }
      },

      addTransaction: async (data) => {
        set({ isLoading: true, error: null });
        try {
          const newTransaction = await transactionApi.create(data);
          set((state) => ({
            transactions: [...state.transactions, newTransaction],
            isLoading: false,
          }));
        } catch (error) {
          const errorMessage = error instanceof Error ? error.message : 'Erro ao adicionar transação';
          set({ error: errorMessage, isLoading: false });
          throw error;
        }
      },

      updateTransaction: async (id, data) => {
        set({ isLoading: true, error: null });
        try {
          const updatedTransaction = await transactionApi.update(id, data);
          set((state) => ({
            transactions: state.transactions.map((t) =>
              t.id === id ? updatedTransaction : t
            ),
            isLoading: false,
          }));
        } catch (error) {
          const errorMessage = error instanceof Error ? error.message : 'Erro ao atualizar transação';
          set({ error: errorMessage, isLoading: false });
          throw error;
        }
      },

      deleteTransaction: async (id) => {
        set({ isLoading: true, error: null });
        try {
          await transactionApi.delete(id);
          set((state) => ({
            transactions: state.transactions.filter((t) => t.id !== id),
            isLoading: false,
          }));
        } catch (error) {
          const errorMessage = error instanceof Error ? error.message : 'Erro ao deletar transação';
          set({ error: errorMessage, isLoading: false });
          throw error;
        }
      },

      setTransactions: (transactions) => set({ transactions, error: null }),
      setLoading: (isLoading) => set({ isLoading }),
      setError: (error) => set({ error, isLoading: false }),
      clearTransactions: () => set({ transactions: [], error: null }),
    }),
    { name: 'TransactionStore' }
  )
);
