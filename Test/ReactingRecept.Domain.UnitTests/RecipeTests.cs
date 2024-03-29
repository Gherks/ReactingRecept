using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Mocking;
using System;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.UnitTests;

public class RecipeTests
{
    private readonly Category _recipeCategory;
    private readonly Ingredient _ingredient;
    private readonly IngredientMeasurement _ingredientMeasurement;

    public RecipeTests()
    {
        Category ingredientCategory = Mocker.MockCategory("Fishy", CategoryType.Ingredient, 1);
        _recipeCategory = Mocker.MockCategory("Fishy soup", CategoryType.Recipe, 1);
        _ingredient = new Ingredient("Tuna", 1.0, 1.0, 1.0, 1.0, ingredientCategory);
        _ingredientMeasurement = new IngredientMeasurement(1.0, MeasurementUnit.Gram, 1.0, "Here is a note", 0, _ingredient);
    }

    [Fact]
    public void CanCreateRecipe()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.Should().NotBeNull();
        sut.Id.Should().BeEmpty();
        sut.Name.Should().Be("Tuna Sandwich");
        sut.Instructions.Should().Be("Do it like this!");
        sut.PortionAmount.Should().Be(2);
        sut.CategoryId.Should().Be(_recipeCategory.Id);
        sut.Category.Should().Be(_recipeCategory);
    }

    [Theory]
    [InlineData("", "Do it like this!", 2)]
    [InlineData("123", "Do it like this!", 2)]
    [InlineData("123.456", "Do it like this!", 2)]
    [InlineData("Tuna Sandwich", "", 2)]
    [InlineData("Tuna Sandwich", "Do it like this!", -1)]
    [InlineData("Tuna Sandwich", "Do it like this!", 0)]
    public void CannotCreateRecipeWithFaultyArguments(string name, string instructions, int portionAmount)
    {
        Exception thrownException = Record.Exception(() => new Recipe(name, instructions, portionAmount, _recipeCategory));

        thrownException.Should().NotBeNull();
    }

    [Fact]
    public void CannotCreateRecipeWithIngredientCategoryType()
    {
        Category category = Mocker.MockCategory("Fishy fish", CategoryType.Ingredient, 1);

        Exception thrownException = Record.Exception(() => new Recipe("RecipeName", "Recipe instructions", 1, category));

        thrownException.Should().NotBeNull();
    }

    [Fact]
    public void CanAddIngredientMeasurement()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.AddIngredientMeasurement(_ingredientMeasurement);

        sut.IngredientMeasurements.Should().HaveCount(1);
    }

    [Fact]
    public void IngredientsWithinIngredientMeasurementListHasToBeUniqueWithinRecipe()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.AddIngredientMeasurement(_ingredientMeasurement);
        sut.AddIngredientMeasurement(_ingredientMeasurement);

        sut.IngredientMeasurements.Should().HaveCount(1);
    }

    [Fact]
    public void CanRemoveIngredientMeasurementById()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.AddIngredientMeasurement(_ingredientMeasurement);
        bool result = sut.RemoveIngredientMeasurement(_ingredientMeasurement.Id);

        sut.IngredientMeasurements.Should().BeEmpty();
        result.Should().BeTrue();
    }

    [Fact]
    public void CanUpdateName()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.SetName("Super Tuna Sandwich");

        sut.Name.Should().Be("Super Tuna Sandwich");
    }

    [Fact]
    public void CannotSetNameToEmptyString()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.SetName(string.Empty);

        sut.Name.Should().Be("Tuna Sandwich");
    }

    [Fact]
    public void CanUpdatePortionAmount()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.SetPortionAmount(4);

        sut.PortionAmount.Should().Be(4);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void CannotSetPortionAmountToLessThanOne(int portionAmount)
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.SetPortionAmount(portionAmount);

        sut.PortionAmount.Should().Be(2);
    }

    [Fact]
    public void CanUpdateCategory()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);
        Category category = Mocker.MockCategory("Some other category", CategoryType.Recipe, 1);

        sut.SetCategory(category);

        sut.Category.Should().Be(category);
    }

    [Fact]
    public void CannotUpdateCategoryToIngredientType()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);
        Category category = Mocker.MockCategory("Some other category", CategoryType.Ingredient, 1);

        sut.SetCategory(category);

        sut.Category.Should().Be(category);
    }

    [Fact]
    public void CanUpdateInstructions()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        string updatedInstructions = "No, do it like this instead!";
        sut.SetInstructions(updatedInstructions);

        sut.Instructions.Should().Be(updatedInstructions);
    }

    [Fact]
    public void CannotSetInstructionsToEmptyString()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);

        sut.SetInstructions(string.Empty);

        sut.Instructions.Should().Be("Do it like this!");
    }

    [Fact]
    public void CanCalculateRecipeFatAmount()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);
        int grams = 10;
        sut.AddIngredientMeasurement(new IngredientMeasurement(grams, MeasurementUnit.Gram, grams, "Here is a note", 0, _ingredient));

        double amount = sut.GetFatAmount();

        amount.Should().Be(_ingredient.Fat * grams * 0.01);
    }

    [Fact]
    public void CanCalculateRecipeCarbohydrateAmount()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);
        int grams = 10;
        sut.AddIngredientMeasurement(new IngredientMeasurement(grams, MeasurementUnit.Gram, grams, "Here is a note", 0, _ingredient));

        double amount = sut.GetCarbohydrateAmount();

        amount.Should().Be(_ingredient.Carbohydrates * grams * 0.01);
    }

    [Fact]
    public void CanCalculateRecipeProteinAmount()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);
        int grams = 10;
        sut.AddIngredientMeasurement(new IngredientMeasurement(grams, MeasurementUnit.Gram, grams, "Here is a note", 0, _ingredient));

        double amount = sut.GetProteinAmount();

        amount.Should().Be(_ingredient.Protein * grams * 0.01);
    }

    [Fact]
    public void CanCalculateRecipeCalorieAmount()
    {
        Recipe sut = new("Tuna Sandwich", "Do it like this!", 2, _recipeCategory);
        int grams = 10;
        sut.AddIngredientMeasurement(new IngredientMeasurement(grams, MeasurementUnit.Gram, grams, "Here is a note", 0, _ingredient));

        double amount = sut.GetCalorieAmount();

        amount.Should().Be(_ingredient.Calories * grams * 0.01);
    }
}
