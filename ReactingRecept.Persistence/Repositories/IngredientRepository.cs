using Microsoft.EntityFrameworkCore;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class IngredientRepository : RepositoryBase<Ingredient>, IIngredientRepository
{
    public IngredientRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }

    public async Task<bool> AnyAsync(string name)
    {
        try
        {
            return await _reactingReceptContext.Ingredient.AnyAsync(ingredient => ingredient.Name.ToLower() == name.ToLower());
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed check existence of ingredient with name: {name}");
            return false;
        }
    }

    public override async Task<Ingredient?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _reactingReceptContext.Set<Ingredient>()
                .Include(ingredient => ingredient.Category)
                .FirstOrDefaultAsync(ingredient => ingredient.Id == id);
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to fetch ingredient with id: {id}");
            return null;
        }
    }

    public async Task<Ingredient?> GetByNameAsync(string name)
    {
        try
        {
            return await _reactingReceptContext.Set<Ingredient>().FirstAsync(ingredient => ingredient.Name.ToLower() == name.ToLower());
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to fetch recipe with name: {name}");
            return null;
        }
    }

    public override async Task<Ingredient[]?> GetAllAsync()
    {
        try
        {
            return await _reactingReceptContext.Set<Ingredient>()
                .Include(ingredient => ingredient.Category)
                .ToArrayAsync();
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, "Repository failed to fetch many ingredients");
            return null;
        }
    }

    public override async Task<Ingredient?> AddAsync(Ingredient ingredient)
    {
        try
        {
            Ingredient? existingIngredient = await GetByNameAsync(ingredient.Name);

            if (existingIngredient != null &&
                existingIngredient.CategoryId == ingredient.CategoryId)
            {
                return existingIngredient;
            }

            Category category = await _reactingReceptContext.Category
                .Where(category => category.Id == ingredient.CategoryId)
                .FirstAsync();
            ingredient.SetCategory(category);

            _reactingReceptContext.Set<Ingredient>().Add(ingredient);

            await _reactingReceptContext.SaveChangesAsync();
            await _reactingReceptContext.Entry(ingredient).ReloadAsync();
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to add ingredient: {ingredient}");
            return null;
        }

        return ingredient;
    }
}
