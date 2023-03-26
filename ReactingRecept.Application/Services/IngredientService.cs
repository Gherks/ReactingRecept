using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Services
{
    public class IngredientService : IIngredientService
    {
        public Task<IIngredientDto?> AddAsync(IIngredientDto ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<IIngredientDto[]?> AddManyAsync(IIngredientDto[] ingredients)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IIngredientDto ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(IIngredientDto[] ingredients)
        {
            throw new NotImplementedException();
        }

        public Task<IIngredientDto[]?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IIngredientDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IIngredientDto?> UpdateAsync(IIngredientDto ingredient)
        {
            throw new NotImplementedException();
        }

        public Task<IIngredientDto[]?> UpdateManyAsync(IIngredientDto[] ingredients)
        {
            throw new NotImplementedException();
        }
    }
}
