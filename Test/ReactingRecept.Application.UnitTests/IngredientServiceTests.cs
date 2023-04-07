using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs.Ingredient;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Mocking;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
    public class IngredientServiceTests
    {
        private readonly Mock<IAsyncRepository<Ingredient>> _ingredientRepositoryMock = new();

        [Fact]
        public async Task CanDetectExistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<Guid>());

            ingredientFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<Guid>());

            ingredientFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanFetchIngredientById()
        {
            string ingredientName = "Fish";
            string ingredientCategoryName = "Fishies";
            CategoryType ingredientCategoryType = CategoryType.Ingredient;

            _ingredientRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Mocker.MockIngredient(ingredientName, ingredientCategoryName, ingredientCategoryType));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetByIdAsync(It.IsAny<Guid>());

            ingredientDTO?.Name.Should().Be(ingredientName);
            ingredientDTO?.Fat.Should().BePositive();
            ingredientDTO?.Carbohydrates.Should().BePositive();
            ingredientDTO?.Protein.Should().BePositive();
            ingredientDTO?.Calories.Should().BePositive();
            ingredientDTO?.CategoryName.Should().Be(ingredientCategoryName);
            ingredientDTO?.CategoryType.Should().Be(ingredientCategoryType);
        }

        [Fact]
        public async Task CannotFetchNonexistingIngredientById()
        {
            _ingredientRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Ingredient?>(null));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetByIdAsync(It.IsAny<Guid>());

            ingredientDTO.Should().BeNull();
        }
    }
}