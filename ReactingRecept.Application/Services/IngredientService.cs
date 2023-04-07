using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Mappers;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository? _ingredientRepository = null;
        private readonly ICategoryRepository? _categoryRepository = null;

        public IngredientService(IIngredientRepository? ingredientRepository, ICategoryRepository? categoryRepository)
        {
            _ingredientRepository = ingredientRepository;
            _categoryRepository = categoryRepository;
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

            return ingredient.MapToDTO();
        }

        public async Task<IngredientDTO[]?> GetAllAsync()
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);

            Ingredient[]? ingredients = await _ingredientRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(ingredients);

            return ingredients.Select(ingredient => ingredient.MapToDTO()).ToArray();
        }

        public async Task<IngredientDTO?> AddAsync(IngredientDTO ingredientDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
            Contracts.LogAndThrowWhenNothingWasReceived(category);

            Ingredient? addedIngredient = await _ingredientRepository.AddAsync(ingredientDTO.MapToDomain(category));
            Contracts.LogAndThrowWhenNothingWasReceived(addedIngredient);

            return addedIngredient.MapToDTO();
        }

        public async Task<IngredientDTO[]?> AddManyAsync(IngredientDTO[] ingredientDTOs)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            List<Ingredient> ingredients = new();

            foreach (IngredientDTO ingredientDTO in ingredientDTOs)
            {
                Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
                Contracts.LogAndThrowWhenNothingWasReceived(category);

                ingredients.Add(ingredientDTO.MapToDomain(category));
            }

            Ingredient[]? addedIngredients = await _ingredientRepository.AddManyAsync(ingredients.ToArray());
            Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

            return addedIngredients.Select(addedIngredient => addedIngredient.MapToDTO()).ToArray();
        }

        public async Task<IngredientDTO?> UpdateAsync(IngredientDTO ingredientDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
            Contracts.LogAndThrowWhenNothingWasReceived(category);

            Ingredient? updatedIngredient = await _ingredientRepository.UpdateAsync(ingredientDTO.MapToDomain(category));
            Contracts.LogAndThrowWhenNothingWasReceived(updatedIngredient);

            return updatedIngredient.MapToDTO();
        }

        public async Task<IngredientDTO[]?> UpdateManyAsync(IngredientDTO[] ingredientDTOs)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            List<Ingredient> ingredients = new();

            foreach (IngredientDTO ingredientDTO in ingredientDTOs)
            {
                Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
                Contracts.LogAndThrowWhenNothingWasReceived(category);

                ingredients.Add(ingredientDTO.MapToDomain(category));
            }

            Ingredient[]? addedIngredients = await _ingredientRepository.UpdateManyAsync(ingredients.ToArray());
            Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

            return addedIngredients.Select(addedIngredient => addedIngredient.MapToDTO()).ToArray();
        }

        public async Task<bool> DeleteAsync(IngredientDTO ingredientDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
            Contracts.LogAndThrowWhenNothingWasReceived(category);

            return await _ingredientRepository.DeleteAsync(ingredientDTO.MapToDomain(category));
        }

        public async Task<bool> DeleteManyAsync(IngredientDTO[] ingredientDTOs)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            List<Ingredient> ingredients = new();

            foreach (IngredientDTO ingredientDTO in ingredientDTOs)
            {
                Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
                Contracts.LogAndThrowWhenNothingWasReceived(category);

                ingredients.Add(ingredientDTO.MapToDomain(category));
            }

            return await _ingredientRepository.DeleteManyAsync(ingredients.ToArray());
        }
    }
}
