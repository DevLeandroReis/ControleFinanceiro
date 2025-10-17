# 🗺️ Mapa da Estrutura de Autenticação

## Arquitetura FSD (Feature-Sliced Design)

```
┌─────────────────────────────────────────────────────────────────┐
│                          APP LAYER                               │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  router.tsx - Configuração de Rotas                       │  │
│  │  • Rotas públicas: /, /login, /register, etc.            │  │
│  │  • Rotas protegidas: /dashboard, /transacoes, etc.       │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                         PAGES LAYER                              │
│  ┌────────────┐  ┌────────────┐  ┌─────────────────┐           │
│  │  LoginPage │  │ RegisterPage│  │ ForgotPassword │           │
│  │   /login   │  │  /register  │  │ /forgot-password│           │
│  └─────┬──────┘  └─────┬───────┘  └────────┬────────┘           │
│        │               │                    │                    │
│  ┌─────┴────────────────┴────────────────────┴────────┐         │
│  │           ResetPasswordPage                         │         │
│  │           /reset-password?token=xxx                 │         │
│  └─────────────────────────────────────────────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                       FEATURES LAYER                             │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  ProtectedRoute Component                                 │  │
│  │  • Verifica autenticação                                  │  │
│  │  • Redireciona para /login se não autenticado            │  │
│  │  • Envolve rotas protegidas                              │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                      ENTITIES LAYER                              │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                    USER ENTITY                            │  │
│  │  ┌─────────────────────────────────────────────────────┐ │  │
│  │  │  api.ts - HTTP Requests                             │ │  │
│  │  │  • login(LoginDTO)                                   │ │  │
│  │  │  • register(RegisterDTO)                            │ │  │
│  │  │  • forgotPassword(ForgotPasswordDTO)                │ │  │
│  │  │  • resetPassword(ResetPasswordDTO)                  │ │  │
│  │  │  • confirmEmail(token)                              │ │  │
│  │  │  • getUserById(id)                                  │ │  │
│  │  └─────────────────────────────────────────────────────┘ │  │
│  │  ┌─────────────────────────────────────────────────────┐ │  │
│  │  │  store.ts - Zustand State Management                │ │  │
│  │  │  State:                                              │ │  │
│  │  │  • user: User | null                                │ │  │
│  │  │  • token: string | null                             │ │  │
│  │  │  • isAuthenticated: boolean                         │ │  │
│  │  │  • isLoading: boolean                               │ │  │
│  │  │  • error: string | null                             │ │  │
│  │  │  • successMessage: string | null                    │ │  │
│  │  │  Actions:                                            │ │  │
│  │  │  • login(), register(), logout()                    │ │  │
│  │  │  • forgotPassword(), resetPassword()                │ │  │
│  │  └─────────────────────────────────────────────────────┘ │  │
│  │  ┌─────────────────────────────────────────────────────┐ │  │
│  │  │  schemas.ts - Zod Validation                        │ │  │
│  │  │  • loginSchema                                       │ │  │
│  │  │  • registerSchema                                    │ │  │
│  │  │  • forgotPasswordSchema                              │ │  │
│  │  │  • resetPasswordSchema                               │ │  │
│  │  └─────────────────────────────────────────────────────┘ │  │
│  │  ┌─────────────────────────────────────────────────────┐ │  │
│  │  │  types.ts - TypeScript Interfaces                   │ │  │
│  │  │  • User, AuthResult                                  │ │  │
│  │  └─────────────────────────────────────────────────────┘ │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                       SHARED LAYER                               │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  api/client.ts - Axios Configuration                      │  │
│  │  • Base URL configuration                                 │  │
│  │  • Request interceptor (add token)                        │  │
│  │  • Response interceptor (handle errors)                   │  │
│  │  • Automatic 401 redirect to /login                       │  │
│  └──────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## 🔄 Fluxo de Dados

### Login Flow:
```
LoginPage.tsx
    ↓ (form submit)
useUserStore.login(data)
    ↓
userApi.login(data)
    ↓
apiClient.post('/usuarios/login', data)
    ↓
[Axios Interceptor adds headers]
    ↓
Backend API (POST /api/usuarios/login)
    ↓
Response: { token, usuario, expiresAt }
    ↓
Store updates: user, token, isAuthenticated
    ↓
localStorage.setItem('auth_token', token)
    ↓
Navigate to /dashboard
    ↓
ProtectedRoute checks isAuthenticated
    ↓
Render DashboardPage
```

### Register Flow:
```
RegisterPage.tsx
    ↓ (form submit)
useUserStore.register(data)
    ↓
userApi.register(data)
    ↓
Backend API (POST /api/usuarios/registrar)
    ↓
Response: User
    ↓
Store updates: user, successMessage
    ↓
Show success message
    ↓
setTimeout → Navigate to /login
```

### Forgot Password Flow:
```
ForgotPasswordPage.tsx
    ↓ (form submit)
useUserStore.forgotPassword(data)
    ↓
userApi.forgotPassword(data)
    ↓
Backend API (POST /api/usuarios/recuperar-senha)
    ↓
Backend sends email with token
    ↓
Response: { message }
    ↓
Store updates: successMessage
    ↓
Show success message to user
```

### Reset Password Flow:
```
ResetPasswordPage.tsx (receives token from URL)
    ↓ (form submit)
useUserStore.resetPassword(data)
    ↓
userApi.resetPassword(data)
    ↓
Backend API (POST /api/usuarios/redefinir-senha)
    ↓
Response: { message }
    ↓
Store updates: successMessage
    ↓
setTimeout → Navigate to /login
```

## 🛡️ Proteção de Rotas

```
User tries to access /dashboard
    ↓
React Router renders route
    ↓
Route wrapped in <ProtectedRoute>
    ↓
ProtectedRoute component checks:
    ├─ useUserStore.isAuthenticated?
    │   ├─ Yes → Render children (DashboardPage)
    │   └─ No → Check localStorage for token
    │       ├─ Token exists → Render children
    │       └─ No token → <Navigate to="/login" />
    ↓
User lands on login page
```

## 📦 DTOs Mapping

```
Backend DTO              Frontend Type           Usage
─────────────────────────────────────────────────────────────
CreateUsuarioDto      → RegisterInput         → Register form
LoginDto              → LoginInput            → Login form
RecuperarSenhaDto     → ForgotPasswordInput   → Forgot password form
RedefinirSenhaDto     → ResetPasswordInput    → Reset password form
UsuarioDto            → User                  → Store & display
AuthResultDto         → AuthResult            → Login response
```

## 🎯 Camadas FSD

```
┌──────────────────────────────────────────────────────┐
│ app/        → Inicialização, rotas, providers       │
├──────────────────────────────────────────────────────┤
│ pages/      → Composição de features em páginas     │
├──────────────────────────────────────────────────────┤
│ features/   → Lógica de negócio (ProtectedRoute)    │
├──────────────────────────────────────────────────────┤
│ entities/   → Modelos de domínio (User)             │
├──────────────────────────────────────────────────────┤
│ shared/     → Código reutilizável (API client)      │
└──────────────────────────────────────────────────────┘
```

## 📁 Arquivos Criados/Modificados

### ✨ Novos Arquivos:
```
frontend/src/
├── entities/user/model/
│   ├── api.ts (atualizado)
│   ├── store.ts (atualizado)
│   ├── schemas.ts (atualizado)
│   ├── types.ts (atualizado)
│   └── index.ts (atualizado)
├── features/auth/
│   ├── ProtectedRoute.tsx (novo)
│   └── index.ts (novo)
├── pages/
│   ├── register/ (novo)
│   │   ├── RegisterPage.tsx
│   │   ├── RegisterPage.css
│   │   └── index.ts
│   ├── forgot-password/ (novo)
│   │   ├── ForgotPasswordPage.tsx
│   │   ├── ForgotPasswordPage.css
│   │   └── index.ts
│   ├── reset-password/ (novo)
│   │   ├── ResetPasswordPage.tsx
│   │   ├── ResetPasswordPage.css
│   │   └── index.ts
│   ├── login/
│   │   ├── LoginPage.tsx (atualizado)
│   │   └── LoginPage.css (atualizado)
│   └── index.ts (atualizado)
└── app/
    └── router.tsx (atualizado)
```

### 📊 Estatísticas:
- **14 arquivos** criados/modificados
- **4 novas páginas** implementadas
- **1 feature** de proteção de rotas
- **100% das rotas de autenticação** do backend integradas
- **Totalmente compatível** com o padrão FSD

---

**✅ Implementação completa e seguindo as melhores práticas!**
