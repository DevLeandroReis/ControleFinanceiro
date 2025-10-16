using ControleFinanceiro.Application.DTOs.Categoria;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<IEnumerable<CategoriaDto>> GetCategoriasAtivasAsync();
        Task<CategoriaDto?> GetByIdAsync(Guid id);
        Task<CategoriaDto?> GetByNomeAsync(string nome);
        Task<bool> ExistsAsync(Guid id);
    }
}