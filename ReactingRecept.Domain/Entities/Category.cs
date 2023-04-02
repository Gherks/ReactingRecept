using ReactingRecept.Domain.Entities.Base;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }
    public int SortOrder { get; private set; }

    private Category() { }

    public Category(string name, CategoryType type, int sortOrder)
    {
        Name = name;
        Type = type;
        SortOrder = sortOrder;
    }

    public void SetSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
    }
}
