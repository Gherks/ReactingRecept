using ReactingRecept.Application.DTOs.Category;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;
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

        public async Task<CategoryDTO[]?> GetManyOfTypeAsync(CategoryType type)
        {
            Category[]? categories = await _categoryRepository.GetManyOfTypeAsync(type);
            Contracts.LogAndThrowWhenNothingWasReceived(categories);

            return categories.Select(category =>
                new CategoryDTO(category.Name, category.Type, category.SortOrder))
                .ToArray();
        }
    }
}
