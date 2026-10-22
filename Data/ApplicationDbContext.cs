
using Microsoft.EntityFrameworkCore;
using NguyenTrungNghia_KtraGK.Models;

namespace NguyenTrungNghia_KtraGK.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<AttendanceRecord> AttendanceRecords
        => Set<AttendanceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Role)
                .HasMaxLength(30)
                .IsRequired();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Status)
                .HasMaxLength(30)
                .IsRequired();

            entity.HasOne(a => a.Employee)
                .WithMany(e => e.AttendanceRecords)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
