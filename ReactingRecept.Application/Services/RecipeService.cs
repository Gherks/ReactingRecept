using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Mappers;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;
using System.Xml.Linq;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository? _recipeRepository = null;
        private readonly ICategoryRepository? _categoryRepository = null;

        public RecipeService(IRecipeRepository recipeRepository, ICategoryRepository? categoryRepository)
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;

        }

        public async Task<bool> AnyAsync(Guid id)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);

            return await _recipeRepository.AnyAsync(id);
        }

        public async Task<bool> AnyAsync(string name)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);

            return await _recipeRepository.AnyAsync(name);
        }

        public async Task<RecipeDTO?> GetAsync(Guid id)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);

            Recipe? recipe = await _recipeRepository.GetByIdAsync(id);

            if (recipe == null)
            {
                return null;
            }

            return recipe.MapToDTO();
        }

        public async Task<RecipeDTO?> GetAsync(string name)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);

            Recipe? recipe = await _recipeRepository.GetByNameAsync(name);

            if (recipe == null)
            {
                return null;
            }

            return recipe.MapToDTO();
        }

        public async Task<RecipeDTO[]?> GetAllAsync()
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);

            Recipe[]? recipes = await _recipeRepository.GetAllAsync();

            if (recipes == null)
            {
                return null;
            }

            return recipes.Select(recipe => recipe.MapToDTO()).ToArray();
        }

        public async Task<RecipeDTO?> AddAsync(RecipeDTO recipeDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            Recipe? recipe = await _recipeRepository.AddAsync(recipeDTO.MapToDomain(categories));

            if (recipe == null)
            {
                return null;
            }

            return recipe.MapToDTO();
        }

        public async Task<RecipeDTO?> UpdateAsync(RecipeDTO recipeDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            Recipe? recipe = await _recipeRepository.UpdateAsync(recipeDTO.MapToDomain(categories));

            if (recipe == null)
            {
                return null;
            }

            return recipe.MapToDTO();
        }

        public async Task<bool> DeleteAsync(RecipeDTO recipeDTO)
        {
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_categoryRepository);

            Category[]? categories = await _categoryRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            return await _recipeRepository.DeleteAsync(recipeDTO.MapToDomain(categories));
        }
    }
}
