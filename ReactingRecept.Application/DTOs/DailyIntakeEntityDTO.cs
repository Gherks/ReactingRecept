namespace ReactingRecept.Application.DTOs;

public class DailyIntakeEntityDTO
{
    public string Name { get; set; } = string.Empty;
    public double Amount { get; set; }
    public double Fat { get; set; }
    public double Carbohydrates { get; set; }
    public double Protein { get; set; }
    public double Calories { get; set; }
    public int SortOrder { get; set; }
    public bool IsRecipe { get; set; }
    public Guid EntityId { get; set; }

    public DailyIntakeEntityDTO(string name, double amount, double fat, double carbohydrates, double protein, double calories, int sortOrder, bool isRecipe, Guid entityId)
    {
        Name = name;
        Amount = amount;
        Fat = fat;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Calories = calories;
        SortOrder = sortOrder;
        IsRecipe = isRecipe;
        EntityId = entityId;
    }
}
