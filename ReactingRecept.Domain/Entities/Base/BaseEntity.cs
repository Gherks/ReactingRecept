using System.ComponentModel.DataAnnotations;

namespace ReactingRecept.Domain.Base;

public class BaseEntity
{
    public Guid Id { get; private set; }
}
