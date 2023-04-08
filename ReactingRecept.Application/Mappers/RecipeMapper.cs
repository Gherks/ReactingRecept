using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Mappers
{
    internal static class RecipeMapper
    {
        public static RecipeDTO MapToDTO(this Recipe recipe)
        {
            Contracts.LogAndThrowWhenNotSet(recipe.Category);

            return new RecipeDTO(
                recipe.Name,
                recipe.Instructions,
                recipe.PortionAmount,
                recipe.Category.Name,
                recipe.Category.Type,
                recipe.IngredientMeasurements.Select(ingredientMeasurement => ingredientMeasurement.MapToDTO()).ToArray());
        }

        public static Recipe MapToDomain(this RecipeDTO recipeDTO, Category[] categories)
        {
            Category? category = categories.FirstOrDefault(category => category.Name == recipeDTO.CategoryName);
            Contracts.LogAndThrowWhenNothingWasReceived(category);

            Recipe recipe = new Recipe(
                recipeDTO.Name,
                recipeDTO.Instructions,
                recipeDTO.PortionAmount,
                category);

            foreach (IngredientMeasurementDTO ingredientMeasurementDTO in recipeDTO.IngredientMeasurementDTOs)
            {
                recipe.AddIngredientMeasurement(ingredientMeasurementDTO.MapToDomain(categories));
            }

            return recipe;
        }
    }
}
