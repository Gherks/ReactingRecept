using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class DailyIntakeRepository : RepositoryBase<DailyIntake>, IDailyIntakeRepository
{
    public DailyIntakeRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }
}
