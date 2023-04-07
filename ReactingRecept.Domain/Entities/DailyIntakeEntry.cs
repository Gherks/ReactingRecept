using ReactingRecept.Domain.Entities.Base;

namespace ReactingRecept.Domain.Entities;

public sealed class DailyIntakeEntry : BaseEntity
{
    public Guid EntryId { get; private set; } = new();
    public int SortOrder { get; private set; }

    private DailyIntakeEntry() { }

    public DailyIntakeEntry(Guid entryId, int sortOrder)
    {
        EntryId = entryId;
        SortOrder = sortOrder;
    }
}
