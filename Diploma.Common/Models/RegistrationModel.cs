using System.ComponentModel.DataAnnotations;
using Diploma.Common.Models.Enums;

namespace Diploma.Common.Models;

public class RegistrationModel
{
    //[Required(ErrorMessage = "Логин обязателен для заполнения.")]
    //[RegularExpression(@"^[A-Za-z0-9.]+$", ErrorMessage = "Логин должен содержать только символы английского алфавита, цифры и точку.")]
    public string? Login { get; set; }
    //[Required(ErrorMessage = "Пароль обязателен для заполнения.")]
    public string? Password { get; set; }
    public long? ChatId { get; set; }
    public string? PhoneNumber { get; set; }
    //[Required(ErrorMessage = "Имя обязательно для заполнения.")]
    public string Name { get; set; }
    public int? GroupId { get; set; }
    public Role Role { get; set; }
}