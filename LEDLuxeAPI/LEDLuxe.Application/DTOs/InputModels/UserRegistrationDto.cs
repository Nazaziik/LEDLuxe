using System.ComponentModel.DataAnnotations;

namespace LEDLuxe.Application.DTOs.InputModels;

public class UserRegistrationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}