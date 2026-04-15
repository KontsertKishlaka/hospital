using System.ComponentModel.DataAnnotations;

namespace HospitalApp.ViewModels;

public class PatientEditViewModel
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
}
