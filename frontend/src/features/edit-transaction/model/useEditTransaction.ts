import { useState } from 'react';
import { useTransactionStore } from '@/entities/transaction';
import type { CreateTransactionInput } from '@/entities/transaction/model';

export const useEditTransaction = () => {
  const { updateTransaction, isLoading: storeLoading, error: storeError } = useTransactionStore();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [localError, setLocalError] = useState<string | null>(null);

  const handleEditTransaction = async (id: string, data: Partial<CreateTransactionInput>) => {
    setIsSubmitting(true);
    setLocalError(null);
    
    try {
      await updateTransaction(id, data);
      return { success: true };
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Erro ao editar transação';
      setLocalError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    editTransaction: handleEditTransaction,
    isLoading: isSubmitting || storeLoading,
    error: localError || storeError,
  };
};
