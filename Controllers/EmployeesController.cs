using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenTrungNghia_KtraGK.Data;
using NguyenTrungNghia_KtraGK.Models;

namespace NguyenTrungNghia_KtraGK.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _context.Employees
            .Select(e => new
            {
                e.Id,
                e.FullName,
                e.Email,
                e.Phone,
                e.UserId
            })
            .ToListAsync();

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _context.Employees
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.FullName,
                e.Email,
                e.Phone,
                e.UserId
            })
            .FirstOrDefaultAsync();

        if (employee == null)
            return NotFound(new { message = "Không tìm thấy nhân viên." });

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        bool userExists = await _context.Users
            .AnyAsync(u => u.Id == employee.UserId);

        if (!userExists)
            return BadRequest(new { message = "Tài khoản không tồn tại." });

        bool emailExists = await _context.Employees
            .AnyAsync(e => e.Email == employee.Email);

        if (emailExists)
            return Conflict(new { message = "Email đã được sử dụng." });

        employee.Id = 0;
        employee.User = null;

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            new
            {
                employee.Id,
                employee.FullName,
                employee.Email,
                employee.Phone,
                employee.UserId
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Employee input)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return NotFound(new { message = "Không tìm thấy nhân viên." });

        bool emailExists = await _context.Employees
            .AnyAsync(e => e.Email == input.Email && e.Id != id);

        if (emailExists)
            return Conflict(new { message = "Email đã được sử dụng." });

        employee.FullName = input.FullName.Trim();
        employee.Email = input.Email.Trim();
        employee.Phone = input.Phone.Trim();

        await _context.SaveChangesAsync();

        return Ok(new { message = "Cập nhật nhân viên thành công." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return NotFound(new { message = "Không tìm thấy nhân viên." });

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Xóa nhân viên thành công." });
    }
}
