using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Appointment")]
public class Appointment
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int IDEmployee { get; set; }

    [Required]
    public int IDMedicalService { get; set; }

    [Required]
    public DateTime DateService { get; set; }

    [ForeignKey("IDEmployee")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("IDMedicalService")]
    public virtual MedicalService MedicalService { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
