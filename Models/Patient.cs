using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Patient")]
public class Patient
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
    [MaxLength(100)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = null!;

    [Required]
    public int IDGender { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirthday { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(50)]
    public string? Login { get; set; }

    [MaxLength(50)]
    public string? Password { get; set; }

    [ForeignKey("IDGender")]
    public virtual Gender Gender { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<PatientDiagnosis> PatientDiagnoses { get; set; } = new List<PatientDiagnosis>();
    public virtual ICollection<TegOfClient> TegOfClients { get; set; } = new List<TegOfClient>();
}
