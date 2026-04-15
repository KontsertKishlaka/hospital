using System.ComponentModel.DataAnnotations;

namespace HospitalApp.ViewModels;

public class AppointmentCreateViewModel
{
    [Required(ErrorMessage = "Выберите услугу")]
    [Display(Name = "Услуга")]
    public int IDMedicalService { get; set; }

    [Required(ErrorMessage = "Выберите дату и время")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Дата и время")]
    public DateTime DateService { get; set; }
}
