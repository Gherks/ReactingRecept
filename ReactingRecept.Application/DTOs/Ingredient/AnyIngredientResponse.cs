namespace ReactingRecept.Application.DTOs.Category;

public class AnyIngredientResponse
{
    public bool Exist { get; private set; }

    public AnyIngredientResponse(bool exist)
    {
        Exist = exist;
    }
}
