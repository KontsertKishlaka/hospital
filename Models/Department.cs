using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Department")]
public class Department
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
