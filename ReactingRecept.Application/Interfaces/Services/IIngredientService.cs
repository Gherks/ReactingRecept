using ReactingRecept.Application.DTOs.Ingredient;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<AnyIngredientResponse> AnyAsync(AnyIngredientRequest request);
        Task<GetIngredientByIdResponse?> GetByIdAsync(GetIngredientByIdRequest request);

        //Task<IIngredientDto?> GetByIdAsync(Guid id);
        //Task<IIngredientDto[]?> GetAllAsync();
        //Task<IIngredientDto?> AddAsync(IIngredientDto ingredient);
        //Task<IIngredientDto[]?> AddManyAsync(IIngredientDto[] ingredients);
        //Task<IIngredientDto?> UpdateAsync(IIngredientDto ingredient);
        //Task<IIngredientDto[]?> UpdateManyAsync(IIngredientDto[] ingredients);
        //Task<bool> DeleteAsync(IIngredientDto ingredient);
        //Task<bool> DeleteManyAsync(IIngredientDto[] ingredients);
    }
}
