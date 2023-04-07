using Microsoft.EntityFrameworkCore;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class DailyIntakeRepository : RepositoryBase<DailyIntake>, IDailyIntakeRepository
{
    public DailyIntakeRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }

    public async Task<bool> AnyAsync(string name)
    {
        try
        {
            return await _reactingReceptContext.DailyIntake.AnyAsync(dailyIntake => dailyIntake.Name.ToLower() == name.ToLower());
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed check existence of dailyIntake with name: {name}");
            return false;
        }
    }

    public async Task<DailyIntake?> GetByNameAsync(string name)
    {
        try
        {
            return await _reactingReceptContext.Set<DailyIntake>().FirstAsync(dailyIntake => dailyIntake.Name.ToLower() == name.ToLower());
        }
        catch (Exception /*exception*/)
        {
            //Log.Error(/*exception*/, $"Repository failed to fetch recipe with name: {name}");
            return null;
        }
    }

    public override async Task<DailyIntake[]?> AddManyAsync(DailyIntake[] dailyIntakes)
    {
        if (!NoDuplicatesInDailyIntakes(dailyIntakes))
        {
            return null;
        }

        return await base.AddManyAsync(dailyIntakes);
    }

    private bool NoDuplicatesInDailyIntakes(DailyIntake[] dailyIntakes)
    {
        List<string> dailyIntakeNames = dailyIntakes.Select(x => x.Name).ToList();

        return dailyIntakeNames.Count == dailyIntakeNames.Distinct().Count();
    }
}
