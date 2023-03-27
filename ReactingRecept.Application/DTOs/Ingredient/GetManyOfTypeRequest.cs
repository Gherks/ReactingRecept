namespace ReactingRecept.Application.DTOs.Category
{
    public class AnyIngredientRequest
    {
        public Guid Id { get; private set; }

        public AnyIngredientRequest(Guid id)
        {
            Id = id;
        }
    }
}
