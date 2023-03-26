using ReactingRecept.Domain;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Category[]?> GetAllOfTypeAsync(CategoryType categoryType);
    }
}
