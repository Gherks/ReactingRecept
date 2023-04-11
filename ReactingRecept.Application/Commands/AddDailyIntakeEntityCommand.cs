namespace ReactingRecept.Application.Commands;

public class AddDailyIntakeCommand
{
    public string Name { get; private set; }

    List<AddDailyIntakeEntity> addDailyIntakeEntities = new List<AddDailyIntakeEntity>();

    public AddDailyIntakeCommand(string name)
    {
        Name = name;
    }

    public void AddEntity(Guid entityId, int amount)
    {
        addDailyIntakeEntities.Add(new AddDailyIntakeEntity(entityId, amount, addDailyIntakeEntities.Count()));
    }

    private class AddDailyIntakeEntity
    {
        public Guid EntityId { get; private set; }
        public int Amount { get; private set; }
        public int SortOrder { get; private set; }

        public AddDailyIntakeEntity(Guid entityId, int amount, int sortOrder)
        {
            EntityId = entityId;
            Amount = amount;
            SortOrder = sortOrder;
        }
    }
}
