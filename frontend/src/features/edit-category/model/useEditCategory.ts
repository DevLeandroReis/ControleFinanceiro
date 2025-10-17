import { useState } from 'react';
import { useCategoryStore } from '@/entities/category';
import type { CreateCategoryInput } from '@/entities/category/model';

export const useEditCategory = () => {
  const { updateCategory, isLoading: storeLoading, error: storeError } = useCategoryStore();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [localError, setLocalError] = useState<string | null>(null);

  const handleEditCategory = async (id: string, data: Partial<CreateCategoryInput>) => {
    setIsSubmitting(true);
    setLocalError(null);
    
    try {
      await updateCategory(id, data);
      return { success: true };
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Erro ao editar categoria';
      setLocalError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    editCategory: handleEditCategory,
    isLoading: isSubmitting || storeLoading,
    error: localError || storeError,
  };
};
