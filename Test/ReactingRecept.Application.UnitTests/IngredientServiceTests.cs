using FluentAssertions;
using Moq;
using ReactingRecept.Application.DTOs.Category;
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
        private Mock<IAsyncRepository<Ingredient>> _ingredientRepositoryMock = new();

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
    }
}