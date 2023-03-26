using ReactingRecept.Domain.Base;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsValid => Id != Guid.Empty;

    public Category() { }

    public Category(string name, CategoryType type, int sortOrder)
    {
        Name = name;
        Type = type;
        SortOrder = sortOrder;
    }
}
