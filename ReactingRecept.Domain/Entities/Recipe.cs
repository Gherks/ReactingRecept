using ReactingRecept.Domain.Entities.Base;

namespace ReactingRecept.Domain.Entities;

public sealed class Recipe : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Instructions { get; private set; } = string.Empty;
    public int PortionAmount { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public List<IngredientMeasurement> IngredientMeasurements { get; private set; } = new();

    private Recipe() { }

    public Recipe(string name, string instructions, int portionAmount, Category category)
    {
        if (!ValidateName(name) ||
            !ValidateInstructions(instructions) ||
            !ValidatePortionAmount(portionAmount))
        {
            throw new ArgumentException(""); // ??
        }

        Name = name;
        Instructions = instructions;
        PortionAmount = portionAmount;
        CategoryId = category.Id;
        Category = category;
    }

    public void AddIngredientMeasurement(IngredientMeasurement ingredientMeasurement)
    {
        bool ingredientMeasurementHasAlreadyBeenAdded = IngredientMeasurements.Any(measurement =>
            measurement.IngredientId == ingredientMeasurement.IngredientId);

        if (ingredientMeasurementHasAlreadyBeenAdded)
        {
            // Log.Warning
            return;
        }

        IngredientMeasurements.Add(ingredientMeasurement);
    }

    public bool RemoveIngredientMeasurement(Guid id)
    {
        IngredientMeasurement? ingredientMeasurement = IngredientMeasurements.Find(ingredientMeasurement => ingredientMeasurement.Id == id);

        if (ingredientMeasurement != null)
        {
            IngredientMeasurements.Remove(ingredientMeasurement);
            return true;
        }

        return false;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            // Log Warning
            return;
        }

        Name = name;
    }

    public void SetPortionAmount(int portionAmount)
    {
        if (portionAmount < 1)
        {
            // Log warning
            return;
        }

        PortionAmount = portionAmount;
    }

    public void SetCategory(Category category)
    {
        if (category == null)
        {
            // Log warning
            return;
        }

        Category = category;
    }

    public void SetInstructions(string instructions)
    {
        if (string.IsNullOrWhiteSpace(instructions))
        {
            // Log Warning
            return;
        }

        Instructions = instructions;
    }

    private static bool ValidateName(string name)
    {
        bool nameIsEmpty = string.IsNullOrWhiteSpace(name);
        bool nameContainsOnlyDigits = double.TryParse(name, out _);

        return !nameIsEmpty && !nameContainsOnlyDigits;
    }

    private static bool ValidateInstructions(string instructions)
    {
        bool instructionsIsEmpty = string.IsNullOrWhiteSpace(instructions);
        bool instructionsContainsOnlyDigits = double.TryParse(instructions, out _);

        return !instructionsIsEmpty && !instructionsContainsOnlyDigits;
    }

    private static bool ValidatePortionAmount(double portionAmount)
    {
        return portionAmount >= 1;
    }
}
