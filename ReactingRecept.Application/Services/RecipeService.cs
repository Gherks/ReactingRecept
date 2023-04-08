using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Services;

namespace ReactingRecept.Application.Services
{
    public class RecipeService : IRecipeService
    {
        public Task<RecipeDTO?> AddAsync(RecipeDTO recipeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(RecipeDTO recipeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDTO[]?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDTO?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDTO?> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDTO?> UpdateAsync(RecipeDTO recipeDTO)
        {
            throw new NotImplementedException();
        }
    }
}
