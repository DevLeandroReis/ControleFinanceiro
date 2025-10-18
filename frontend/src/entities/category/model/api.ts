import { apiClient } from '@/shared/api';
import type { Category } from './types';

export interface CreateCategoryDTO {
  nome: string;
  tipo: 'income' | 'expense';
  cor?: string;
  icone?: string;
  descricao?: string;
}

export interface UpdateCategoryDTO extends Partial<CreateCategoryDTO> {
  id: string;
}

export const categoryApi = {
  /**
   * Busca todas as categorias do usuário
   */
  async getAll(): Promise<Category[]> {
    const response = await apiClient.get<Category[]>('/api/Categorias');
    return response.data;
  },

  /**
   * Busca uma categoria por ID
   */
  async getById(id: string): Promise<Category> {
    const response = await apiClient.get<Category>(`/api/Categorias/${id}`);
    return response.data;
  },

  /**
   * Cria uma nova categoria
   */
  async create(data: CreateCategoryDTO): Promise<Category> {
    const response = await apiClient.post<Category>('/api/Categorias', data);
    return response.data;
  },

  /**
   * Atualiza uma categoria existente
   */
  async update(id: string, data: Partial<CreateCategoryDTO>): Promise<Category> {
    const response = await apiClient.put<Category>(`/api/Categorias/${id}`, data);
    return response.data;
  },

  /**
   * Remove uma categoria
   */
  async delete(id: string): Promise<void> {
    await apiClient.delete<void>(`/api/Categorias/${id}`);
  },

  /**
   * Busca categorias por tipo
   */
  async getByType(tipo: 'income' | 'expense'): Promise<Category[]> {
    const response = await apiClient.get<Category[]>('/api/Categorias', {
      params: { tipo }
    });
    return response.data;
  },

  /**
   * Cria categorias padrão para o usuário
   */
  async createDefaults(): Promise<Category[]> {
    const response = await apiClient.post<Category[]>('/api/Categorias/padrao');
    return response.data;
  }
};
