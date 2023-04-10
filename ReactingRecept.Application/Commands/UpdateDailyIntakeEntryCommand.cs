namespace ReactingRecept.Application.Commands;

public class UpdateDailyIntakeEntryCommand
{
    public Guid ProductId { get; private set; }
    public int SortOrder { get; private set; }

    public UpdateDailyIntakeEntryCommand(Guid productId, int sortOrder)
    {
        ProductId = productId;
        SortOrder = sortOrder;
    }
}
