using Moq;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
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

    public static Category MockCategory(string name, CategoryType type, int sortOrder)
    {
        Category mockedCategory = new(name, type, sortOrder);

        MockId(mockedCategory);

        return mockedCategory;
    }

    private static void MockId(Category category)
    {
        string idPropertyName = nameof(category.Id);

        PropertyInfo? property = category.GetType().GetProperty(idPropertyName);

        if (property != null &&
            property.DeclaringType != null)
        {
            PropertyInfo? deckar�ngTypeProperty = property.DeclaringType.GetProperty(idPropertyName);

            if (deckar�ngTypeProperty != null)
            {
                deckar�ngTypeProperty.SetValue(category, Guid.NewGuid(), null);
            }
        }
    }
}