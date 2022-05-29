using ReactingRecept.Server.Entities.Bases;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Server.Entities;

public sealed class Category : DomainEntityBase
{
    public string? Name { get; set; } = null;
    public CategoryType CategoryType { get; set; } = CategoryType.Unassigned;
    public int SortOrder { get; set; } = -1;
}
