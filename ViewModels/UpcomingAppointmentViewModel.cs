namespace HospitalApp.ViewModels;

public class UpcomingAppointmentViewModel
{
    public DateTime DateService { get; set; }
    public string ServiceTitle { get; set; } = null!;
    public string DoctorShortName { get; set; } = null!;
}
