using Microsoft.EntityFrameworkCore;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Shared;

namespace ReactingRecept.Persistence.Repositories;

public class RecipeRepository : RepositoryBase<Recipe>, IRecipeRepository
{
    public RecipeRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }

    public async Task<bool> AnyAsync(string name)
    {
        try
        {
            return await _reactingReceptContext.Recipe.AnyAsync(recipe => recipe.Name.ToLower() == name.ToLower());
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed check existence of recipe with name: {name}");
            return false;
        }
    }

    public override async Task<Recipe?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _reactingReceptContext.Recipe
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.IngredientMeasurements)
                    .ThenInclude(ingredientMeasurement => ingredientMeasurement.Ingredient)
                        .ThenInclude(ingredient => ingredient!.Category)
                .FirstOrDefaultAsync(recipe => recipe.Id == id);
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to fetch recipe with id: {id}");
            return null;
        }
    }

    public async Task<Recipe?> GetByNameAsync(string name)
    {
        try
        {
            return await _reactingReceptContext.Set<Recipe>().FirstAsync(recipe => recipe.Name.ToLower() == name.ToLower());
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to fetch recipe with name: {name}");
            return null;
        }
    }

    public override async Task<Recipe[]?> GetAllAsync()
    {
        try
        {
            return await _reactingReceptContext.Recipe
                .Include(recipe => recipe.Category)
                .Include(recipe => recipe.IngredientMeasurements)
                    .ThenInclude(ingredientMeasurement => ingredientMeasurement.Ingredient)
                        .ThenInclude(ingredient => ingredient!.Category)
                .ToArrayAsync();
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, "Repository failed to fetch all recipes.");
            return null;
        }
    }

    public override async Task<Recipe?> AddAsync(Recipe recipe)
    {
        try
        {
            foreach (IngredientMeasurement ingredientMeasurement in recipe.IngredientMeasurements)
            {
                Ingredient ingredient = await _reactingReceptContext.Ingredient
                    .Where(ingredient => ingredient.Id == ingredientMeasurement.IngredientId)
                    .Include(ingredient => ingredient.Category)
                    .FirstAsync();

                ingredientMeasurement.SetIngredient(ingredient);
            }

            Category category = await _reactingReceptContext.Category
                .Where(category => category.Id == recipe.CategoryId)
                .FirstAsync();

            recipe.SetCategory(category);

            _reactingReceptContext.Add(recipe);

            await _reactingReceptContext.SaveChangesAsync();
            await _reactingReceptContext.Entry(recipe).ReloadAsync();
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to add recipe: {recipe}");
            return null;
        }

        return recipe;
    }

    public override async Task<Recipe?> UpdateAsync(Recipe updatedRecipe)
    {
        Recipe? currentRecipe = await GetByIdAsync(updatedRecipe.Id);
        Contracts.LogAndThrowWhenNothingWasReceived(currentRecipe);

        try
        {
            await UpdateRecipeIngredientMeasurements(currentRecipe, updatedRecipe);

            currentRecipe.SetName(updatedRecipe.Name);
            currentRecipe.SetInstructions(updatedRecipe.Instructions);
            currentRecipe.SetPortionAmount(updatedRecipe.PortionAmount);
            currentRecipe.SetCategory(updatedRecipe.Category);

            _reactingReceptContext.Entry(currentRecipe).State = EntityState.Modified;

            await _reactingReceptContext.SaveChangesAsync();
            await _reactingReceptContext.Entry(currentRecipe).ReloadAsync();

        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to update recipe: {currentRecipe}");
        }

        return currentRecipe;
    }

    private async Task UpdateRecipeIngredientMeasurements(Recipe currentRecipe, Recipe updatedRecipe)
    {
        for (int index = 0; index < currentRecipe.IngredientMeasurements.Count; ++index)
        {
            IngredientMeasurement ingredientMeasurementInCurrentRecipe = currentRecipe.IngredientMeasurements[index];

            IngredientMeasurement? ingredientMeasurementInUpdatedRecipe = updatedRecipe.IngredientMeasurements
                .FirstOrDefault(ingredientMeasurement => ingredientMeasurement.Id == ingredientMeasurementInCurrentRecipe.Id);

            if (ingredientMeasurementInUpdatedRecipe != null)
            {
                ingredientMeasurementInCurrentRecipe.SetMeasurement(ingredientMeasurementInUpdatedRecipe.Measurement);
                ingredientMeasurementInCurrentRecipe.SetMeasurementUnit(ingredientMeasurementInUpdatedRecipe.MeasurementUnit);
                ingredientMeasurementInCurrentRecipe.SetGrams(ingredientMeasurementInUpdatedRecipe.Grams);
                ingredientMeasurementInCurrentRecipe.SetNote(ingredientMeasurementInUpdatedRecipe.Note);
                ingredientMeasurementInCurrentRecipe.SetSortOrder(ingredientMeasurementInUpdatedRecipe.SortOrder);
                ingredientMeasurementInCurrentRecipe.SetIngredient(ingredientMeasurementInUpdatedRecipe.Ingredient);

                _reactingReceptContext.Entry(ingredientMeasurementInCurrentRecipe).State = EntityState.Modified;
            }
            else
            {
                currentRecipe.IngredientMeasurements.Remove(ingredientMeasurementInCurrentRecipe);
                _reactingReceptContext.Entry(ingredientMeasurementInCurrentRecipe).State = EntityState.Deleted;

                --index;
            }
        }

        foreach (IngredientMeasurement ingredientMeasurementInUpdatedRecipe in updatedRecipe.IngredientMeasurements)
        {
            if (ingredientMeasurementInUpdatedRecipe.Id == Guid.Empty)
            {
                Ingredient ingredient = await _reactingReceptContext.Ingredient
                        .Where(ingredient => ingredient.Id == ingredientMeasurementInUpdatedRecipe.IngredientId)
                        .Include(ingredient => ingredient.Category)
                        .FirstAsync();

                ingredientMeasurementInUpdatedRecipe.SetIngredient(ingredient);

                currentRecipe.IngredientMeasurements.Add(ingredientMeasurementInUpdatedRecipe);
            }
        }
    }
}
