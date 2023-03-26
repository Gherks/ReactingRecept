using ReactingRecept.Application.DTOs.Category;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<GetCategoryOfTypeResponse[]?> GetAllOfTypeAsync(GetCategoryOfTypeRequest request);
    }
}
