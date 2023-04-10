using FluentAssertions;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Repositories;
using ReactingRecept.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReactingRecept.Persistence.IntegrationTests;

public class DailyIntakeRepositoryTests : IDisposable
{
    private readonly TestFramework _testFramework = new();

    private readonly string _nonexistingDailIntakeName = "I do not exist";

    public void Dispose()
    {
        _testFramework.Dispose();
    }

    [Fact]
    public async Task CanAcknowledgeExistanceOfDailyIntakeByName()
    {
        DailyIntakeRepository dailyIntakeRepository = await DailyIntakeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllDailyIntakes);

        bool dailyIntakeFound = await dailyIntakeRepository.AnyAsync(_testFramework.AllDailyIntakes[0].Name);

        dailyIntakeFound.Should().BeTrue();
    }

    [Fact]
    public async Task CannotAcknowledgeExistanceOfNonexistingDailyIntakeByName()
    {
        DailyIntakeRepository dailyIntakeRepository = await DailyIntakeRepositoryTestSetup();

        bool dailyIntakeFound = await dailyIntakeRepository.AnyAsync(_nonexistingDailIntakeName);

        dailyIntakeFound.Should().BeFalse();
    }

    [Fact]
    public async Task CanGetDailyIntakeByName()
    {
        DailyIntakeRepository dailyIntakeRepository = await DailyIntakeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllDailyIntakes);
        DailyIntake addedDailyIntake = _testFramework.AllDailyIntakes[0];

        DailyIntake? dailyIntake = await dailyIntakeRepository.GetByNameAsync(addedDailyIntake.Name);

        dailyIntake?.Id.Should().NotBeEmpty();
        dailyIntake?.Name.Should().Be(_testFramework.AllDailyIntakes[0].Name);

        for (int index = 0; index < dailyIntake?.Entities.Count; ++index)
        {
            dailyIntake.Entities[index].Id.Should().Be(addedDailyIntake.Entities[index].Id);
            dailyIntake.Entities[index].SortOrder.Should().Be(index);
            dailyIntake.Entities[index].EntityId.Should().Be(addedDailyIntake.Entities[index].EntityId);
        }
    }

    [Fact]
    public async Task CannotGetNonexistingDailyIntakeByName()
    {
        DailyIntakeRepository dailyIntakeRepository = await DailyIntakeRepositoryTestSetup();

        DailyIntake? dailyIntake = await dailyIntakeRepository.GetByNameAsync(_nonexistingDailIntakeName);

        dailyIntake.Should().BeNull();
    }

    [Fact]
    public async Task CannotAddManyDailyIntakesWithSameName()
    {
        DailyIntakeRepository dailyIntakeRepository = await DailyIntakeRepositoryTestSetup();
        DailyIntake[] createdDailyIntake = _testFramework.CreateDailyIntakes();
        DailyIntake[] duplicateDailyIntakes = new DailyIntake[]
        {
            createdDailyIntake[0],
            createdDailyIntake[0],
        };

        DailyIntake[]? addedDailyIntakes = await dailyIntakeRepository.AddManyAsync(duplicateDailyIntakes);

        addedDailyIntakes.Should().BeNull();
    }

    [Fact]
    public async Task CannotAddManyDailyIntakessWithSameNameInDatabase()
    {
        DailyIntakeRepository dailyIntakeRepository = await DailyIntakeRepositoryTestSetup();
        Contracts.LogAndThrowWhenNotSet(_testFramework.AllDailyIntakes);

        DailyIntake[] createdDailyIntakes = _testFramework.CreateDailyIntakes();
        DailyIntake[] dailyIntakesContainingDuplicate = new DailyIntake[]
        {
            createdDailyIntakes[0],
            _testFramework.AllDailyIntakes[0],
        };

        DailyIntake[]? addedRecipes = await dailyIntakeRepository.AddManyAsync(dailyIntakesContainingDuplicate);

        addedRecipes.Should().BeNull();
    }

    private async Task<DailyIntakeRepository> DailyIntakeRepositoryTestSetup()
    {
        await _testFramework.PrepareCategoryRepository();
        await _testFramework.PrepareIngredientRepository();
        await _testFramework.PrepareRecipeRepository();
        return await _testFramework.PrepareDailyIntakeRepository();
    }
}
