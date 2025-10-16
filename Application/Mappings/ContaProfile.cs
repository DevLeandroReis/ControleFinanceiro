using AutoMapper;
using ControleFinanceiro.Application.DTOs.Conta;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappings
{
    public class ContaProfile : Profile
    {
        public ContaProfile()
        {
            CreateMap<Conta, ContaDto>()
                .ForMember(dest => dest.ProprietarioNome, opt => opt.MapFrom(src => src.Proprietario.Nome))
                .ForMember(dest => dest.Usuarios, opt => opt.MapFrom(src => src.UsuarioContas));

            CreateMap<CreateContaDto, Conta>()
                .ForMember(dest => dest.ProprietarioId, opt => opt.Ignore()); // Será definido no service

            CreateMap<UpdateContaDto, Conta>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProprietarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Ativa, opt => opt.Ignore());

            CreateMap<SolicitacaoAcessoConta, SolicitacaoAcessoContaDto>()
                .ForMember(dest => dest.SolicitanteNome, opt => opt.MapFrom(src => src.Solicitante.Nome))
                .ForMember(dest => dest.SolicitanteEmail, opt => opt.MapFrom(src => src.Solicitante.Email))
                .ForMember(dest => dest.ProprietarioNome, opt => opt.MapFrom(src => src.Proprietario.Nome))
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));

            CreateMap<UsuarioConta, UsuarioContaDto>()
                .ForMember(dest => dest.UsuarioNome, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.UsuarioEmail, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Conta.Nome));
        }
    }
}