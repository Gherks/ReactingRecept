using FluentAssertions;
using ReactingRecept.Contract;
using ReactingRecept.Infrastructure.Repositories;
using ReactingRecept.Server.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Infrastructure.IntegrationTests;

public class CategoryRepositoryTests : IClassFixture<TestDatabaseFixture>
{
    public TestDatabaseFixture Fixture { get; }

    public CategoryRepositoryTests(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public async Task CanFetchAllCategoriesOfCertainType()
    {
        CategoryRepository categoryRepository = CreateCategoryRepository();

        IReadOnlyList<Category>? categories = await categoryRepository.ListAllOfTypeAsync(CategoryType.Recipe);

        categories.Should().HaveCount(3);
        categories.Should().Contain(category => category.Name == "Snacks");
        categories.Should().Contain(category => category.Name == "Meal");
        categories.Should().Contain(category => category.Name == "Dessert");
    }

    private CategoryRepository CreateCategoryRepository()
    {
        const string errorMessage = "Failed to create category repository for tests because DbContext has not been set.";
        Contracts.LogAndThrowWhenNull(Fixture.ReactingReceptContext, errorMessage);

        return new CategoryRepository(Fixture.ReactingReceptContext);
    }
}
