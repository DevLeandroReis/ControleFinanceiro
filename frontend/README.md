# 💰 Controle Financeiro - Frontend

Frontend do sistema de Controle Financeiro desenvolvido com **React**, **TypeScript**, **Vite** e arquitetura **Feature-Sliced Design (FSD)**.

## 🚀 Início Rápido

```bash
# Instalar dependências
npm install

# Iniciar servidor de desenvolvimento
npm run dev

# Build para produção
npm run build

# Preview do build
npm run preview
```

## 🏗️ Arquitetura

Este projeto utiliza **Feature-Sliced Design (FSD)** para organização do código em camadas hierárquicas:

```
src/
├── app/         # Inicialização da aplicação
├── processes/   # Processos globais
├── pages/       # Páginas/rotas
├── widgets/     # Componentes complexos
├── features/    # Features de negócio
├── entities/    # Entidades de negócio
└── shared/      # Código compartilhado
```

📖 **Para mais detalhes:**
- `FSD_STRUCTURE.md` - Documentação completa da arquitetura FSD
- `PROJECT_SETUP.md` - Guia de configuração e próximos passos

## 🛠️ Tecnologias

- **React** 18.3.1 - Biblioteca UI
- **TypeScript** 5.7.2 - Tipagem estática
- **Vite** 7.1.10 - Build tool e dev server
- **React Router** 7.1.3 - Roteamento e navegação
- **Zustand** 5.0.2 - Gerenciamento de estado
- **Feature-Sliced Design** - Arquitetura de projeto

## 📦 Path Aliases

O projeto utiliza path aliases para imports mais limpos:

```typescript
import { Button } from '@shared/ui';
import { HomePage } from '@pages/home';
import { User } from '@entities/user';
```

Aliases disponíveis:
- `@/` → `src/`
- `@app/` → `src/app/`
- `@processes/` → `src/processes/`
- `@pages/` → `src/pages/`
- `@widgets/` → `src/widgets/`
- `@features/` → `src/features/`
- `@entities/` → `src/entities/`
- `@shared/` → `src/shared/`

## 📋 Scripts NPM

```bash
npm run dev      # Inicia o servidor de desenvolvimento
npm run build    # Compila para produção
npm run preview  # Preview do build de produção
npm run lint     # Executa o linter
```

## 🌐 Variáveis de Ambiente

Copie o arquivo `.env.example` para `.env` e configure as variáveis:

```bash
VITE_API_BASE_URL=http://localhost:5000
VITE_APP_NAME=Controle Financeiro
VITE_APP_VERSION=0.0.1
```

## 📚 Documentação

- [FSD_STRUCTURE.md](./FSD_STRUCTURE.md) - Arquitetura Feature-Sliced Design
- [PROJECT_SETUP.md](./PROJECT_SETUP.md) - Setup completo do projeto
- [REACT_ROUTER.md](./REACT_ROUTER.md) - Documentação do React Router
- [ZUSTAND.md](./ZUSTAND.md) - Gerenciamento de estado com Zustand
- [FSD_EXAMPLES.md](./FSD_EXAMPLES.md) - Exemplos práticos de implementação
- [Vite Docs](https://vitejs.dev/)
- [React Docs](https://react.dev/)
- [Feature-Sliced Design](https://feature-sliced.design/)

## 🧭 Navegação

O projeto possui as seguintes rotas configuradas:

- `/` - Página inicial
- `/dashboard` - Dashboard com visão geral
- `/transacoes` - Gerenciamento de lançamentos
- `/contas` - Gerenciamento de contas
- `/categorias` - Gerenciamento de categorias

Consulte `REACT_ROUTER.md` para mais detalhes sobre roteamento.

## 🎯 Próximos Passos

Consulte `PROJECT_SETUP.md` para sugestões de bibliotecas e configurações adicionais:
- ✅ ~~React Router para roteamento~~ (Implementado!)
- ✅ ~~Zustand para gerenciamento de estado~~ (Implementado!)
- Axios para requisições HTTP
- React Hook Form para formulários
- Material UI ou Chakra UI para componentes

## 📄 Licença

Este projeto é parte do sistema Controle Financeiro.
    languageOptions: {
      parserOptions: {
        project: ['./tsconfig.node.json', './tsconfig.app.json'],
        tsconfigRootDir: import.meta.dirname,
      },
      // other options...
    },
  },
])
```
