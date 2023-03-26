using FluentAssertions;
using System;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.DomainTests;

public class CategoryTests
{
    [Fact]
    public void CodeInstantiatedDefaultCategoryHasNoId()
    {
        Category sut = new();

        sut.Should().NotBeNull();
        sut.Id.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Dairy", CategoryType.Ingredient, 0)]
    [InlineData("Chicken wings", CategoryType.Recipe, 0)]
    public void CodeInstantiatedCategoryHasNoId(string name, CategoryType categoryType, int sortOrder)
    {
        Category sut = new(name, categoryType, sortOrder);

        sut.Should().NotBeNull();
        sut.Id.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Dairy", CategoryType.Ingredient, 0)]
    [InlineData("Chicken wings", CategoryType.Recipe, 0)]
    public void CodeInstantiatedCategoryIsInvalid(string name, CategoryType categoryType, int sortOrder)
    {
        Category sut = new(name, categoryType, sortOrder);

        sut.Should().NotBeNull();
        sut.IsValid.Should().BeFalse();
    }
}
