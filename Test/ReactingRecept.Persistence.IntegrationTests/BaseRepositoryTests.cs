using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReactingRecept.Persistence.IntegrationTests;

public class RepositoryBaseTests : IDisposable
{
    private readonly TestFramework _testFramework = new();

    public void Dispose()
    {
        _testFramework.Dispose();
    }

    [Fact]
    public async Task CanAcknowledgeExistanceOfEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        bool categoryFound = await baseRepository.AnyAsync(_testFramework.AllCategories[0].Id);

        categoryFound.Should().BeTrue();
    }

    [Fact]
    public async Task CannotAcknowledgeExistanceOfNonexistingEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();

        bool ingredientFound = await baseRepository.AnyAsync(Guid.NewGuid());

        ingredientFound.Should().BeFalse();
    }

    [Fact]
    public async Task CanGetEntityById()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Category? category = await baseRepository.GetByIdAsync(_testFramework.AllCategories[0].Id);

        category?.Id.Should().Be(_testFramework.AllCategories[0].Id);
        category?.Name.Should().Be(_testFramework.AllCategories[0].Name);
        category?.Type.Should().Be(_testFramework.AllCategories[0].Type);
        category?.SortOrder.Should().Be(_testFramework.AllCategories[0].SortOrder);
    }

    [Fact]
    public async Task CannotGetNonexistingEntityById()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();

        Category? category = await baseRepository.GetByIdAsync(Guid.NewGuid());

        category.Should().BeNull();
    }

    [Fact]
    public async Task CanGetAllEntities()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Category[]? categories = await baseRepository.GetAllAsync();

        categories.Should().HaveCount(_testFramework.AllCategories.Length);

        foreach (Category existingCategory in _testFramework.AllCategories)
        {
            categories.Should().Contain(category => category.Name == existingCategory.Name);
        }
    }

    [Fact]
    public async Task CanAddEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Category newCategory = _testFramework.CreateNewCategory();

        Category? category = await baseRepository.AddAsync(_testFramework.CreateNewCategory());

        category?.Id.Should().NotBeEmpty();
        category?.Name.Should().Be(newCategory.Name);
        category?.Type.Should().Be(newCategory.Type);
        category?.SortOrder.Should().Be(newCategory.SortOrder);
    }

    [Fact]
    public async Task CanAddManyEntities()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Category[] initialCategories = _testFramework.CreateNewCategories();

        Category[]? addedCategories = await baseRepository.AddManyAsync(_testFramework.CreateNewCategories());

        addedCategories.Should().HaveCount(initialCategories.Length);
        foreach (Category initialCategory in initialCategories)
        {
            addedCategories.Should().Contain(addedIngredient => addedIngredient.Name == initialCategory.Name);
        }
    }

    [Fact]
    public async Task CanUpdateEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Category category = _testFramework.AllCategories[0];

        int updateSortOrder = 999;
        category.SetSortOrder(updateSortOrder);

        Category? updatedCategory = await baseRepository.UpdateAsync(category);

        updatedCategory?.SortOrder.Should().Be(updateSortOrder);
    }

    [Fact]
    public async Task CannotUpdateNonexistingEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();

        Category? updatedCategory = await baseRepository.UpdateAsync(_testFramework.CreateNewCategory());

        updatedCategory.Should().BeNull();
    }

    [Fact]
    public async Task CanUpdateManyEntites()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        int updateSortOrder = 999;

        foreach (Category category in _testFramework.AllCategories)
        {
            category.SetSortOrder(updateSortOrder);
        }

        Category[]? updatedCategories = await baseRepository.UpdateManyAsync(_testFramework.AllCategories);
        Contracts.LogAndThrowWhenNothingWasReceived(updatedCategories);

        updatedCategories.Should().HaveCount(_testFramework.AllCategories.Length);

        foreach (Category category in updatedCategories)
        {
            category.SortOrder.Should().Be(updateSortOrder);
        }
    }

    [Fact]
    public async Task CannotUpdateManyNonexistingEntities()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();

        Category[]? updatedCategories = await baseRepository.UpdateManyAsync(_testFramework.CreateNewCategories());
        Contracts.LogAndThrowWhenNothingWasReceived(updatedCategories);

        updatedCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task CannotUpdateManyEntitiesWithNonexistingEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        _testFramework.AllCategories[0] = _testFramework.CreateNewCategory();

        int updateSortOrder = 999;

        foreach (Category category in _testFramework.AllCategories)
        {
            category.SetSortOrder(updateSortOrder);
        }

        Category[]? updatedCategories = await baseRepository.UpdateManyAsync(_testFramework.AllCategories);

        updatedCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task CanDeleteEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        Category category = _testFramework.AllCategories[0];

        bool categoryDeleted = await baseRepository.DeleteAsync(category);
        Category? deletedCategory = await baseRepository.GetByIdAsync(category.Id);

        categoryDeleted.Should().BeTrue();
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public async Task CannotDeleteNonexistingEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();

        bool categoryDeleted = await baseRepository.DeleteAsync(_testFramework.CreateNewCategory());

        categoryDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task CanDeleteManyEntities()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        bool categoriesDeleted = await baseRepository.DeleteManyAsync(_testFramework.AllCategories);
        Category[]? categories = await baseRepository.GetAllAsync();

        categoriesDeleted.Should().BeTrue();
        categories.Should().BeEmpty();
    }

    [Fact]
    public async Task CannotDeleteManyNonexistingEntities()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();

        bool categoriesDeleted = await baseRepository.DeleteManyAsync(_testFramework.CreateNewCategories());

        categoriesDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task CannotDeleteManyEntitiesWithNonexistingEntity()
    {
        RepositoryBase<Category> baseRepository = await _testFramework.PrepareCategoryRepository();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllCategories);

        _testFramework.AllCategories[0] = _testFramework.CreateNewCategory();

        bool categoriesDeleted = await baseRepository.DeleteManyAsync(_testFramework.AllCategories);

        categoriesDeleted.Should().BeFalse();
    }
}
