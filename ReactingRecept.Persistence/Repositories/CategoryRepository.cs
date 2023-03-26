﻿using Microsoft.EntityFrameworkCore;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Shared;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ReactingReceptContext _reactingReceptContext;

    public CategoryRepository(ReactingReceptContext reactingReceptContext)
    {
        _reactingReceptContext = reactingReceptContext;
    }

    public async Task<Category[]?> ListAllOfTypeAsync(CategoryType type)
    {
        try
        {
            return await _reactingReceptContext.Category
                .Where(category => category.CategoryType == type)
                .ToArrayAsync();
        }
        catch (Exception exception)
        {
            Log.Error(exception, $"Repository failed to delete recipe with id: {type}");
            return null;
        }
    }
}
