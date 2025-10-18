// Shared Config
// Application configuration, constants

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001';

export const APP_CONFIG = {
  name: 'Controle Financeiro',
  version: '0.0.1',
} as const;
