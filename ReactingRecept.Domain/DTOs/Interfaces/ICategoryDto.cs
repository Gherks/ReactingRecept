using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain;

public interface ICategoryDto
{
    string Name { get; }
    CategoryType CategoryType { get; }
    int SortOrder { get; }
}
