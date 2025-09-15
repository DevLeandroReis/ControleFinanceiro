using AutoMapper;
using ControleFinanceiro.Application.DTOs.Categoria;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Entities;
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

        public async Task<CategoriaDto> CreateAsync(CreateCategoriaDto createDto)
        {
            // Validar se o nome já existe
            if (await NomeJaExisteAsync(createDto.Nome))
            {
                throw new InvalidOperationException($"Já existe uma categoria com o nome '{createDto.Nome}'");
            }

            var categoria = _mapper.Map<Categoria>(createDto);
            var categoriaCriada = await _categoriaRepository.AddAsync(categoria);
            
            return _mapper.Map<CategoriaDto>(categoriaCriada);
        }

        public async Task<CategoriaDto> UpdateAsync(Guid id, UpdateCategoriaDto updateDto)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException($"Categoria com ID '{id}' não encontrada");
            }

            // Validar se o nome já existe (excluindo a categoria atual)
            if (await NomeJaExisteAsync(updateDto.Nome, id))
            {
                throw new InvalidOperationException($"Já existe uma categoria com o nome '{updateDto.Nome}'");
            }

            _mapper.Map(updateDto, categoria);
            await _categoriaRepository.UpdateAsync(categoria);
            
            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task DeleteAsync(Guid id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException($"Categoria com ID '{id}' não encontrada");
            }

            await _categoriaRepository.DeleteAsync(categoria);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _categoriaRepository.ExistsAsync(id);
        }

        public async Task<bool> NomeJaExisteAsync(string nome, Guid? excludeId = null)
        {
            return await _categoriaRepository.NomeJaExisteAsync(nome, excludeId);
        }
    }
}