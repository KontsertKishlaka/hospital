using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Gender")]
public class Gender
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string GenderName { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
