using ReactingRecept.Domain.Entities;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Interfaces.Persistence;

public interface ICategoryRepository : IAsyncRepository<Category>
{
    Task<Category[]?> GetManyOfTypeAsync(CategoryType categoryType);
}
