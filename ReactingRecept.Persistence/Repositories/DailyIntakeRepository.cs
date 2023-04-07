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
}
