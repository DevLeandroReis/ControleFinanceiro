using AutoMapper;
using ControleFinanceiro.Application.DTOs.Usuario;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<CreateUsuarioDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore()); // Será configurado no service
        }
    }
}