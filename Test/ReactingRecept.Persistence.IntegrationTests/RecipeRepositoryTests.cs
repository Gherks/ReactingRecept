using FluentAssertions;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

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


    //Task<bool> AnyAsync(string name)
    //Task<Recipe?> GetByIdAsync(Guid id)
    //Task<Recipe?> GetByNameAsync(string name)
    //Task<Recipe[]?> GetAllAsync()
    //Task<Recipe?> AddAsync(Recipe recipe)
    //Task<Recipe?> UpdateAsync(Recipe updatedRecipe)

    private async Task<RecipeRepository> RecipeRepositoryTestSetup()
    {
        await _testFramework.PrepareCategoryRepository();
        await _testFramework.PrepareIngredientRepository();
        return await _testFramework.PrepareRecipeRepository();
    }
}
