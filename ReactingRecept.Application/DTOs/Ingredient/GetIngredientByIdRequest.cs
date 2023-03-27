namespace ReactingRecept.Application.DTOs.Ingredient;

public class GetIngredientByIdRequest
{
    public Guid Id { get; private set; }

    public GetIngredientByIdRequest(Guid id)
    {
        Id = id;
    }
}
