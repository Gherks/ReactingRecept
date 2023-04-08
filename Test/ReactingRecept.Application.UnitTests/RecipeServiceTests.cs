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
    public class RecipeServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = Mocker.GetCategoryRepositoryMock();
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock = new();

        [Fact]
        public async Task CanDetectExistingRecipeById()
        {
            _recipeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool recipeFound = await sut.AnyAsync(It.IsAny<Guid>());

            recipeFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingRecipeById()
        {
            _recipeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool recipeFound = await sut.AnyAsync(It.IsAny<Guid>());

            recipeFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanDetectExistingRecipeByName()
        {
            _recipeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(true);
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool recipeFound = await sut.AnyAsync(It.IsAny<string>());

            recipeFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingRecipeByName()
        {
            _recipeRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<string>())).ReturnsAsync(false);
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool recipeFound = await sut.AnyAsync(It.IsAny<string>());

            recipeFound.Should().BeFalse();
        }

        [Fact]
        public async Task CanFetchRecipeById()
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

            RecipeDTO desiredRecipeDTO = new(
                "Fishers mash",
                "Fish the fishy fish, yum yum",
                3,
                "Fishy recipes",
                CategoryType.Recipe,
                ingredientMeasurementDTOs
            );

            _recipeRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(Mocker.MockRecipe(desiredRecipeDTO));
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO? recipeDTO = await sut.GetAsync(It.IsAny<Guid>());

            EntityValidators.Validate(recipeDTO, desiredRecipeDTO);
        }

        [Fact]
        public async Task CannotFetchNonexistingRecipeById()
        {
            _recipeRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Recipe?>(null));
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO? recipeDTO = await sut.GetAsync(It.IsAny<Guid>());

            recipeDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchRecipeByName()
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

            RecipeDTO desiredRecipeDTO = new(
                "Fishers mash",
                "Fish the fishy fish, yum yum",
                3,
                "Fishy recipes",
                CategoryType.Recipe,
                ingredientMeasurementDTOs
            );

            _recipeRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(Mocker.MockRecipe(desiredRecipeDTO));
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO? recipeDTO = await sut.GetAsync(It.IsAny<string>());

            EntityValidators.Validate(recipeDTO, desiredRecipeDTO);
        }

        [Fact]
        public async Task CannotFetchNonexistingRecipeByName()
        {
            _recipeRepositoryMock.Setup(mock => mock.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult<Recipe?>(null));
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO? recipeDTO = await sut.GetAsync(It.IsAny<string>());

            recipeDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchAllRecipes()
        {
            RecipeDTO[] desiredRecipeDTOs = new RecipeDTO[] {
                new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Fishy recipes", CategoryType.Recipe, Array.Empty<IngredientMeasurementDTO>()),
                new("Tomato mash", "Tomatoes tomaoes", 3, "Veggie recipes", CategoryType.Recipe, Array.Empty<IngredientMeasurementDTO>()),
            };

            _recipeRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new Recipe[] {
                Mocker.MockRecipe(desiredRecipeDTOs[0]),
                Mocker.MockRecipe(desiredRecipeDTOs[1]),
            });
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO[]? recipeDTOs = await sut.GetAllAsync();

            EntityValidators.Validate(recipeDTOs?[0], desiredRecipeDTOs?[0]);
            EntityValidators.Validate(recipeDTOs?[1], desiredRecipeDTOs?[1]);
        }

        [Fact]
        public async Task CanAddRecipe()
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

            RecipeDTO desiredRecipeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.Recipe, ingredientMeasurementDTOs);
            _recipeRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<Recipe>())).ReturnsAsync(Mocker.MockRecipe(desiredRecipeDTO));
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO? recipeDTO = await sut.AddAsync(desiredRecipeDTO);

            EntityValidators.Validate(recipeDTO, desiredRecipeDTO);
        }

        [Fact]
        public async Task CanUpdateRecipe()
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

            RecipeDTO desiredRecipeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.Recipe, ingredientMeasurementDTOs);
            _recipeRepositoryMock.Setup(mock => mock.UpdateAsync(It.IsAny<Recipe>())).ReturnsAsync(Mocker.MockRecipe(desiredRecipeDTO));
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            RecipeDTO? recipeDTO = await sut.UpdateAsync(desiredRecipeDTO);

            EntityValidators.Validate(recipeDTO, desiredRecipeDTO);
        }

        [Fact]
        public async Task CanDeleteExistingRecipe()
        {
            RecipeDTO recipeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.Recipe, Array.Empty<IngredientMeasurementDTO>());
            _recipeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Recipe>())).ReturnsAsync(true);
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool recipeDeleted = await sut.DeleteAsync(recipeDTO);

            recipeDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDeleteNonexistingRecipe()
        {
            RecipeDTO recipeDTO = new("Fishers mash", "Fish the fishy fish, yum yum", 3, "Meal", CategoryType.Recipe, Array.Empty<IngredientMeasurementDTO>());
            _recipeRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Recipe>())).ReturnsAsync(false);
            IRecipeService sut = new RecipeService(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool recipeDeleted = await sut.DeleteAsync(recipeDTO);

            recipeDeleted.Should().BeFalse();
        }
    }
}