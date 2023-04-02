using ReactingRecept.Application.DTOs.Category;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain.Entities;
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

        public async Task<GetManyOfTypeResponse[]?> GetManyOfTypeAsync(GetManyOfTypeRequest request)
        {
            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(request.Type);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            return categories.Select(category =>
                new GetManyOfTypeResponse(category.Name, category.Type, category.SortOrder))
                .ToArray();
        }
    }
}
