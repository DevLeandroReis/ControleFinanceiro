import { apiClient } from '@/shared/api';
import type { Transaction } from './types';

export interface CreateTransactionDTO {
  descricao: string;
  valor: number;
  data: string;
  tipo: 'income' | 'expense';
  categoriaId: string;
  contaId: string;
  observacoes?: string;
}

export interface UpdateTransactionDTO extends Partial<CreateTransactionDTO> {
  id: string;
}

export const transactionApi = {
  /**
   * Busca todas as transações do usuário
   */
  async getAll(): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/lancamentos');
    return response.data;
  },

  /**
   * Busca uma transação por ID
   */
  async getById(id: string): Promise<Transaction> {
    const response = await apiClient.get<Transaction>(`/lancamentos/${id}`);
    return response.data;
  },

  /**
   * Cria uma nova transação
   */
  async create(data: CreateTransactionDTO): Promise<Transaction> {
    const response = await apiClient.post<Transaction>('/lancamentos', data);
    return response.data;
  },

  /**
   * Atualiza uma transação existente
   */
  async update(id: string, data: Partial<CreateTransactionDTO>): Promise<Transaction> {
    const response = await apiClient.put<Transaction>(`/lancamentos/${id}`, data);
    return response.data;
  },

  /**
   * Remove uma transação
   */
  async delete(id: string): Promise<void> {
    await apiClient.delete<void>(`/lancamentos/${id}`);
  },

  /**
   * Busca transações por período
   */
  async getByPeriod(startDate: string, endDate: string): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/lancamentos', {
      params: { startDate, endDate }
    });
    return response.data;
  },

  /**
   * Busca transações por conta
   */
  async getByAccount(contaId: string): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>(`/lancamentos/conta/${contaId}`);
    return response.data;
  },

  /**
   * Busca transações por categoria
   */
  async getByCategory(categoriaId: string): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>(`/lancamentos/categoria/${categoriaId}`);
    return response.data;
  }
};
