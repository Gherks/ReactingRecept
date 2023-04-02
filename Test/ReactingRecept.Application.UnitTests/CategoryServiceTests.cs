using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs.Category;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Mocking;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = Mocker.GetCategoryRepositoryMock();

        [Fact]
        public async Task CanFetchIngredientCategories()
        {
            ICategoryService sut = new CategoryService(_categoryRepositoryMock.Object);

            GetManyOfTypeRequest request = new(CategoryType.Ingredient);
            GetManyOfTypeResponse[]? responses = await sut.GetManyOfTypeAsync(request);

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

            GetManyOfTypeRequest request = new(CategoryType.Recipe);
            GetManyOfTypeResponse[]? responses = await sut.GetManyOfTypeAsync(request);

            responses.Should().HaveCount(3);
            responses.Should().Contain(category => category.Name == "Snacks");
            responses.Should().Contain(category => category.Name == "Meal");
            responses.Should().Contain(category => category.Name == "Dessert");
        }
    }
}