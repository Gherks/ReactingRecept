using ReactingRecept.Application.DTOs.Category;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO[]?> GetManyOfTypeAsync(CategoryType type);
    }
}
