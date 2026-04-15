using System.ComponentModel.DataAnnotations;

namespace HospitalApp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите логин")]
    [Display(Name = "Логин")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }
}
