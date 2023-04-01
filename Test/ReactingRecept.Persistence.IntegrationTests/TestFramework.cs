using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.IntegrationTests;

public class TestFramework : IDisposable
{
    private ReactingReceptContext _reactingReceptContext = TestDatabaseCreator.Create();
    private CategoryRepository? _categoryRepository = null;
    private IngredientRepository? _ingredientRepository = null;
    private RecipeRepository? _recipeRepository = null;

    public Category[]? AllCategories { get; private set; } = null;
    public Ingredient[]? AllIngredients { get; private set; } = null;
    public Recipe[]? AllRecipes { get; private set; } = null;

    public TestFramework()
    {
        _categoryRepository = new(_reactingReceptContext);
        _ingredientRepository = new(_reactingReceptContext);
        _recipeRepository = new(_reactingReceptContext);
    }

    public void Dispose()
    {
        _reactingReceptContext.Database.EnsureDeleted();
    }

    public Category CreateNewCategory()
    {
        return new Category("Juice", CategoryType.Ingredient, 5);
    }

    public Category[] CreateNewCategories()
    {
        return new Category[]
        {
            new("Fruit", CategoryType.Ingredient, 6),
            new("Trash", CategoryType.Ingredient, 7),
            new("Sticks", CategoryType.Ingredient, 8),
        };
    }

    public Ingredient CreateIngredient()
    {
        Contracts.LogAndThrowWhenNotSet(AllCategories);

        return new Ingredient("Pork", 1, 1, 1, 1, AllCategories[0]);
    }

    public Ingredient[] CreateIngredients()
    {
        Contracts.LogAndThrowWhenNotSet(AllCategories);

        return new Ingredient[]
        {
            new Ingredient("Beans", 1, 1, 1, 1, AllCategories[0]),
            new Ingredient("Peas", 1, 1, 1, 1, AllCategories[0]),
            new Ingredient("Pear", 1, 1, 1, 1, AllCategories[0]),
        };
    }

    public Recipe CreateRecipe()
    {
        Contracts.LogAndThrowWhenNotSet(AllCategories);

        return new Recipe("Porky soup", "Porky instructions!", 1, AllCategories[0]);
    }

    public Recipe[] CreateRecipes()
    {
        Contracts.LogAndThrowWhenNotSet(AllCategories);

        return new Recipe[]
        {
            new Recipe("Bean soup", "Do it like this!", 1, AllCategories[0]),
            new Recipe("Pea soup", "Do it like that!", 1, AllCategories[0]),
            new Recipe("Pear soup", "Do it like this or that!", 1, AllCategories[0]),
        };
    }

    public async Task<CategoryRepository> PrepareCategoryRepository()
    {
        Contracts.LogAndThrowWhenNotSet(_categoryRepository);

        Category[]? ingredientCategories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Ingredient);
        Contracts.LogAndThrowWhenNothingWasReceived(ingredientCategories);

        Category[]? recipeCategories = await _categoryRepository.GetManyOfTypeAsync(CategoryType.Recipe);
        Contracts.LogAndThrowWhenNothingWasReceived(recipeCategories);

        AllCategories = ingredientCategories.Concat(recipeCategories).ToArray();

        return _categoryRepository;
    }

    public async Task<IngredientRepository> PrepareIngredientRepository()
    {
        Contracts.LogAndThrowWhenNotSet(_ingredientRepository);
        Contracts.LogAndThrowWhenNotSet(AllCategories);

        AllIngredients = await _ingredientRepository.AddManyAsync(new Ingredient[]
        {
            new Ingredient("Tomato", 1, 1, 1, 1, AllCategories[0]),
            new Ingredient("Cucumber", 1, 1, 1, 1, AllCategories[0]),
            new Ingredient("Celery", 1, 1, 1, 1, AllCategories[0]),
        });

        return _ingredientRepository;
    }

    public async Task<RecipeRepository> PrepareRecipeRepository()
    {
        Contracts.LogAndThrowWhenNotSet(_recipeRepository);
        Contracts.LogAndThrowWhenNotSet(AllCategories);
        Contracts.LogAndThrowWhenNotSet(AllIngredients);

        IngredientMeasurement ingredientMeasurement1 = new(1.0, MeasurementUnit.Gram, 1.0, "A note", 1, AllIngredients[0]);
        IngredientMeasurement ingredientMeasurement2 = new(1.0, MeasurementUnit.Deciliters, 1.0, "Another note", 1, AllIngredients[1]);
        IngredientMeasurement ingredientMeasurement3 = new(1.0, MeasurementUnit.Kilogram, 1.0, "Notes notes notes", 1, AllIngredients[2]);

        Recipe recipe1 = new Recipe("Tomato soup", "Instructions", 1, AllCategories[0]);
        recipe1.AddIngredientMeasurement(ingredientMeasurement1);
        recipe1.AddIngredientMeasurement(ingredientMeasurement2);
        recipe1.AddIngredientMeasurement(ingredientMeasurement3);

        Recipe recipe2 = new Recipe("Cucumber soup", "Many instructions", 1, AllCategories[0]);
        recipe2.AddIngredientMeasurement(ingredientMeasurement1);
        recipe2.AddIngredientMeasurement(ingredientMeasurement2);

        Recipe recipe3 = new Recipe("Celery soup", "Lots of instructions", 1, AllCategories[0]);
        recipe3.AddIngredientMeasurement(ingredientMeasurement2);
        recipe3.AddIngredientMeasurement(ingredientMeasurement3);

        AllRecipes = await _recipeRepository.AddManyAsync(new Recipe[]
        {
            recipe1,
            recipe2,
            recipe3,
        });

        return _recipeRepository;
    }
}
