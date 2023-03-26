using ReactingRecept.Domain.Entities;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class RecipeRepository : RepositoryBase<Recipe>
{
    public RecipeRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }
}
