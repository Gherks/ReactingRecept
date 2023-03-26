namespace ReactingRecept.Domain;

public interface IIngredientDto
{
    string Name { get; }
    double Fat { get; }
    double Carbohydrates { get; }
    double Protein { get; }
    double Calories { get; }
    ICategoryDto? Category { get; }
}
