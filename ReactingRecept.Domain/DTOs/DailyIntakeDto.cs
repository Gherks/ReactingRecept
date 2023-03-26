using ReactingRecept.Domain.Base;

namespace ReactingRecept.Domain;

public sealed class DailyIntakeDto : IDailyIntakeDto
{
    public string Name { get; private set; } = string.Empty;
    public IDailyIntakeEntryDto[] Entries { get; private set; } = Array.Empty<IDailyIntakeEntryDto>();
}
