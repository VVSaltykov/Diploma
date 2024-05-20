using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Common.Models;

public class Messages : BaseEntity<int>
{
    public string Tittle { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsAnonymous { get; set; }
    public long? RecepientId { get; set; }
    
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
}