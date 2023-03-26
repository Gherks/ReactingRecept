using FluentAssertions;
using Moq;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Domain;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new Mock<ICategoryRepository>();

        public CategoryServiceTests()
        {
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

            _categoryRepositoryMock.Setup(mock => mock.ListAllOfTypeAsync(CategoryType.Ingredient)).ReturnsAsync(_ingredientCategories);
            _categoryRepositoryMock.Setup(mock => mock.ListAllOfTypeAsync(CategoryType.Recipe)).ReturnsAsync(_recipeCategories);
        }

        [Fact]
        public async Task CanFetchIngredientCategories()
        {
            ICategoryService sut = new CategoryService(_categoryRepositoryMock.Object);

            Category[]? categories = await sut.GetAllOfTypeAsync(CategoryType.Ingredient);

            categories.Should().HaveCount(5);
            categories.Should().Contain(category => category.Name == "Dairy");
            categories.Should().Contain(category => category.Name == "Pantry");
            categories.Should().Contain(category => category.Name == "Vegetables");
            categories.Should().Contain(category => category.Name == "Meat");
            categories.Should().Contain(category => category.Name == "Other");
        }

        [Fact]
        public async Task CanFetchRecipeCategories()
        {
            ICategoryService sut = new CategoryService(_categoryRepositoryMock.Object);

            Category[]? categories = await sut.GetAllOfTypeAsync(CategoryType.Recipe);

            categories.Should().HaveCount(3);
            categories.Should().Contain(category => category.Name == "Snacks");
            categories.Should().Contain(category => category.Name == "Meal");
            categories.Should().Contain(category => category.Name == "Dessert");
        }
    }
}