namespace HospitalApp.ViewModels;

public class AppointmentHistoryViewModel
{
    public DateTime DateService { get; set; }
    public string ServiceTitle { get; set; } = null!;
    public string DoctorShortName { get; set; } = null!;
    public decimal Cost { get; set; }
}
