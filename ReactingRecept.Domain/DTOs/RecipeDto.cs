namespace ReactingRecept.Domain;

public sealed class RecipeDto : IRecipeDto
{
    public string Name { get; private set; } = string.Empty;
    public string Instructions { get; private set; } = string.Empty;
    public int PortionAmount { get; private set; }
    public ICategoryDto? Category { get; private set; }
    public IIngredientMeasurementDto[] IngredientMeasurements { get; private set; } = Array.Empty<IngredientMeasurementDto>();
}
