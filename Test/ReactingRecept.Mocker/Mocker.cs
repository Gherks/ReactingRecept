using Moq;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Domain.Entities.Base;
using ReactingRecept.Shared;
using System.Reflection;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Mocking;

public static class Mocker
{
    public static Mock<ICategoryRepository> GetCategoryRepositoryMock()
    {
        Mock<ICategoryRepository> categoryRepositoryMock = new();

        Category[] _ingredientCategories = new Category[]
        {
            MockCategory("Dairy", CategoryType.Ingredient, 1),
            MockCategory("Pantry", CategoryType.Ingredient, 2),
            MockCategory("Vegetables", CategoryType.Ingredient, 3),
            MockCategory("Meat", CategoryType.Ingredient, 4),
            MockCategory("Other", CategoryType.Ingredient, 5),
        };

        Category[] _recipeCategories = new Category[]
        {
            MockCategory("Snacks", CategoryType.Recipe, 1),
            MockCategory("Meal", CategoryType.Recipe, 2),
            MockCategory("Dessert", CategoryType.Recipe, 3),
        };

        categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(CategoryType.Ingredient)).ReturnsAsync(_ingredientCategories);
        categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(CategoryType.Recipe)).ReturnsAsync(_recipeCategories);
        categoryRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(_ingredientCategories.Concat(_recipeCategories).ToArray());

        return categoryRepositoryMock;
    }

    public static Recipe MockRecipe(RecipeDTO recipeDTO)
    {
        Category category = MockCategory(recipeDTO.CategoryName, recipeDTO.CategoryType);

        Recipe recipe = new(
            recipeDTO.Name, 
            recipeDTO.Instructions, 
            recipeDTO.PortionAmount, 
            category);
        MockId(recipe);

        foreach (IngredientMeasurementDTO ingredientMeasurementDTO in recipeDTO.IngredientMeasurementDTOs)
        {
            IngredientMeasurement ingredientMeasurement = MockIngredientMeasurement(ingredientMeasurementDTO);
            recipe.AddIngredientMeasurement(ingredientMeasurement);
        }

        return recipe;
    }

    public static IngredientMeasurement MockIngredientMeasurement(IngredientMeasurementDTO ingredientMeasurementDTO)
    {
        Contracts.LogAndThrowWhenNotSet(ingredientMeasurementDTO.IngredientDTO);

        IngredientMeasurement ingredientMeasurement = new(
            ingredientMeasurementDTO.Measurement, 
            ingredientMeasurementDTO.MeasurementUnit, 
            ingredientMeasurementDTO.Grams, 
            ingredientMeasurementDTO.Note, 
            ingredientMeasurementDTO.SortOrder,
            MockIngredient(ingredientMeasurementDTO.IngredientDTO));
        MockId(ingredientMeasurement);

        return ingredientMeasurement;
    }

    public static Ingredient MockIngredient(IngredientDTO ingredientDTO)
    {
        Category category = MockCategory(ingredientDTO.CategoryName, ingredientDTO.CategoryType);

        Ingredient ingredient = new(
            ingredientDTO.Name, 
            ingredientDTO.Fat, 
            ingredientDTO.Carbohydrates, 
            ingredientDTO.Protein, 
            ingredientDTO.Calories, 
            category);
        MockId(ingredient);

        return ingredient;
    }

    public static Category MockCategory(string name, CategoryType type, int sortOrder = 1)
    {
        Category category = new(name, type, sortOrder);
        MockId(category);

        return category;
    }

    private static void MockId(BaseEntity baseEntity)
    {
        string idPropertyName = nameof(baseEntity.Id);

        PropertyInfo? property = baseEntity.GetType().GetProperty(idPropertyName);

        if (property != null &&
            property.DeclaringType != null)
        {
            PropertyInfo? deckaringTypeProperty = property.DeclaringType.GetProperty(idPropertyName);

            if (deckaringTypeProperty != null)
            {
                deckaringTypeProperty.SetValue(baseEntity, Guid.NewGuid(), null);
            }
        }
    }
}
