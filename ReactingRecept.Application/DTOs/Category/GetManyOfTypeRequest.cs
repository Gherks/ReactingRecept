using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.DTOs.Category
{
    public class GetManyOfTypeRequest
    {
        public CategoryType Type { get; private set; }

        public GetManyOfTypeRequest(CategoryType type)
        {
            Type = type;
        }
    }
}
