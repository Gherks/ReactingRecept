using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class IngredientRepository : RepositoryBase<Ingredient>
{
    public IngredientRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }
}
