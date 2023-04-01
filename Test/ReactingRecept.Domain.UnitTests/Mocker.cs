using ReactingRecept.Domain.Entities;
using ReactingRecept.Shared;
using System;
using System.Reflection;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Domain.UnitTests;

public static class Mocker
{
    public static Category MockCategory(string name, CategoryType type, int sortOrder)
    {
        Category mockedCategory = new Category(name, type, sortOrder);

        MockId(mockedCategory);

        return mockedCategory;
    }

    private static void MockId(Category category)
    {
        string idPropertyName = nameof(category.Id);

        PropertyInfo? property = category.GetType().GetProperty(idPropertyName);

        if (property != null &&
            property.DeclaringType != null)
        {
            PropertyInfo? deckaríngTypeProperty = property.DeclaringType.GetProperty(idPropertyName);

            if (deckaríngTypeProperty != null)
            {
                deckaríngTypeProperty.SetValue(category, Guid.NewGuid(), null);
            }
        }
    }
}
