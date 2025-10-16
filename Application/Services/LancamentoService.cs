using AutoMapper;
using ControleFinanceiro.Application.DTOs.Lancamento;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Interfaces.Repositories;

namespace ControleFinanceiro.Application.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IMapper _mapper;

        public LancamentoService(
            ILancamentoRepository lancamentoRepository,
            ICategoriaRepository categoriaRepository,
            IContaRepository contaRepository,
            IMapper mapper)
        {
            _lancamentoRepository = lancamentoRepository;
            _categoriaRepository = categoriaRepository;
            _contaRepository = contaRepository;
            _mapper = mapper;
        }

        // Métodos auxiliares de verificação de acesso
        private async Task VerificarELancarExcecaoSeNaoTemAcessoAsync(Guid contaId, Guid usuarioId)
        {
            if (!await _contaRepository.UsuarioTemAcessoContaAsync(usuarioId, contaId))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para acessar recursos desta conta");
            }
        }

        private async Task VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            if (contaIds == null || !contaIds.Any())
            {
                throw new ArgumentException("É necessário fornecer pelo menos uma conta");
            }

            foreach (var contaId in contaIds)
            {
                if (!await _contaRepository.UsuarioTemAcessoContaAsync(usuarioId, contaId))
                {
                    throw new UnauthorizedAccessException($"Você não tem permissão para acessar a conta com ID '{contaId}'");
                }
            }
        }

        private async Task<bool> VerificarAcessoContaAsync(Guid contaId, Guid usuarioId)
        {
            return await _contaRepository.UsuarioTemAcessoContaAsync(usuarioId, contaId);
        }

        private async Task<IEnumerable<Lancamento>> FiltrarLancamentosPorAcessoAsync(IEnumerable<Lancamento> lancamentos, Guid usuarioId)
        {
            var lancamentosFiltrados = new List<Lancamento>();
            foreach (var lancamento in lancamentos)
            {
                if (await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
                {
                    lancamentosFiltrados.Add(lancamento);
                }
            }
            return lancamentosFiltrados;
        }

        public async Task<IEnumerable<LancamentoDto>> GetAllAsync(Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosComCategoriaAsync();
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<LancamentoDto?> GetByIdAsync(Guid id, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null) return null;

            // Verificar acesso à conta do lançamento antes de retornar os dados
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamento.ContaId, usuarioId);

            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(dataInicio, dataFim, contaIds);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorCategoriaAsync(Guid categoriaId, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorCategoriaEContasAsync(categoriaId, contaIds);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorTipoAsync(TipoLancamento tipo, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorTipoEContasAsync(tipo, contaIds);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorStatusAsync(StatusLancamento status, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorStatusEContasAsync(status, contaIds);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosVencidosAsync(IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosVencidosPorContasAsync(contaIds);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosRecorrentesAsync(IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosRecorrentesPorContasAsync(contaIds);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorContaAsync(Guid contaId, Guid usuarioId)
        {
            // Verificar acesso à conta ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(contaId, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorContaAsync(contaId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(dataInicio, dataFim, contaIds);
            
            return lancamentos
                .Where(l => l.Status == StatusLancamento.Pago)
                .Sum(l => l.Tipo == TipoLancamento.Receita ? l.Valor : -l.Valor);
        }

        public async Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(dataInicio, dataFim, contaIds);
            
            return lancamentos
                .Where(l => l.Tipo == TipoLancamento.Receita && l.Status == StatusLancamento.Pago)
                .Sum(l => l.Valor);
        }

        public async Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IEnumerable<Guid> contaIds, Guid usuarioId)
        {
            // Verificar acesso a todas as contas ANTES de buscar os lançamentos
            await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(dataInicio, dataFim, contaIds);
            
            return lancamentos
                .Where(l => l.Tipo == TipoLancamento.Despesa && l.Status == StatusLancamento.Pago)
                .Sum(l => l.Valor);
        }

        public async Task<LancamentoDto> CreateAsync(CreateLancamentoDto createDto, Guid usuarioId)
        {
            // Verificar acesso à conta ANTES de qualquer outra operação
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(createDto.ContaId, usuarioId);

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(createDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{createDto.CategoriaId}' não encontrada");
            }

            var lancamento = _mapper.Map<Lancamento>(createDto);
            
            // Se for recorrente, configurar como lançamento pai e gerar filhos
            if (createDto.EhRecorrente && createDto.TipoRecorrencia != TipoRecorrencia.Nenhuma)
            {
                lancamento.ConfigurarRecorrencia(createDto.TipoRecorrencia, createDto.QuantidadeParcelas);
                
                // Criar o lançamento pai primeiro
                var lancamentoPai = await _lancamentoRepository.AddAsync(lancamento);
                
                // Gerar lançamentos filhos automaticamente
                await GerarLancamentosFilhosAsync(lancamentoPai, createDto);
                
                return _mapper.Map<LancamentoDto>(lancamentoPai);
            }

            var lancamentoCriado = await _lancamentoRepository.AddAsync(lancamento);
            return _mapper.Map<LancamentoDto>(lancamentoCriado);
        }

        public async Task<LancamentoDto> UpdateAsync(Guid id, UpdateLancamentoDto updateDto, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdWithRelacionamentosAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Verificar acesso ao lançamento atual imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamento.ContaId, usuarioId);

            // Verificar acesso à nova conta se for diferente
            if (updateDto.ContaId != lancamento.ContaId)
            {
                await VerificarELancarExcecaoSeNaoTemAcessoAsync(updateDto.ContaId, usuarioId);
            }

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(updateDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{updateDto.CategoriaId}' não encontrada");
            }

            // Se for um lançamento pai recorrente, usar o método específico
            if (lancamento.EhLancamentoPai())
            {
                var lancamentosAtualizados = await UpdateLancamentoRecorrenteAsync(id, updateDto, usuarioId);
                return lancamentosAtualizados.First();
            }

            // Atualização normal para lançamentos não recorrentes ou filhos
            _mapper.Map(updateDto, lancamento);
            
            // Atualizar configuração de recorrência
            if (updateDto.EhRecorrente && updateDto.TipoRecorrencia != TipoRecorrencia.Nenhuma)
            {
                lancamento.ConfigurarRecorrencia(updateDto.TipoRecorrencia, updateDto.QuantidadeParcelas);
            }
            else
            {
                lancamento.RemoverRecorrencia();
            }

            await _lancamentoRepository.UpdateAsync(lancamento);
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> UpdateLancamentoRecorrenteAsync(Guid id, UpdateLancamentoDto updateDto, Guid usuarioId)
        {
            var lancamentoPai = await _lancamentoRepository.GetByIdWithRelacionamentosAsync(id);
            if (lancamentoPai == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Verificar acesso imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamentoPai.ContaId, usuarioId);

            if (!lancamentoPai.EhLancamentoPai())
            {
                throw new InvalidOperationException("Este método só pode ser usado para lançamentos pai recorrentes");
            }

            // Verificar acesso à nova conta se for diferente
            if (updateDto.ContaId != lancamentoPai.ContaId)
            {
                await VerificarELancarExcecaoSeNaoTemAcessoAsync(updateDto.ContaId, usuarioId);
            }

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(updateDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{updateDto.CategoriaId}' não encontrada");
            }

            var lancamentosAtualizados = new List<Lancamento>();

            // Atualizar o lançamento pai
            var dadosOriginaisData = lancamentoPai.DataVencimento;
            _mapper.Map(updateDto, lancamentoPai);

            if (updateDto.EhRecorrente && updateDto.TipoRecorrencia != TipoRecorrencia.Nenhuma)
            {
                lancamentoPai.ConfigurarRecorrencia(updateDto.TipoRecorrencia, updateDto.QuantidadeParcelas);
            }
            else
            {
                lancamentoPai.RemoverRecorrencia();
            }

            await _lancamentoRepository.UpdateAsync(lancamentoPai);
            lancamentosAtualizados.Add(lancamentoPai);

            // Buscar e atualizar lançamentos filhos futuros (não pagos)
            var lancamentosFilhosFuturos = await _lancamentoRepository.GetLancamentosFilhosFuturosAsync(id);
            var filhosFuturos = lancamentosFilhosFuturos.Where(f => f.Status == StatusLancamento.Pendente).ToList();

            if (filhosFuturos.Any())
            {
                var diferencaDias = (updateDto.DataVencimento - dadosOriginaisData).Days;

                foreach (var filho in filhosFuturos)
                {
                    // Atualizar dados básicos (exceto data de vencimento que será recalculada)
                    filho.AtualizarDadosBasicos(updateDto.Descricao, updateDto.Valor, updateDto.Tipo, updateDto.CategoriaId, updateDto.ContaId, updateDto.Observacoes);
                    
                    // Ajustar data de vencimento se houver mudança na data do pai
                    if (diferencaDias != 0)
                    {
                        filho.DataVencimento = filho.DataVencimento.AddDays(diferencaDias);
                    }

                    // Atualizar configuração de recorrência
                    if (updateDto.EhRecorrente && updateDto.TipoRecorrencia != TipoRecorrencia.Nenhuma)
                    {
                        filho.ConfigurarRecorrencia(updateDto.TipoRecorrencia, updateDto.QuantidadeParcelas);
                    }
                    else
                    {
                        filho.RemoverRecorrencia();
                    }

                    await _lancamentoRepository.UpdateAsync(filho);
                    lancamentosAtualizados.Add(filho);
                }
            }

            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosAtualizados);
        }

        public async Task DeleteAsync(Guid id, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Verificar acesso imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamento.ContaId, usuarioId);

            await _lancamentoRepository.DeleteAsync(lancamento);
        }

        public async Task<LancamentoDto> MarcarComoPagoAsync(Guid id, Guid usuarioId, DateTime? dataPagamento = null)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Verificar acesso imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamento.ContaId, usuarioId);

            lancamento.MarcarComoPago(dataPagamento);
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<LancamentoDto> MarcarComoPendenteAsync(Guid id, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Verificar acesso imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamento.ContaId, usuarioId);

            lancamento.MarcarComoPendente();
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<LancamentoDto> CancelarAsync(Guid id, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Verificar acesso imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamento.ContaId, usuarioId);

            lancamento.Cancelar();
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> CriarLancamentosRecorrentesAsync(CreateLancamentoDto createDto, Guid usuarioId)
        {
            // Verificar acesso à conta ANTES de qualquer outra operação
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(createDto.ContaId, usuarioId);

            if (!createDto.EhRecorrente || createDto.TipoRecorrencia == TipoRecorrencia.Nenhuma)
            {
                throw new InvalidOperationException("Só é possível criar lançamentos recorrentes para DTOs marcados como recorrentes");
            }

            if (!await _categoriaRepository.ExistsAsync(createDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{createDto.CategoriaId}' não encontrada");
            }

            var lancamentos = new List<Lancamento>();
            var dataAtual = createDto.DataVencimento;
            var quantidadeParcelas = createDto.QuantidadeParcelas ?? 12; // Default 12 parcelas se não especificado

            for (int i = 1; i <= quantidadeParcelas; i++)
            {
                var lancamento = _mapper.Map<Lancamento>(createDto);
                lancamento.DataVencimento = dataAtual;
                lancamento.ParcelaAtual = i;
                lancamento.ConfigurarRecorrencia(createDto.TipoRecorrencia, quantidadeParcelas);

                if (i == 1)
                {
                    // O primeiro lançamento é o pai
                    lancamentos.Add(lancamento);
                }
                else
                {
                    // Os demais são filhos do primeiro
                    lancamento.LancamentoPaiId = lancamentos[0].Id;
                    lancamentos.Add(lancamento);
                }

                // Calcular próxima data baseada no tipo de recorrência
                dataAtual = createDto.TipoRecorrencia switch
                {
                    TipoRecorrencia.Diaria => dataAtual.AddDays(1),
                    TipoRecorrencia.Semanal => dataAtual.AddDays(7),
                    TipoRecorrencia.Mensal => dataAtual.AddMonths(1),
                    TipoRecorrencia.Anual => dataAtual.AddYears(1),
                    _ => dataAtual
                };
            }

            var lancamentosCriados = await _lancamentoRepository.AddRangeAsync(lancamentos);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosCriados);
        }

        public async Task<IEnumerable<LancamentoDto>> GerarLancamentosFuturosAsync(Guid lancamentoPaiId, Guid usuarioId)
        {
            var lancamentoPai = await _lancamentoRepository.GetByIdAsync(lancamentoPaiId);
            if (lancamentoPai == null || !lancamentoPai.EhLancamentoPai())
            {
                throw new InvalidOperationException("Lançamento pai não encontrado ou não é um lançamento recorrente válido");
            }

            // Verificar acesso imediatamente após buscar
            await VerificarELancarExcecaoSeNaoTemAcessoAsync(lancamentoPai.ContaId, usuarioId);

            var createDto = _mapper.Map<CreateLancamentoDto>(lancamentoPai);
            await GerarLancamentosFilhosAsync(lancamentoPai, createDto);

            var lancamentosFilhos = await _lancamentoRepository.GetLancamentosFilhosAsync(lancamentoPaiId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFilhos);
        }

        private async Task GerarLancamentosFilhosAsync(Lancamento lancamentoPai, CreateLancamentoDto createDto)
        {
            var quantidadeParcelas = createDto.QuantidadeParcelas ?? 12;
            var lancamentosFilhos = new List<Lancamento>();
            var dataAtual = lancamentoPai.DataVencimento;

            for (int i = 2; i <= quantidadeParcelas; i++) // Começa em 2 porque o pai já é a parcela 1
            {
                dataAtual = createDto.TipoRecorrencia switch
                {
                    TipoRecorrencia.Diaria => dataAtual.AddDays(1),
                    TipoRecorrencia.Semanal => dataAtual.AddDays(7),
                    TipoRecorrencia.Mensal => dataAtual.AddMonths(1),
                    TipoRecorrencia.Anual => dataAtual.AddYears(1),
                    _ => dataAtual
                };

                var lancamentoFilho = new Lancamento(
                    createDto.Descricao,
                    createDto.Valor,
                    dataAtual,
                    createDto.Tipo,
                    createDto.CategoriaId,
                    createDto.ContaId)
                {
                    Observacoes = createDto.Observacoes,
                    LancamentoPaiId = lancamentoPai.Id,
                    ParcelaAtual = i
                };

                lancamentoFilho.ConfigurarRecorrencia(createDto.TipoRecorrencia, quantidadeParcelas);
                lancamentosFilhos.Add(lancamentoFilho);
            }

            if (lancamentosFilhos.Any())
            {
                await _lancamentoRepository.AddRangeAsync(lancamentosFilhos);
            }
        }
    }
}