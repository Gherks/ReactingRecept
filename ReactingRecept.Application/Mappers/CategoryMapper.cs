using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Application.Mappers
{
    internal static class CategoryMapper
    {
        public static CategoryDTO MapToDTO(this Category ingredient)
        {
            return new CategoryDTO(ingredient.Name, ingredient.Type, ingredient.SortOrder);
        }

        public static Category MapToDomain(this CategoryDTO ingredientDTO)
        {
            return new Category(ingredientDTO.Name, ingredientDTO.Type, ingredientDTO.SortOrder);
        }
    }
}
