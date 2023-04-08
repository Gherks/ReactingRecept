namespace ReactingRecept.Application.DTOs;

public class DailyIntakeEntryDTO
{
    public string ProductName { get; set; } = string.Empty;
    public double Amount { get; set; }
    public double Fat { get; set; }
    public double Carbohydrates { get; set; }
    public double Protein { get; set; }
    public double Calories { get; set; }
    public double ProteinPerCalorie { get; set; }
    public int SortOrder { get; set; }
    public bool IsRecipe { get; set; }
    public Guid ProductId { get; set; }

    public DailyIntakeEntryDTO(string productName, double amount, double fat, double carbohydrates, double protein, double calories, double proteinPerCalorie, int sortOrder, bool isRecipe, Guid productId)
    {
        ProductName = productName;
        Amount = amount;
        Fat = fat;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Calories = calories;
        ProteinPerCalorie = proteinPerCalorie;
        SortOrder = sortOrder;
        IsRecipe = isRecipe;
        ProductId = productId;
    }
}
