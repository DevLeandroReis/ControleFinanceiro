import { apiClient } from '@/shared/api';
import type { User } from './types';

export interface LoginDTO {
  email: string;
  senha: string;
}

export interface RegisterDTO {
  nome: string;
  email: string;
  senha: string;
}

export interface LoginResponse {
  token: string;
  usuario: User;
}

export interface UpdateUserDTO {
  nome?: string;
  email?: string;
  senha?: string;
}

export const userApi = {
  /**
   * Faz login do usuário
   */
  async login(data: LoginDTO): Promise<LoginResponse> {
    const response = await apiClient.post<LoginResponse>('/usuarios/login', data);
    return response.data;
  },

  /**
   * Registra um novo usuário
   */
  async register(data: RegisterDTO): Promise<LoginResponse> {
    const response = await apiClient.post<LoginResponse>('/usuarios/registro', data);
    return response.data;
  },

  /**
   * Busca os dados do usuário logado
   */
  async getProfile(): Promise<User> {
    const response = await apiClient.get<User>('/usuarios/perfil');
    return response.data;
  },

  /**
   * Atualiza os dados do usuário
   */
  async updateProfile(data: UpdateUserDTO): Promise<User> {
    const response = await apiClient.post<User>('/usuarios/perfil', data);
    return response.data;
  },

  /**
   * Faz logout do usuário
   */
  async logout(): Promise<void> {
    // Limpa o token do localStorage
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user-storage');
  },

  /**
   * Solicita recuperação de senha
   */
  async forgotPassword(email: string): Promise<{ message: string }> {
    const response = await apiClient.post<{ message: string }>('/usuarios/esqueci-senha', { email });
    return response.data;
  },

  /**
   * Reseta a senha com o token recebido por email
   */
  async resetPassword(token: string, newPassword: string): Promise<{ message: string }> {
    const response = await apiClient.post<{ message: string }>('/usuarios/resetar-senha', {
      token,
      novaSenha: newPassword
    });
    return response.data;
  }
};
