namespace ReactingRecept.Application.Commands;

public class AddDailyIntakeEntryCommand
{
    public Guid EntityId { get; private set; }
    public int Amount { get; private set; }
    public int SortOrder { get; private set; }

    public AddDailyIntakeEntryCommand(Guid entityId, int amount, int sortOrder)
    {
        EntityId = entityId;
        Amount = amount;
        SortOrder = sortOrder;
    }
}
