using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain;

namespace ReactingRecept.Application.Services
{
    public class RecipeService : IRecipeService
    {
        public Task<IRecipeDto?> AddAsync(IRecipeDto recipe)
        {
            throw new NotImplementedException();
        }

        public Task<IRecipeDto[]?> AddManyAsync(IRecipeDto[] recipes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IRecipeDto recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(IRecipeDto[] recipes)
        {
            throw new NotImplementedException();
        }

        public Task<IRecipeDto[]?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IRecipeDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IRecipeDto?> UpdateAsync(IRecipeDto recipe)
        {
            throw new NotImplementedException();
        }

        public Task<IRecipeDto[]?> UpdateManyAsync(IRecipeDto[] recipes)
        {
            throw new NotImplementedException();
        }
    }
}
