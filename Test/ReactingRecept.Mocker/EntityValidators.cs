using FluentAssertions;
using ReactingRecept.Application.DTOs;

namespace ReactingRecept.Mocking
{
    public static class EntityValidators
    {
        public static void Validate(IngredientDTO? validatedIngredientDTO, IngredientDTO? desiredIngredientDTO)
        {
            validatedIngredientDTO.Should().NotBeNull();
            desiredIngredientDTO.Should().NotBeNull();

            validatedIngredientDTO?.Name.Should().Be(desiredIngredientDTO?.Name);
            validatedIngredientDTO?.Fat.Should().Be(desiredIngredientDTO?.Fat);
            validatedIngredientDTO?.Carbohydrates.Should().Be(desiredIngredientDTO?.Carbohydrates);
            validatedIngredientDTO?.Protein.Should().Be(desiredIngredientDTO?.Protein);
            validatedIngredientDTO?.Calories.Should().Be(desiredIngredientDTO?.Calories);
            validatedIngredientDTO?.CategoryName.Should().Be(desiredIngredientDTO?.CategoryName);
            validatedIngredientDTO?.CategoryType.Should().Be(desiredIngredientDTO?.CategoryType);
        }

        public static void Validate(IngredientMeasurementDTO? validatedIngredientMeasurementDTO, IngredientMeasurementDTO? desiredIngredientMeasurementDTO)
        {
            validatedIngredientMeasurementDTO.Should().NotBeNull();
            desiredIngredientMeasurementDTO.Should().NotBeNull();

            validatedIngredientMeasurementDTO?.Measurement.Should().Be(desiredIngredientMeasurementDTO?.Measurement);
            validatedIngredientMeasurementDTO?.MeasurementUnit.Should().Be(desiredIngredientMeasurementDTO?.MeasurementUnit);
            validatedIngredientMeasurementDTO?.Grams.Should().Be(desiredIngredientMeasurementDTO?.Grams);
            validatedIngredientMeasurementDTO?.Note.Should().Be(desiredIngredientMeasurementDTO?.Note);
            validatedIngredientMeasurementDTO?.SortOrder.Should().Be(desiredIngredientMeasurementDTO?.SortOrder);
            Validate(validatedIngredientMeasurementDTO?.IngredientDTO, desiredIngredientMeasurementDTO?.IngredientDTO);
        }

        public static void Validate(RecipeDTO? validatedRecipeDTO, RecipeDTO? desiredRecipeDTO)
        {
            validatedRecipeDTO.Should().NotBeNull();
            desiredRecipeDTO.Should().NotBeNull();

            validatedRecipeDTO?.Name.Should().Be(desiredRecipeDTO?.Name);
            validatedRecipeDTO?.Instructions.Should().Be(desiredRecipeDTO?.Instructions);
            validatedRecipeDTO?.PortionAmount.Should().Be(desiredRecipeDTO?.PortionAmount);
            validatedRecipeDTO?.CategoryName.Should().Be(desiredRecipeDTO?.CategoryName);
            validatedRecipeDTO?.CategoryType.Should().Be(desiredRecipeDTO?.CategoryType);

            validatedRecipeDTO?.IngredientMeasurementDTOs.Should().HaveCount(desiredRecipeDTO!.IngredientMeasurementDTOs.Count());

            for (int i = 0; i < desiredRecipeDTO?.IngredientMeasurementDTOs.Count(); i++)
            {
                IngredientMeasurementDTO validatedIngredientMeasurementDTO = validatedRecipeDTO!.IngredientMeasurementDTOs.ElementAt(i);
                IngredientMeasurementDTO desiredIngredientMeasurementDTO = desiredRecipeDTO.IngredientMeasurementDTOs.ElementAt(i);
                Validate(validatedIngredientMeasurementDTO, desiredIngredientMeasurementDTO);
            }
        }
    }
}
