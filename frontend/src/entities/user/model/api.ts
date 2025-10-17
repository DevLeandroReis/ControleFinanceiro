import { apiClient } from '@/shared/api';
import type { User, AuthResult } from './types';

/**
 * DTOs matching backend endpoints
 */
export interface LoginDTO {
  email: string;
  senha: string;
}

export interface RegisterDTO {
  nome: string;
  email: string;
  senha: string;
  confirmarSenha: string;
}

export interface ForgotPasswordDTO {
  email: string;
}

export interface ResetPasswordDTO {
  token: string;
  novaSenha: string;
  confirmarNovaSenha: string;
}

export interface UpdateUserDTO {
  nome?: string;
  email?: string;
  senha?: string;
}

/**
 * User API - handles all user-related HTTP requests
 */
export const userApi = {
  /**
   * Login - POST /api/usuarios/login
   * @param data LoginDTO
   * @returns AuthResult with token and user data
   */
  async login(data: LoginDTO): Promise<AuthResult> {
    const response = await apiClient.post<AuthResult>('/usuarios/login', data);
    return response.data;
  },

  /**
   * Register - POST /api/usuarios/registrar
   * @param data RegisterDTO
   * @returns User data
   */
  async register(data: RegisterDTO): Promise<User> {
    const response = await apiClient.post<User>('/usuarios/registrar', data);
    return response.data;
  },

  /**
   * Forgot Password - POST /api/usuarios/recuperar-senha
   * @param data ForgotPasswordDTO
   * @returns Success message
   */
  async forgotPassword(data: ForgotPasswordDTO): Promise<{ message: string }> {
    const response = await apiClient.post<{ message: string }>('/usuarios/recuperar-senha', data);
    return response.data;
  },

  /**
   * Reset Password - POST /api/usuarios/redefinir-senha
   * @param data ResetPasswordDTO
   * @returns Success message
   */
  async resetPassword(data: ResetPasswordDTO): Promise<{ message: string }> {
    const response = await apiClient.post<{ message: string }>('/usuarios/redefinir-senha', data);
    return response.data;
  },

  /**
   * Confirm Email - GET /api/usuarios/confirmar-email?token={token}
   * @param token Email confirmation token
   * @returns Success message
   */
  async confirmEmail(token: string): Promise<{ message: string }> {
    const response = await apiClient.get<{ message: string }>(`/usuarios/confirmar-email?token=${token}`);
    return response.data;
  },

  /**
   * Get User by ID - GET /api/usuarios/{id}
   * @param id User ID
   * @returns User data
   */
  async getUserById(id: string): Promise<User> {
    const response = await apiClient.get<User>(`/usuarios/${id}`);
    return response.data;
  },

  /**
   * Logout - clears local storage
   */
  async logout(): Promise<void> {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user-storage');
  },
};
