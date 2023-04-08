using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Mappers
{
    internal static class IngredientMeasurementMapper
    {
        public static IngredientMeasurementDTO MapToDTO(this IngredientMeasurement ingredientMeasurement)
        {
            Contracts.LogAndThrowWhenNotSet(ingredientMeasurement.Ingredient);

            return new IngredientMeasurementDTO(
                ingredientMeasurement.Measurement,
                ingredientMeasurement.MeasurementUnit,
                ingredientMeasurement.Grams,
                ingredientMeasurement.Note,
                ingredientMeasurement.SortOrder,
                ingredientMeasurement.Ingredient.MapToDTO());
        }

        public static IngredientMeasurement MapToDomain(this IngredientMeasurementDTO ingredientMeasurementDTO, Category[] categories)
        {
            Contracts.LogAndThrowWhenNotSet(ingredientMeasurementDTO.IngredientDTO);

            return new IngredientMeasurement(
                ingredientMeasurementDTO.Measurement,
                ingredientMeasurementDTO.MeasurementUnit,
                ingredientMeasurementDTO.Grams,
                ingredientMeasurementDTO.Note,
                ingredientMeasurementDTO.SortOrder,
                ingredientMeasurementDTO.IngredientDTO.MapToDomain(categories));
        }
    }
}
