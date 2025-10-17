import { useState } from 'react';
import { useAccountStore } from '@/entities/account';
import type { CreateAccountInput } from '@/entities/account/model';

export const useEditAccount = () => {
  const { updateAccount, isLoading: storeLoading, error: storeError } = useAccountStore();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [localError, setLocalError] = useState<string | null>(null);

  const handleEditAccount = async (id: string, data: Partial<CreateAccountInput>) => {
    setIsSubmitting(true);
    setLocalError(null);
    
    try {
      await updateAccount(id, data);
      return { success: true };
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Erro ao editar conta';
      setLocalError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    editAccount: handleEditAccount,
    isLoading: isSubmitting || storeLoading,
    error: localError || storeError,
  };
};
