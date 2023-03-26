using ReactingRecept.Domain.Base;

namespace ReactingRecept.Domain;

public interface IRecipeDto
{
    string Name { get; }
    string Instructions { get; }
    int PortionAmount { get; }
    ICategoryDto? Category { get; }
    IIngredientMeasurementDto[] IngredientMeasurements { get; }
}
