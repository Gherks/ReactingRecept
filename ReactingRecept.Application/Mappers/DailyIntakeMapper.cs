using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Domain.Entities.Base;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Mappers
{
    internal static class DailyIntakeMapper
    {
        public static DailyIntakeDTO MapToDTO(this DailyIntake dailyIntake, Ingredient[] ingredients, Recipe[] recipes)
        {
            return new DailyIntakeDTO(
                dailyIntake.Name,
                dailyIntake.Entities.Select(entity => 
                    entity.MapToDTO(ingredients, recipes))
                        .OrderBy(entity => entity.SortOrder)
                        .ToArray());
        }

        private static DailyIntakeEntityDTO MapToDTO(this DailyIntakeEntity dailyIntakeEntity, Ingredient[] ingredients, Recipe[] recipes)
        {
            Ingredient? ingredient = ingredients.FirstOrDefault(ingredient => ingredient.Id == dailyIntakeEntity.EntityId);

            if (ingredient != null)
            {
                return new DailyIntakeEntityDTO(
                    ingredient.Name,
                    dailyIntakeEntity.Amount,
                    ingredient.Fat,
                    ingredient.Carbohydrates,
                    ingredient.Protein,
                    ingredient.Calories,
                    dailyIntakeEntity.SortOrder,
                    false,
                    dailyIntakeEntity.EntityId);
            }

            Recipe? recipe = recipes.FirstOrDefault(recipe => recipe.Id == dailyIntakeEntity.EntityId);

            if (recipe != null)
            {
                return new DailyIntakeEntityDTO(
                    recipe.Name,
                    dailyIntakeEntity.Amount,
                    recipe.GetFatAmount(),
                    recipe.GetCarbohydrateAmount(),
                    recipe.GetProteinAmount(),
                    recipe.GetCalorieAmount(),
                    dailyIntakeEntity.SortOrder,
                    true,
                    dailyIntakeEntity.EntityId);
            }

            throw new Exception(); // ??
        }

        //public static DailyIntake MapToDomain(this DailyIntakeDTO recipeDTO, Category[] categories)
        //{
        //    Category? category = categories.FirstOrDefault(category => category.Name == recipeDTO.CategoryName);
        //    Contracts.LogAndThrowWhenNothingWasReceived(category);

        //    DailyIntake dailyIntake = new DailyIntake(
        //        recipeDTO.Name,
        //        recipeDTO.Instructions,
        //        recipeDTO.PortionAmount,
        //        category);

        //    foreach (IngredientMeasurementDTO ingredientMeasurementDTO in recipeDTO.IngredientMeasurementDTOs)
        //    {
        //        dailyIntake.AddIngredientMeasurement(ingredientMeasurementDTO.MapToDomain(categories));
        //    }

        //    return dailyIntake;
        //}
    }
}
