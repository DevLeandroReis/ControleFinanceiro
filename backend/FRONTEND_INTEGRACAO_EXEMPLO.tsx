// Exemplo de implementação no frontend para integração com a nova API

// 1. Adicionar método na API (frontend\src\entities\user\model\api.ts)
export const userApi = {
  // ... outros métodos existentes ...

  /**
   * Reenviar Email de Confirmação - POST /api/Usuarios/reenviar-email-confirmacao
   * @param data ReenviarEmailConfirmacaoDto
   * @returns Success message
   */
  async resendConfirmationEmail(email: string): Promise<{ message: string }> {
    const response = await apiClient.post<{ message: string }>(
      '/api/Usuarios/reenviar-email-confirmacao',
      { email }
    );
    return response.data;
  },
};

// 2. Adicionar ação no store (frontend\src\entities\user\model\store.ts)
interface UserState {
  // ... propriedades existentes ...
  resendConfirmationEmail: (email: string) => Promise<void>;
}

export const useUserStore = create<UserState>()(
  devtools(
    persist(
      (set) => ({
        // ... estado existente ...

        resendConfirmationEmail: async (email: string) => {
          set({ isLoading: true, error: null, successMessage: null });
          try {
            const response = await userApi.resendConfirmationEmail(email);
            set({
              isLoading: false,
              error: null,
              successMessage: response.message,
            });
          } catch (error) {
            const errorMessage = getErrorMessage(error);
            set({ error: errorMessage, isLoading: false });
            throw error;
          }
        },
      }),
      // ... configuração do persist ...
    ),
    { name: 'UserStore' }
  )
);

// 3. Componente para verificar email não confirmado
// frontend\src\components\EmailVerificationBanner.tsx
import { FC } from 'react';
import { useUserStore } from '@/entities/user';
import { Mail, Loader2 } from 'lucide-react';

interface EmailVerificationBannerProps {
  userEmail: string;
  emailConfirmed: boolean;
}

export const EmailVerificationBanner: FC<EmailVerificationBannerProps> = ({
  userEmail,
  emailConfirmed,
}) => {
  const { resendConfirmationEmail, isLoading, successMessage, error } = useUserStore();

  if (emailConfirmed) {
    return null;
  }

  const handleResendEmail = async () => {
    try {
      await resendConfirmationEmail(userEmail);
    } catch (error) {
      console.error('Erro ao reenviar email:', error);
    }
  };

  return (
    <div className="email-verification-banner">
      <div className="banner-content">
        <Mail size={24} />
        <div className="banner-text">
          <strong>Verifique seu email</strong>
          <p>
            Enviamos um email de confirmação para <strong>{userEmail}</strong>.
            Por favor, verifique sua caixa de entrada.
          </p>
        </div>
        <button
          onClick={handleResendEmail}
          disabled={isLoading}
          className="btn btn--secondary"
        >
          {isLoading ? (
            <>
              <Loader2 size={16} className="spinning" />
              Enviando...
            </>
          ) : (
            'Reenviar email'
          )}
        </button>
      </div>

      {successMessage && (
        <div className="banner-success">{successMessage}</div>
      )}

      {error && <div className="banner-error">{error}</div>}

      <style jsx>{`
        .email-verification-banner {
          background-color: #fff3cd;
          border: 1px solid #ffc107;
          border-radius: 8px;
          padding: 16px;
          margin: 16px 0;
        }

        .banner-content {
          display: flex;
          align-items: center;
          gap: 16px;
        }

        .banner-text {
          flex: 1;
        }

        .banner-text strong {
          color: #856404;
        }

        .banner-text p {
          margin: 4px 0 0 0;
          color: #856404;
        }

        .banner-success {
          margin-top: 12px;
          padding: 8px;
          background-color: #d4edda;
          border: 1px solid #c3e6cb;
          border-radius: 4px;
          color: #155724;
        }

        .banner-error {
          margin-top: 12px;
          padding: 8px;
          background-color: #f8d7da;
          border: 1px solid #f5c6cb;
          border-radius: 4px;
          color: #721c24;
        }

        .spinning {
          animation: spin 1s linear infinite;
        }

        @keyframes spin {
          from {
            transform: rotate(0deg);
          }
          to {
            transform: rotate(360deg);
          }
        }
      `}</style>
    </div>
  );
};

// 4. Atualizar tipo do usuário para incluir emailConfirmado
// frontend\src\entities\user\model\types.ts
export interface User {
  id: string;
  nome: string;
  email: string;
  emailConfirmado: boolean; // Adicionar esta propriedade
  ativo: boolean;
  criadoEm: string;
  atualizadoEm: string;
}

// 5. Exemplo de uso no Dashboard ou Home
// frontend\src\pages\dashboard\DashboardPage.tsx
import { FC } from 'react';
import { useUserStore } from '@/entities/user';
import { EmailVerificationBanner } from '@/components/EmailVerificationBanner';

export const DashboardPage: FC = () => {
  const { user } = useUserStore();

  if (!user) {
    return <div>Carregando...</div>;
  }

  return (
    <div className="dashboard">
      <EmailVerificationBanner
        userEmail={user.email}
        emailConfirmed={user.emailConfirmado}
      />

      {/* Resto do conteúdo do dashboard */}
      <h1>Dashboard</h1>
      <p>Bem-vindo, {user.nome}!</p>
    </div>
  );
};

// 6. Atualizar a página de registro para mostrar mensagem apropriada
// frontend\src\pages\register\RegisterPage.tsx
const onSubmit = async (data: RegisterInput) => {
  try {
    await registerUser(data);
    // Mostrar mensagem de sucesso diferente
    setSuccessMessage(
      'Cadastro realizado com sucesso! ' +
      'Enviamos um email de confirmação. ' +
      'Você será redirecionado para o login.'
    );
    
    // Redirecionar após 3 segundos
    setTimeout(() => {
      navigate('/login');
    }, 3000);
  } catch (error) {
    console.error('Erro ao registrar:', error);
    // Erro já está no estado do store
  }
};

// 7. Opcional: Adicionar verificação no login
// Se o backend retornar informação de email não confirmado
const onLogin = async (data: LoginInput) => {
  try {
    await login(data);
    
    // Se o usuário fez login mas o email não está confirmado
    if (user && !user.emailConfirmado) {
      // Mostrar banner de aviso
      setWarning('Seu email ainda não foi confirmado. Verifique sua caixa de entrada.');
    }
    
    navigate('/dashboard');
  } catch (error) {
    console.error('Erro ao fazer login:', error);
  }
};
