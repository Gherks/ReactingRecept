namespace ReactingRecept.Domain;

public interface IDailyIntakeEntryDto
{
    string Name { get; }
    string Measurement { get; }
    double Fat { get; }
    double Carbohydrates { get; }
    double Protein { get; }
    double Calories { get; }
    int Order { get; }
}
