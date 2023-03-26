using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Category[]?> GetAllOfTypeAsync(CategoryType categoryType)
        {
            return await _categoryRepository.ListAllOfTypeAsync(categoryType);
        }
    }
}
