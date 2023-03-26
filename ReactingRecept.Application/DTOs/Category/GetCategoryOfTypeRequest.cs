using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.DTOs.Category
{
    public class GetCategoryOfTypeRequest
    {
        public CategoryType Type { get; private set; }

        public GetCategoryOfTypeRequest(CategoryType type)
        {
            Type = type;
        }
    }
}
