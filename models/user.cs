using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace learn_dotnet.models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Role { get; set; } = "user";
}
