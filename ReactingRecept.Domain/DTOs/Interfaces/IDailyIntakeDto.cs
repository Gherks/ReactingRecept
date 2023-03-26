using ReactingRecept.Domain.Base;

namespace ReactingRecept.Domain;

public interface IDailyIntakeDto
{
    public string Name { get; }
    public IDailyIntakeEntryDto[] Entries { get; }
}
