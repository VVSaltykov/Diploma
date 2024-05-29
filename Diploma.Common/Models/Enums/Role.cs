using System.ComponentModel.DataAnnotations;

namespace Diploma.Common.Models.Enums;

public enum Role
{
    [Display(Name = "Администратор")]
    Admin,
    [Display(Name = "Преподаватель")]
    Professor,
    [Display(Name = "Студент")]
    Student,
    [Display(Name = "Абитуриент")]
    Applicant,
    [Display(Name = "Выпускник")]
    Graduate
}