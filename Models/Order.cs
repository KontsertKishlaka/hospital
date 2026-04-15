using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Order")]
public class Order
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int IDAppointment { get; set; }

    [Required]
    public int IDPatient { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [ForeignKey("IDAppointment")]
    public virtual Appointment Appointment { get; set; } = null!;

    [ForeignKey("IDPatient")]
    public virtual Patient Patient { get; set; } = null!;
}
