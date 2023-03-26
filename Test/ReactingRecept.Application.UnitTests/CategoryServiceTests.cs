using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs.Category;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Domain.Entities;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();

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

            GetCategoryOfTypeRequest request = new(CategoryType.Ingredient);
            GetCategoryOfTypeResponse[]? responses = await sut.GetAllOfTypeAsync(request);

            responses.Should().HaveCount(5);
            responses.Should().Contain(category => category.Name == "Dairy");
            responses.Should().Contain(category => category.Name == "Pantry");
            responses.Should().Contain(category => category.Name == "Vegetables");
            responses.Should().Contain(category => category.Name == "Meat");
            responses.Should().Contain(category => category.Name == "Other");
        }

        [Fact]
        public async Task CanFetchRecipeCategories()
        {
            ICategoryService sut = new CategoryService(_categoryRepositoryMock.Object);

            GetCategoryOfTypeRequest request = new(CategoryType.Recipe);
            GetCategoryOfTypeResponse[]? responses = await sut.GetAllOfTypeAsync(request);

            responses.Should().HaveCount(3);
            responses.Should().Contain(category => category.Name == "Snacks");
            responses.Should().Contain(category => category.Name == "Meal");
            responses.Should().Contain(category => category.Name == "Dessert");
        }
    }
}