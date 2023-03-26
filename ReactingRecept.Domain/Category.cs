using ReactingRecept.Domain.Base;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType CategoryType { get; private set; }
    public int SortOrder { get; private set; }

    public Category()
    {
        
    }

    public Category(string name, CategoryType categoryType, int sortOrder)
    {
        Name = name;
        CategoryType = categoryType;
        SortOrder = sortOrder;
    }
}
