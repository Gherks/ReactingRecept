namespace ReactingRecept.Application.DTOs.Ingredient;

public class AnyIngredientResponse
{
    public bool Exist { get; private set; }

    public AnyIngredientResponse(bool exist)
    {
        Exist = exist;
    }
}
