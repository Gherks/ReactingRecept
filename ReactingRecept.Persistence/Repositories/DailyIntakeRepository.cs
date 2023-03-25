using ReactingRecept.Domain;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class DailyIntakeRepository : RepositoryBase<DailyIntake>
{
    public DailyIntakeRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }
}
