using ReactingRecept.Infrastructure.Repositories.Interfaces;
using ReactingRecept.Server.Entities;
using Microsoft.EntityFrameworkCore;
using ReactingRecept.Infrastructure.Context;
using ReactingRecept.Logging;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ReactingReceptContext _reactingReceptContext;

    public CategoryRepository(ReactingReceptContext reactingReceptContext) 
    {
        _reactingReceptContext = reactingReceptContext;
    }

    public async Task<IReadOnlyList<Category>?> ListAllOfTypeAsync(CategoryType categoryType)
    {
        try
        {
            return await _reactingReceptContext.Category
                .Where(category => category.CategoryType == categoryType)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            Log.Error(exception, $"Repository failed to delete recipe with id: {categoryType}");
            return null;
        }
    }
}
