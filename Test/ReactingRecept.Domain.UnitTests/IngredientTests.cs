using FluentAssertions;
using ReactingRecept.Domain.Entities;
using System;
using Xunit;

namespace ReactingRecept.Domain.UnitTests;

public class IngredientTests
{
    private const double _zero = 0.0;
    private const double _minusOne = -1.0;

    private readonly Category _category = new();

    [Fact]
    public void CanCreateIngredient()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.Should().NotBeNull();
        sut.Id.Should().BeEmpty();
        sut.Name.Should().Be("Tuna");
        sut.Fat.Should().Be(1.0);
        sut.Carbohydrates.Should().Be(1.0);
        sut.Protein.Should().Be(1.0);
        sut.Calories.Should().Be(1.0);
        sut.CategoryId.Should().Be(_category.Id);
        sut.Category.Should().Be(_category);
    }

    [Theory]
    [InlineData("", 1.0, 1.0, 1.0, 1.0)]
    [InlineData("123", 1.0, 1.0, 1.0, 1.0)]
    [InlineData("123.456", 1.0, 1.0, 1.0, 1.0)]
    [InlineData("Tuna", _minusOne, 1.0, 1.0, 1.0)]
    [InlineData("Tuna", _zero, 1.0, 1.0, 1.0)]
    [InlineData("Tuna", 1.0, _minusOne, 1.0, 1.0)]
    [InlineData("Tuna", 1.0, _zero, 1.0, 1.0)]
    [InlineData("Tuna", 1.0, 1.0, _minusOne, 1.0)]
    [InlineData("Tuna", 1.0, 1.0, _zero, 1.0)]
    [InlineData("Tuna", 1.0, 1.0, 1.0, _minusOne)]
    [InlineData("Tuna", 1.0, 1.0, 1.0, _zero)]
    public void CannotCreateIngredientWithFaultyArguments(string name, double fat, double carbohydrates, double protein, double calories)
    {
        Exception thrownException = Record.Exception(() => new Ingredient(name, fat, carbohydrates, protein, calories, _category));

        thrownException.Should().NotBeNull();
    }

    [Fact]
    public void CannotSetNameToEmptyString()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetName(string.Empty);

        sut.Name.Should().Be("Tuna");
    }

    [Fact]
    public void CannotSetNameToOnlyWhitespace()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetName(" ");

        sut.Name.Should().Be("Tuna");
    }

    [Fact]
    public void CannotSetNameToOnlyDigits()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetName("123");

        sut.Name.Should().Be("Tuna");
    }

    [Fact]
    public void CanUpdateFat()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        double newValue = 5.0;
        sut.SetFat(newValue);

        sut.Fat.Should().Be(newValue);
    }

    [Fact]
    public void CanUpdateCarbohydrate()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        double newValue = 5.0;
        sut.SetCarbohydrates(newValue);

        sut.Carbohydrates.Should().Be(newValue);
    }

    [Fact]
    public void CanUpdateProtein()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        double newValue = 5.0;
        sut.SetProtein(newValue);

        sut.Protein.Should().Be(newValue);
    }

    [Fact]
    public void CanUpdateCalories()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        double newValue = 5.0;
        sut.SetCalories(newValue);

        sut.Calories.Should().Be(newValue);
    }

    [Fact]
    public void CannotSetFatToLessThanZero()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetFat(-1.0);

        sut.Fat.Should().Be(1.0);
    }

    [Fact]
    public void CannotSetCarbohydrateToLessThanZero()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetCarbohydrates(-1.0);

        sut.Carbohydrates.Should().Be(1.0);
    }

    [Fact]
    public void CannotSetProteinToLessThanZero()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetProtein(-1.0);

        sut.Protein.Should().Be(1.0);
    }

    [Fact]
    public void CannotSetCaloriesToLessThanZero()
    {
        Ingredient sut = new("Tuna", 1.0, 1.0, 1.0, 1.0, _category);

        sut.SetCalories(-1.0);

        sut.Calories.Should().Be(1.0);
    }
}
