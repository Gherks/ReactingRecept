using ReactingRecept.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactingRecept.Domain;

public sealed class DailyIntakeEntry : DomainEntityBase
{
    public Guid EntryId { get; private set; } = new();
    public int Order { get; private set; }

    private DailyIntakeEntry() { }

    public DailyIntakeEntry(Guid entryId, int order)
    {
        EntryId = entryId;
        Order = order;
    }
}
