using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    public static Recipe MockRecipe(RecipeDTO recipeDTO)
    {
        Category category = MockCategory(recipeDTO.CategoryName, recipeDTO.CategoryType);

        Recipe recipe = new(
            recipeDTO.Name,
            recipeDTO.Instructions,
            recipeDTO.PortionAmount,
            category);
        MockId(recipe);

        foreach (IngredientMeasurementDTO ingredientMeasurementDTO in recipeDTO.IngredientMeasurementDTOs)
        {
            IngredientMeasurement ingredientMeasurement = MockIngredientMeasurement(ingredientMeasurementDTO);
            recipe.AddIngredientMeasurement(ingredientMeasurement);
        }

        return recipe;
    }
}
