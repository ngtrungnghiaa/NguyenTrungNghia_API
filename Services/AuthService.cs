
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NguyenTrungNghia_KtraGK.Data;
using NguyenTrungNghia_KtraGK.DTOs;
using NguyenTrungNghia_KtraGK.Models;

namespace NguyenTrungNghia_KtraGK.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(
        RegisterDto dto)
    {
        string username = dto.Username.Trim();
        string email = dto.Email.Trim();

        if (await _context.Users.AnyAsync(u => u.Username == username))
            return (false, "Tên đăng nhập đã tồn tại.");

        if (await _context.Employees.AnyAsync(e => e.Email == email))
            return (false, "Email đã được sử dụng.");

        var user = new User
        {
            Username = username,
            Role = "Employee"
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var employee = new Employee
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Phone = dto.Phone.Trim(),
            UserId = user.Id
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return (true, "Đăng ký thành công.");
    }

    public async Task<User?> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username.Trim());

        if (user == null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        return user;
    }
}
