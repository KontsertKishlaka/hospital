using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Employee")]
public class Employee
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string FName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LName { get; set; } = null!;

    [MaxLength(50)]
    public string? MName { get; set; }

    [Required]
    [MaxLength(4)]
    public string PassportSeries { get; set; } = null!;

    [Required]
    [MaxLength(6)]
    public string PassportNumber { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirthday { get; set; }

    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = null!;

    [Required]
    public int IDDepartment { get; set; }

    [Required]
    public int IDPost { get; set; }

    [Required]
    public int IDGender { get; set; }

    [ForeignKey("IDDepartment")]
    public virtual Department Department { get; set; } = null!;

    [ForeignKey("IDPost")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("IDGender")]
    public virtual Gender Gender { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
