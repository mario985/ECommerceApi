using System.ComponentModel.DataAnnotations;

public class LogInDto
{
    [EmailAddress]
    [Required]
    public string Email { set; get; } = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { set; get; } = string.Empty;
}