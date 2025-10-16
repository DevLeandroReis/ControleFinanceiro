using AutoMapper;
using ControleFinanceiro.Application.DTOs.Categoria;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Interfaces.Repositories;

namespace ControleFinanceiro.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<IEnumerable<CategoriaDto>> GetCategoriasAtivasAsync()
        {
            var categorias = await _categoriaRepository.GetCategoriasAtivasAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<IEnumerable<CategoriaDto>> GetCategoriasDestacadasAsync()
        {
            var categorias = await _categoriaRepository.GetCategoriasDestacadasAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> GetByIdAsync(Guid id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            return categoria == null ? null : _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<CategoriaDto?> GetByNomeAsync(string nome)
        {
            var categoria = await _categoriaRepository.GetByNomeAsync(nome);
            return categoria == null ? null : _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _categoriaRepository.ExistsAsync(id);
        }
    }
}
