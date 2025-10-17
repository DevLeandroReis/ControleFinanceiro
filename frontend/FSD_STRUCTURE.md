# Controle Financeiro - Frontend

Frontend desenvolvido com **Vite**, **React** e **TypeScript**, seguindo a arquitetura **Feature-Sliced Design (FSD)**.

## 🏗️ Arquitetura Feature-Sliced Design

Este projeto utiliza a metodologia FSD que organiza o código em camadas hierárquicas:

### 📂 Estrutura de Camadas

```
src/
├── app/                    # 🎯 Camada de Aplicação
│   ├── providers/          # Provedores globais (Router, Theme, Store)
│   ├── styles/             # Estilos globais
│   └── config/             # Configurações da aplicação
│
├── processes/              # 🔄 Processos
│   └── ...                 # Processos que afetam múltiplas features
│
├── pages/                  # 📄 Páginas
│   └── ...                 # Páginas/rotas da aplicação
│
├── widgets/                # 🧩 Widgets
│   └── ...                 # Componentes complexos que combinam features
│
├── features/               # ⚡ Features
│   └── ...                 # Funcionalidades de negócio (add-transaction, edit-category)
│
├── entities/               # 📦 Entidades
│   └── ...                 # Entidades de negócio (user, transaction, account, category)
│
└── shared/                 # 🔧 Compartilhado
    ├── ui/                 # Componentes UI reutilizáveis
    ├── lib/                # Utilitários e helpers
    ├── api/                # Cliente API e interceptors
    ├── config/             # Constantes e configurações
    └── types/              # Types TypeScript compartilhados
```

### 🎯 Descrição das Camadas

#### 1. **App** (Inicialização)
- Ponto de entrada da aplicação
- Provedores globais (Router, State Management, Theme)
- Estilos globais
- Configurações de inicialização

#### 2. **Processes** (Processos)
- Fluxos que afetam múltiplas features
- Exemplo: autenticação, onboarding

#### 3. **Pages** (Páginas)
- Componentes de página/rota
- Composição de widgets e features
- Exemplo: HomePage, DashboardPage, TransactionsPage

#### 4. **Widgets** (Widgets)
- Componentes complexos e independentes
- Combinam múltiplas features e entities
- Exemplo: UserProfile, TransactionsList, CategoryChart

#### 5. **Features** (Funcionalidades)
- Ações e interações do usuário
- Lógica de negócio específica
- Exemplo: add-transaction, edit-category, filter-accounts

#### 6. **Entities** (Entidades)
- Modelos de negócio
- Estado e lógica das entidades
- Exemplo: user, transaction, account, category

#### 7. **Shared** (Compartilhado)
- Código reutilizável em toda aplicação
- UI components (Button, Input, Card)
- Utilitários, helpers, hooks
- API client
- Types compartilhados

## 🛠️ Tecnologias

- **Vite** - Build tool e dev server
- **React 18** - Biblioteca UI
- **TypeScript** - Tipagem estática
- **FSD** - Arquitetura de projeto

## 🚀 Scripts Disponíveis

```bash
# Instalar dependências
npm install

# Iniciar servidor de desenvolvimento
npm run dev

# Build para produção
npm run build

# Preview do build de produção
npm run preview

# Linting
npm run lint
```

## 📝 Path Aliases

O projeto está configurado com path aliases para facilitar imports:

```typescript
import { Button } from '@shared/ui';
import { UserProfile } from '@widgets/user-profile';
import { addTransaction } from '@features/add-transaction';
import { Transaction } from '@entities/transaction';
```

Aliases disponíveis:
- `@/` - src/
- `@app/` - src/app/
- `@processes/` - src/processes/
- `@pages/` - src/pages/
- `@widgets/` - src/widgets/
- `@features/` - src/features/
- `@entities/` - src/entities/
- `@shared/` - src/shared/

## 📋 Regras de Importação FSD

1. **Camadas superiores podem importar de camadas inferiores**
   - ✅ Pages → Widgets
   - ✅ Widgets → Features
   - ✅ Features → Entities
   - ✅ Qualquer camada → Shared

2. **Camadas inferiores NÃO podem importar de camadas superiores**
   - ❌ Entities → Features
   - ❌ Features → Widgets
   - ❌ Widgets → Pages

3. **Camadas do mesmo nível não devem ter dependências diretas**
   - ❌ Feature A → Feature B
   - ✅ Use composição em uma camada superior

## 🔗 Links Úteis

- [Feature-Sliced Design](https://feature-sliced.design/)
- [Vite Documentation](https://vitejs.dev/)
- [React Documentation](https://react.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/)
