using Moq;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Domain.Entities.Base;
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
            new Category("Dairy", CategoryType.Ingredient, 1),
            new Category("Pantry", CategoryType.Ingredient, 2),
            new Category("Vegetables", CategoryType.Ingredient, 3),
            new Category("Meat", CategoryType.Ingredient, 4),
            new Category("Other", CategoryType.Ingredient, 5),
        };

        Category[] _recipeCategories = new Category[]
        {
            new Category("Snacks", CategoryType.Recipe, 1),
            new Category("Meal", CategoryType.Recipe, 2),
            new Category("Dessert", CategoryType.Recipe, 3),
        };

        categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(CategoryType.Ingredient)).ReturnsAsync(_ingredientCategories);
        categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(CategoryType.Recipe)).ReturnsAsync(_recipeCategories);

        return categoryRepositoryMock;
    }

    public static Ingredient MockIngredient(string name, string categoryName, CategoryType categoryType)
    {
        Random random = new();

        Category category = MockCategory(categoryName, categoryType, 1);
        Ingredient ingredient = new(name, random.NextDouble(), random.NextDouble(), random.NextDouble(), random.NextDouble(), category);
        MockId(ingredient);

        return ingredient;
    }

    public static Ingredient MockIngredient(string name, double fat, double carbohydrates, double protein, double calories, string categoryName, CategoryType categoryType)
    {
        Category category = MockCategory(categoryName, categoryType, 1);
        Ingredient ingredient = new(name, fat, carbohydrates, protein, calories, category);
        MockId(ingredient);

        return ingredient;
    }

    public static Category MockCategory(string name, CategoryType type, int sortOrder)
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
