using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs;
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

        [Fact]
        public async Task CanFetchAllIngredients()
        {
            string ingredientName1 = "Fish";
            string ingredientCategoryName1 = "Fishies";
            CategoryType ingredientCategoryType1 = CategoryType.Ingredient;

            string ingredientName2 = "Cucumber";
            string ingredientCategoryName2 = "Veggies";
            CategoryType ingredientCategoryType2 = CategoryType.Ingredient;

            _ingredientRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new Ingredient[]
            {
                Mocker.MockIngredient(ingredientName1, ingredientCategoryName1, ingredientCategoryType1),
                Mocker.MockIngredient(ingredientName2, ingredientCategoryName2, ingredientCategoryType2),
            });
                
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);

            IngredientDTO[]? ingredientDTOs = await sut.GetAllAsync();

            ingredientDTOs?[0]?.Name.Should().Be(ingredientName1);
            ingredientDTOs?[0]?.Fat.Should().BePositive();
            ingredientDTOs?[0]?.Carbohydrates.Should().BePositive();
            ingredientDTOs?[0]?.Protein.Should().BePositive();
            ingredientDTOs?[0]?.Calories.Should().BePositive();
            ingredientDTOs?[0]?.CategoryName.Should().Be(ingredientCategoryName1);
            ingredientDTOs?[0]?.CategoryType.Should().Be(ingredientCategoryType1);

            ingredientDTOs?[1]?.Name.Should().Be(ingredientName2);
            ingredientDTOs?[1]?.Fat.Should().BePositive();
            ingredientDTOs?[1]?.Carbohydrates.Should().BePositive();
            ingredientDTOs?[1]?.Protein.Should().BePositive();
            ingredientDTOs?[1]?.Calories.Should().BePositive();
            ingredientDTOs?[1]?.CategoryName.Should().Be(ingredientCategoryName2);
            ingredientDTOs?[1]?.CategoryType.Should().Be(ingredientCategoryType2);
        }
    }
}