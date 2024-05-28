using System.ComponentModel.DataAnnotations;

namespace Diploma.Common.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Логин обязателен для заполнения.")]
    [RegularExpression(@"^[A-Za-z0-9.]+$", ErrorMessage = "Логин должен содержать только символы английского алфавита, цифры и точку.")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
    public string Password { get; set; }
}