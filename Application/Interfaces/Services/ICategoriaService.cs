using ControleFinanceiro.Application.DTOs.Categoria;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<IEnumerable<CategoriaDto>> GetCategoriasAtivasAsync();
        Task<CategoriaDto?> GetByIdAsync(Guid id);
        Task<CategoriaDto?> GetByNomeAsync(string nome);
        Task<CategoriaDto> CreateAsync(CreateCategoriaDto createDto);
        Task<CategoriaDto> UpdateAsync(Guid id, UpdateCategoriaDto updateDto);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> NomeJaExisteAsync(string nome, Guid? excludeId = null);
    }
}