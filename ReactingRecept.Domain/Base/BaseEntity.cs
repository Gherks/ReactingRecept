using System.ComponentModel.DataAnnotations;

namespace ReactingRecept.Server.Entities.Bases;

public class DomainEntityBase
{
    [Required]
    public Guid Id { get; set; }
}
