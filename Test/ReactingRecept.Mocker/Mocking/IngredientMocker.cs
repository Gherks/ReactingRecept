using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    public static Ingredient[] MockIngredients(IngredientDTO[] ingredientDTOs)
    {
        return ingredientDTOs.Select(MockIngredient).ToArray();
    }
    public static Ingredient MockIngredient(IngredientDTO ingredientDTO)
    {
        Category category = MockCategory(ingredientDTO.CategoryName, ingredientDTO.CategoryType);

        Ingredient ingredient = new(
            ingredientDTO.Name,
            ingredientDTO.Fat,
            ingredientDTO.Carbohydrates,
            ingredientDTO.Protein,
            ingredientDTO.Calories,
            category);
        MockId(ingredient);

        return ingredient;
    }
}
