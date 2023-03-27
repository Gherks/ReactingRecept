using FluentAssertions;
using Moq;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Domain;
using ReactingRecept.Domain.Entities;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
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

        public static Category MockCategory()
        {
            return new Category("CategoryName", It.IsAny<CategoryType>(), It.IsAny<int>());
        }

        public static Ingredient MockIngredient()
        {
            return new Ingredient("IngredientName", 1.0, 1.0, 1.0, 1.0, MockCategory());
        }
    }
}