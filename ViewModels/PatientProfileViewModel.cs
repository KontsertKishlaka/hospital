namespace HospitalApp.ViewModels;

public class PatientProfileViewModel
{
    public int ID { get; set; }
    public string FName { get; set; } = null!;
    public string LName { get; set; } = null!;
    public string? MName { get; set; }
    public DateTime DateOfBirthday { get; set; }
    public string GenderName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
}
