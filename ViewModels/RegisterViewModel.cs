using System.ComponentModel.DataAnnotations;

namespace HospitalApp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите фамилию")]
    [Display(Name = "Фамилия")]
    public string LName { get; set; } = null!;

    [Required(ErrorMessage = "Введите имя")]
    [Display(Name = "Имя")]
    public string FName { get; set; } = null!;

    [Display(Name = "Отчество")]
    public string? MName { get; set; }

    [Required(ErrorMessage = "Выберите дату рождения")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата рождения")]
    public DateTime DateOfBirthday { get; set; }

    [Required(ErrorMessage = "Выберите пол")]
    [Display(Name = "Пол")]
    public int IDGender { get; set; }

    [Required(ErrorMessage = "Введите адрес")]
    [Display(Name = "Адрес")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "Введите телефон")]
    [Phone]
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Введите Email")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Введите логин")]
    [Display(Name = "Логин")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} символов.", MinimumLength = 3)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; } = null!;
}
