using Microsoft.AspNetCore.Identity;

namespace HospitalApp.Models;

public class ApplicationUser : IdentityUser
{
    // ID пациента в таблице Patient
    public int? PatientId { get; set; }
    public virtual Patient? Patient { get; set; }
}
