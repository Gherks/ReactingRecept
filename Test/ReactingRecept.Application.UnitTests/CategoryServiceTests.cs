using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs;
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

            CategoryDTO[]? categoryDTOs = await sut.GetManyOfTypeAsync(CategoryType.Ingredient);

            categoryDTOs.Should().HaveCount(5);
            categoryDTOs.Should().Contain(category => category.Name == "Dairy");
            categoryDTOs.Should().Contain(category => category.Name == "Pantry");
            categoryDTOs.Should().Contain(category => category.Name == "Vegetables");
            categoryDTOs.Should().Contain(category => category.Name == "Meat");
            categoryDTOs.Should().Contain(category => category.Name == "Other");
        }

        [Fact]
        public async Task CanFetchRecipeCategories()
        {
            ICategoryService sut = new CategoryService(_categoryRepositoryMock.Object);

            CategoryDTO[]? categoryDTOs = await sut.GetManyOfTypeAsync(CategoryType.Recipe);

            categoryDTOs.Should().HaveCount(3);
            categoryDTOs.Should().Contain(category => category.Name == "Snacks");
            categoryDTOs.Should().Contain(category => category.Name == "Meal");
            categoryDTOs.Should().Contain(category => category.Name == "Dessert");
        }
    }
}