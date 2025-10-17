// Category entity types
export type CategoryType = 'income' | 'expense';

export interface Category {
  id: string;
  nome: string;
  tipo: CategoryType;
  cor?: string;
  icone?: string;
  usuarioId: string;
}

export interface CategoryState {
  categories: Category[];
  isLoading: boolean;
  error: string | null;
  addCategory: (category: Omit<Category, 'id'>) => void;
  updateCategory: (id: string, category: Partial<Category>) => void;
  deleteCategory: (id: string) => void;
  setCategories: (categories: Category[]) => void;
  getCategoriesByType: (type: CategoryType) => Category[];
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearCategories: () => void;
}
