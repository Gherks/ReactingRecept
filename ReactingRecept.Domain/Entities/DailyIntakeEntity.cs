using ReactingRecept.Domain.Entities.Base;

namespace ReactingRecept.Domain.Entities;

public sealed class DailyIntakeEntity : BaseEntity
{
    public Guid EntityId { get; private set; }
    public int Amount { get; private set; }
    public int SortOrder { get; private set; }

    private DailyIntakeEntity() { }

    public DailyIntakeEntity(Guid entityId, int amount, int sortOrder)
    {
        EntityId = entityId;
        Amount = amount;
        SortOrder = sortOrder;
    }
}
