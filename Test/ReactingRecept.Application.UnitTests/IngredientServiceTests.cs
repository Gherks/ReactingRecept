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
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock = new();
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = Mocker.GetCategoryRepositoryMock();

        [Fact]
        public async Task CanDetectExistingIngredientById()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<Guid>());

            ingredientFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingIngredientById()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<Guid>());

            ingredientFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectExistingIngredientByName()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(true);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<string>());

            ingredientFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingIngredientByName()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(false);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<string>());

            ingredientFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanFetchIngredientById()
        {
            IngredientDTO desiredIngredientDTO = new("Fish", 2, 2, 2, 2, "Meat", CategoryType.Ingredient);

            _ingredientRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Mocker.MockIngredient(desiredIngredientDTO));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetAsync(It.IsAny<Guid>());

            EntityValidators.Validate(ingredientDTO, desiredIngredientDTO);
        }

        [Fact]
        public async Task CannotFetchNonexistingIngredientById()
        {
            _ingredientRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Ingredient?>(null));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetAsync(It.IsAny<Guid>());

            ingredientDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchIngredientByName()
        {
            IngredientDTO desiredIngredientDTO = new("Fish", 2, 2, 2, 2, "Meat", CategoryType.Ingredient);

            _ingredientRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(Mocker.MockIngredient(desiredIngredientDTO));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetAsync(It.IsAny<string>());

            EntityValidators.Validate(ingredientDTO, desiredIngredientDTO);
        }

        [Fact]
        public async Task CannotFetchNonexistingIngredientByName()
        {
            _ingredientRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<Ingredient?>(null));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetAsync(It.IsAny<string>());

            ingredientDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchAllIngredients()
        {
            IngredientDTO[] sourceIngredientDTOs = new IngredientDTO[]
            {
                new("Fish", 1, 1, 1, 1, "Meat", CategoryType.Ingredient),
                new("Cucumber", 1, 1, 1, 1, "Vegetables", CategoryType.Ingredient),
            };

            _ingredientRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new Ingredient[]
            {
                Mocker.MockIngredient(sourceIngredientDTOs[0]),
                Mocker.MockIngredient(sourceIngredientDTOs[1]),
            });

            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO[]? ingredientDTOs = await sut.GetAllAsync();

            EntityValidators.Validate(ingredientDTOs?[0], sourceIngredientDTOs[0]);
            EntityValidators.Validate(ingredientDTOs?[1], sourceIngredientDTOs[1]);
        }

        [Fact]
        public async Task CanAddIngredient()
        {
            IngredientDTO desiredIngredientDTO = new("Fish", 2, 2, 2, 2, "Meat", CategoryType.Ingredient);

            _ingredientRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<Ingredient>())).ReturnsAsync(Mocker.MockIngredient(desiredIngredientDTO));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? addedIngredientDTO = await sut.AddAsync(desiredIngredientDTO);

            EntityValidators.Validate(addedIngredientDTO, desiredIngredientDTO);
        }

        [Fact]
        public async Task CanUpdateIngredient()
        {
            IngredientDTO desiredIngredientDTO = new("Fish", 2, 2, 2, 2, "Meat", CategoryType.Ingredient);

            _ingredientRepositoryMock.Setup(mock => mock.UpdateAsync(It.IsAny<Ingredient>())).ReturnsAsync(Mocker.MockIngredient(desiredIngredientDTO));
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? updatedIngredientDTO = await sut.UpdateAsync(desiredIngredientDTO);

            EntityValidators.Validate(updatedIngredientDTO, desiredIngredientDTO);
        }

        [Fact]
        public async Task CanDeleteExistingIngredient()
        {
            IngredientDTO desiredIngredientDTO = new("Fish", 2, 2, 2, 2, "Meat", CategoryType.Ingredient);

            _ingredientRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Ingredient>())).ReturnsAsync(true);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientDeleted = await sut.DeleteAsync(desiredIngredientDTO);

            ingredientDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDeleteNonexistingIngredient()
        {
            IngredientDTO desiredIngredientDTO = new("Fish", 2, 2, 2, 2, "Meat", CategoryType.Ingredient);

            _ingredientRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Ingredient>())).ReturnsAsync(false);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientDeleted = await sut.DeleteAsync(desiredIngredientDTO);

            ingredientDeleted.Should().BeFalse();
        }
    }
}