using Microsoft.EntityFrameworkCore;
using NguyenTrungNghia_KtraGK.Data;
using NguyenTrungNghia_KtraGK.Models;

namespace NguyenTrungNghia_KtraGK.Services;

public class AttendanceService
{
    private readonly ApplicationDbContext _context;

    public AttendanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AttendanceRecord?> CheckInAsync(int employeeId)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(e => e.Id == employeeId);

        if (!employeeExists)
            return null;

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        bool alreadyCheckedIn = await _context.AttendanceRecords
            .AnyAsync(a => a.EmployeeId == employeeId
                && a.CheckInTime >= today
                && a.CheckInTime < tomorrow);

        if (alreadyCheckedIn)
            throw new InvalidOperationException(
                "Nhân viên đã chấm công vào hôm nay.");

        var record = new AttendanceRecord
        {
            EmployeeId = employeeId,
            CheckInTime = DateTime.Now,
            Status = "Present"
        };

        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync();

        return record;
    }

    public async Task<AttendanceRecord?> CheckOutAsync(int employeeId)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var record = await _context.AttendanceRecords
            .Where(a => a.EmployeeId == employeeId
                && a.CheckInTime >= today
                && a.CheckInTime < tomorrow
                && a.CheckOutTime == null)
            .OrderByDescending(a => a.CheckInTime)
            .FirstOrDefaultAsync();

        if (record == null)
            return null;

        record.CheckOutTime = DateTime.Now;

        await _context.SaveChangesAsync();

        return record;
    }

    public async Task<List<AttendanceRecord>> GetByEmployeeAsync(
        int employeeId)
    {
        return await _context.AttendanceRecords
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.CheckInTime)
            .ToListAsync();
    }
}
