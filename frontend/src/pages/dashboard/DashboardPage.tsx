import type { FC } from 'react';
import { 
  BarChart3, 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  Wallet,
  PiggyBank,
  Calendar,
  AlertCircle,
  CheckCircle,
  Clock
} from 'lucide-react';
import {
  AreaChart,
  Area,
  BarChart,
  Bar,
  PieChart,
  Pie,
  Cell,
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer
} from 'recharts';
import './DashboardPage.css';

const mockData = {
  summary: {
    totalReceitas: 15750.00,
    totalDespesas: 8420.50,
    saldo: 7329.50,
    receitasPendentes: 1250.00,
    despesasPendentes: 1850.00,
    economiaMedia: 46.5,
    variacaoReceitas: 12.5,
    variacaoDespesas: 3.8,
  },
  evolucaoMensal: [
    { mes: 'Mai', receitas: 14200, despesas: 7800, economia: 45.1 },
    { mes: 'Jun', receitas: 13800, despesas: 8200, economia: 40.6 },
    { mes: 'Jul', receitas: 15000, despesas: 7500, economia: 50.0 },
    { mes: 'Ago', receitas: 14500, despesas: 8900, economia: 38.6 },
    { mes: 'Set', receitas: 16200, despesas: 8100, economia: 50.0 },
    { mes: 'Out', receitas: 15750, despesas: 8420.50, economia: 46.5 },
  ],
  receitasPorCategoria: [
    { categoria: 'Salário', valor: 12000.00, percentual: 76.2 },
    { categoria: 'Freelance', valor: 2500.00, percentual: 15.9 },
    { categoria: 'Investimentos', valor: 1250.00, percentual: 7.9 },
  ],
  despesasPorCategoria: [
    { categoria: 'Moradia', valor: 3200.00, percentual: 38.0 },
    { categoria: 'Alimentação', valor: 1800.00, percentual: 21.4 },
    { categoria: 'Lazer', valor: 1200.00, percentual: 14.3 },
    { categoria: 'Transporte', valor: 850.00, percentual: 10.1 },
    { categoria: 'Educação', valor: 700.00, percentual: 8.3 },
    { categoria: 'Saúde', valor: 670.50, percentual: 8.0 },
  ],
  statusLancamentos: [
    { status: 'Pagos', quantidade: 18 },
    { status: 'Pendentes', quantidade: 7 },
    { status: 'Vencidos', quantidade: 2 },
  ],
  contas: [
    { nome: 'Banco Inter', saldo: 4200.00, tipo: 'Corrente', cor: '#f97316' },
    { nome: 'Nubank', saldo: 2129.50, tipo: 'Corrente', cor: '#8b5cf6' },
    { nome: 'Poupança', saldo: 1000.00, tipo: 'Poupança', cor: '#10b981' },
  ],
  transacoesRecentes: [
    { id: 1, descricao: 'Salário Outubro', tipo: 'Receita', valor: 12000.00, data: '2025-10-05', categoria: 'Salário', status: 'Pago' },
    { id: 2, descricao: 'Aluguel', tipo: 'Despesa', valor: 2500.00, data: '2025-10-10', categoria: 'Moradia', status: 'Pago' },
    { id: 3, descricao: 'Freelance - Site WordPress', tipo: 'Receita', valor: 2500.00, data: '2025-10-12', categoria: 'Freelance', status: 'Pago' },
    { id: 4, descricao: 'Supermercado', tipo: 'Despesa', valor: 680.00, data: '2025-10-14', categoria: 'Alimentação', status: 'Pago' },
    { id: 5, descricao: 'Condomínio', tipo: 'Despesa', valor: 450.00, data: '2025-10-20', categoria: 'Moradia', status: 'Pendente' },
    { id: 6, descricao: 'Conta de Luz', tipo: 'Despesa', valor: 180.00, data: '2025-10-22', categoria: 'Moradia', status: 'Pendente' },
  ],
};

export const DashboardPage: FC = () => {
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short' });
  };

  const COLORS_RECEITAS = ['#10b981', '#14b8a6', '#06b6d4'];
  const COLORS_DESPESAS = ['#ef4444', '#f97316', '#f59e0b', '#eab308', '#84cc16', '#22c55e'];
  const COLORS_STATUS = ['#10b981', '#f59e0b', '#ef4444'];

  return (
    <div className="dashboard-page">
      <header className="dashboard-header">
        <div>
          <h1><BarChart3 size={32} className="header-icon" />Dashboard Financeiro</h1>
          <p className="subtitle">Visão geral completa das suas finanças</p>
        </div>
        <div className="dashboard-period">
          <Calendar size={18} />
          <span>Outubro 2025</span>
        </div>
      </header>

      <div className="kpi-grid">
        <div className="kpi-card kpi-card--income">
          <div className="kpi-header">
            <div className="kpi-icon"><TrendingUp size={24} /></div>
            <div className="kpi-trend kpi-trend--positive">+{mockData.summary.variacaoReceitas}%</div>
          </div>
          <div className="kpi-body">
            <h3 className="kpi-label">Receitas do Mês</h3>
            <p className="kpi-value">{formatCurrency(mockData.summary.totalReceitas)}</p>
            <span className="kpi-info">{formatCurrency(mockData.summary.receitasPendentes)} pendentes</span>
          </div>
        </div>

        <div className="kpi-card kpi-card--expense">
          <div className="kpi-header">
            <div className="kpi-icon"><TrendingDown size={24} /></div>
            <div className="kpi-trend kpi-trend--negative">+{mockData.summary.variacaoDespesas}%</div>
          </div>
          <div className="kpi-body">
            <h3 className="kpi-label">Despesas do Mês</h3>
            <p className="kpi-value">{formatCurrency(mockData.summary.totalDespesas)}</p>
            <span className="kpi-info">{formatCurrency(mockData.summary.despesasPendentes)} pendentes</span>
          </div>
        </div>

        <div className="kpi-card kpi-card--balance">
          <div className="kpi-header">
            <div className="kpi-icon"><DollarSign size={24} /></div>
            <div className="kpi-trend kpi-trend--positive">{mockData.summary.economiaMedia.toFixed(1)}%</div>
          </div>
          <div className="kpi-body">
            <h3 className="kpi-label">Saldo do Mês</h3>
            <p className="kpi-value">{formatCurrency(mockData.summary.saldo)}</p>
            <span className="kpi-info">Taxa de economia</span>
          </div>
        </div>

        <div className="kpi-card kpi-card--savings">
          <div className="kpi-header">
            <div className="kpi-icon"><PiggyBank size={24} /></div>
            <div className="kpi-trend kpi-trend--neutral">{mockData.contas.length}x</div>
          </div>
          <div className="kpi-body">
            <h3 className="kpi-label">Total em Contas</h3>
            <p className="kpi-value">{formatCurrency(mockData.contas.reduce((acc, conta) => acc + conta.saldo, 0))}</p>
            <span className="kpi-info">{mockData.contas.length} contas ativas</span>
          </div>
        </div>
      </div>

      <div className="charts-grid">
        <div className="chart-card chart-card--full">
          <div className="chart-header">
            <h2><BarChart3 size={22} />Evolução Mensal de Receitas e Despesas</h2>
            <span className="chart-subtitle">Últimos 6 meses</span>
          </div>
          <div className="chart-container">
            <ResponsiveContainer width="100%" height={300}>
              <AreaChart data={mockData.evolucaoMensal}>
                <defs>
                  <linearGradient id="colorReceitas" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#10b981" stopOpacity={0.8}/>
                    <stop offset="95%" stopColor="#10b981" stopOpacity={0.1}/>
                  </linearGradient>
                  <linearGradient id="colorDespesas" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#ef4444" stopOpacity={0.8}/>
                    <stop offset="95%" stopColor="#ef4444" stopOpacity={0.1}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                <XAxis dataKey="mes" stroke="#6b7280" />
                <YAxis stroke="#6b7280" />
                <Tooltip contentStyle={{ backgroundColor: '#fff', border: '1px solid #e5e7eb', borderRadius: '8px', padding: '12px' }} formatter={(value: number) => formatCurrency(value)} />
                <Legend />
                <Area type="monotone" dataKey="receitas" stroke="#10b981" fillOpacity={1} fill="url(#colorReceitas)" name="Receitas" strokeWidth={2} />
                <Area type="monotone" dataKey="despesas" stroke="#ef4444" fillOpacity={1} fill="url(#colorDespesas)" name="Despesas" strokeWidth={2} />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="chart-card">
          <div className="chart-header">
            <h2><TrendingUp size={22} />Receitas por Categoria</h2>
            <span className="chart-subtitle">Distribuição mensal</span>
          </div>
          <div className="chart-container">
            <ResponsiveContainer width="100%" height={280}>
              <PieChart>
                <Pie 
                  data={mockData.receitasPorCategoria} 
                  cx="50%" 
                  cy="50%" 
                  labelLine={false} 
                  // eslint-disable-next-line @typescript-eslint/no-explicit-any
                  label={(entry: any) => `${entry.categoria} (${entry.percentual.toFixed(1)}%)`}
                  outerRadius={80} 
                  fill="#8884d8" 
                  dataKey="valor"
                >
                  {mockData.receitasPorCategoria.map((_entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS_RECEITAS[index % COLORS_RECEITAS.length]} />
                  ))}
                </Pie>
                <Tooltip formatter={(value: number) => formatCurrency(value)} />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="chart-card">
          <div className="chart-header">
            <h2><TrendingDown size={22} />Despesas por Categoria</h2>
            <span className="chart-subtitle">Distribuição mensal</span>
          </div>
          <div className="chart-container">
            <ResponsiveContainer width="100%" height={280}>
              <PieChart>
                <Pie 
                  data={mockData.despesasPorCategoria} 
                  cx="50%" 
                  cy="50%" 
                  labelLine={false} 
                  // eslint-disable-next-line @typescript-eslint/no-explicit-any
                  label={(entry: any) => `${entry.categoria} (${entry.percentual.toFixed(1)}%)`}
                  outerRadius={80} 
                  fill="#8884d8" 
                  dataKey="valor"
                >
                  {mockData.despesasPorCategoria.map((_entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS_DESPESAS[index % COLORS_DESPESAS.length]} />
                  ))}
                </Pie>
                <Tooltip formatter={(value: number) => formatCurrency(value)} />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="chart-card">
          <div className="chart-header">
            <h2><CheckCircle size={22} />Status dos Lançamentos</h2>
            <span className="chart-subtitle">Situação atual</span>
          </div>
          <div className="chart-container">
            <ResponsiveContainer width="100%" height={280}>
              <BarChart data={mockData.statusLancamentos}>
                <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                <XAxis dataKey="status" stroke="#6b7280" />
                <YAxis stroke="#6b7280" />
                <Tooltip contentStyle={{ backgroundColor: '#fff', border: '1px solid #e5e7eb', borderRadius: '8px' }} />
                <Bar dataKey="quantidade" name="Lançamentos" radius={[8, 8, 0, 0]}>
                  {mockData.statusLancamentos.map((_entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS_STATUS[index]} />
                  ))}
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="chart-card">
          <div className="chart-header">
            <h2><PiggyBank size={22} />Taxa de Economia</h2>
            <span className="chart-subtitle">Tendência mensal (%)</span>
          </div>
          <div className="chart-container">
            <ResponsiveContainer width="100%" height={280}>
              <LineChart data={mockData.evolucaoMensal}>
                <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                <XAxis dataKey="mes" stroke="#6b7280" />
                <YAxis stroke="#6b7280" />
                <Tooltip contentStyle={{ backgroundColor: '#fff', border: '1px solid #e5e7eb', borderRadius: '8px' }} formatter={(value: number) => `${value.toFixed(1)}%`} />
                <Line type="monotone" dataKey="economia" stroke="#8b5cf6" strokeWidth={3} dot={{ fill: '#8b5cf6', r: 5 }} activeDot={{ r: 7 }} name="Taxa de Economia" />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>

      <div className="bottom-section">
        <div className="accounts-card">
          <div className="section-header">
            <h2><Wallet size={22} />Contas Bancárias</h2>
          </div>
          <div className="accounts-list">
            {mockData.contas.map((conta, index) => (
              <div key={index} className="account-item">
                <div className="account-info">
                  <div className="account-icon" style={{ backgroundColor: conta.cor }}>
                    <Wallet size={20} />
                  </div>
                  <div className="account-details">
                    <h4>{conta.nome}</h4>
                    <span className="account-type">{conta.tipo}</span>
                  </div>
                </div>
                <div className="account-balance">{formatCurrency(conta.saldo)}</div>
              </div>
            ))}
          </div>
        </div>

        <div className="transactions-card">
          <div className="section-header">
            <h2><Clock size={22} />Transações Recentes</h2>
          </div>
          <div className="transactions-list">
            {mockData.transacoesRecentes.map((transacao) => (
              <div key={transacao.id} className="transaction-item">
                <div className="transaction-info">
                  <div className={`transaction-icon transaction-icon--${transacao.tipo.toLowerCase()}`}>
                    {transacao.tipo === 'Receita' ? <TrendingUp size={18} /> : <TrendingDown size={18} />}
                  </div>
                  <div className="transaction-details">
                    <h4>{transacao.descricao}</h4>
                    <div className="transaction-meta">
                      <span className="transaction-category">{transacao.categoria}</span>
                      <span className="transaction-date">{formatDate(transacao.data)}</span>
                    </div>
                  </div>
                </div>
                <div className="transaction-right">
                  <div className={`transaction-value transaction-value--${transacao.tipo.toLowerCase()}`}>
                    {transacao.tipo === 'Receita' ? '+' : '-'} {formatCurrency(transacao.valor)}
                  </div>
                  <span className={`transaction-status transaction-status--${transacao.status.toLowerCase()}`}>
                    {transacao.status === 'Pago' ? <CheckCircle size={14} /> : <AlertCircle size={14} />}
                    {transacao.status}
                  </span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};
