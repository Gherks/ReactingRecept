using Moq;
using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Domain.Entities;
using ReactingRecept.Domain.Entities.Base;
using ReactingRecept.Shared;
using System.Reflection;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    private static void MockId(BaseEntity baseEntity)
    {
        string idPropertyName = nameof(baseEntity.Id);

        PropertyInfo? property = baseEntity.GetType().GetProperty(idPropertyName);

        if (property != null &&
            property.DeclaringType != null)
        {
            PropertyInfo? deckaringTypeProperty = property.DeclaringType.GetProperty(idPropertyName);

            if (deckaringTypeProperty != null)
            {
                deckaringTypeProperty.SetValue(baseEntity, Guid.NewGuid(), null);
            }
        }
    }
}
