using ReactingRecept.Application.DTOs;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IRecipeService
    {
        Task<bool> AnyAsync(Guid id);
        Task<bool> AnyAsync(string name);
        Task<RecipeDTO?> GetAsync(Guid id);
        Task<RecipeDTO?> GetAsync(string name);
        Task<RecipeDTO[]?> GetAllAsync();
        Task<RecipeDTO?> AddAsync(RecipeDTO recipeDTO);
        Task<RecipeDTO?> UpdateAsync(RecipeDTO recipeDTO);
        Task<bool> DeleteAsync(RecipeDTO recipeDTO);
    }
}
