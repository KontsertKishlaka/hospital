using HospitalApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Data;

public class HospitalDbContext : IdentityDbContext<ApplicationUser>
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Gender> Genders { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<MedicalService> MedicalServices { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Diagnosis> Diagnoses { get; set; } = null!;
    public DbSet<PatientDiagnosis> PatientDiagnoses { get; set; } = null!;
    public DbSet<TegOfClient> TegOfClients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Настройка связей, если они не подхватились атрибутами
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Patient)
            .WithMany()
            .HasForeignKey(u => u.PatientId);

        // Настройка decimal точности
        builder.Entity<MedicalService>()
            .Property(m => m.Cost)
            .HasPrecision(10, 2);

        builder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasPrecision(10, 2);

        // Исправление ошибки с триггерами (EF Core 7/8+)
        // При наличии триггеров на таблице SQL Server, EF Core должен знать об этом, 
        // чтобы не использовать несовместимый оператор OUTPUT.
        builder.Entity<Patient>().ToTable(tb => tb.HasTrigger("trg_Patient"));
        builder.Entity<Appointment>().ToTable(tb => tb.HasTrigger("trg_Appointment"));
        builder.Entity<Order>().ToTable(tb => tb.HasTrigger("trg_Order"));
    }
}
