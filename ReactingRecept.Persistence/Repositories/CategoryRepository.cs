using Microsoft.EntityFrameworkCore;
using ReactingRecept.Domain;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Persistence.Context;
using static ReactingRecept.Shared.Enums;
using ReactingRecept.Shared;

namespace ReactingRecept.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ReactingReceptContext _reactingReceptContext;

    public CategoryRepository(ReactingReceptContext reactingReceptContext)
    {
        _reactingReceptContext = reactingReceptContext;
    }

    public async Task<Category[]?> ListAllOfTypeAsync(CategoryType categoryType)
    {
        try
        {
            return await _reactingReceptContext.Category
                .Where(category => category.CategoryType == categoryType)
                .ToArrayAsync();
        }
        catch (Exception exception)
        {
            Log.Error(exception, $"Repository failed to delete recipe with id: {categoryType}");
            return null;
        }
    }
}
