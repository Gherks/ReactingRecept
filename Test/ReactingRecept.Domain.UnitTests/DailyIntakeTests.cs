using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Mocking;
using System;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.UnitTests;

public class DailyIntakeTests
{
    private readonly Ingredient _ingredient;
    private readonly IngredientMeasurement _ingredientMeasurement;
    private readonly Recipe _recipe;

    public DailyIntakeTests()
    {
        Category ingredientCategory = Mocker.MockCategory("Fishy fish", CategoryType.Ingredient, 1);
        Category recipeCategory = Mocker.MockCategory("Soupy soup", CategoryType.Recipe, 1);

        _ingredient = new Ingredient("Tuna", 1.0, 1.0, 1.0, 1.0, ingredientCategory);
        _ingredientMeasurement = new IngredientMeasurement(1.0, MeasurementUnit.Gram, 1.0, "Here is a note", 0, _ingredient);
        _recipe = new Recipe("Tuna Sandwich", "Do it like this!", 2, recipeCategory);
    }

    [Fact]
    public void CanCreateDailyIntake()
    {
        string dailyIntakeName = "Normal day";

        DailyIntake sut = new(dailyIntakeName);

        sut.Should().NotBeNull();
        sut.Id.Should().BeEmpty();
        sut.Name.Should().Be(dailyIntakeName);
        sut.Entries.Should().BeEmpty();
    }

    [Fact]
    public void CannotCreateDailyIntakeWithoutName()
    {
        Exception thrownException = Record.Exception(() => new DailyIntake(string.Empty));

        thrownException.Should().NotBeNull();
    }

    [Fact]
    public void CanChangeNameOnDailyIntake()
    {
        DailyIntake sut = new("Normal day");

        string newDailyIntakeName = "Special day";
        sut.SetName(newDailyIntakeName);

        sut.Name.Should().Be(newDailyIntakeName);
    }

    [Fact]
    public void CanAddRecipeToDailyIntake()
    {
        DailyIntake sut = new("Normal day");

        sut.AddEntry(_recipe);

        sut.Entries.Should().HaveCount(1);
    }

    [Fact]
    public void CanAddIngredientToDailyIntake()
    {
        DailyIntake sut = new("Normal day");

        sut.AddEntry(_ingredient);

        sut.Entries.Should().HaveCount(1);
    }

    [Fact]
    public void CanChangeOrderOfEntriesInDailyIntake()
    {
        DailyIntake sut = new("Normal day");
        sut.AddEntry(_recipe);
        sut.AddEntry(_ingredient);

        sut.ChangeOrderOfEntries(0, 1);

        sut.Entries[0].EntryId.Should().Be(_ingredient.Id);
        sut.Entries[0].Order.Should().Be(0);
        sut.Entries[1].EntryId.Should().Be(_recipe.Id);
        sut.Entries[1].Order.Should().Be(1);
    }

    [Fact]
    public void NothingHappensWhenChangingOrderOfEmptyDailyIntakeEntryList()
    {
        DailyIntake sut = new("Normal day");

        sut.ChangeOrderOfEntries(0, 1);

        sut.Entries.Should().BeEmpty();
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(1, -2)]
    [InlineData(3, 1)]
    [InlineData(1, 2)]
    public void NothingHappensWhenChangingOrderOfEntriesWithInvalidIndices(int firstIndex, int secondIndex)
    {
        DailyIntake sut = new("Normal day");
        sut.AddEntry(_recipe);
        sut.AddEntry(_ingredient);

        sut.ChangeOrderOfEntries(firstIndex, secondIndex);

        sut.Entries[0].EntryId.Should().Be(_recipe.Id);
        sut.Entries[0].Order.Should().Be(0);
        sut.Entries[1].EntryId.Should().Be(_ingredient.Id);
        sut.Entries[1].Order.Should().Be(1);
    }
}
