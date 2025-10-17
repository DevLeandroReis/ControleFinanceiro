using AutoMapper;
using ControleFinanceiro.Application.DTOs.Conta;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Interfaces.Repositories;

namespace ControleFinanceiro.Application.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _contaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioContaRepository _usuarioContaRepository;
        private readonly ISolicitacaoAcessoContaRepository _solicitacaoRepository;
        private readonly IMapper _mapper;

        public ContaService(
            IContaRepository contaRepository,
            IUsuarioRepository usuarioRepository,
            IUsuarioContaRepository usuarioContaRepository,
            ISolicitacaoAcessoContaRepository solicitacaoRepository,
            IMapper mapper)
        {
            _contaRepository = contaRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioContaRepository = usuarioContaRepository;
            _solicitacaoRepository = solicitacaoRepository;
            _mapper = mapper;
        }

        public async Task<ContaDto> CreateAsync(CreateContaDto createDto, Guid proprietarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(proprietarioId);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado");
            }

            var conta = new Conta(createDto.Nome, proprietarioId, createDto.Descricao);
            var contaCriada = await _contaRepository.AddAsync(conta);

            return _mapper.Map<ContaDto>(contaCriada);
        }

        public async Task<ContaDto> UpdateAsync(Guid id, UpdateContaDto updateDto, Guid usuarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(id);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada");
            }

            if (!conta.UsuarioTemAcesso(usuarioId))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para atualizar esta conta");
            }

            conta.AlterarNome(updateDto.Nome);
            conta.AlterarDescricao(updateDto.Descricao);

            await _contaRepository.UpdateAsync(conta);
            return _mapper.Map<ContaDto>(conta);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid usuarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(id);
            if (conta == null)
            {
                return false;
            }

            if (conta.ProprietarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode excluir a conta");
            }

            await _contaRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ContaDto?> GetByIdAsync(Guid id, Guid usuarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(id);
            if (conta == null)
            {
                return null;
            }

            if (!conta.UsuarioTemAcesso(usuarioId))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para visualizar esta conta");
            }

            return _mapper.Map<ContaDto>(conta);
        }

        public async Task<IEnumerable<ContaDto>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            var contas = await _contaRepository.GetByUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<ContaDto>>(contas);
        }

        public async Task<bool> AtivarAsync(Guid id, Guid usuarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(id);
            if (conta == null)
            {
                return false;
            }

            if (conta.ProprietarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode ativar a conta");
            }

            conta.Ativar();
            await _contaRepository.UpdateAsync(conta);
            return true;
        }

        public async Task<bool> DesativarAsync(Guid id, Guid usuarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(id);
            if (conta == null)
            {
                return false;
            }

            if (conta.ProprietarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode desativar a conta");
            }

            conta.Desativar();
            await _contaRepository.UpdateAsync(conta);
            return true;
        }

        // Gerenciamento de usuários
        public async Task<SolicitacaoAcessoContaDto> SolicitarAcessoAsync(SolicitarAcessoContaDto solicitarDto, Guid solicitanteId)
        {
            var conta = await _contaRepository.GetByIdAsync(solicitarDto.ContaId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada");
            }

            // Verificar se já tem acesso
            if (conta.UsuarioTemAcesso(solicitanteId))
            {
                throw new InvalidOperationException("Você já tem acesso a esta conta");
            }

            // Verificar se já existe solicitação pendente
            var solicitacaoPendente = await _solicitacaoRepository.GetPendenteBySolicitanteContaAsync(solicitanteId, solicitarDto.ContaId);
            if (solicitacaoPendente != null)
            {
                throw new InvalidOperationException("Já existe uma solicitação pendente para esta conta");
            }

            var solicitacao = new SolicitacaoAcessoConta(solicitanteId, conta.ProprietarioId, solicitarDto.ContaId);
            var solicitacaoCriada = await _solicitacaoRepository.AddAsync(solicitacao);

            return _mapper.Map<SolicitacaoAcessoContaDto>(solicitacaoCriada);
        }

        public async Task<bool> AprovarSolicitacaoAsync(Guid solicitacaoId, Guid proprietarioId)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(solicitacaoId);
            if (solicitacao == null)
            {
                return false;
            }

            if (solicitacao.ProprietarioId != proprietarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode aprovar solicitações");
            }

            solicitacao.Aprovar();
            await _solicitacaoRepository.UpdateAsync(solicitacao);

            // Criar UsuarioConta
            var usuarioConta = new UsuarioConta(solicitacao.SolicitanteId, solicitacao.ContaId);
            await _usuarioContaRepository.AddAsync(usuarioConta);

            return true;
        }

        public async Task<bool> RejeitarSolicitacaoAsync(Guid solicitacaoId, Guid proprietarioId)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(solicitacaoId);
            if (solicitacao == null)
            {
                return false;
            }

            if (solicitacao.ProprietarioId != proprietarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode rejeitar solicitações");
            }

            solicitacao.Rejeitar();
            await _solicitacaoRepository.UpdateAsync(solicitacao);

            return true;
        }

        public async Task<bool> CancelarSolicitacaoAsync(Guid solicitacaoId, Guid solicitanteId)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(solicitacaoId);
            if (solicitacao == null)
            {
                return false;
            }

            if (solicitacao.SolicitanteId != solicitanteId)
            {
                throw new UnauthorizedAccessException("Apenas o solicitante pode cancelar a solicitação");
            }

            solicitacao.Cancelar();
            await _solicitacaoRepository.UpdateAsync(solicitacao);

            return true;
        }

        public async Task<IEnumerable<SolicitacaoAcessoContaDto>> GetSolicitacoesPendentesByProprietarioAsync(Guid proprietarioId)
        {
            var solicitacoes = await _solicitacaoRepository.GetByProprietarioIdAsync(proprietarioId);
            return _mapper.Map<IEnumerable<SolicitacaoAcessoContaDto>>(solicitacoes);
        }

        public async Task<IEnumerable<SolicitacaoAcessoContaDto>> GetSolicitacoesBySolicitanteAsync(Guid solicitanteId)
        {
            var solicitacoes = await _solicitacaoRepository.GetBySolicitanteIdAsync(solicitanteId);
            return _mapper.Map<IEnumerable<SolicitacaoAcessoContaDto>>(solicitacoes);
        }

        // Gerenciamento de permissões
        public async Task<bool> ConcederPermissaoAdicionarUsuariosAsync(Guid contaId, Guid usuarioId, Guid proprietarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null)
            {
                return false;
            }

            if (conta.ProprietarioId != proprietarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode conceder permissões");
            }

            var usuarioConta = await _usuarioContaRepository.GetByUsuarioContaAsync(usuarioId, contaId);
            if (usuarioConta == null)
            {
                throw new InvalidOperationException("Usuário não tem acesso à conta");
            }

            usuarioConta.ConcederPermissaoAdicionarUsuarios();
            await _usuarioContaRepository.UpdateAsync(usuarioConta);

            return true;
        }

        public async Task<bool> RemoverPermissaoAdicionarUsuariosAsync(Guid contaId, Guid usuarioId, Guid proprietarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null)
            {
                return false;
            }

            if (conta.ProprietarioId != proprietarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode remover permissões");
            }

            var usuarioConta = await _usuarioContaRepository.GetByUsuarioContaAsync(usuarioId, contaId);
            if (usuarioConta == null)
            {
                throw new InvalidOperationException("Usuário não tem acesso à conta");
            }

            usuarioConta.RemoverPermissaoAdicionarUsuarios();
            await _usuarioContaRepository.UpdateAsync(usuarioConta);

            return true;
        }

        public async Task<bool> RemoverUsuarioContaAsync(Guid contaId, Guid usuarioId, Guid proprietarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null)
            {
                return false;
            }

            if (conta.ProprietarioId != proprietarioId)
            {
                throw new UnauthorizedAccessException("Apenas o proprietário pode remover usuários");
            }

            var usuarioConta = await _usuarioContaRepository.GetByUsuarioContaAsync(usuarioId, contaId);
            if (usuarioConta == null)
            {
                throw new InvalidOperationException("Usuário não tem acesso à conta");
            }

            usuarioConta.Desativar();
            await _usuarioContaRepository.UpdateAsync(usuarioConta);

            return true;
        }

        public async Task<IEnumerable<UsuarioContaDto>> GetUsuariosByContaIdAsync(Guid contaId, Guid usuarioId)
        {
            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta não encontrada");
            }

            if (!conta.UsuarioTemAcesso(usuarioId))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para visualizar os usuários desta conta");
            }

            var usuariosContas = await _usuarioContaRepository.GetByContaIdAsync(contaId);
            return _mapper.Map<IEnumerable<UsuarioContaDto>>(usuariosContas);
        }
    }
}
