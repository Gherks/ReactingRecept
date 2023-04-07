using ReactingRecept.Application.DTOs;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<bool> AnyAsync(Guid id);
        Task<IngredientDTO?> GetByIdAsync(Guid id);
        Task<IngredientDTO[]?> GetAllAsync();
        Task<IngredientDTO?> AddAsync(IngredientDTO ingredientDTO);
        Task<IngredientDTO[]?> AddManyAsync(IngredientDTO[] ingredientDTOs);
        Task<IngredientDTO?> UpdateAsync(IngredientDTO ingredientDTO);
        Task<IngredientDTO[]?> UpdateManyAsync(IngredientDTO[] ingredientDTOs);
        Task<bool> DeleteAsync(IngredientDTO ingredientDTO);
        Task<bool> DeleteManyAsync(IngredientDTO[] ingredientDTOs);
    }
}
