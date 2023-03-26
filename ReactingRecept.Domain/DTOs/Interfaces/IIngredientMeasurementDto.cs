using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public interface IIngredientMeasurementDto
{
    double Measurement { get; }
    MeasurementUnit MeasurementUnit { get; }
    double Grams { get; }
    string Note { get; }
    int SortOrder { get; }
    IIngredientDto? Ingredient { get; }
}
