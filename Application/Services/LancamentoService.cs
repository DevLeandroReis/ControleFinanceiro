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
        private readonly IMapper _mapper;

        public LancamentoService(
            ILancamentoRepository lancamentoRepository,
            ICategoriaRepository categoriaRepository,
            IMapper mapper)
        {
            _lancamentoRepository = lancamentoRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LancamentoDto>> GetAllAsync()
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosComCategoriaAsync();
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<LancamentoDto?> GetByIdAsync(Guid id)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            return lancamento == null ? null : _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorCategoriaAsync(Guid categoriaId)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorCategoriaAsync(categoriaId);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorTipoAsync(TipoLancamento tipo)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorTipoAsync(tipo);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorStatusAsync(StatusLancamento status)
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosPorStatusAsync(status);
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosVencidosAsync()
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosVencidosAsync();
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<IEnumerable<LancamentoDto>> GetLancamentosRecorrentesAsync()
        {
            var lancamentos = await _lancamentoRepository.GetLancamentosRecorrentesAsync();
            return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
        }

        public async Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _lancamentoRepository.GetSaldoPorPeriodoAsync(dataInicio, dataFim);
        }

        public async Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _lancamentoRepository.GetTotalReceitasPorPeriodoAsync(dataInicio, dataFim);
        }

        public async Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _lancamentoRepository.GetTotalDespesasPorPeriodoAsync(dataInicio, dataFim);
        }

        public async Task<LancamentoDto> CreateAsync(CreateLancamentoDto createDto)
        {
            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(createDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{createDto.CategoriaId}' não encontrada");
            }

            var lancamento = _mapper.Map<Lancamento>(createDto);
            
            // Se for recorrente, criar apenas o primeiro lançamento
            if (createDto.EhRecorrente && createDto.TipoRecorrencia != TipoRecorrencia.Nenhuma)
            {
                lancamento.ConfigurarRecorrencia(createDto.TipoRecorrencia, createDto.QuantidadeParcelas);
            }

            var lancamentoCriado = await _lancamentoRepository.AddAsync(lancamento);
            return _mapper.Map<LancamentoDto>(lancamentoCriado);
        }

        public async Task<LancamentoDto> UpdateAsync(Guid id, UpdateLancamentoDto updateDto)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            // Validar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(updateDto.CategoriaId))
            {
                throw new KeyNotFoundException($"Categoria com ID '{updateDto.CategoriaId}' não encontrada");
            }

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

        public async Task DeleteAsync(Guid id)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            await _lancamentoRepository.DeleteAsync(lancamento);
        }

        public async Task<LancamentoDto> MarcarComoPagoAsync(Guid id, DateTime? dataPagamento = null)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            lancamento.MarcarComoPago(dataPagamento);
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<LancamentoDto> MarcarComoPendenteAsync(Guid id)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            lancamento.MarcarComoPendente();
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<LancamentoDto> CancelarAsync(Guid id)
        {
            var lancamento = await _lancamentoRepository.GetByIdAsync(id);
            if (lancamento == null)
            {
                throw new KeyNotFoundException($"Lançamento com ID '{id}' não encontrado");
            }

            lancamento.Cancelar();
            await _lancamentoRepository.UpdateAsync(lancamento);
            
            return _mapper.Map<LancamentoDto>(lancamento);
        }

        public async Task<IEnumerable<LancamentoDto>> CriarLancamentosRecorrentesAsync(CreateLancamentoDto createDto)
        {
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
    }
}