using ReactingRecept.Application.DTOs;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO[]?> GetManyOfTypeAsync(CategoryType type);
    }
}
