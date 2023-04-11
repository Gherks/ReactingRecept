using FluentAssertions;
using Moq;
using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Application.Services;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Domain.Entities.Base;
using ReactingRecept.Mocking;
using ReactingRecept.Shared;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
    public class DailyIntakeServiceTests
    {
        private readonly Mock<IDailyIntakeRepository> _dailyIntakeRepositoryMock = new();
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock = new();
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock = new();

        private IngredientDTO[]? _ingredientDTOs = Array.Empty<IngredientDTO>();
        private IngredientMeasurementDTO[]? _ingredientMeasurementDTOs = Array.Empty<IngredientMeasurementDTO>();
        private RecipeDTO? _recipeDTO = null;

        private Ingredient[] _ingredients = Array.Empty<Ingredient>();
        private Recipe? _recipe = null;

        private BaseEntity[] _baseEntities = Array.Empty<BaseEntity>();

        public DailyIntakeServiceTests()
        {
            _ingredientDTOs = new IngredientDTO[]
{
                new("Fish", 1, 1, 1, 1, "Meat", CategoryType.Ingredient),
                new("Salt", 1, 1, 1, 1, "Other", CategoryType.Ingredient),
                new("Pepper", 1, 1, 1, 1, "Other", CategoryType.Ingredient)
};

            _ingredientMeasurementDTOs = new IngredientMeasurementDTO[]
            {
                new(3, MeasurementUnit.Gram, 3, "Fishies note", 1, _ingredientDTOs[0]),
                new(1, MeasurementUnit.Kilogram, 1000, "Salt note", 2, _ingredientDTOs[1]),
                new(300, MeasurementUnit.Gram, 300, "Pepper note", 3, _ingredientDTOs[2]),
            };

            _recipeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.Recipe, _ingredientMeasurementDTOs);

            _ingredients = Mocker.MockIngredients(_ingredientDTOs);
            _recipe = Mocker.MockRecipe(_recipeDTO);

            _baseEntities = _ingredients.Concat(new BaseEntity[] { _recipe }).ToArray();

            _ingredientRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(_ingredients);
            _recipeRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new Recipe[] { _recipe });
        }

        [Fact]
        public async Task CanDetectExistingDailyIntakeById()
        {
            _dailyIntakeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            bool dailyIntakeFound = await sut.AnyAsync(It.IsAny<Guid>());

            dailyIntakeFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingDailyIntakeById()
        {
            _dailyIntakeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            bool dailyIntakeFound = await sut.AnyAsync(It.IsAny<Guid>());

            dailyIntakeFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectExistingDailyIntakeByName()
        {
            _dailyIntakeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(true);
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            bool dailyIntakeFound = await sut.AnyAsync(It.IsAny<string>());

            dailyIntakeFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingDailyIntakeByName()
        {
            _dailyIntakeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(false);
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            bool dailyIntakeFound = await sut.AnyAsync(It.IsAny<string>());

            dailyIntakeFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanFetchDailyIntakeById()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = new("Daily intake: Fish and spice");
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Mocker.MockDailyIntake(dailyIntake));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<Guid>());

            dailyIntakeDTO?.Name.Should().Be(dailyIntake.Name);
            dailyIntakeDTO?.DailyIntakeEntityDTOs.Should().HaveCount(dailyIntake.Entities.Count());
            for (int index = 0; index < dailyIntakeDTO?.DailyIntakeEntityDTOs.Length; ++index)
            {
                ValidateDailyIntakeEntity(dailyIntakeDTO.DailyIntakeEntityDTOs[index], index, dailyIntake, _baseEntities);
            }
        }

        [Fact]
        public async Task CannotFetchNonexistingDailyIntakeById()
        {
            _dailyIntakeRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<DailyIntake?>(null));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<Guid>());

            dailyIntakeDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchDailyIntakeByName()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = new("Daily intake: Fish and spice");
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(Mocker.MockDailyIntake(dailyIntake));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<string>());

            dailyIntakeDTO?.Name.Should().Be(dailyIntake.Name);
            dailyIntakeDTO?.DailyIntakeEntityDTOs.Should().HaveCount(dailyIntake.Entities.Count());
            for (int index = 0; index < dailyIntakeDTO?.DailyIntakeEntityDTOs.Length; ++index)
            {
                ValidateDailyIntakeEntity(dailyIntakeDTO.DailyIntakeEntityDTOs[index], index, dailyIntake, _baseEntities);
            }
        }

        [Fact]
        public async Task CannotFetchNonexistingDailyIntakeByName()
        {
            _dailyIntakeRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<DailyIntake?>(null));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<string>());

            dailyIntakeDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchAllDailyIntakes()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            string[] names = new string[] {
                "Daily intake: Yippie",
                "Daily intake: Yum",
            };

            _dailyIntakeRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new DailyIntake[] {
                Mocker.MockDailyIntake(new DailyIntake(names[0])),
                Mocker.MockDailyIntake(new DailyIntake(names[1]))
            });

            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO[]? dailyIntakeDTOs = await sut.GetAllAsync();

            dailyIntakeDTOs.Should().HaveCount(names.Length);
            if (dailyIntakeDTOs != null)
            {
                foreach (DailyIntakeDTO dailyIntakeDTO in dailyIntakeDTOs)
                {
                    dailyIntakeDTO.Name.Should().ContainAny(names);
                }
            }
        }

        [Fact]
        public async Task CanAddDailyIntake()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = new("Daily intake: Fish and spice");
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<DailyIntake>())).ReturnsAsync(Mocker.MockDailyIntake(dailyIntake));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.AddAsync(dailyIntake);

            dailyIntakeDTO?.Name.Should().Be(dailyIntake.Name);
            dailyIntakeDTO?.DailyIntakeEntityDTOs.Should().HaveCount(dailyIntake.Entities.Count());
            for (int index = 0; index < dailyIntakeDTO?.DailyIntakeEntityDTOs.Length; ++index)
            {
                ValidateDailyIntakeEntity(dailyIntakeDTO.DailyIntakeEntityDTOs[index], index, dailyIntake, _baseEntities);
            }
        }

        [Fact]
        public async Task CannotAddDailyIntakeContainingInvalidEntityId()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = new("Daily intake: Fish and spice");
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);
            dailyIntake.AddEntity(Guid.NewGuid(), 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<DailyIntake>())).ReturnsAsync(Mocker.MockDailyIntake(dailyIntake));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.AddAsync(dailyIntake);

            dailyIntakeDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanUpdateDailyIntake()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = new("Daily intake: Fish and spice");
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(true);
            _dailyIntakeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _dailyIntakeRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<DailyIntake>())).ReturnsAsync(Mocker.MockDailyIntake(dailyIntake));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.UpdateAsync(dailyIntake);

            dailyIntakeDTO?.Name.Should().Be(dailyIntake.Name);
            dailyIntakeDTO?.DailyIntakeEntityDTOs.Should().HaveCount(dailyIntake.Entities.Count());
            for (int index = 0; index < dailyIntakeDTO?.DailyIntakeEntityDTOs.Length; ++index)
            {
                ValidateDailyIntakeEntity(dailyIntakeDTO.DailyIntakeEntityDTOs[index], index, dailyIntake, _baseEntities);
            }
            _dailyIntakeRepositoryMock.Verify(mock => mock.DeleteAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Fact]
        public async Task CannotUpdateDailyIntakeThatDoesNotExist()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = new("Daily intake: Fish and spice");
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(false);
            _dailyIntakeRepositoryMock.Setup(mock => mock.UpdateAsync(It.IsAny<DailyIntake>())).ReturnsAsync(Mocker.MockDailyIntake(dailyIntake));
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.UpdateAsync(dailyIntake);

            dailyIntakeDTO.Should().BeNull();
            _dailyIntakeRepositoryMock.Verify(mock => mock.DeleteAsync(It.IsAny<DailyIntake>()), Times.Never());
        }

        [Fact]
        public async Task CanDeleteExistingDailyIntake()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = Mocker.MockDailyIntake(new("Daily intake: Fish and spice"));
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            bool dailyIntakeDeleted = await sut.DeleteAsync(dailyIntake.Id);

            dailyIntakeDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDeleteNonexistingDailyIntake()
        {
            Contracts.LogAndThrowWhenNotSet(_recipe);

            DailyIntake dailyIntake = Mocker.MockDailyIntake(new("Daily intake: Fish and spice"));
            dailyIntake.AddEntity(_ingredients[0].Id, 30);
            dailyIntake.AddEntity(_ingredients[1].Id, 10);
            dailyIntake.AddEntity(_recipe.Id, 20);

            _dailyIntakeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            bool dailyIntakeDeleted = await sut.DeleteAsync(dailyIntake.Id);

            dailyIntakeDeleted.Should().BeFalse();
        }

        private void ValidateDailyIntakeEntity(DailyIntakeEntityDTO validatedEntity, int expectedSortOrder, DailyIntake dailyIntake, BaseEntity[] baseEntities)
        {
            DailyIntakeEntity dailyIntakeEntity = dailyIntake.Entities.First(entity => entity.EntityId == validatedEntity.EntityId);
            BaseEntity baseEntity = baseEntities.First(entity => entity.Id == dailyIntakeEntity.EntityId);

            if (baseEntity is Ingredient ingredient)
            {
                validatedEntity.Name.Should().Be(ingredient.Name);
                validatedEntity.Amount.Should().Be(dailyIntakeEntity.Amount);
                validatedEntity.Fat.Should().Be(ingredient.Fat);
                validatedEntity.Carbohydrates.Should().Be(ingredient.Carbohydrates);
                validatedEntity.Protein.Should().Be(ingredient.Protein);
                validatedEntity.Calories.Should().Be(ingredient.Calories);
                validatedEntity.SortOrder.Should().Be(expectedSortOrder);
                validatedEntity.IsRecipe.Should().Be(false);
                validatedEntity.EntityId.Should().Be(dailyIntakeEntity.EntityId);
            }
            else if (baseEntity is Recipe recipe)
            {
                validatedEntity.Name.Should().Be(recipe.Name);
                validatedEntity.Amount.Should().Be(dailyIntakeEntity.Amount);
                validatedEntity.Fat.Should().Be(recipe.GetFatAmount());
                validatedEntity.Carbohydrates.Should().Be(recipe.GetCarbohydrateAmount());
                validatedEntity.Protein.Should().Be(recipe.GetProteinAmount());
                validatedEntity.Calories.Should().Be(recipe.GetCalorieAmount());
                validatedEntity.SortOrder.Should().Be(expectedSortOrder);
                validatedEntity.IsRecipe.Should().Be(true);
                validatedEntity.EntityId.Should().Be(dailyIntakeEntity.EntityId);
            }
        }
    }
}