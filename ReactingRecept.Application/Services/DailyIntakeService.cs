using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Mappers;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Domain.Entities.Base;
using ReactingRecept.Shared;
using System.Xml.Linq;

namespace ReactingRecept.Application.Services
{
    public class DailyIntakeService : IDailyIntakeService
    {
        private readonly IDailyIntakeRepository? _dailyIntakeRepository = null;
        private readonly IIngredientRepository? _ingredientRepository = null;
        private readonly IRecipeRepository? _recipeRepository = null;

        public DailyIntakeService(IDailyIntakeRepository? dailyIntakeRepository, IIngredientRepository? ingredientRepository, IRecipeRepository? recipeRepository)
        {
            _dailyIntakeRepository = dailyIntakeRepository;
            _ingredientRepository = ingredientRepository;
            _recipeRepository = recipeRepository;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            Contracts.LogAndThrowWhenNotInjected(_dailyIntakeRepository);

            return await _dailyIntakeRepository.AnyAsync(id);
        }

        public async Task<bool> AnyAsync(string name)
        {
            Contracts.LogAndThrowWhenNotInjected(_dailyIntakeRepository);

            return await _dailyIntakeRepository.AnyAsync(name);
        }

        public async Task<DailyIntakeDTO?> GetAsync(Guid id)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_dailyIntakeRepository);

            DailyIntake? dailyIntake = await _dailyIntakeRepository.GetByIdAsync(id);

            if (dailyIntake == null)
            {
                return null;
            }

            Ingredient[]? ingredients = await _ingredientRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(ingredients);

            Recipe[]? recipes = await _recipeRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(recipes);

            return dailyIntake.MapToDTO(ingredients, recipes);
        }

        public async Task<DailyIntakeDTO?> GetAsync(string name)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_dailyIntakeRepository);

            DailyIntake? dailyIntake = await _dailyIntakeRepository.GetByNameAsync(name);

            if (dailyIntake == null)
            {
                return null;
            }

            Ingredient[]? ingredients = await _ingredientRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(ingredients);

            Recipe[]? recipes = await _recipeRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(recipes);

            return dailyIntake.MapToDTO(ingredients, recipes);
        }

        public async Task<DailyIntakeDTO[]?> GetAllAsync()
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_dailyIntakeRepository);

            DailyIntake[]? dailyIntakes = await _dailyIntakeRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(dailyIntakes);

            Ingredient[]? ingredients = await _ingredientRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(ingredients);

            Recipe[]? recipes = await _recipeRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(recipes);

            return dailyIntakes.Select(dailyIntake => dailyIntake.MapToDTO(ingredients, recipes)).ToArray();
        }

        public async Task<DailyIntakeDTO?> AddAsync(string name, AddDailyIntakeEntityCommand[] addDailyIntakeEntityCommands)
        {
            Contracts.LogAndThrowWhenNotInjected(_ingredientRepository);
            Contracts.LogAndThrowWhenNotInjected(_recipeRepository);
            Contracts.LogAndThrowWhenNotInjected(_dailyIntakeRepository);

            Ingredient[]? ingredients = await _ingredientRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(ingredients);

            Recipe[]? recipes = await _recipeRepository.GetAllAsync();
            Contracts.LogAndThrowWhenNothingWasReceived(recipes);

            DailyIntake dailyIntake = new DailyIntake(name);

            foreach (AddDailyIntakeEntityCommand addDailyIntakeEntryCommand in addDailyIntakeEntityCommands)
            {
                bool ingredientFound = ingredients.Any(ingredient => ingredient.Id == addDailyIntakeEntryCommand.EntityId);
                bool recipeFound = recipes.Any(recipe => recipe.Id == addDailyIntakeEntryCommand.EntityId);

                if (ingredientFound || recipeFound)
                {
                    dailyIntake.AddEntity(addDailyIntakeEntryCommand.EntityId, addDailyIntakeEntryCommand.Amount);
                }
                else
                {
                    // Log // ??
                    return null;
                }
            }

            DailyIntake? addedDailyIntake = await _dailyIntakeRepository.AddAsync(dailyIntake);

            if (addedDailyIntake == null)
            {
                return null;
            }

            return dailyIntake.MapToDTO(ingredients, recipes);
        }

        public async Task<DailyIntakeDTO?> UpdateAsync(UpdateDailyIntakeEntryCommand[] addDailyIntakeEntityCommands)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(DailyIntakeDTO dailyIntakeDTO)
        {
            throw new NotImplementedException();
        }
    }
}
