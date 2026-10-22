
namespace NguyenTrungNghia_KtraGK.Models;

public class AttendanceRecord
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string Status { get; set; } = "Present";

    public Employee? Employee { get; set; }
}
