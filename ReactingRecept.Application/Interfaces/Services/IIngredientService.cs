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
        //Task<IngredientDTO?> UpdateAsync(IngredientDTO ingredient);
        //Task<IngredientDTO[]?> UpdateManyAsync(IngredientDTO[] ingredients);
        //Task<bool> DeleteAsync(IngredientDTO ingredient);
        //Task<bool> DeleteManyAsync(IngredientDTO[] ingredients);
    }
}
