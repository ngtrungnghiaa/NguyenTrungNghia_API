
namespace NguyenTrungNghia_KtraGK.Models;

public class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User? User { get; set; }

    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new List<AttendanceRecord>();
}
