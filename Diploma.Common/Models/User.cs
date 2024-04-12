namespace Diploma.Common.Models;

public class User
{
    public Guid Id { get; set; }
    public string? Login { get; set; } = null;
    public string? Password { get; set; } = null;
    public string PhoneNumber { get; set; }
    public long? ChatId { get; set; }
}