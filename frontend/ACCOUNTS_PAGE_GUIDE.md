# Tela de Gerenciamento de Contas - Guia de Uso

## 📋 Visão Geral

A tela de contas foi completamente redesenhada com um design moderno e intuitivo para gerenciar contas compartilhadas entre usuários.

## 🎨 Estrutura Visual

### Header
- **Título e Subtítulo**: Indicam claramente a funcionalidade da página
- **Botões de Ação**:
  - `Solicitar Acesso`: Abre modal para solicitar acesso a uma conta usando ID
  - `Nova Conta`: Alterna formulário de criação de conta

### Sistema de Tabs
1. **Minhas Contas**: Lista todas as contas que você possui ou tem acesso
2. **Recebidas**: Solicitações de acesso às suas contas (com badge de alerta para pendentes)
3. **Enviadas**: Suas solicitações para acessar outras contas

## 🔑 Funcionalidades

### Criação/Edição de Conta
- Formulário simples com nome e descrição
- Validação em tempo real
- Feedback visual de sucesso/erro

### Card de Conta
- **Informações Exibidas**:
  - Nome e descrição
  - Proprietário
  - Número de usuários
  - Data de criação
  - Status (Ativa/Inativa)
  
- **ID Compartilhável**:
  - Botão para mostrar/ocultar ID
  - Botão de copiar com feedback visual
  - Instrução de como compartilhar

- **Ações Disponíveis**:
  - Gerenciar Usuários (todos)
  - Editar (apenas proprietário)
  - Ativar/Desativar (apenas proprietário)
  - Excluir (apenas proprietário)

### Solicitações de Acesso

#### Recebidas (como proprietário)
- Visualiza quem está solicitando acesso
- Email e nome do solicitante
- Mensagem opcional do solicitante
- Ações: Aprovar ou Rejeitar

#### Enviadas (como solicitante)
- Visualiza suas solicitações
- Status: Pendente, Aprovada, Rejeitada, Cancelada
- Nome do proprietário
- Ação: Cancelar (apenas pendentes)

### Gerenciamento de Usuários
Modal completo com:
- Lista de todos os membros
- Avatar personalizado (inicial do nome)
- Badges: Proprietário, Admin
- Informações: Nome, Email, Data de adesão
- Ações (apenas proprietário):
  - Tornar Admin / Remover Admin
  - Remover usuário da conta

## 🎨 Design System

### Cores
- **Primary**: `#646cff` (Roxo/Azul)
- **Success**: `#28a745` (Verde)
- **Danger**: `#dc3545` (Vermelho)
- **Warning**: `#ffc107` (Amarelo)
- **Muted**: `#6c757d` (Cinza)

### Componentes
- Botões com hover effects e sombras
- Cards com gradientes sutis
- Modais com backdrop blur
- Animações suaves (fadeIn, slideDown, slideUp)
- Loading states com spinner

### Responsividade
- **Desktop** (>1024px): Grid de 3 colunas
- **Tablet** (768px-1024px): Grid de 2 colunas
- **Mobile** (<768px): Grid de 1 coluna, layout vertical

## ⚡ Feedback Visual

### Estados
- **Loading**: Spinner animado
- **Success**: Ícone verde com check
- **Error**: Mensagem vermelha com ícone de alerta
- **Empty**: Ilustração com sugestão de ação

### Interações
- Hover: Elevação e mudança de cor
- Active: Badge pulsante para notificações
- Disabled: Opacidade reduzida
- Focus: Borda colorida com sombra

## 🔐 Permissões

### Proprietário
- Criar/editar/excluir conta
- Ativar/desativar conta
- Aprovar/rejeitar solicitações
- Gerenciar todos os usuários
- Conceder/remover permissões de admin

### Admin (podeAdicionarUsuarios)
- Visualizar todos os dados
- Gerenciar usuários (limitado)

### Membro Regular
- Visualizar conta
- Usar conta para lançamentos
- Ver outros membros

## 📱 UX Highlights

1. **Feedback Imediato**: Todas as ações têm feedback visual instantâneo
2. **Confirmações**: Ações destrutivas requerem confirmação
3. **Estados Vazios**: Sugestões claras de próximos passos
4. **Badges de Notificação**: Alertas visuais para ações pendentes
5. **Tooltips**: Dicas em todos os botões de ação
6. **Animações Sutis**: Transições suaves sem distrair
7. **Mobile-First**: Design pensado primeiro para mobile

## 🐛 Troubleshooting

### CSS não carrega
- Verificar se `AccountsPage.css` está no mesmo diretório
- Limpar cache do navegador
- Recarregar dev server

### Componentes não aparecem
- Verificar imports corretos
- Confirmar que todos os arquivos CSS dos features foram criados
- Verificar console para erros

### Layout quebrado
- Verificar se não há CSS conflitante
- Confirmar que variáveis CSS globais estão definidas
- Testar em diferentes resoluções

## 🎯 Próximos Passos Sugeridos

1. Adicionar filtros e busca
2. Implementar paginação para muitas contas
3. Adicionar gráficos de atividade
4. Notificações push para novas solicitações
5. Histórico de alterações
6. Exportar dados da conta

---

**Desenvolvido com**: React + TypeScript + Zustand + Lucide Icons
**Design**: Mobile-First, Responsive, Modern UI
