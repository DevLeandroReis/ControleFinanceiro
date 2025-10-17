import { z } from 'zod';

// Schema para login
export const loginSchema = z.object({
  email: z
    .string()
    .email('E-mail inválido')
    .min(1, 'E-mail é obrigatório'),
  senha: z
    .string()
    .min(6, 'A senha deve ter no mínimo 6 caracteres'),
});

// Schema para registro
export const registerSchema = z.object({
  nome: z
    .string()
    .min(3, 'O nome deve ter no mínimo 3 caracteres')
    .max(100, 'O nome deve ter no máximo 100 caracteres'),
  email: z
    .string()
    .email('E-mail inválido')
    .min(1, 'E-mail é obrigatório'),
  senha: z
    .string()
    .min(6, 'A senha deve ter no mínimo 6 caracteres')
    .max(100, 'A senha deve ter no máximo 100 caracteres'),
});

// Tipos inferidos
export type LoginInput = z.infer<typeof loginSchema>;
export type RegisterInput = z.infer<typeof registerSchema>;
