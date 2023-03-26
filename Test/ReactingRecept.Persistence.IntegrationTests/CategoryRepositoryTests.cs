using FluentAssertions;
using ReactingRecept.Domain;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.IntegrationTests;

public class CategoryRepositoryTests : IDisposable
{
    private readonly ReactingReceptContext _context;

    public CategoryRepositoryTests()
    {
        _context = TestDatabaseCreator.Create();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task CanFetchAllRecipeCategories()
    {
        CategoryRepository categoryRepository = CreateCategoryRepository();

        Category[]? categories = await categoryRepository.ListAllOfTypeAsync(CategoryType.Recipe);

        categories.Should().HaveCount(3);
        categories.Should().Contain(category => category.Name == "Snacks");
        categories.Should().Contain(category => category.Name == "Meal");
        categories.Should().Contain(category => category.Name == "Dessert");
        categories?.Select(category => category.IsValid.Should().BeTrue());
    }

    [Fact]
    public async Task CanFetchAllIngredientCategories()
    {
        CategoryRepository categoryRepository = CreateCategoryRepository();

        Category[]? categories = await categoryRepository.ListAllOfTypeAsync(CategoryType.Ingredient);

        categories.Should().HaveCount(5);
        categories.Should().Contain(category => category.Name == "Dairy");
        categories.Should().Contain(category => category.Name == "Pantry");
        categories.Should().Contain(category => category.Name == "Vegetables");
        categories.Should().Contain(category => category.Name == "Meat");
        categories.Should().Contain(category => category.Name == "Other");
        categories?.Select(category => category.IsValid.Should().BeTrue());
    }

    private CategoryRepository CreateCategoryRepository()
    {
        return new CategoryRepository(_context);
    }
}
