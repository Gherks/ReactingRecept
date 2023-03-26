using ReactingRecept.Domain;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IRecipeService
    {
        Task<bool> AnyAsync(Guid id);
        Task<IRecipeDto?> GetByIdAsync(Guid id);
        Task<IRecipeDto[]?> GetAllAsync();
        Task<IRecipeDto?> AddAsync(IRecipeDto recipe);
        Task<IRecipeDto[]?> AddManyAsync(IRecipeDto[] recipes);
        Task<IRecipeDto?> UpdateAsync(IRecipeDto recipe);
        Task<IRecipeDto[]?> UpdateManyAsync(IRecipeDto[] recipes);
        Task<bool> DeleteAsync(IRecipeDto recipe);
        Task<bool> DeleteManyAsync(IRecipeDto[] recipes);
    }
}
