using ReactingRecept.Domain.Entities.Base;

namespace ReactingRecept.Domain.Entities;

public sealed class DailyIntake : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public List<DailyIntakeEntity> Entities { get; private set; } = new();

    private DailyIntake() { }

    public DailyIntake(string name)
    {
        if (!ValidateName(name))
        {
            throw new ArgumentException(""); // ??
        }

        Name = name;
    }

    public void SetName(string name)
    {
        if (ValidateName(name))
        {
            Name = name;
        }
    }

    public void AddEntity(Ingredient ingredient, int amount)
    {
        if (amount <= 0)
        {
            // Log // ??
            return;
        }

        AddDomainEntity(ingredient, amount);
    }

    public void AddEntity(Recipe recipe, int amount)
    {
        if (amount <= 0)
        {
            // Log // ??
            return;
        }

        AddDomainEntity(recipe, amount);
    }

    private static bool ValidateName(string name)
    {
        bool nameIsEmpty = string.IsNullOrWhiteSpace(name);
        bool nameContainsOnlyDigits = double.TryParse(name, out _);

        return !nameIsEmpty && !nameContainsOnlyDigits;
    }

    private void AddDomainEntity(BaseEntity entity, int amount)
    {
        Entities.Add(new DailyIntakeEntity(entity.Id, amount, Entities.Count));
    }
}
