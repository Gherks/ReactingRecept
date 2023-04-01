using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Application.Interfaces.Persistence;

public interface IRecipeRepository : IAsyncRepository<Recipe>
{
    Task<bool> AnyAsync(string name);
    Task<Recipe?> GetByNameAsync(string name);
}
