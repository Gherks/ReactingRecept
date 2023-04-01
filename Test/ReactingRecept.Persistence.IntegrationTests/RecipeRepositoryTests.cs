using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.IntegrationTests;

public class RecipeRepositoryTests : IDisposable
{
    private readonly TestFramework _testFramework = new();

    private readonly string _nonexistingIngredientName = "Mushroom";

    public void Dispose()
    {
        _testFramework.Dispose();
    }

    [Fact]
    public async Task CanAcknowledgeExistanceOfIngredientByName()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllRecipes);

        bool recipeFound = await recipeRepository.AnyAsync(_testFramework.AllRecipes[0].Name);

        recipeFound.Should().BeTrue();
    }

    [Fact]
    public async Task CannotAcknowledgeExistanceOfNonexistingIngredientByName()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();

        bool recipeFound = await recipeRepository.AnyAsync(_nonexistingIngredientName);

        recipeFound.Should().BeFalse();
    }

    [Fact]
    public async Task CanGetRecipeById()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllRecipes);
        Recipe addedRecipe = _testFramework.AllRecipes[0];

        Recipe? recipe = await recipeRepository.GetByIdAsync(addedRecipe.Id);

        recipe?.Id.Should().Be(addedRecipe.Id);
        recipe?.Name.Should().Be(addedRecipe.Name);
        recipe?.Instructions.Should().Be(addedRecipe.Instructions);
        recipe?.PortionAmount.Should().Be(addedRecipe.PortionAmount);
        recipe?.CategoryId.Should().Be(addedRecipe.CategoryId);
        recipe?.Category.Should().Be(addedRecipe.Category);

        for (int index = 0; index < recipe?.IngredientMeasurements.Count; ++index)
        {
            recipe.IngredientMeasurements[index].Id.Should().Be(addedRecipe.IngredientMeasurements[index].Id);
            recipe.IngredientMeasurements[index].Measurement.Should().Be(addedRecipe.IngredientMeasurements[index].Measurement);
            recipe.IngredientMeasurements[index].MeasurementUnit.Should().Be(addedRecipe.IngredientMeasurements[index].MeasurementUnit);
            recipe.IngredientMeasurements[index].Grams.Should().Be(addedRecipe.IngredientMeasurements[index].Grams);
            recipe.IngredientMeasurements[index].Note.Should().Be(addedRecipe.IngredientMeasurements[index].Note);
            recipe.IngredientMeasurements[index].SortOrder.Should().Be(index);
            recipe.IngredientMeasurements[index].IngredientId.Should().Be(addedRecipe.IngredientMeasurements[index].IngredientId);
            recipe.IngredientMeasurements[index].Ingredient.Should().Be(addedRecipe.IngredientMeasurements[index].Ingredient);
        }
    }

    [Fact]
    public async Task CannotGetNonexistingRecipeById()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();

        Recipe? recipe = await recipeRepository.GetByIdAsync(Guid.NewGuid());

        recipe.Should().BeNull();
    }

    [Fact]
    public async Task CanGetRecipeByName()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllRecipes);
        Recipe addedRecipe = _testFramework.AllRecipes[0];

        Recipe? recipe = await recipeRepository.GetByNameAsync(addedRecipe.Name);

        recipe?.Id.Should().NotBeEmpty();
        recipe?.Name.Should().Be(_testFramework.AllRecipes[0].Name);
        recipe?.Instructions.Should().NotBeNullOrWhiteSpace();
        recipe?.PortionAmount.Should().BePositive();
        recipe?.CategoryId.Should().NotBeEmpty();
        recipe?.Category.Should().NotBeNull();

        for (int index = 0; index < recipe?.IngredientMeasurements.Count; ++index)
        {
            recipe.IngredientMeasurements[index].Id.Should().Be(addedRecipe.IngredientMeasurements[index].Id);
            recipe.IngredientMeasurements[index].Measurement.Should().Be(addedRecipe.IngredientMeasurements[index].Measurement);
            recipe.IngredientMeasurements[index].MeasurementUnit.Should().Be(addedRecipe.IngredientMeasurements[index].MeasurementUnit);
            recipe.IngredientMeasurements[index].Grams.Should().Be(addedRecipe.IngredientMeasurements[index].Grams);
            recipe.IngredientMeasurements[index].Note.Should().Be(addedRecipe.IngredientMeasurements[index].Note);
            recipe.IngredientMeasurements[index].SortOrder.Should().Be(index);
            recipe.IngredientMeasurements[index].IngredientId.Should().Be(addedRecipe.IngredientMeasurements[index].IngredientId);
            recipe.IngredientMeasurements[index].Ingredient.Should().Be(addedRecipe.IngredientMeasurements[index].Ingredient);
        }
    }

    [Fact]
    public async Task CannotGetNonexistingRecipeByName()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();

        Recipe? recipe = await recipeRepository.GetByNameAsync(_nonexistingIngredientName);

        recipe.Should().BeNull();
    }

    [Fact]
    public async Task CanGetAllRecipess()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllRecipes);

        Recipe[]? recipes = await recipeRepository.GetAllAsync();

        recipes.Should().HaveCount(_testFramework.AllRecipes.Length);

        foreach (Recipe addedRecipe in _testFramework.AllRecipes)
        {
            recipes.Should().Contain(recipe => recipe.Name == addedRecipe.Name);
        }
    }

    [Fact]
    public async Task AddingRecipeWithSameNameAndCategoryAsExistingRecipeReturnsExistingRecipe()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Recipe firstRecipe = new Recipe("Beef soup", "Instructions", 1, _testFramework.AllCategories[0]);
        Recipe secondRecipe = new Recipe("Beef soup", "Instructions", 1, _testFramework.AllCategories[0]);

        Recipe? firstAddedRecipe = await recipeRepository.AddAsync(firstRecipe);
        Recipe? secondAddedIngredient = await recipeRepository.AddAsync(secondRecipe);

        Contracts.LogAndThrowWhenNotSet(firstAddedRecipe);
        secondAddedIngredient?.Id.Should().Be(firstAddedRecipe.Id);
    }

    [Fact]
    public async Task AddingRecipeWithSameNameButDifferingCategoryAsExistingRecipeReturnsNewRecipe()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Recipe firstRecipe = new Recipe("Beef soup", "Instructions", 1, _testFramework.AllCategories[0]);
        Recipe secondRecipe = new Recipe("Beef soup", "Instructions", 1, _testFramework.AllCategories[1]);

        Recipe? firstAddedRecipe = await recipeRepository.AddAsync(firstRecipe);
        Recipe? secondAddedRecipe = await recipeRepository.AddAsync(secondRecipe);

        Contracts.LogAndThrowWhenNotSet(firstAddedRecipe);
        secondAddedRecipe?.Id.Should().NotBeEmpty();
        secondAddedRecipe?.Id.Should().NotBe(firstAddedRecipe.Id);
        secondAddedRecipe?.Name.Should().Be(secondRecipe.Name);
        secondAddedRecipe?.Instructions.Should().Be(secondRecipe.Instructions);
        secondAddedRecipe?.PortionAmount.Should().Be(secondRecipe.PortionAmount);
        secondAddedRecipe?.CategoryId.Should().Be(secondRecipe.CategoryId);
        secondAddedRecipe?.Category.Should().Be(secondRecipe.Category);
    }

    [Fact]
    public async Task CanAddManyIngredients()
    {
        RecipeRepository recipeRepository = await RecipeRepositoryTestSetup();
        Recipe[] initialRecipes = _testFramework.CreateRecipes();

        Recipe[]? addedRecipes = await recipeRepository.AddManyAsync(_testFramework.CreateRecipes());
        Contracts.LogAndThrowWhenNothingWasReceived(addedRecipes);

        addedRecipes.Should().HaveCount(initialRecipes.Length);
        foreach (Recipe initialRecipe in initialRecipes)
        {
            Recipe? foundIngredient = addedRecipes.FirstOrDefault(addedRecipe => addedRecipe.Name == initialRecipe.Name);

            foundIngredient?.Id.Should().NotBeEmpty();
            foundIngredient?.Name.Should().Be(initialRecipe.Name);
            foundIngredient?.Instructions.Should().Be(initialRecipe.Instructions);
            foundIngredient?.PortionAmount.Should().Be(initialRecipe.PortionAmount);
            foundIngredient?.CategoryId.Should().Be(initialRecipe.CategoryId);
            foundIngredient?.Category.Should().Be(initialRecipe.Category);

            for (int index = 0; index < foundIngredient?.IngredientMeasurements.Count; ++index)
            {
                foundIngredient.IngredientMeasurements[index].Id.Should().Be(initialRecipe.IngredientMeasurements[index].Id);
                foundIngredient.IngredientMeasurements[index].Measurement.Should().Be(initialRecipe.IngredientMeasurements[index].Measurement);
                foundIngredient.IngredientMeasurements[index].MeasurementUnit.Should().Be(initialRecipe.IngredientMeasurements[index].MeasurementUnit);
                foundIngredient.IngredientMeasurements[index].Grams.Should().Be(initialRecipe.IngredientMeasurements[index].Grams);
                foundIngredient.IngredientMeasurements[index].Note.Should().Be(initialRecipe.IngredientMeasurements[index].Note);
                foundIngredient.IngredientMeasurements[index].SortOrder.Should().Be(index);
                foundIngredient.IngredientMeasurements[index].IngredientId.Should().Be(initialRecipe.IngredientMeasurements[index].IngredientId);
                foundIngredient.IngredientMeasurements[index].Ingredient.Should().Be(initialRecipe.IngredientMeasurements[index].Ingredient);
            }
        }
    }


    //Task<Recipe?> UpdateAsync(Recipe updatedRecipe)

    private async Task<RecipeRepository> RecipeRepositoryTestSetup()
    {
        await _testFramework.PrepareCategoryRepository();
        await _testFramework.PrepareIngredientRepository();
        return await _testFramework.PrepareRecipeRepository();
    }
}
