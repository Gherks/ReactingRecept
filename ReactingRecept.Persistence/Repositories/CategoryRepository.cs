using Microsoft.EntityFrameworkCore;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Shared;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }

    public async Task<Category[]?> GetManyOfTypeAsync(CategoryType type)
    {
        try
        {
            return await _reactingReceptContext.Category
                .Where(category => category.Type == type)
                .ToArrayAsync();
        }
        catch (Exception exception)
        {
            Log.Error(exception, $"Repository failed to delete recipe with id: {type}");
            return null;
        }
    }
}
