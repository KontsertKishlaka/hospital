using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("PatientDiagnosis")]
public class PatientDiagnosis
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int IDPatient { get; set; }

    [Required]
    public int IDDiagnosis { get; set; }

    [ForeignKey("IDPatient")]
    public virtual Patient Patient { get; set; } = null!;

    [ForeignKey("IDDiagnosis")]
    public virtual Diagnosis Diagnosis { get; set; } = null!;
}
