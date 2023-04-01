using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReactingRecept.Persistence.IntegrationTests;

public class IngredientRepositoryTests : IDisposable
{
    private readonly TestFramework _testFramework = new();

    private readonly string _nonexistingIngredientName = "Butter";

    public void Dispose()
    {
        _testFramework.Dispose();
    }

    [Fact]
    public async Task CanAcknowledgeExistanceOfIngredientByName()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllIngredients);

        bool categoryFound = await ingredientRepository.AnyAsync(_testFramework.AllIngredients[0].Name);

        categoryFound.Should().BeTrue();
    }

    [Fact]
    public async Task CannotAcknowledgeExistanceOfNonexistingIngredientByName()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();

        bool categoryFound = await ingredientRepository.AnyAsync(_nonexistingIngredientName);

        categoryFound.Should().BeFalse();
    }

    [Fact]
    public async Task CanGetIngredientById()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllIngredients);
        Ingredient addedIngredient = _testFramework.AllIngredients[0];

        Ingredient? ingredient = await ingredientRepository.GetByIdAsync(addedIngredient.Id);

        ingredient?.Id.Should().Be(addedIngredient.Id);
        ingredient?.Name.Should().Be(addedIngredient.Name);
        ingredient?.Fat.Should().Be(addedIngredient.Fat);
        ingredient?.Carbohydrates.Should().Be(addedIngredient.Carbohydrates);
        ingredient?.Protein.Should().Be(addedIngredient.Protein);
        ingredient?.Calories.Should().Be(addedIngredient.Calories);
        ingredient?.CategoryId.Should().Be(addedIngredient.CategoryId);
        ingredient?.Category.Should().Be(addedIngredient.Category);
    }

    [Fact]
    public async Task CannotGetNonexistingIngredientById()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();

        Ingredient? ingredient = await ingredientRepository.GetByIdAsync(Guid.NewGuid());

        ingredient.Should().BeNull();
    }

    [Fact]
    public async Task CanGetIngredientByName()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllIngredients);

        Ingredient? ingredient = await ingredientRepository.GetByNameAsync(_testFramework.AllIngredients[0].Name);

        ingredient?.Id.Should().NotBeEmpty();
        ingredient?.Name.Should().Be(_testFramework.AllIngredients[0].Name);
        ingredient?.Fat.Should().BePositive();
        ingredient?.Carbohydrates.Should().BePositive();
        ingredient?.Protein.Should().BePositive();
        ingredient?.Calories.Should().BePositive();
        ingredient?.CategoryId.Should().NotBeEmpty();
        ingredient?.Category.Should().NotBeNull();
    }

    [Fact]
    public async Task CannotGetNonexistingIngredientByName()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();

        Ingredient? ingredient = await ingredientRepository.GetByNameAsync(_nonexistingIngredientName);

        ingredient.Should().BeNull();
    }

    [Fact]
    public async Task CanGetAllIngredients()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllIngredients);

        Ingredient[]? ingredients = await ingredientRepository.GetAllAsync();

        ingredients.Should().HaveCount(_testFramework.AllIngredients.Length);

        foreach (Ingredient addedIngredient in _testFramework.AllIngredients)
        {
            ingredients.Should().Contain(ingredient => ingredient.Name == addedIngredient.Name);
        }
    }

    [Fact]
    public async Task CanAddIngredient()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Ingredient? initialIngredient = _testFramework.CreateIngredient();

        Ingredient? addedIngredient = await ingredientRepository.AddAsync(_testFramework.CreateIngredient());

        addedIngredient?.Id.Should().NotBeEmpty();
        addedIngredient?.Name.Should().Be(initialIngredient.Name);
        addedIngredient?.Fat.Should().Be(initialIngredient.Fat);
        addedIngredient?.Carbohydrates.Should().Be(initialIngredient.Carbohydrates);
        addedIngredient?.Protein.Should().Be(initialIngredient.Protein);
        addedIngredient?.Calories.Should().Be(initialIngredient.Calories);
        addedIngredient?.CategoryId.Should().Be(initialIngredient.CategoryId);
        addedIngredient?.Category.Should().Be(initialIngredient.Category);
    }

    [Fact]
    public async Task AddingIngredientWithSameNameAndCategoryAsExistingIngredientReturnsExistingIngredient()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Ingredient firstIngredient = new("Beef", 1, 1, 1, 1, _testFramework.AllCategories[0]);
        Ingredient secondIngredient = new("Beef", 2, 2, 2, 2, _testFramework.AllCategories[0]);

        Ingredient? firstAddedIngredient = await ingredientRepository.AddAsync(firstIngredient);
        Ingredient? secondAddedIngredient = await ingredientRepository.AddAsync(secondIngredient);

        Contracts.LogAndThrowWhenNotSet(firstAddedIngredient);
        secondAddedIngredient?.Id.Should().Be(firstAddedIngredient.Id);
    }


    [Fact]
    public async Task AddingIngredientWithSameNameButDifferingCategoryAsExistingIngredientReturnsNewIngredient()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Ingredient firstIngredient = new("Beef", 1, 1, 1, 1, _testFramework.AllCategories[0]);
        Ingredient secondIngredient = new("Beef", 2, 2, 2, 2, _testFramework.AllCategories[1]);

        Ingredient? firstAddedIngredient = await ingredientRepository.AddAsync(firstIngredient);
        Ingredient? secondAddedIngredient = await ingredientRepository.AddAsync(secondIngredient);

        Contracts.LogAndThrowWhenNotSet(firstAddedIngredient);
        secondAddedIngredient?.Id.Should().NotBeEmpty();
        secondAddedIngredient?.Id.Should().NotBe(firstAddedIngredient.Id);
        secondAddedIngredient?.Name.Should().Be(secondIngredient.Name);
        secondAddedIngredient?.Fat.Should().Be(secondIngredient.Fat);
        secondAddedIngredient?.Carbohydrates.Should().Be(secondIngredient.Carbohydrates);
        secondAddedIngredient?.Protein.Should().Be(secondIngredient.Protein);
        secondAddedIngredient?.Calories.Should().Be(secondIngredient.Calories);
        secondAddedIngredient?.CategoryId.Should().Be(secondIngredient.CategoryId);
        secondAddedIngredient?.Category.Should().Be(secondIngredient.Category);
    }

    [Fact]
    public async Task CanAddManyIngredients()
    {
        IngredientRepository ingredientRepository = await IngredientRepositoryTestSetup();
        Ingredient[] initialIngredients = _testFramework.CreateIngredients();

        Ingredient[]? addedIngredients = await ingredientRepository.AddManyAsync(_testFramework.CreateIngredients());
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

        addedIngredients.Should().HaveCount(initialIngredients.Length);
        foreach (Ingredient initialIngredient in initialIngredients)
        {
            Ingredient? foundIngredient = addedIngredients.FirstOrDefault(addedIngredient => addedIngredient.Name == initialIngredient.Name);

            foundIngredient?.Id.Should().NotBeEmpty();
            foundIngredient?.Name.Should().Be(initialIngredient.Name);
            foundIngredient?.Fat.Should().Be(initialIngredient.Fat);
            foundIngredient?.Carbohydrates.Should().Be(initialIngredient.Carbohydrates);
            foundIngredient?.Protein.Should().Be(initialIngredient.Protein);
            foundIngredient?.Calories.Should().Be(initialIngredient.Calories);
            foundIngredient?.CategoryId.Should().Be(initialIngredient.CategoryId);
            foundIngredient?.Category.Should().Be(initialIngredient.Category);
        }
    }

    private async Task<IngredientRepository> IngredientRepositoryTestSetup()
    {
        await _testFramework.PrepareCategoryRepository();
        return await _testFramework.PrepareIngredientRepository();
    }
}
