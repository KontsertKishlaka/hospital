using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models;

[Table("Post")]
public class Post
{
    [Key]
    public int ID { get; set; }

    [MaxLength(50)]
    public string? PostName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
