using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("TegOfClient")]
public class TegOfClient
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int IDPatient { get; set; }

    [MaxLength(100)]
    public string? TagName { get; set; }

    public DateTime? DateAssigned { get; set; }

    [ForeignKey("IDPatient")]
    public virtual Patient Patient { get; set; } = null!;
}
