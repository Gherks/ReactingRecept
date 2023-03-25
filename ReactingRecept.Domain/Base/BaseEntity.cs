using System.ComponentModel.DataAnnotations;

namespace ReactingRecept.Domain.Base;

public class BaseEntity
{
    [Required]
    public Guid Id { get; private set; }
}
