import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { categoryApi, type CreateCategoryDTO } from './api';
import type { Category, CategoryType } from './types';

interface CategoryState {
  categories: Category[];
  isLoading: boolean;
  error: string | null;

  // Actions com API
  fetchCategories: () => Promise<void>;
  addCategory: (data: CreateCategoryDTO) => Promise<void>;
  updateCategory: (id: string, data: Partial<CreateCategoryDTO>) => Promise<void>;
  deleteCategory: (id: string) => Promise<void>;
  createDefaultCategories: () => Promise<void>;
  
  // Helpers
  setCategories: (categories: Category[]) => void;
  getCategoriesByType: (type: CategoryType) => Category[];
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearCategories: () => void;
}

export const useCategoryStore = create<CategoryState>()(
  devtools(
    persist(
      (set, get) => ({
        categories: [],
        isLoading: false,
        error: null,

        fetchCategories: async () => {
          set({ isLoading: true, error: null });
          try {
            const categories = await categoryApi.getAll();
            set({ categories, isLoading: false });
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao carregar categorias';
            set({ error: errorMessage, isLoading: false });
          }
        },

        addCategory: async (data) => {
          set({ isLoading: true, error: null });
          try {
            const newCategory = await categoryApi.create(data);
            set((state) => ({
              categories: [...state.categories, newCategory],
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao adicionar categoria';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        updateCategory: async (id, data) => {
          set({ isLoading: true, error: null });
          try {
            const updatedCategory = await categoryApi.update(id, data);
            set((state) => ({
              categories: state.categories.map((c) =>
                c.id === id ? updatedCategory : c
              ),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao atualizar categoria';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        deleteCategory: async (id) => {
          set({ isLoading: true, error: null });
          try {
            await categoryApi.delete(id);
            set((state) => ({
              categories: state.categories.filter((c) => c.id !== id),
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao deletar categoria';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        createDefaultCategories: async () => {
          set({ isLoading: true, error: null });
          try {
            const defaultCategories = await categoryApi.createDefaults();
            set((state) => ({
              categories: [...state.categories, ...defaultCategories],
              isLoading: false,
            }));
          } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Erro ao criar categorias padrão';
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },

        setCategories: (categories) => set({ categories, error: null }),
        
        getCategoriesByType: (type: CategoryType) => {
          const { categories } = get();
          return categories.filter((c) => c.tipo === type);
        },

        setLoading: (isLoading) => set({ isLoading }),
        setError: (error) => set({ error, isLoading: false }),
        clearCategories: () => set({ categories: [], error: null }),
      }),
      {
        name: 'categories-storage',
      }
    ),
    { name: 'CategoryStore' }
  )
);
