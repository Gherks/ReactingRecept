using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.DTOs;

public class RecipeDTO
{
    public string Name { get; private set; } = string.Empty;
    public string Instructions { get; private set; } = string.Empty;
    public int PortionAmount { get; private set; }
    public string CategoryName { get; private set; } = string.Empty;
    public CategoryType CategoryType { get; private set; }
    public IngredientMeasurementDTO[] IngredientMeasurementDTOs { get; private set; } = Array.Empty<IngredientMeasurementDTO>();

    public RecipeDTO(string name, string instructions, int portionAmount, string categoryName, CategoryType categoryType, IngredientMeasurementDTO[] ingredientMeasurementDTOs)
    {
        Name = name;
        Instructions = instructions;
        PortionAmount = portionAmount;
        CategoryName = categoryName;
        CategoryType = categoryType;
        IngredientMeasurementDTOs = ingredientMeasurementDTOs;
    }
}
