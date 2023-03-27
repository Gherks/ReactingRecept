using ReactingRecept.Application.DTOs.Category;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<GetManyOfTypeResponse[]?> GetManyOfTypeAsync(GetManyOfTypeRequest request);
    }
}
