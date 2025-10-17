import type { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useUserStore, registerSchema, type RegisterInput } from '../../entities/user';
import './RegisterPage.css';

export const RegisterPage: FC = () => {
  const navigate = useNavigate();
  const { register: registerUser, isLoading, error, successMessage } = useUserStore();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterInput>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      nome: '',
      email: '',
      senha: '',
      confirmarSenha: '',
    },
  });

  const onSubmit = async (data: RegisterInput) => {
    try {
      await registerUser(data);
      // Redirect to login after successful registration
      setTimeout(() => {
        navigate('/login');
      }, 3000);
    } catch (error) {
      console.error('Erro ao registrar:', error);
    }
  };

  return (
    <div className="register-page">
      <div className="register-container">
        <div className="register-header">
          <h1>💰 Controle Financeiro</h1>
          <p>Crie sua conta gratuitamente</p>
        </div>

        <form className="register-form" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label htmlFor="nome">Nome completo</label>
            <input
              id="nome"
              type="text"
              {...register('nome')}
              placeholder="Seu nome completo"
              disabled={isSubmitting}
              autoComplete="name"
            />
            {errors.nome && (
              <span className="error-message">{errors.nome.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              {...register('email')}
              placeholder="seu@email.com"
              disabled={isSubmitting}
              autoComplete="email"
            />
            {errors.email && (
              <span className="error-message">{errors.email.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="senha">Senha</label>
            <input
              id="senha"
              type="password"
              {...register('senha')}
              placeholder="Mínimo 6 caracteres"
              disabled={isSubmitting}
              autoComplete="new-password"
            />
            {errors.senha && (
              <span className="error-message">{errors.senha.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="confirmarSenha">Confirmar senha</label>
            <input
              id="confirmarSenha"
              type="password"
              {...register('confirmarSenha')}
              placeholder="Repita sua senha"
              disabled={isSubmitting}
              autoComplete="new-password"
            />
            {errors.confirmarSenha && (
              <span className="error-message">{errors.confirmarSenha.message}</span>
            )}
          </div>

          {error && (
            <div className="form-error" role="alert">
              {error}
            </div>
          )}

          {successMessage && (
            <div className="form-success" role="status">
              {successMessage}
            </div>
          )}

          <button 
            type="submit" 
            className="btn btn--primary btn--full" 
            disabled={isSubmitting || isLoading}
          >
            {isSubmitting || isLoading ? '⏳ Cadastrando...' : '✨ Criar conta'}
          </button>
        </form>

        <div className="register-footer">
          <p>Já tem uma conta?</p>
          <button className="btn btn--link" onClick={() => navigate('/login')}>
            Fazer login
          </button>
        </div>
      </div>
    </div>
  );
};
