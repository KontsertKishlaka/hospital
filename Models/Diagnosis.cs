using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Diagnosis")]
public class Diagnosis
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string DiagnosisName { get; set; } = null!;

    public virtual ICollection<PatientDiagnosis> PatientDiagnoses { get; set; } = new List<PatientDiagnosis>();
}
