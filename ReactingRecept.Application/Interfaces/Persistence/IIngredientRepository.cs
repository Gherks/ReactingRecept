using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Application.Interfaces.Persistence;

public interface IIngredientRepository : IAsyncRepository<Ingredient>
{
    Task<bool> AnyAsync(string name);
    Task<Ingredient?> GetByNameAsync(string name);
}
