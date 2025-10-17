import { z } from 'zod';

// Schema para criar conta
export const createAccountSchema = z.object({
  nome: z
    .string()
    .min(3, 'O nome deve ter no mínimo 3 caracteres')
    .max(100, 'O nome deve ter no máximo 100 caracteres'),
  tipo: z.enum(['corrente', 'poupanca', 'investimento', 'carteira'], {
    message: 'Tipo de conta inválido',
  }),
  saldoInicial: z.number(),
  cor: z.string().optional(),
  usuarioId: z.string().optional(),
});

// Schema para atualizar conta
export const updateAccountSchema = createAccountSchema.partial();

// Tipos inferidos
export type CreateAccountInput = z.infer<typeof createAccountSchema>;
export type UpdateAccountInput = z.infer<typeof updateAccountSchema>;
