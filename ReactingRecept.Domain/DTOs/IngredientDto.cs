namespace ReactingRecept.Domain;

public sealed class IngredientDto : IIngredientDto
{
    public string Name { get; private set; } = string.Empty;
    public double Fat { get; private set; }
    public double Carbohydrates { get; private set; }
    public double Protein { get; private set; }
    public double Calories { get; private set; }
    public ICategoryDto? Category { get; private set; }
}
