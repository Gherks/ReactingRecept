using FluentAssertions;
using ReactingRecept.Domain.Entities;
using System;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.UnitTests;

public class IngredientMeasurementTests
{
    private readonly Category _category = Mocker.MockCategory("Sauce", CategoryType.Ingredient, 1);
    private readonly Ingredient _ingredient = new("Something yummy", 1, 1, 1, 1, Mocker.MockCategory("Casserole", CategoryType.Recipe, 1));

    [Fact]
    public void CanCreateIngredientMeasurement()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        sut.Should().NotBeNull();
        sut.Id.Should().BeEmpty();
        sut.Measurement.Should().Be(1.0);
        sut.MeasurementUnit.Should().Be(MeasurementUnit.Gram);
        sut.Grams.Should().Be(1.0);
        sut.Note.Should().Be("Here is a hint");
        sut.SortOrder.Should().Be(0);
        sut.IngredientId.Should().Be(_ingredient.Id);
        sut.Ingredient.Should().Be(_ingredient);
    }

    [Theory]
    [InlineData(-1.0, 1.0, "Here is a note")]
    [InlineData(0.0, 1.0, "Here is a note")]
    [InlineData(1.0, -1.0, "Here is a note")]
    [InlineData(1.0, 0.0, "Here is a note")]
    [InlineData(1.0, 1.0, "")]
    [InlineData(1.0, 1.0, "123")]
    [InlineData(1.0, 1.0, "123.456")]
    public void CannotCreateIngredientMeasurementWithFaultyArguments(double measurement, double grams, string note)
    {
        Exception thrownException = Record.Exception(() => new IngredientMeasurement(measurement, MeasurementUnit.Gram, grams, note, 0, _ingredient));

        thrownException.Should().NotBeNull();
    }

    [Fact]
    public void CanUpdateMeasurement()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        double newMeasurement = 50.0;

        sut.SetMeasurement(newMeasurement);

        sut.Measurement.Should().Be(newMeasurement);
    }

    [Fact]
    public void CanUpdateMeasurementUnit()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        MeasurementUnit measurementUnit = MeasurementUnit.Kilogram;

        sut.SetMeasurementUnit(measurementUnit);

        sut.MeasurementUnit.Should().Be(measurementUnit);
    }

    [Fact]
    public void CanUpdateGrams()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        double newGrams = 50.0;

        sut.SetGrams(newGrams);

        sut.Grams.Should().Be(newGrams);
    }

    [Fact]
    public void CanUpdateNote()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        sut.SetNote("Updated note");

        sut.Note.Should().Be("Updated note");
    }

    [Fact]
    public void CanUpdateSortOrder()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        int newSortOrder = 1;

        sut.SetSortOrder(newSortOrder);

        sut.SortOrder.Should().Be(newSortOrder);
    }

    [Fact]
    public void CannotSetMeasurementToLessThanZero()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        sut.SetMeasurement(-1.0);

        sut.Measurement.Should().Be(1.0);
    }

    [Fact]
    public void CannotSetGramsToLessThanZero()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        sut.SetGrams(-1.0);

        sut.Grams.Should().Be(1.0);
    }

    [Fact]
    public void CannotSetNoteToEmptyString()
    {
        IngredientMeasurement sut = new(1.0, MeasurementUnit.Gram, 1.0, "Here is a hint", 0, _ingredient);

        sut.SetNote("");

        sut.Note.Should().Be("Here is a hint");
    }
}
