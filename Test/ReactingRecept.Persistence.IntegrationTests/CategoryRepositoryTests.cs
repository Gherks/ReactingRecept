using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.IntegrationTests;

public class CategoryRepositoryTests : IDisposable
{
    private readonly TestFramework _testFramework = new();

    public void Dispose()
    {
        _testFramework.Dispose();
    }

    [Fact]
    public async Task CanFetchAllRecipeCategories()
    {
        CategoryRepository categoryRepository = await _testFramework.PrepareCategoryRepository();

        Category[]? categories = await categoryRepository.GetManyOfTypeAsync(CategoryType.Recipe);

        categories.Should().HaveCount(3);
        categories.Should().Contain(category => category.Name == "Snacks");
        categories.Should().Contain(category => category.Name == "Meal");
        categories.Should().Contain(category => category.Name == "Dessert");
    }

    [Fact]
    public async Task CanFetchAllIngredientCategories()
    {
        CategoryRepository categoryRepository = await _testFramework.PrepareCategoryRepository();

        Category[]? categories = await categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);

        categories.Should().HaveCount(5);
        categories.Should().Contain(category => category.Name == "Dairy");
        categories.Should().Contain(category => category.Name == "Pantry");
        categories.Should().Contain(category => category.Name == "Vegetables");
        categories.Should().Contain(category => category.Name == "Meat");
        categories.Should().Contain(category => category.Name == "Other");
    }
}
