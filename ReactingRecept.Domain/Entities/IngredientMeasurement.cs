using ReactingRecept.Domain.Entities.Base;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.Entities;

public sealed class IngredientMeasurement : BaseEntity
{
    public double Measurement { get; private set; }
    public MeasurementUnit MeasurementUnit { get; private set; }
    public double Grams { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public Guid IngredientId { get; private set; }
    public Ingredient? Ingredient { get; private set; }

    private IngredientMeasurement() { }

    public IngredientMeasurement(double measurement, MeasurementUnit measurementUnit, double grams, string note, int sortOrder, Ingredient ingredient)
    {
        if (!ValidateMeasurement(measurement) ||
            !ValidateGrams(grams) ||
            !ValidateNote(note))
        {
            throw new ArgumentException(""); // ??
        }

        Measurement = measurement;
        MeasurementUnit = measurementUnit;
        Grams = grams;
        Note = note;
        SortOrder = sortOrder;
        IngredientId = ingredient.Id;
        Ingredient = ingredient;
    }

    public void SetMeasurement(double measurement)
    {
        if (measurement < 0)
        {
            // Log warning
            return;
        }

        Measurement = measurement;
    }

    public void SetMeasurementUnit(MeasurementUnit measurementUnit)
    {
        MeasurementUnit = measurementUnit;
    }

    public void SetGrams(double grams)
    {
        if (grams < 0)
        {
            // Log warning
            return;
        }

        Grams = grams;
    }

    public void SetNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            // Log Warning
            return;
        }

        Note = note;
    }

    public void SetSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
    }

    private static bool ValidateMeasurement(double measurement)
    {
        return measurement >= double.Epsilon;
    }

    private static bool ValidateGrams(double grams)
    {
        return grams >= double.Epsilon;
    }

    private static bool ValidateNote(string note)
    {
        bool noteIsEmpty = string.IsNullOrWhiteSpace(note);
        bool noteContainsOnlyDigits = double.TryParse(note, out _);

        return !noteIsEmpty && !noteContainsOnlyDigits;
    }
}
