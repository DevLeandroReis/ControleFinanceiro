import { apiClient } from '@/shared/api';
import type { Account } from './types';

export interface CreateAccountDTO {
  nome: string;
  tipo: 'corrente' | 'poupanca' | 'investimento' | 'carteira';
  saldoInicial: number;
  cor?: string;
  descricao?: string;
}

export interface UpdateAccountDTO extends Partial<CreateAccountDTO> {
  id: string;
}

export const accountApi = {
  /**
   * Busca todas as contas do usuário
   */
  async getAll(): Promise<Account[]> {
    const response = await apiClient.get<Account[]>('/api/Contas');
    return response.data;
  },

  /**
   * Busca uma conta por ID
   */
  async getById(id: string): Promise<Account> {
    const response = await apiClient.get<Account>(`/api/Contas/${id}`);
    return response.data;
  },

  /**
   * Cria uma nova conta
   */
  async create(data: CreateAccountDTO): Promise<Account> {
    const response = await apiClient.post<Account>('/api/Contas', data);
    return response.data;
  },

  /**
   * Atualiza uma conta existente
   */
  async update(id: string, data: Partial<CreateAccountDTO>): Promise<Account> {
    const response = await apiClient.put<Account>(`/api/Contas/${id}`, data);
    return response.data;
  },

  /**
   * Remove uma conta
   */
  async delete(id: string): Promise<void> {
    await apiClient.delete<void>(`/api/Contas/${id}`);
  },

  /**
   * Busca o saldo total de todas as contas
   */
  async getTotalBalance(): Promise<{ total: number }> {
    const response = await apiClient.get<{ total: number }>('/api/Contas/saldo-total');
    return response.data;
  },

  /**
   * Busca o saldo de uma conta específica
   */
  async getBalance(id: string): Promise<{ saldo: number }> {
    const response = await apiClient.get<{ saldo: number }>(`/api/Contas/${id}/saldo`);
    return response.data;
  }
};
