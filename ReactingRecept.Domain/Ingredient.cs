using ReactingRecept.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactingRecept.Domain;

public sealed class Ingredient : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public double Fat { get; private set; }
    public double Carbohydrates { get; private set; }
    public double Protein { get; private set; }
    public double Calories { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private Ingredient() { }

    public Ingredient(string name, double fat, double carbohydrates, double protein, double calories, Category category)
    {
        if (!ValidateName(name) ||
            !ValidateFat(fat) ||
            !ValidateCarbohydrates(carbohydrates) ||
            !ValidateProtein(protein) ||
            !ValidateCalories(calories))
        {
            throw new ArgumentException(""); // ??
        }

        Name = name;
        Fat = fat;
        Carbohydrates = carbohydrates;
        Protein = protein;
        Calories = calories;
        CategoryId = category.Id;
        Category = category;
    }

    public void SetName(string name)
    {
        if (ValidateName(name))
        {
            Name = name;
        }
    }

    public void SetFat(double fat)
    {
        if (ValidateFat(fat))
        {
            Fat = fat;
        }
    }

    public void SetCarbohydrates(double carbohydrates)
    {
        if (ValidateCarbohydrates(carbohydrates))
        {
            Carbohydrates = carbohydrates;
        }
    }

    public void SetProtein(double protein)
    {
        if (ValidateProtein(protein))
        {
            Protein = protein;
        }
    }

    public void SetCalories(double calories)
    {
        if (ValidateCalories(calories))
        {
            Calories = calories;
        }
    }

    private static bool ValidateName(string name)
    {
        bool nameIsEmpty = string.IsNullOrWhiteSpace(name);
        bool nameContainsOnlyDigits = double.TryParse(name, out _);

        if (nameIsEmpty || nameContainsOnlyDigits)
        {
            return false;
        }

        return true;
    }

    private static bool ValidateFat(double fat)
    {
        if (fat < double.Epsilon)
        {
            return false;
        }

        return true;
    }

    private static bool ValidateCarbohydrates(double carbohydrates)
    {
        if (carbohydrates < double.Epsilon)
        {
            return false;
        }

        return true;
    }

    private static bool ValidateProtein(double protein)
    {
        if (protein < double.Epsilon)
        {
            return false;
        }

        return true;
    }

    private static bool ValidateCalories(double calories)
    {
        if (calories < double.Epsilon)
        {
            return false;
        }

        return true;
    }
}
