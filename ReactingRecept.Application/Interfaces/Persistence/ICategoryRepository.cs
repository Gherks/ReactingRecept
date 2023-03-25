using ReactingRecept.Domain;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task<Category[]?> ListAllOfTypeAsync(CategoryType categoryType);
}
