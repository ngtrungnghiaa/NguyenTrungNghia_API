using Microsoft.AspNetCore.Mvc;
using NguyenTrungNghia_KtraGK.Services;

namespace NguyenTrungNghia_KtraGK.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly AttendanceService _attendanceService;

    public AttendanceController(AttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpPost("checkin/{employeeId:int}")]
    public async Task<IActionResult> CheckIn(int employeeId)
    {
        try
        {
            var record = await _attendanceService.CheckInAsync(employeeId);

            if (record == null)
                return NotFound(new { message = "Không tìm thấy nhân viên." });

            return Ok(new
            {
                message = "Chấm công vào thành công.",
                record.Id,
                record.EmployeeId,
                record.CheckInTime,
                record.Status
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("checkout/{employeeId:int}")]
    public async Task<IActionResult> CheckOut(int employeeId)
    {
        var record = await _attendanceService.CheckOutAsync(employeeId);

        if (record == null)
        {
            return BadRequest(new
            {
                message = "Không tìm thấy lượt chấm công chưa kết thúc hôm nay."
            });
        }

        return Ok(new
        {
            message = "Chấm công ra thành công.",
            record.Id,
            record.EmployeeId,
            record.CheckInTime,
            record.CheckOutTime,
            record.Status
        });
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetHistory(int employeeId)
    {
        var records = await _attendanceService
            .GetByEmployeeAsync(employeeId);

        return Ok(records);
    }
}
