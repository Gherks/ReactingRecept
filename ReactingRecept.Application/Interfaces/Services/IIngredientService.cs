using ReactingRecept.Application.DTOs;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<bool> AnyAsync(Guid id);
        Task<bool> AnyAsync(string name);
        Task<IngredientDTO?> GetAsync(Guid id);
        Task<IngredientDTO?> GetAsync(string name);
        Task<IngredientDTO[]?> GetAllAsync();
        Task<IngredientDTO?> AddAsync(IngredientDTO ingredientDTO);
        Task<IngredientDTO?> UpdateAsync(IngredientDTO ingredientDTO);
        Task<bool> DeleteAsync(IngredientDTO ingredientDTO);
    }
}
