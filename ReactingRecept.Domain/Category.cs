using ReactingRecept.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public CategoryType CategoryType { get; private set; }
    public int SortOrder { get; private set; }
}
