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
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();

        [Fact]
        public async Task CanDetectExistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientFound = await sut.AnyAsync(It.IsAny<Guid>());

            ingredientFound.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDetectNonexistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

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
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

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
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO? ingredientDTO = await sut.GetByIdAsync(It.IsAny<Guid>());

            ingredientDTO.Should().BeNull();
        }

        [Fact]
        public async Task CanFetchAllIngredients()
        {
            string name1 = "Fish";
            string categoryName1 = "Fishies";
            CategoryType categoryType1 = CategoryType.Ingredient;

            string name2 = "Cucumber";
            string categoryName2 = "Veggies";
            CategoryType categoryType2 = CategoryType.Ingredient;

            _ingredientRepositoryMock.Setup(mock => mock.GetAllAsync()).ReturnsAsync(new Ingredient[]
            {
                Mocker.MockIngredient(name1, categoryName1, categoryType1),
                Mocker.MockIngredient(name2, categoryName2, categoryType2),
            });
                
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            IngredientDTO[]? ingredientDTOs = await sut.GetAllAsync();

            ingredientDTOs?[0].Name.Should().Be(name1);
            ingredientDTOs?[0].Fat.Should().BePositive();
            ingredientDTOs?[0].Carbohydrates.Should().BePositive();
            ingredientDTOs?[0].Protein.Should().BePositive();
            ingredientDTOs?[0].Calories.Should().BePositive();
            ingredientDTOs?[0].CategoryName.Should().Be(categoryName1);
            ingredientDTOs?[0].CategoryType.Should().Be(categoryType1);

            ingredientDTOs?[1].Name.Should().Be(name2);
            ingredientDTOs?[1].Fat.Should().BePositive();
            ingredientDTOs?[1].Carbohydrates.Should().BePositive();
            ingredientDTOs?[1].Protein.Should().BePositive();
            ingredientDTOs?[1].Calories.Should().BePositive();
            ingredientDTOs?[1].CategoryName.Should().Be(categoryName2);
            ingredientDTOs?[1].CategoryType.Should().Be(categoryType2);
        }

        [Fact]
        public async Task CanAddIngredient()
        {
            string name = "Fish";
            double fat = 1;
            double carbohydrates = 1;
            double protein = 1;
            double calories = 1;
            string categoryName = "Fishies";
            CategoryType categoryType = CategoryType.Ingredient;
            int categorySortOrder = 1;

            _ingredientRepositoryMock.Setup(mock => mock.AddAsync(It.IsAny<Ingredient>())).ReturnsAsync(Mocker.MockIngredient(name, fat, carbohydrates, protein, calories, categoryName, categoryType));
            _categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(It.IsAny<CategoryType>())).ReturnsAsync(new Category[] {
                Mocker.MockCategory(categoryName, categoryType, categorySortOrder)
            });

            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);
            IngredientDTO ingredientDTO = new(name, fat, carbohydrates, protein, calories, categoryName, categoryType);

            IngredientDTO? addedIngredientDTO = await sut.AddAsync(ingredientDTO);

            addedIngredientDTO?.Name.Should().Be(ingredientDTO.Name);
            addedIngredientDTO?.Fat.Should().Be(fat);
            addedIngredientDTO?.Carbohydrates.Should().Be(carbohydrates);
            addedIngredientDTO?.Protein.Should().Be(protein);
            addedIngredientDTO?.Calories.Should().Be(calories);
            addedIngredientDTO?.CategoryName.Should().Be(ingredientDTO.CategoryName);
            addedIngredientDTO?.CategoryType.Should().Be(ingredientDTO.CategoryType);
        }

        [Fact]
        public async Task CanAddManyIngredients()
        {
            string name1 = "Fish";
            double fat1 = 1;
            double carbohydrates1 = 1;
            double protein1 = 1;
            double calories1 = 1;
            string categoryName1 = "Fishies";
            CategoryType categoryType1 = CategoryType.Ingredient;
            int categorySortOrder1 = 1;

            string name2 = "Cucumber";
            double fat2 = 1;
            double carbohydrates2 = 1;
            double protein2 = 1;
            double calories2 = 1;
            string categoryName2 = "Veggies";
            CategoryType categoryType2 = CategoryType.Ingredient;
            int categorySortOrder2 = 2;

            _ingredientRepositoryMock.Setup(mock => mock.AddManyAsync(It.IsAny<Ingredient[]>())).ReturnsAsync(new Ingredient[] {
                Mocker.MockIngredient(name1, fat1, carbohydrates1, protein1, calories1, categoryName1, categoryType1),
                Mocker.MockIngredient(name2, fat2, carbohydrates2, protein2, calories2, categoryName2, categoryType2),
            });

            _categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(It.IsAny<CategoryType>())).ReturnsAsync(new Category[] {
                Mocker.MockCategory(categoryName1, categoryType1, categorySortOrder1),
                Mocker.MockCategory(categoryName2, categoryType2, categorySortOrder2),
            });

            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);
            IngredientDTO[] ingredientDTOs = new IngredientDTO[]{
                new(name1, fat1, carbohydrates1, protein1, calories1, categoryName1, categoryType1),
                new(name2, fat2, carbohydrates2, protein2, calories2, categoryName2, categoryType2)
            };

            IngredientDTO[]? addedIngredientDTOs = await sut.AddManyAsync(ingredientDTOs);

            IngredientDTO addedIngredientDTO1 = addedIngredientDTOs!.First(ingredient => ingredient.Name == name1);
            IngredientDTO addedIngredientDTO2 = addedIngredientDTOs!.First(ingredient => ingredient.Name == name2);

            addedIngredientDTO1.Fat.Should().Be(fat1);
            addedIngredientDTO1.Carbohydrates.Should().Be(carbohydrates1);
            addedIngredientDTO1.Protein.Should().Be(protein1);
            addedIngredientDTO1.Calories.Should().Be(calories1);
            addedIngredientDTO1.CategoryName.Should().Be(categoryName1);
            addedIngredientDTO1.CategoryType.Should().Be(categoryType1);

            addedIngredientDTO2.Fat.Should().Be(fat2);
            addedIngredientDTO2.Carbohydrates.Should().Be(carbohydrates2);
            addedIngredientDTO2.Protein.Should().Be(protein2);
            addedIngredientDTO2.Calories.Should().Be(calories2);
            addedIngredientDTO2.CategoryName.Should().Be(categoryName2);
            addedIngredientDTO2.CategoryType.Should().Be(categoryType2);
        }

        [Fact]
        public async Task CanUpdateIngredient()
        {
            string name = "Fish";
            double fat = 2;
            double carbohydrates = 2;
            double protein = 2;
            double calories = 2;
            string categoryName = "Fishies";
            CategoryType categoryType = CategoryType.Ingredient;
            int categorySortOrder = 1;

            _ingredientRepositoryMock.Setup(mock => mock.UpdateAsync(It.IsAny<Ingredient>())).ReturnsAsync(Mocker.MockIngredient(name, fat, carbohydrates, protein, calories, categoryName, categoryType));
            _categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(It.IsAny<CategoryType>())).ReturnsAsync(new Category[] {
                Mocker.MockCategory(categoryName, categoryType, categorySortOrder)
            });

            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);
            IngredientDTO ingredientDTO = new(name, fat, carbohydrates, protein, calories, categoryName, categoryType);

            IngredientDTO? updatedIngredientDTO = await sut.UpdateAsync(ingredientDTO);

            updatedIngredientDTO?.Name.Should().Be(ingredientDTO.Name);
            updatedIngredientDTO?.Fat.Should().Be(fat);
            updatedIngredientDTO?.Carbohydrates.Should().Be(carbohydrates);
            updatedIngredientDTO?.Protein.Should().Be(protein);
            updatedIngredientDTO?.Calories.Should().Be(calories);
            updatedIngredientDTO?.CategoryName.Should().Be(ingredientDTO.CategoryName);
            updatedIngredientDTO?.CategoryType.Should().Be(ingredientDTO.CategoryType);
        }

        [Fact]
        public async Task CanUpdateManyIngredients()
        {
            string name1 = "Fish";
            double fat1 = 1;
            double carbohydrates1 = 1;
            double protein1 = 1;
            double calories1 = 1;
            string categoryName1 = "Fishies";
            CategoryType categoryType1 = CategoryType.Ingredient;
            int categorySortOrder1 = 1;

            string name2 = "Cucumber";
            double fat2 = 1;
            double carbohydrates2 = 1;
            double protein2 = 1;
            double calories2 = 1;
            string categoryName2 = "Veggies";
            CategoryType categoryType2 = CategoryType.Ingredient;
            int categorySortOrder2 = 2;

            _ingredientRepositoryMock.Setup(mock => mock.UpdateManyAsync(It.IsAny<Ingredient[]>())).ReturnsAsync(new Ingredient[] {
                Mocker.MockIngredient(name1, fat1, carbohydrates1, protein1, calories1, categoryName1, categoryType1),
                Mocker.MockIngredient(name2, fat2, carbohydrates2, protein2, calories2, categoryName2, categoryType2),
            });

            _categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(It.IsAny<CategoryType>())).ReturnsAsync(new Category[] {
                Mocker.MockCategory(categoryName1, categoryType1, categorySortOrder1),
                Mocker.MockCategory(categoryName2, categoryType2, categorySortOrder2),
            });

            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);
            IngredientDTO[] ingredientDTOs = new IngredientDTO[]{
                new(name1, fat1, carbohydrates1, protein1, calories1, categoryName1, categoryType1),
                new(name2, fat2, carbohydrates2, protein2, calories2, categoryName2, categoryType2)
            };

            IngredientDTO[]? updatedIngredientDTOs = await sut.UpdateManyAsync(ingredientDTOs);

            IngredientDTO updatedIngredientDTO1 = updatedIngredientDTOs!.First(ingredient => ingredient.Name == name1);
            IngredientDTO updatedIngredientDTO2 = updatedIngredientDTOs!.First(ingredient => ingredient.Name == name2);

            updatedIngredientDTO1.Fat.Should().Be(fat1);
            updatedIngredientDTO1.Carbohydrates.Should().Be(carbohydrates1);
            updatedIngredientDTO1.Protein.Should().Be(protein1);
            updatedIngredientDTO1.Calories.Should().Be(calories1);
            updatedIngredientDTO1.CategoryName.Should().Be(categoryName1);
            updatedIngredientDTO1.CategoryType.Should().Be(categoryType1);

            updatedIngredientDTO2.Fat.Should().Be(fat2);
            updatedIngredientDTO2.Carbohydrates.Should().Be(carbohydrates2);
            updatedIngredientDTO2.Protein.Should().Be(protein2);
            updatedIngredientDTO2.Calories.Should().Be(calories2);
            updatedIngredientDTO2.CategoryName.Should().Be(categoryName2);
            updatedIngredientDTO2.CategoryType.Should().Be(categoryType2);
        }

        [Fact]
        public async Task CanDeleteExistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Ingredient>())).ReturnsAsync(true);
            _categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(It.IsAny<CategoryType>())).ReturnsAsync(new Category[] {
                Mocker.MockCategory("Fishy", CategoryType.Ingredient, 1)
            });
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientDeleted = await sut.DeleteAsync(new IngredientDTO("Fish", 1, 1, 1, 1, "Fishy", CategoryType.Ingredient));

            ingredientDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task CannotDeleteNonexistingIngredient()
        {
            _ingredientRepositoryMock.Setup(mock => mock.DeleteAsync(It.IsAny<Ingredient>())).ReturnsAsync(false);
            _categoryRepositoryMock.Setup(mock => mock.GetManyOfTypeAsync(It.IsAny<CategoryType>())).ReturnsAsync(new Category[] {
                Mocker.MockCategory("Fishy", CategoryType.Ingredient, 1)
            });
            IIngredientService sut = new IngredientService(_ingredientRepositoryMock.Object, _categoryRepositoryMock.Object);

            bool ingredientDeleted = await sut.DeleteAsync(new IngredientDTO("Fish", 1, 1, 1, 1, "Fishy", CategoryType.Ingredient));

            ingredientDeleted.Should().BeFalse();
        }
    }
}