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

        // M�todos auxiliares de verifica��o de acesso
        private async Task<bool> VerificarAcessoContaAsync(Guid contaId, Guid usuarioId)
        {
            return await _contaRepository.UsuarioTemAcessoContaAsync(usuarioId, contaId);
        }

        private async Task<bool> VerificarAcessoLancamentoAsync(Guid lancamentoId, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(lancamentoId);
            if (lancamento == null) return false;
            
            return await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId);
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

            if (!await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para visualizar este lan�amento");
            }

            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorCategoriaAsync(Guid categoriaId, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorCategoriaAsync(categoriaId);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorTipoAsync(TipoLancamento tipo, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorTipoAsync(tipo);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorStatusAsync(StatusLancamento status, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorStatusAsync(status);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosVencidosAsync(Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosVencidosAsync();
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosRecorrentesAsync(Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosRecorrentesAsync();
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentosFiltrados);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorContaAsync(Guid contaId, Guid usuarioId)
        {
            if (!await VerificarAcessoContaAsync(contaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para visualizar lan�amentos desta conta");
            }

            var lancamentos = await _lancamentoRepository.GetLancamentosPorContaAsync(contaId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            
            return lancamentosFiltrados
                .Where(l => l.Status == StatusLancamento.Pago)
                .Sum(l => l.Tipo == TipoLancamento.Receita ? l.Valor : -l.Valor);
        }

        public async Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            
            return lancamentosFiltrados
                .Where(l => l.Tipo == TipoLancamento.Receita && l.Status == StatusLancamento.Pago)
                .Sum(l => l.Valor);
        }

        public async Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
            var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
            
            return lancamentosFiltrados
                .Where(l => l.Tipo == TipoLancamento.Despesa && l.Status == StatusLancamento.Pago)
                .Sum(l => l.Valor);
        }

        public async Task<LancamentoDto> CreateAsync(CreateLancamentoDto createDto, Guid usuarioId)
        {
            // Verificar se o usu�rio tem acesso � conta
            if (!await VerificarAcessoContaAsync(createDto.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para criar lan�amentos nesta conta");
            }

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(createDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{createDto.CategoriaId}' n�o encontrada");
            }

            var lancamento = _mapper.Map<Lancamento>(createDto);
            
            // Se for recorrente, configurar como lan�amento pai e gerar filhos
            if (createDto.EhRecorrente && createDto.TipoRecorrencia != TipoRecorrencia.Nenhuma)
            {
                lancamento.ConfigurarRecorrencia(createDto.TipoRecorrencia, createDto.QuantidadeParcelas);
                
                // Criar o lan�amento pai primeiro
                var lancamentoPai = await _lancamentoRepository.AddAsync(lancamento);
                
                // Gerar lan�amentos filhos automaticamente
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
                throw new KeyNotFoundException($"Lan�amento com ID '{id}' n�o encontrado");
            }

            // Verificar acesso ao lan�amento atual
            if (!await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para atualizar este lan�amento");
            }

            // Verificar acesso � nova conta se for diferente
            if (updateDto.ContaId != lancamento.ContaId && !await VerificarAcessoContaAsync(updateDto.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para mover o lan�amento para esta conta");
            }

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(updateDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{updateDto.CategoriaId}' n�o encontrada");
            }

            // Se for um lan�amento pai recorrente, usar o m�todo espec�fico
            if (lancamento.EhLancamentoPai())
            {
                var lancamentosAtualizados = await UpdateLancamentoRecorrenteAsync(id, updateDto, usuarioId);
                return lancamentosAtualizados.First();
            }

            // Atualiza��o normal para lan�amentos n�o recorrentes ou filhos
            _mapper.Map(updateDto, lancamento);
            
            // Atualizar configura��o de recorr�ncia
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
                throw new KeyNotFoundException($"Lan�amento com ID '{id}' n�o encontrado");
            }

            if (!await VerificarAcessoContaAsync(lancamentoPai.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para atualizar este lan�amento");
            }

            if (!lancamentoPai.EhLancamentoPai())
            {
                throw new InvalidOperationException("Este m�todo s� pode ser usado para lan�amentos pai recorrentes");
            }

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(updateDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{updateDto.CategoriaId}' n�o encontrada");
            }

            var lancamentosAtualizados = new List<Lancamento>();

            // Atualizar o lan�amento pai
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

            // Buscar e atualizar lan�amentos filhos futuros (n�o pagos)
            var lancamentosFilhosFuturos = await _lancamentoRepository.GetLancamentosFilhosFuturosAsync(id);
            var filhosFuturos = lancamentosFilhosFuturos.Where(f => f.Status == StatusLancamento.Pendente).ToList();

            if (filhosFuturos.Any())
            {
                var diferencaDias = (updateDto.DataVencimento - dadosOriginaisData).Days;

                foreach (var filho in filhosFuturos)
                {
                    // Atualizar dados b�sicos (exceto data de vencimento que ser� recalculada)
                    filho.AtualizarDadosBasicos(updateDto.Descricao, updateDto.Valor, updateDto.Tipo, updateDto.CategoriaId, updateDto.ContaId, updateDto.Observacoes);
                    
                    // Ajustar data de vencimento se houver mudan�a na data do pai
                    if (diferencaDias != 0)
                    {
                        filho.DataVencimento = filho.DataVencimento.AddDays(diferencaDias);
                    }

                    // Atualizar configura��o de recorr�ncia
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
                throw new KeyNotFoundException($"Lan�amento com ID '{id}' n�o encontrado");
            }

            if (!await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para excluir este lan�amento");
            }

            await _lancamentoRepository.DeleteAsync(lancamento);
        }

        public async Task<LancamentoDto> MarcarComoPagoAsync(Guid id, Guid usuarioId, DateTime? dataPagamento = null)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lan�amento com ID '{id}' n�o encontrado");
            }

            if (!await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para modificar este lan�amento");
            }

            lancamento.MarcarComoPago(dataPagamento);
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<LancamentoDto> MarcarComoPendenteAsync(Guid id, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lan�amento com ID '{id}' n�o encontrado");
            }

            if (!await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para modificar este lan�amento");
            }

            lancamento.MarcarComoPendente();
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<LancamentoDto> CancelarAsync(Guid id, Guid usuarioId)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lan�amento com ID '{id}' n�o encontrado");
            }

            if (!await VerificarAcessoContaAsync(lancamento.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para cancelar este lan�amento");
            }

            lancamento.Cancelar();
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> CriarLancamentosRecorrentesAsync(CreateLancamentoDto createDto, Guid usuarioId)
        {
            if (!await VerificarAcessoContaAsync(createDto.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para criar lan�amentos nesta conta");
            }

            if (!createDto.EhRecorrente || createDto.TipoRecorrencia == TipoRecorrencia.Nenhuma)
            {
                throw new InvalidOperationException("S� � poss�vel criar lan�amentos recorrentes para DTOs marcados como recorrentes");
            }

            if (!await _categoriaRepository.ExistsAsync(createDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{createDto.CategoriaId}' n�o encontrada");
            }

            var lancamentos = new List<Lancamento>();
            var dataAtual = createDto.DataVencimento;
            var quantidadeParcelas = createDto.QuantidadeParcelas ?? 12; // Default 12 parcelas se n�o especificado

            for (int i = 1; i <= quantidadeParcelas; i++)
            {
                var lancamento = _mapper.Map<Lancamento>(createDto);
                lancamento.DataVencimento = dataAtual;
                lancamento.ParcelaAtual = i;
                lancamento.ConfigurarRecorrencia(createDto.TipoRecorrencia, quantidadeParcelas);

                if (i == 1)
                {
                    // O primeiro lan�amento � o pai
                    lancamentos.Add(lancamento);
                }
                else
                {
                    // Os demais s�o filhos do primeiro
                    lancamento.LancamentoPaiId = lancamentos[0].Id;
                    lancamentos.Add(lancamento);
                }

                // Calcular pr�xima data baseada no tipo de recorr�ncia
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
                throw new InvalidOperationException("Lan�amento pai n�o encontrado ou n�o � um lan�amento recorrente v�lido");
            }

            if (!await VerificarAcessoContaAsync(lancamentoPai.ContaId, usuarioId))
            {
                throw new UnauthorizedAccessException("Voc� n�o tem permiss�o para gerar lan�amentos desta conta");
            }

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

            for (int i = 2; i <= quantidadeParcelas; i++) // Come�a em 2 porque o pai j� � a parcela 1
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