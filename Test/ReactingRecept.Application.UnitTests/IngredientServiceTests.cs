using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs.Ingredient;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Domain.Entities;
using Xunit;

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
            AnyIngredientRequest request = new(It.IsAny<Guid>());

            AnyIngredientResponse response = await sut.AnyAsync(request);

            response.Exist.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);
            AnyIngredientRequest request = new(It.IsAny<Guid>());

            AnyIngredientResponse response = await sut.AnyAsync(request);

            response.Exist.Should().BeFalse();
        }

        //[Fact]
        //public async Task CanFetchIngredientById()
        //{
        //    _ingredientRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Mocker.MockIngredient());
        //    IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);
        //    GetIngredientByIdRequest request = new(It.IsAny<Guid>());

        //    GetIngredientByIdResponse? response = await sut.GetByIdAsync(request);

        //    response?.Name.Should().NotBeNullOrWhiteSpace();
        //    response?.Fat.Should().BePositive();
        //    response?.Carbohydrates.Should().BePositive();
        //    response?.Protein.Should().BePositive();
        //    response?.Calories.Should().BePositive();
        //    response?.CategoryName.Should().NotBeNullOrWhiteSpace();
        //    response?.CategoryType.Should().BeDefined();
        //}

        [Fact]
        public async Task CannotFetchNonexistingIngredientById()
        {
            _ingredientRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Ingredient?>(null));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object);
            GetIngredientByIdRequest request = new(It.IsAny<Guid>());

            GetIngredientByIdResponse? response = await sut.GetByIdAsync(request);

            response.Should().BeNull();
        }
    }
}