namespace Diploma.Common.Models;

public class RegistrationModel
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public int GroupId { get; set; }
}