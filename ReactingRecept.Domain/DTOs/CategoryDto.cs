using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public sealed class CategoryDto : ICategoryDto
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType CategoryType { get; private set; }
    public int SortOrder { get; private set; }
}
