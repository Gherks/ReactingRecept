using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Threading.Tasks;
using Xunit;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Persistence.IntegrationTests;

public class RepositoryBaseTests : IDisposable
{
    private readonly ReactingReceptContext _context;

    private Category[] _categories = Array.Empty<Category>();

    public RepositoryBaseTests()
    {
        _context = TestDatabaseCreator.Create();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task CanAddEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? ingredient = await baseRepository.AddAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));

        ingredient.Should().NotBeNull();
        ingredient?.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CanAddManyEntities()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? addedIngredients = await baseRepository.AddManyAsync(ingredients);

        addedIngredients.Should().NotBeNull();

        if (addedIngredients != null)
        {
            addedIngredients.Should().HaveCount(ingredients.Length);

            foreach (Ingredient ingredient in ingredients)
            {
                addedIngredients.Should().Contain(addedIngredient => addedIngredient.Name == ingredient.Name);
            }
        }
    }

    [Fact]
    public async Task CanAcknowledgeExistanceOfEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? addedIngredient = await baseRepository.AddAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredient);

        bool ingredientFound = await baseRepository.AnyAsync(addedIngredient.Id);

        ingredientFound.Should().BeTrue();
    }

    [Fact]
    public async Task CannotAcknowledgeExistanceOfNonexistingEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        bool ingredientFound = await baseRepository.AnyAsync(Guid.NewGuid());

        ingredientFound.Should().BeFalse();
    }

    [Fact]
    public async Task CanGetEntityById()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? addedIngredient = await baseRepository.AddAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredient);

        Ingredient? ingredient = await baseRepository.GetByIdAsync(addedIngredient.Id);

        ingredient.Should().NotBeNull();
        ingredient?.Id.Should().Be(addedIngredient.Id);
    }

    [Fact]
    public async Task CannotGetNonexistingEntityById()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? ingredient = await baseRepository.GetByIdAsync(Guid.NewGuid());

        ingredient.Should().BeNull();
    }

    [Fact]
    public async Task CanGetAllEntities()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? addedIngredients = await baseRepository.AddManyAsync(ingredients);
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

        Ingredient[]? allIngredients = await baseRepository.GetAllAsync();

        allIngredients.Should().NotBeNull();

        if (allIngredients != null)
        {
            allIngredients.Should().HaveCount(addedIngredients.Length);

            foreach (Ingredient ingredient in addedIngredients)
            {
                allIngredients.Should().Contain(allIngredient => allIngredient.Name == ingredient.Name);
            }
        }
    }

    [Fact]
    public async Task CanUpdateEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? addedIngredient = await baseRepository.AddAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredient);

        string updatedName = "NewIngredientName";
        addedIngredient.SetName(updatedName);

        Ingredient? updatedIngredient = await baseRepository.UpdateAsync(addedIngredient);

        updatedIngredient.Should().NotBeNull();
        updatedIngredient?.Name.Should().Be(updatedName);
    }

    [Fact]
    public async Task CannotUpdateNonexistingEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? ingredient = await baseRepository.UpdateAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));
        Contracts.LogAndThrowWhenNothingWasReceived(ingredient);

        ingredient.Id.Should().BeEmpty();
    }

    [Fact]
    public async Task CanUpdateManyEntites()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? addedIngredients = await baseRepository.AddManyAsync(ingredients);
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

        string updatedName = "NewIngredientName";

        foreach (Ingredient ingredient in addedIngredients)
        {
            ingredient.SetName(updatedName);
        }

        Ingredient[]? updatedIngredients = await baseRepository.UpdateManyAsync(addedIngredients);

        updatedIngredients.Should().NotBeNull();

        if (updatedIngredients != null)
        {
            updatedIngredients.Should().HaveCount(addedIngredients.Length);

            foreach (Ingredient ingredient in updatedIngredients)
            {
                ingredient.Name.Should().Be(updatedName);
            }
        }
    }

    [Fact]
    public async Task CannotUpdateManyNonexistingEntities()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? updatedIngredients = await baseRepository.UpdateManyAsync(ingredients);
        Contracts.LogAndThrowWhenNothingWasReceived(updatedIngredients);

        updatedIngredients.Should().BeEmpty();
    }

    [Fact]
    public async Task CannotUpdateManyEntitiesWithNonexistingEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? addedIngredients = await baseRepository.AddManyAsync(ingredients);
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

        addedIngredients[0] = new Ingredient("Shoes", 1, 1, 1, 1, _categories[0]);

        string updatedName = "NewIngredientName";

        foreach (Ingredient ingredient in addedIngredients)
        {
            ingredient.SetName(updatedName);
        }

        Ingredient[]? updatedIngredients = await baseRepository.UpdateManyAsync(addedIngredients);

        updatedIngredients.Should().BeEmpty();
    }

    [Fact]
    public async Task CanDeleteEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient? addedIngredient = await baseRepository.AddAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredient);

        bool ingredientDeleted = await baseRepository.DeleteAsync(addedIngredient);

        ingredientDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task CannotDeleteNonexistingEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        bool ingredientDeleted = await baseRepository.DeleteAsync(new Ingredient("IngredientName", 1, 1, 1, 1, _categories[0]));

        ingredientDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task CanDeleteManyEntities()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? addedIngredients = await baseRepository.AddManyAsync(ingredients);
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

        bool ingredientsDeleted = await baseRepository.DeleteManyAsync(addedIngredients);

        ingredientsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task CannotDeleteManyNonexistingEntities()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        bool ingredientsDeleted = await baseRepository.DeleteManyAsync(ingredients);

        ingredientsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task CannotDeleteManyEntitiesWithNonexistingEntity()
    {
        RepositoryBase<Ingredient> baseRepository = await RepositoryBaseTestSetup();

        Ingredient[]? ingredients = new Ingredient[]
        {
            new Ingredient("Milk", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Egg", 1, 1, 1, 1, _categories[0]),
            new Ingredient("Bread", 1, 1, 1, 1, _categories[0]),
        };

        Ingredient[]? addedIngredients = await baseRepository.AddManyAsync(ingredients);
        Contracts.LogAndThrowWhenNothingWasReceived(addedIngredients);

        addedIngredients[0] = new Ingredient("Shoes", 1, 1, 1, 1, _categories[0]);

        bool ingredientsDeleted = await baseRepository.DeleteManyAsync(addedIngredients);

        ingredientsDeleted.Should().BeFalse();
    }

    private async Task<RepositoryBase<Ingredient>> RepositoryBaseTestSetup()
    {
        CategoryRepository categoryRepository = new(_context);

        Category[]? categories = await categoryRepository.ListAllOfTypeAsync(CategoryType.Ingredient);
        Contracts.LogAndThrowWhenNothingWasReceived(categories);

        _categories = categories;

        return new RepositoryBase<Ingredient>(_context);
    }
}
