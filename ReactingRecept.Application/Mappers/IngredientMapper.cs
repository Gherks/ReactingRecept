using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Mappers
{
    internal static class IngredientMapper
    {
        public static IngredientDTO MapToDTO(this Ingredient ingredient)
        {
            Contracts.LogAndThrowWhenNotSet(ingredient.Category);

            return new IngredientDTO(ingredient.Name, ingredient.Fat, ingredient.Carbohydrates, ingredient.Protein, ingredient.Calories, ingredient.Category.Name, ingredient.Category.Type);
        }

        public static Ingredient MapToDomain(this IngredientDTO ingredientDTO, Category category)
        {
            return new Ingredient(ingredientDTO.Name, ingredientDTO.Fat, ingredientDTO.Carbohydrates, ingredientDTO.Protein, ingredientDTO.Calories, category);
        }
    }
}
