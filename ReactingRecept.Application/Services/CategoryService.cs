using ReactingRecept.Application.DTOs.Category;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain;
using ReactingRecept.Shared;

namespace ReactingRecept.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryOfTypeResponse[]?> GetAllOfTypeAsync(GetCategoryOfTypeRequest request)
        {
            Category[]? categories = await _categoryRepository.ListAllOfTypeAsync(request.Type);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            return categories.Select(category => 
                new GetCategoryOfTypeResponse(category.Name, category.CategoryType, category.SortOrder))
                .ToArray();
        }
    }
}
