using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public sealed class IngredientMeasurementDto : IIngredientMeasurementDto
{
    public double Measurement { get; private set; }
    public MeasurementUnit MeasurementUnit { get; private set; }
    public double Grams { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public IIngredientDto? Ingredient { get; private set; }
}
