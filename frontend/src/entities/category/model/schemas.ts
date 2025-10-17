import { z } from 'zod';

// Schema para criar categoria
export const createCategorySchema = z.object({
  nome: z
    .string()
    .min(2, 'O nome deve ter no mínimo 2 caracteres')
    .max(50, 'O nome deve ter no máximo 50 caracteres'),
  tipo: z.enum(['income', 'expense'], {
    message: 'Tipo deve ser receita ou despesa',
  }),
  cor: z.string().optional(),
  icone: z.string().optional(),
  usuarioId: z.string().optional(),
});

// Schema para atualizar categoria
export const updateCategorySchema = createCategorySchema.partial();

// Tipos inferidos
export type CreateCategoryInput = z.infer<typeof createCategorySchema>;
export type UpdateCategoryInput = z.infer<typeof updateCategorySchema>;
