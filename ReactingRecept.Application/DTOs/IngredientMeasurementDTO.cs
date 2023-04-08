using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.DTOs;

public class IngredientMeasurementDTO
{
    public double Measurement { get; private set; }
    public MeasurementUnit MeasurementUnit { get; private set; }
    public double Grams { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public IngredientDTO? IngredientDTO { get; private set; }

    public IngredientMeasurementDTO(double measurement, MeasurementUnit measurementUnit, double grams, string note, int sortOrder, IngredientDTO ingredientDTO)
    {
        Measurement = measurement;
        MeasurementUnit = measurementUnit;
        Grams = grams;
        Note = note;
        SortOrder = sortOrder;
        IngredientDTO = ingredientDTO;
    }
}
