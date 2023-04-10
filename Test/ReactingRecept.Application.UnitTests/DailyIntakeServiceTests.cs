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
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.UnitTests
{
    public class DailyIntakeServiceTests
    {
        private readonly Mock<IDailyIntakeRepository> _dailyIntakeRepositoryMock = new();
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock = new();
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock = new();

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
            IngredientDTO[] ingredientDTOs = new IngredientDTO[]
            {
                new("Fish", 1, 1, 1, 1, "Meat", CategoryType.Ingredient),
                new("Salt", 1, 1, 1, 1, "Other", CategoryType.Ingredient),
                new("Pepper", 1, 1, 1, 1, "Other", CategoryType.Ingredient)
            };

            IngredientMeasurementDTO[] ingredientMeasurementDTOs = new IngredientMeasurementDTO[]
            {
                new(3, MeasurementUnit.Gram, 3, "Fishies note", 1, ingredientDTOs[0]),
                new(1, MeasurementUnit.Kilogram, 1000, "Salt note", 2, ingredientDTOs[1]),
                new(300, MeasurementUnit.Gram, 300, "Pepper note", 3, ingredientDTOs[2]),
            };

            RecipeDTO recipeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.Recipe, ingredientMeasurementDTOs);

            Ingredient[] ingredients = Mocker.MockIngredients(ingredientDTOs);
            Recipe recipe = Mocker.MockRecipe(recipeDTO);

            BaseEntity[] baseEntities = ingredients.Concat(new BaseEntity[] { recipe }).ToArray();

            string name = "Daily intake: Fish and spice";

            AddDailyIntakeEntryCommand[] addDailyIntakeEntryCommands = new AddDailyIntakeEntryCommand[]
            {
                new AddDailyIntakeEntryCommand(ingredients[0].Id, 30, 1),
                new AddDailyIntakeEntryCommand(ingredients[1].Id, 10, 2),
                new AddDailyIntakeEntryCommand(recipe.Id, 20, 0),
            };

            _dailyIntakeRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Mocker.MockDailyIntake(name, addDailyIntakeEntryCommands));
            _ingredientRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(ingredients);
            _recipeRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new Recipe[] { recipe });
            IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object);

            DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<Guid>());

            dailyIntakeDTO?.Name.Should().Be(name);
            dailyIntakeDTO?.DailyIntakeEntityDTOs.Should().HaveCount(addDailyIntakeEntryCommands.Length);

            for(int index = 0; index < dailyIntakeDTO?.DailyIntakeEntityDTOs.Length; ++index)
            {
                ValidateDailyIntakeEntity(dailyIntakeDTO.DailyIntakeEntityDTOs[index], index, addDailyIntakeEntryCommands, baseEntities);
            }
        }

        private void ValidateDailyIntakeEntity(DailyIntakeEntityDTO validatedEntity, int expectedSortOrder, AddDailyIntakeEntryCommand[] addDailyIntakeEntryCommands, BaseEntity[] baseEntities)
        {
            AddDailyIntakeEntryCommand addDailyIntakeEntryCommand = addDailyIntakeEntryCommands.First(entity => entity.EntityId == validatedEntity.EntityId);
            BaseEntity baseEntity = baseEntities.First(entity => entity.Id == addDailyIntakeEntryCommand.EntityId);

            if (baseEntity is Ingredient ingredient)
            {
                validatedEntity.Name.Should().Be(ingredient.Name);
                validatedEntity.Amount.Should().Be(addDailyIntakeEntryCommand.Amount);
                validatedEntity.Fat.Should().Be(ingredient.Fat);
                validatedEntity.Carbohydrates.Should().Be(ingredient.Carbohydrates);
                validatedEntity.Protein.Should().Be(ingredient.Protein);
                validatedEntity.Calories.Should().Be(ingredient.Calories);
                validatedEntity.SortOrder.Should().Be(expectedSortOrder);
                validatedEntity.IsRecipe.Should().Be(false);
                validatedEntity.EntityId.Should().Be(addDailyIntakeEntryCommand.EntityId);
            }
            else if (baseEntity is Recipe recipe)
            {
                validatedEntity.Name.Should().Be(recipe.Name);
                validatedEntity.Amount.Should().Be(addDailyIntakeEntryCommand.Amount);
                validatedEntity.Fat.Should().Be(recipe.GetFatAmount());
                validatedEntity.Carbohydrates.Should().Be(recipe.GetCarbohydrateAmount());
                validatedEntity.Protein.Should().Be(recipe.GetProteinAmount());
                validatedEntity.Calories.Should().Be(recipe.GetCalorieAmount());
                validatedEntity.SortOrder.Should().Be(expectedSortOrder);
                validatedEntity.IsRecipe.Should().Be(true);
                validatedEntity.EntityId.Should().Be(addDailyIntakeEntryCommand.EntityId);
            }
        }

        //[Fact]
        //public async Task CannotFetchNonexistingDailyIntakeById()
        //{
        //    _dailyIntakeRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<DailyIntake?>(null));
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<Guid>());

        //    dailyIntakeDTO.Should().BeNull();
        //}

        //[Fact]
        //public async Task CanFetchDailyIntakeByName()
        //{
        //    IngredientDTO[] ingredientDTOs = new IngredientDTO[]
        //    {
        //        new("Fish", 1, 1, 1, 1, "Meat", CategoryType.Ingredient),
        //        new("Salt", 1, 1, 1, 1, "Other", CategoryType.Ingredient),
        //        new("Pepper", 1, 1, 1, 1, "Other", CategoryType.Ingredient)
        //    };

        //    IngredientMeasurementDTO[] ingredientMeasurementDTOs = new IngredientMeasurementDTO[]
        //    {
        //        new(3, MeasurementUnit.Gram, 3, "Fishies note", 1, ingredientDTOs[0]),
        //        new(1, MeasurementUnit.Kilogram, 1000, "Salt note", 2, ingredientDTOs[1]),
        //        new(300, MeasurementUnit.Gram, 300, "Pepper note", 3, ingredientDTOs[2]),
        //    };

        //    DailyIntakeDTO desiredDailyIntakeDTO = new(
        //        "Fishers mash",
        //        "Fish the fishy fish, yum yum",
        //        3,
        //        "Fishy dailyIntakes",
        //        CategoryType.DailyIntake,
        //        ingredientMeasurementDTOs
        //    );

        //    _dailyIntakeRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(Mocker.MockDailyIntake(desiredDailyIntakeDTO));
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<string>());

        //    EntityValidators.Validate(dailyIntakeDTO, desiredDailyIntakeDTO);
        //}

        //[Fact]
        //public async Task CannotFetchNonexistingDailyIntakeByName()
        //{
        //    _dailyIntakeRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<DailyIntake?>(null));
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    DailyIntakeDTO? dailyIntakeDTO = await sut.GetAsync(It.IsAny<string>());

        //    dailyIntakeDTO.Should().BeNull();
        //}

        //[Fact]
        //public async Task CanFetchAllDailyIntakes()
        //{
        //    DailyIntakeDTO[] desiredDailyIntakeDTOs = new DailyIntakeDTO[] {
        //        new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Fishy dailyIntakes", CategoryType.DailyIntake, Array.Empty<IngredientMeasurementDTO>()),
        //        new("Tomato mash", "Tomatoes tomaoes", 3, "Veggie dailyIntakes", CategoryType.DailyIntake, Array.Empty<IngredientMeasurementDTO>()),
        //    };

        //    _dailyIntakeRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new DailyIntake[] {
        //        Mocker.MockDailyIntake(desiredDailyIntakeDTOs[0]),
        //        Mocker.MockDailyIntake(desiredDailyIntakeDTOs[1]),
        //    });
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    DailyIntakeDTO[]? dailyIntakeDTOs = await sut.GetAllAsync();

        //    EntityValidators.Validate(dailyIntakeDTOs?[0], desiredDailyIntakeDTOs?[0]);
        //    EntityValidators.Validate(dailyIntakeDTOs?[1], desiredDailyIntakeDTOs?[1]);
        //}

        //[Fact]
        //public async Task CanAddDailyIntake()
        //{
        //    IngredientDTO[] ingredientDTOs = new IngredientDTO[]
        //    {
        //        new("Fish", 1, 1, 1, 1, "Meat", CategoryType.Ingredient),
        //        new("Salt", 1, 1, 1, 1, "Other", CategoryType.Ingredient),
        //        new("Pepper", 1, 1, 1, 1, "Other", CategoryType.Ingredient)
        //    };

        //    IngredientMeasurementDTO[] ingredientMeasurementDTOs = new IngredientMeasurementDTO[]
        //    {
        //        new(3, MeasurementUnit.Gram, 3, "Fishies note", 1, ingredientDTOs[0]),
        //        new(1, MeasurementUnit.Kilogram, 1000, "Salt note", 2, ingredientDTOs[1]),
        //        new(300, MeasurementUnit.Gram, 300, "Pepper note", 3, ingredientDTOs[2]),
        //    };

        //    DailyIntakeDTO desiredDailyIntakeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.DailyIntake, ingredientMeasurementDTOs);
        //    _dailyIntakeRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<DailyIntake>())).ReturnsAsync(Mocker.MockDailyIntake(desiredDailyIntakeDTO));
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    DailyIntakeDTO? dailyIntakeDTO = await sut.AddAsync(desiredDailyIntakeDTO);

        //    EntityValidators.Validate(dailyIntakeDTO, desiredDailyIntakeDTO);
        //}

        //[Fact]
        //public async Task CanUpdateDailyIntake()
        //{
        //    IngredientDTO[] ingredientDTOs = new IngredientDTO[]
        //    {
        //        new("Fish", 1, 1, 1, 1, "Meat", CategoryType.Ingredient),
        //        new("Salt", 1, 1, 1, 1, "Other", CategoryType.Ingredient),
        //        new("Pepper", 1, 1, 1, 1, "Other", CategoryType.Ingredient)
        //    };

        //    IngredientMeasurementDTO[] ingredientMeasurementDTOs = new IngredientMeasurementDTO[]
        //    {
        //        new(3, MeasurementUnit.Gram, 3, "Fishies note", 1, ingredientDTOs[0]),
        //        new(1, MeasurementUnit.Kilogram, 1000, "Salt note", 2, ingredientDTOs[1]),
        //        new(300, MeasurementUnit.Gram, 300, "Pepper note", 3, ingredientDTOs[2]),
        //    };

        //    DailyIntakeDTO desiredDailyIntakeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.DailyIntake, ingredientMeasurementDTOs);
        //    _dailyIntakeRepositoryMock.Setup(mock => mock.UpdateAsync(It.IsAny<DailyIntake>())).ReturnsAsync(Mocker.MockDailyIntake(desiredDailyIntakeDTO));
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    DailyIntakeDTO? dailyIntakeDTO = await sut.UpdateAsync(desiredDailyIntakeDTO);

        //    EntityValidators.Validate(dailyIntakeDTO, desiredDailyIntakeDTO);
        //}

        //[Fact]
        //public async Task CanDeleteExistingDailyIntake()
        //{
        //    DailyIntakeDTO dailyIntakeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.DailyIntake, Array.Empty<IngredientMeasurementDTO>());
        //    _dailyIntakeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<DailyIntake>())).ReturnsAsync(true);
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    bool dailyIntakeDeleted = await sut.DeleteAsync(dailyIntakeDTO);

        //    dailyIntakeDeleted.Should().BeTrue();
        //}

        //[Fact]
        //public async Task CannotDeleteNonexistingDailyIntake()
        //{
        //    DailyIntakeDTO dailyIntakeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.DailyIntake, Array.Empty<IngredientMeasurementDTO>());
        //    _dailyIntakeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<DailyIntake>())).ReturnsAsync(false);
        //    IDailyIntakeService sut = new DailyIntakeService(_dailyIntakeRepositoryMock.Object);

        //    bool dailyIntakeDeleted = await sut.DeleteAsync(dailyIntakeDTO);

        //    dailyIntakeDeleted.Should().BeFalse();
        //}
    }
}