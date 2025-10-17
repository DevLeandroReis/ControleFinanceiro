# 🔐 Sistema de Autenticação - Frontend

## Visão Geral

Sistema completo de autenticação implementado no frontend seguindo o padrão **Feature-Sliced Design (FSD)**, integrado com as APIs do backend.

## ✨ Funcionalidades Implementadas

### 1. **Login** (`/login`)
- Autenticação de usuários existentes
- Validação de formulário com Zod
- Feedback de erros em tempo real
- Redirecionamento automático após login
- Link para recuperação de senha
- Link para cadastro

### 2. **Cadastro** (`/register`)
- Registro de novos usuários
- Validação de campos (nome, email, senha, confirmação)
- Mensagem de sucesso com instrução para confirmar email
- Redirecionamento automático após 3 segundos
- Link para login

### 3. **Recuperação de Senha** (`/forgot-password`)
- Solicitação de link para redefinir senha
- Envio de email com instruções
- Feedback de sucesso/erro
- Link para voltar ao login

### 4. **Redefinição de Senha** (`/reset-password?token=xxx`)
- Redefinição de senha com token do email
- Validação de token
- Confirmação de nova senha
- Redirecionamento automático após sucesso
- Tratamento de token inválido/expirado

## 📁 Estrutura FSD

A implementação segue rigorosamente o Feature-Sliced Design:

```
frontend/src/
├── entities/user/              # Entidade de usuário
│   └── model/
│       ├── api.ts             # API calls (userApi)
│       ├── store.ts           # Zustand store (useUserStore)
│       ├── schemas.ts         # Zod schemas de validação
│       ├── types.ts           # TypeScript interfaces
│       └── index.ts           # Public exports
│
├── features/auth/              # Feature de autenticação
│   ├── ProtectedRoute.tsx     # Componente de proteção de rotas
│   └── index.ts               # Public exports
│
├── pages/                      # Páginas da aplicação
│   ├── login/
│   │   ├── LoginPage.tsx
│   │   ├── LoginPage.css
│   │   └── index.ts
│   ├── register/
│   │   ├── RegisterPage.tsx
│   │   ├── RegisterPage.css
│   │   └── index.ts
│   ├── forgot-password/
│   │   ├── ForgotPasswordPage.tsx
│   │   ├── ForgotPasswordPage.css
│   │   └── index.ts
│   └── reset-password/
│       ├── ResetPasswordPage.tsx
│       ├── ResetPasswordPage.css
│       └── index.ts
│
└── app/
    └── router.tsx              # Configuração de rotas
```

## 🔌 Endpoints do Backend

Todas as telas estão integradas com os seguintes endpoints:

| Endpoint | Método | Descrição |
|----------|--------|-----------|
| `/api/usuarios/login` | POST | Login de usuário |
| `/api/usuarios/registrar` | POST | Registro de novo usuário |
| `/api/usuarios/recuperar-senha` | POST | Solicitar recuperação de senha |
| `/api/usuarios/redefinir-senha` | POST | Redefinir senha com token |
| `/api/usuarios/confirmar-email` | GET | Confirmar email (query param) |
| `/api/usuarios/{id}` | GET | Obter dados do usuário |

## 🛡️ Proteção de Rotas

O componente `ProtectedRoute` protege rotas que requerem autenticação:

```tsx
// Uso no router.tsx
{
  path: 'dashboard',
  element: (
    <ProtectedRoute>
      <DashboardPage />
    </ProtectedRoute>
  ),
}
```

### Rotas Protegidas:
- `/dashboard` - Dashboard principal
- `/transacoes` - Gerenciamento de transações
- `/contas` - Gerenciamento de contas
- `/categorias` - Gerenciamento de categorias

### Rotas Públicas:
- `/` - Home page
- `/login` - Login
- `/register` - Cadastro
- `/forgot-password` - Recuperação de senha
- `/reset-password` - Redefinição de senha

## 📊 Gerenciamento de Estado

### Zustand Store (`useUserStore`)

```typescript
const {
  user,              // Dados do usuário atual
  token,             // Token JWT
  isAuthenticated,   // Status de autenticação
  isLoading,         // Estado de carregamento
  error,             // Mensagens de erro
  successMessage,    // Mensagens de sucesso
  
  // Actions
  login,             // Fazer login
  register,          // Registrar usuário
  logout,            // Fazer logout
  forgotPassword,    // Solicitar recuperação
  resetPassword,     // Redefinir senha
  fetchUserById,     // Buscar usuário por ID
} = useUserStore();
```

### Persistência

O estado do usuário é persistido no `localStorage` usando o middleware `persist` do Zustand:
- Token JWT armazenado em `auth_token`
- Estado do usuário em `user-storage`

## 🎨 Design e UX

### Características Visuais:
- 🎨 Gradiente moderno (roxo/violeta)
- 📱 Responsivo para mobile e desktop
- ♿ Acessível (labels, ARIA roles)
- 💅 Animações suaves de hover/focus
- ✅ Feedback visual imediato
- 🌐 Mensagens claras em português

### Validações:
- **Email**: Formato válido, máximo 255 caracteres
- **Senha**: Mínimo 6 caracteres, máximo 100
- **Nome**: Mínimo 3 caracteres, máximo 100
- **Confirmação**: Senhas devem coincidir

## 🔄 Fluxos de Usuário

### Fluxo de Cadastro:
1. Usuário acessa `/register`
2. Preenche formulário com validação
3. Submit → API cria usuário
4. Mensagem: "Verifique seu email para confirmar"
5. Redirecionamento para `/login` após 3s

### Fluxo de Login:
1. Usuário acessa `/login`
2. Insere credenciais
3. Submit → API valida e retorna token
4. Token salvo no localStorage
5. Redirecionamento para `/dashboard`

### Fluxo de Recuperação de Senha:
1. Usuário clica "Esqueceu sua senha?" no login
2. Acessa `/forgot-password`
3. Insere email
4. API envia email com token
5. Usuário clica no link do email
6. Acessa `/reset-password?token=xxx`
7. Define nova senha
8. Redirecionamento para `/login`

## 🔐 Segurança

### Implementações:
- ✅ Tokens JWT armazenados com segurança
- ✅ Interceptor Axios adiciona token automaticamente
- ✅ Redirecionamento em caso de 401 (não autorizado)
- ✅ Limpeza de dados ao fazer logout
- ✅ Validação de entrada em client e server
- ✅ Proteção de rotas sensíveis
- ✅ HTTPS recomendado em produção

## 🧪 Como Testar

### 1. Iniciar o Backend
```bash
cd backend/ControleFinanceiro.Presentation
dotnet run
```

### 2. Iniciar o Frontend
```bash
cd frontend
npm run dev
```

### 3. Testar Fluxos

#### Cadastro:
```
1. Acesse http://localhost:5173/register
2. Preencha os campos
3. Clique em "Criar conta"
4. Verifique o console do backend para o email de confirmação
```

#### Login:
```
1. Acesse http://localhost:5173/login
2. Use as credenciais cadastradas
3. Verifique redirecionamento para dashboard
```

#### Recuperação de Senha:
```
1. No login, clique em "Esqueceu sua senha?"
2. Insira email cadastrado
3. Verifique o console do backend para o link de redefinição
4. Acesse o link e defina nova senha
```

## 📝 DTOs e Tipos

### Backend → Frontend Mapping:

```typescript
// CreateUsuarioDto → RegisterInput
{
  nome: string;
  email: string;
  senha: string;
  confirmarSenha: string;
}

// LoginDto → LoginInput
{
  email: string;
  senha: string;
}

// AuthResultDto → AuthResult
{
  token: string;
  usuario: User;
  expiresAt: string;
}

// RecuperarSenhaDto → ForgotPasswordInput
{
  email: string;
}

// RedefinirSenhaDto → ResetPasswordInput
{
  token: string;
  novaSenha: string;
  confirmarNovaSenha: string;
}
```

## 🚀 Próximos Passos

### Melhorias Sugeridas:
1. [ ] Adicionar loading spinner visual
2. [ ] Implementar toast notifications
3. [ ] Adicionar tema escuro/claro
4. [ ] Implementar refresh token
5. [ ] Adicionar autenticação social (Google, GitHub)
6. [ ] Implementar 2FA (Two-Factor Authentication)
7. [ ] Adicionar página de perfil do usuário
8. [ ] Implementar mudança de senha autenticada

## 📚 Tecnologias Utilizadas

- **React 18** - Framework UI
- **TypeScript** - Tipagem estática
- **React Router 7** - Roteamento
- **Zustand** - Gerenciamento de estado
- **Zod** - Validação de schemas
- **React Hook Form** - Gerenciamento de formulários
- **Axios** - Cliente HTTP
- **CSS3** - Estilização

## 🎯 Conformidade com FSD

A implementação segue os princípios do Feature-Sliced Design:

1. ✅ **Entities**: Lógica de negócio do usuário isolada
2. ✅ **Features**: Funcionalidade de proteção de rotas
3. ✅ **Pages**: Componentes de página simples e focados
4. ✅ **App**: Configuração de rotas e providers
5. ✅ **Shared**: API client reutilizável

---

**Desenvolvido com ❤️ seguindo as melhores práticas de arquitetura frontend**
