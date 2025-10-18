import { apiClient } from '@/shared/api';
import type { Account, AccessRequest, UserAccount } from './types';

export interface CreateAccountDTO {
  nome: string;
  descricao?: string;
}

export interface UpdateAccountDTO {
  nome?: string;
  descricao?: string;
}

export interface RequestAccessDTO {
  contaId: string;
  mensagem?: string;
}

export const accountApi = {
  // ========== CRUD Operations ==========
  
  /**
   * Busca todas as contas do usuário
   */
  async getAll(): Promise<Account[]> {
    const response = await apiClient.get<Account[]>('/api/Contas/minhas');
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
  async update(id: string, data: UpdateAccountDTO): Promise<Account> {
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
   * Ativa uma conta
   */
  async activate(id: string): Promise<void> {
    await apiClient.patch<void>(`/api/Contas/${id}/ativar`);
  },

  /**
   * Desativa uma conta
   */
  async deactivate(id: string): Promise<void> {
    await apiClient.patch<void>(`/api/Contas/${id}/desativar`);
  },

  // ========== Access Request Operations ==========
  
  /**
   * Solicita acesso a uma conta usando seu ID
   */
  async requestAccess(data: RequestAccessDTO): Promise<AccessRequest> {
    const response = await apiClient.post<AccessRequest>('/api/Contas/solicitar-acesso', data);
    return response.data;
  },

  /**
   * Aprova uma solicitação de acesso
   */
  async approveRequest(solicitacaoId: string): Promise<void> {
    await apiClient.post<void>(`/api/Contas/solicitacoes/${solicitacaoId}/aprovar`);
  },

  /**
   * Rejeita uma solicitação de acesso
   */
  async rejectRequest(solicitacaoId: string): Promise<void> {
    await apiClient.post<void>(`/api/Contas/solicitacoes/${solicitacaoId}/rejeitar`);
  },

  /**
   * Cancela uma solicitação de acesso enviada
   */
  async cancelRequest(solicitacaoId: string): Promise<void> {
    await apiClient.post<void>(`/api/Contas/solicitacoes/${solicitacaoId}/cancelar`);
  },

  /**
   * Busca solicitações de acesso recebidas (como proprietário)
   */
  async getReceivedRequests(): Promise<AccessRequest[]> {
    const response = await apiClient.get<AccessRequest[]>('/api/Contas/solicitacoes/recebidas');
    return response.data;
  },

  /**
   * Busca solicitações de acesso enviadas (como solicitante)
   */
  async getSentRequests(): Promise<AccessRequest[]> {
    const response = await apiClient.get<AccessRequest[]>('/api/Contas/solicitacoes/enviadas');
    return response.data;
  },

  // ========== User Management Operations ==========
  
  /**
   * Busca os usuários de uma conta
   */
  async getAccountUsers(contaId: string): Promise<UserAccount[]> {
    const response = await apiClient.get<UserAccount[]>(`/api/Contas/${contaId}/usuarios`);
    return response.data;
  },

  /**
   * Remove um usuário de uma conta
   */
  async removeUser(contaId: string, usuarioId: string): Promise<void> {
    await apiClient.delete<void>(`/api/Contas/${contaId}/usuarios/${usuarioId}`);
  },

  /**
   * Concede permissão para adicionar usuários
   */
  async grantPermission(contaId: string, usuarioId: string): Promise<void> {
    await apiClient.post<void>(`/api/Contas/${contaId}/usuarios/${usuarioId}/conceder-permissao`);
  },

  /**
   * Remove permissão para adicionar usuários
   */
  async revokePermission(contaId: string, usuarioId: string): Promise<void> {
    await apiClient.post<void>(`/api/Contas/${contaId}/usuarios/${usuarioId}/remover-permissao`);
  },
};
