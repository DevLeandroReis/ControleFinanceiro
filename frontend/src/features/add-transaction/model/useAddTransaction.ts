import { useState } from 'react';
import { useTransactionStore } from '@/entities/transaction';
import type { CreateTransactionInput } from '@/entities/transaction/model';

export const useAddTransaction = () => {
  const { addTransaction, isLoading: storeLoading, error: storeError } = useTransactionStore();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [localError, setLocalError] = useState<string | null>(null);

  const handleAddTransaction = async (data: CreateTransactionInput) => {
    setIsSubmitting(true);
    setLocalError(null);
    
    try {
      await addTransaction(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Erro ao adicionar transação';
      setLocalError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    addTransaction: handleAddTransaction,
    isLoading: isSubmitting || storeLoading,
    error: localError || storeError,
  };
};
