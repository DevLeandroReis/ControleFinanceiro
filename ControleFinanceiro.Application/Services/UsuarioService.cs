using AutoMapper;
using ControleFinanceiro.Application.DTOs.Usuario;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using BCrypt.Net;

namespace ControleFinanceiro.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IJwtService jwtService,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto createDto)
        {
            // Verificar se email j� existe
            if (await _usuarioRepository.ExisteEmailAsync(createDto.Email))
            {
                throw new InvalidOperationException("Email j� est� em uso");
            }

            var usuario = _mapper.Map<Usuario>(createDto);
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(createDto.Senha);
            usuario.DefinirTokenConfirmacaoEmail(Guid.NewGuid().ToString());

            var usuarioCriado = await _usuarioRepository.AddAsync(usuario);
            
            // Aqui voc� pode implementar o envio de email de confirma��o
            // await _emailService.EnviarConfirmacaoEmailAsync(usuarioCriado.Email, usuarioCriado.TokenConfirmacaoEmail);

            return _mapper.Map<UsuarioDto>(usuarioCriado);
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            {
                throw new UnauthorizedAccessException("Email ou senha inv�lidos");
            }

            if (!usuario.Ativo)
            {
                throw new UnauthorizedAccessException("Usu�rio desativado");
            }

            // Atualizar �ltimo login
            usuario.AtualizarUltimoLogin();
            await _usuarioRepository.UpdateAsync(usuario);

            var token = _jwtService.GenerateToken(usuario);
            
            return new AuthResultDto
            {
                Token = token,
                Usuario = _mapper.Map<UsuarioDto>(usuario),
                ExpiresAt = DateTime.UtcNow.AddHours(24) // Configur�vel
            };
        }

        public async Task<bool> RecuperarSenhaAsync(RecuperarSenhaDto recuperarSenhaDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(recuperarSenhaDto.Email);
            
            if (usuario == null || !usuario.Ativo)
            {
                // Por seguran�a, n�o informamos se o email existe ou n�o
                return true;
            }

            var token = Guid.NewGuid().ToString();
            var expiracao = DateTime.UtcNow.AddHours(2); // Token v�lido por 2 horas
            
            usuario.DefinirTokenRecuperacaoSenha(token, expiracao);
            await _usuarioRepository.UpdateAsync(usuario);

            // Aqui voc� pode implementar o envio de email com o token
            //await _emailService.EnviarTokenRecuperacaoSenhaAsync(usuario.Email, token);

            return true;
        }

        public async Task<bool> RedefinirSenhaAsync(RedefinirSenhaDto redefinirSenhaDto)
        {
            var usuario = await _usuarioRepository.GetByTokenRecuperacaoSenhaAsync(redefinirSenhaDto.Token);
            
            if (usuario == null || !usuario.PodeRecuperarSenha())
            {
                throw new InvalidOperationException("Token inv�lido ou expirado");
            }

            var novaSenhaHash = BCrypt.Net.BCrypt.HashPassword(redefinirSenhaDto.NovaSenha);
            usuario.AlterarSenha(novaSenhaHash);
            
            await _usuarioRepository.UpdateAsync(usuario);
            
            return true;
        }

        public async Task<bool> ConfirmarEmailAsync(string token)
        {
            var usuario = await _usuarioRepository.GetByTokenConfirmacaoEmailAsync(token);
            
            if (usuario == null)
            {
                return false;
            }

            usuario.ConfirmarEmail();
            await _usuarioRepository.UpdateAsync(usuario);
            
            return true;
        }

        public async Task<UsuarioDto?> GetByIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario == null ? null : _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> GetByEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            return usuario == null ? null : _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }

        public async Task<bool> AtivarAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            
            if (usuario == null)
            {
                return false;
            }

            usuario.Ativar();
            await _usuarioRepository.UpdateAsync(usuario);
            
            return true;
        }

        public async Task<bool> DesativarAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            
            if (usuario == null)
            {
                return false;
            }

            usuario.Desativar();
            await _usuarioRepository.UpdateAsync(usuario);
            
            return true;
        }
    }
}