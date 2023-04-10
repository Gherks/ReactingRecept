using Moq;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Mocking;

public static partial class Mocker
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

    public static Category MockCategory(string name, CategoryType type, int sortOrder = 1)
    {
        Category category = new(name, type, sortOrder);
        MockId(category);

        return category;
    }
}
