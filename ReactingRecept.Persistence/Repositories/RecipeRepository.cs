using ReactingRecept.Domain;
using ReactingRecept.Persistence.Context;

namespace ReactingRecept.Persistence.Repositories;

public class RecipeRepository : RepositoryBase<Recipe>
{
    public RecipeRepository(ReactingReceptContext reactingReceptContext) : base(reactingReceptContext) { }
}
