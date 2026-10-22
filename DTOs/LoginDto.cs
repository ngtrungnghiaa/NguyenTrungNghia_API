
using System.ComponentModel.DataAnnotations;

namespace NguyenTrungNghia_KtraGK.DTOs;

public class LoginDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
