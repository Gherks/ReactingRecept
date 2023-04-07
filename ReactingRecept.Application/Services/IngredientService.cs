using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
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

        public async Task<IngredientDTO?> AddAsync(IngredientDTO ingredientDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
            Contracts.LogAndThrowWhenNothingWasReceived(category);

            Ingredient? addedIngredient = await _ingredientRepository.AddAsync(new Ingredient(
                ingredientDTO.Name,
                ingredientDTO.Fat,
                ingredientDTO.Carbohydrates,
                ingredientDTO.Protein,
                ingredientDTO.Calories,
                category));
            Contracts.LogAndThrowWhenNothingWasReceived(addedIngredient);

            return new IngredientDTO(
                addedIngredient.Name,
                addedIngredient.Fat,
                addedIngredient.Carbohydrates,
                addedIngredient.Protein,
                addedIngredient.Calories,
                addedIngredient.Category!.Name,
                addedIngredient.Category.Type);
        }

        public async Task<IngredientDTO[]?> AddManyAsync(IngredientDTO[] ingredientDTOs)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            List<Ingredient> ingredients = new();

            foreach(IngredientDTO ingredientDTO in ingredientDTOs)
            {
                Category? category = categories.FirstOrDefault(category => category.Name == ingredientDTO.CategoryName);
                Contracts.LogAndThrowWhenNothingWasReceived(category);

                ingredients.Add(new Ingredient(
                    ingredientDTO.Name,
                    ingredientDTO.Fat,
                    ingredientDTO.Carbohydrates,
                    ingredientDTO.Protein,
                    ingredientDTO.Calories,
                    category));
            }

            Ingredient[]? addedIngredients = await _ingredientRepository.AddManyAsync(ingredients.ToArray());
            Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

            return addedIngredients.Select(addedIngredient => new IngredientDTO(
                addedIngredient.Name,
                addedIngredient.Fat,
                addedIngredient.Carbohydrates,
                addedIngredient.Protein,
                addedIngredient.Calories,
                addedIngredient.Category!.Name,
                addedIngredient.Category.Type)).ToArray();
        }
    }
}
