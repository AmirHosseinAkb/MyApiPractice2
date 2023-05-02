using Entities.User;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.DTOs;

public class UserDto
{
    
    [Required]
    [MaxLength(200)]
    public string UserName { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(200)]
    public string Password { get; set; }
    public int Age { get; set; }
    public GenderType Gender { get; set; }
}