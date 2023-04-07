using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.DTOs.Ingredient;

public class IngredientDTO
{
    public string Name { get; private set; } = string.Empty;
    public double Fat { get; private set; }
    public double Carbohydrates { get; private set; }
    public double Protein { get; private set; }
    public double Calories { get; private set; }
    public string CategoryName { get; private set; } = string.Empty;
    public CategoryType CategoryType { get; private set; }

    public IngredientDTO(string name, double fat, double carbohydrates, double protein, double calories, string categoryName, CategoryType categoryType)
    {
        Name = name;
        Fat = fat;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Calories = calories;
        CategoryName = categoryName;
        CategoryType = categoryType;
    }
}
