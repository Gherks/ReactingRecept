using ReactingRecept.Domain.Base;

namespace ReactingRecept.Domain;

public sealed class DailyIntake : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public List<DailyIntakeEntry> Entries { get; private set; } = new();

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

    public void AddEntry(Ingredient ingredient)
    {
        AddDomainEntityEntry(ingredient);
    }

    public void AddEntry(Recipe recipe)
    {
        AddDomainEntityEntry(recipe);
    }

    public void ChangeOrderOfEntries(int firstIndex, int secondIndex)
    {
        if (firstIndex < 0 || firstIndex >= Entries.Count ||
            secondIndex < 0 || secondIndex >= Entries.Count)
        {
            return;
        }

        Guid firstEntryId = Entries[firstIndex].EntryId;
        Guid secondEntryId = Entries[secondIndex].EntryId;

        Entries[firstIndex] = new DailyIntakeEntry(secondEntryId, firstIndex);
        Entries[secondIndex] = new DailyIntakeEntry(firstEntryId, secondIndex);
    }

    private static bool ValidateName(string name)
    {
        bool nameIsEmpty = string.IsNullOrWhiteSpace(name);
        bool nameContainsOnlyDigits = double.TryParse(name, out _);

        return !nameIsEmpty && !nameContainsOnlyDigits;
    }

    private void AddDomainEntityEntry(BaseEntity entry)
    {
        Entries.Add(new DailyIntakeEntry(entry.Id, Entries.Count));
    }
}
