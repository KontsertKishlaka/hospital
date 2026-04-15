using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("MedicalService")]
public class MedicalService
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string TitleService { get; set; } = null!;

    [Required]
    public int Duration { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Cost { get; set; }

    [Required]
    public int QNT { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
