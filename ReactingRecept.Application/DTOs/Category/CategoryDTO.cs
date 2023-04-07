using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.DTOs.Category;

public class CategoryDTO
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }
    public int SortOrder { get; private set; }

    public CategoryDTO(string name, CategoryType type, int sortOrder)
    {
        Name = name;
        Type = type;
        SortOrder = sortOrder;
    }
}
