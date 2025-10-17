import axios, { type AxiosInstance, type AxiosError, type InternalAxiosRequestConfig } from 'axios';
import { API_BASE_URL } from '../config';

// Criar instância do Axios
export const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor de Request - Adiciona token de autenticação
apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Adicionar token do localStorage se existir
    const token = localStorage.getItem('auth_token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error);
  }
);

// Interceptor de Response - Tratamento de erros
apiClient.interceptors.response.use(
  (response) => {
    return response;
  },
  (error: AxiosError) => {
    // Tratamento de erros específicos
    if (error.response) {
      switch (error.response.status) {
        case 401:
          // Não autorizado - limpar token e redirecionar para login
          localStorage.removeItem('auth_token');
          localStorage.removeItem('user-storage');
          window.location.href = '/';
          break;
        case 403:
          console.error('Acesso negado');
          break;
        case 404:
          console.error('Recurso não encontrado');
          break;
        case 500:
          console.error('Erro interno do servidor');
          break;
        default:
          console.error('Erro na requisição:', error.response.data);
      }
    } else if (error.request) {
      console.error('Erro de conexão: Não foi possível conectar ao servidor');
    } else {
      console.error('Erro:', error.message);
    }
    return Promise.reject(error);
  }
);

// Tipos para respostas da API
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
}
