using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    public static IngredientMeasurement MockIngredientMeasurement(IngredientMeasurementDTO ingredientMeasurementDTO)
    {
        Contracts.LogAndThrowWhenNotSet(ingredientMeasurementDTO.IngredientDTO);

        IngredientMeasurement ingredientMeasurement = new(
            ingredientMeasurementDTO.Measurement,
            ingredientMeasurementDTO.MeasurementUnit,
            ingredientMeasurementDTO.Grams,
            ingredientMeasurementDTO.Note,
            ingredientMeasurementDTO.SortOrder,
            MockIngredient(ingredientMeasurementDTO.IngredientDTO));
        MockId(ingredientMeasurement);

        return ingredientMeasurement;
    }
}
