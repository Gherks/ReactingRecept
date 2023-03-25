using System.ComponentModel.DataAnnotations;

namespace ReactingRecept.Domain.Base;

public class DomainEntityBase
{
    [Required]
    public Guid Id { get; private set; }
}
