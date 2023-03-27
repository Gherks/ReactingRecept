using ReactingRecept.Application.DTOs.Category;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private IAsyncRepository<Ingredient>? _ingredientRepository = null;

        public IngredientService(IAsyncRepository<Ingredient>? ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<AnyIngredientResponse> AnyAsync(AnyIngredientRequest request)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);

            return new AnyIngredientResponse(await _ingredientRepository.AnyAsync(request.Id));
        }
    }
}
