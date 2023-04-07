using ReactingRecept.Application.DTOs.Ingredient;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IAsyncRepository<Ingredient>? _ingredientRepository = null;

        public IngredientService(IAsyncRepository<Ingredient>? ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);

            return await _ingredientRepository.AnyAsync(id);
        }

        public async Task<IngredientDTO?> GetByIdAsync(Guid id)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);

            Ingredient? ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null ||
                ingredient.Category == null)
            {
                return null;
            }

            return new IngredientDTO(
                ingredient.Name,
                ingredient.Fat,
                ingredient.Carbohydrates,
                ingredient.Protein,
                ingredient.Calories,
                ingredient.Category.Name,
                ingredient.Category.Type);
        }

        public async Task<IngredientDTO[]?> GetAllAsync()
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);

            Ingredient[]? ingredients = await _ingredientRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(ingredients);

            return ingredients.Select(ingredient => new IngredientDTO(
                ingredient.Name,
                ingredient.Fat,
                ingredient.Carbohydrates,
                ingredient.Protein,
                ingredient.Calories,
                ingredient.Category!.Name,
                ingredient.Category.Type)).ToArray();
        }
    }
}
