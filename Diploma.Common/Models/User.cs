using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Common.Models;

public class User : BaseEntity<Guid>
{
    public string? Login { get; set; } = null;
    public string? Password { get; set; } = null;
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public long? ChatId { get; set; }
    
    public int GroupId { get; set; }
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
}