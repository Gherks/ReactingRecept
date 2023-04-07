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
    private readonly ReactingReceptContext _reactingReceptContext = TestDatabaseCreator.Create();
    private readonly CategoryRepository? _categoryRepository = null;
    private readonly IngredientRepository? _ingredientRepository = null;
    private readonly RecipeRepository? _recipeRepository = null;
    private readonly DailyIntakeRepository? _dailyIntakeRepository = null;

    public Category[]? AllCategories { get; private set; } = null;
    public Ingredient[]? AllIngredients { get; private set; } = null;
    public Recipe[]? AllRecipes { get; private set; } = null;
    public DailyIntake[]? AllDailyIntakes { get; private set; } = null;

    public TestFramework()
    {
        _categoryRepository = new(_reactingReceptContext);
        _ingredientRepository = new(_reactingReceptContext);
        _recipeRepository = new(_reactingReceptContext);
        _dailyIntakeRepository = new(_reactingReceptContext);
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

        Category recipeCategory = AllCategories.First(category => category.Type == CategoryType.Recipe);

        return new Recipe[]
        {
            new Recipe("Bean soup", "Do it like this!", 1, recipeCategory),
            new Recipe("Pea soup", "Do it like that!", 1, recipeCategory),
            new Recipe("Pear soup", "Do it like this or that!", 1, recipeCategory),
        };
    }

    public DailyIntake[] CreateDailyIntakes()
    {
        Contracts.LogAndThrowWhenNotSet(AllIngredients);
        Contracts.LogAndThrowWhenNotSet(AllRecipes);

        DailyIntake dailyIntake1 = new("Created intake 1");
        DailyIntake dailyIntake2 = new("Created intake 2");
        DailyIntake dailyIntake3 = new("Created intake 3");

        dailyIntake1.AddEntry(AllIngredients[1]);
        dailyIntake1.AddEntry(AllIngredients[1]);

        dailyIntake2.AddEntry(AllIngredients[1]);
        dailyIntake2.AddEntry(AllIngredients[1]);

        dailyIntake3.AddEntry(AllRecipes[1]);
        dailyIntake3.AddEntry(AllIngredients[1]);

        return new DailyIntake[]
        {
            dailyIntake1,
            dailyIntake2,
            dailyIntake3,
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

        Category[] recipeCategories = AllCategories.Where(category => category.Type == CategoryType.Recipe).ToArray();

        IngredientMeasurement ingredientMeasurement1 = new(1.0, MeasurementUnit.Gram, 1.0, "A note", 1, AllIngredients[0]);
        IngredientMeasurement ingredientMeasurement2 = new(1.0, MeasurementUnit.Deciliters, 1.0, "Another note", 1, AllIngredients[1]);
        IngredientMeasurement ingredientMeasurement3 = new(1.0, MeasurementUnit.Kilogram, 1.0, "Notes notes notes", 1, AllIngredients[2]);

        Recipe recipe1 = new("Tomato soup", "Instructions", 1, recipeCategories[0]);
        recipe1.AddIngredientMeasurement(ingredientMeasurement1);
        recipe1.AddIngredientMeasurement(ingredientMeasurement2);
        recipe1.AddIngredientMeasurement(ingredientMeasurement3);

        Recipe recipe2 = new("Cucumber soup", "Many instructions", 1, recipeCategories[0]);
        recipe2.AddIngredientMeasurement(ingredientMeasurement1);
        recipe2.AddIngredientMeasurement(ingredientMeasurement2);

        Recipe recipe3 = new("Celery soup", "Lots of instructions", 1, recipeCategories[0]);
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

    public async Task<DailyIntakeRepository> PrepareDailyIntakeRepository()
    {
        Contracts.LogAndThrowWhenNotSet(_dailyIntakeRepository);
        Contracts.LogAndThrowWhenNotSet(AllCategories);
        Contracts.LogAndThrowWhenNotSet(AllIngredients);
        Contracts.LogAndThrowWhenNotSet(AllRecipes);

        DailyIntake dailyIntake1 = new("Inake 1");
        dailyIntake1.AddEntry(AllRecipes[0]);
        dailyIntake1.AddEntry(AllIngredients[0]);
        dailyIntake1.AddEntry(AllRecipes[1]);

        DailyIntake dailyIntake2 = new("Inake 2");
        dailyIntake2.AddEntry(AllRecipes[1]);
        dailyIntake2.AddEntry(AllRecipes[2]);
        dailyIntake2.AddEntry(AllIngredients[2]);

        DailyIntake dailyIntake3 = new("Empty daily intake");

        AllDailyIntakes = await _dailyIntakeRepository.AddManyAsync(new DailyIntake[]
        {
            dailyIntake1,
            dailyIntake2,
            dailyIntake3,
        });

        return _dailyIntakeRepository;
    }
}
