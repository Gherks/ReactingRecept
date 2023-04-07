using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Application.Interfaces.Persistence;

public interface IDailyIntakeRepository : IAsyncRepository<DailyIntake>
{
    Task<bool> AnyAsync(string name);
    Task<DailyIntake?> GetByNameAsync(string name);
}
