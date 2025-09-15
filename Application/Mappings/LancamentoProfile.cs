using AutoMapper;
using ControleFinanceiro.Application.DTOs.Lancamento;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappings
{
    public class LancamentoProfile : Profile
    {
        public LancamentoProfile()
        {
            CreateMap<Lancamento, LancamentoDto>()
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria.Nome));
                
            CreateMap<CreateLancamentoDto, Lancamento>();
            CreateMap<UpdateLancamentoDto, Lancamento>();
        }
    }
}