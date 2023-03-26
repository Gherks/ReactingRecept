using ReactingRecept.Domain.Base;

namespace ReactingRecept.Domain;

public sealed class DailyIntakeEntryDto : IDailyIntakeEntryDto
{
    public string Name { get; private set; } = string.Empty;
    public string Measurement { get; private set; } = string.Empty;
    public double Fat { get; private set; }
    public double Carbohydrates { get; private set; }
    public double Protein { get; private set; }
    public double Calories { get; private set; }
    public int Order { get; private set; }
}
