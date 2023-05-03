using Entities.User;
using System.ComponentModel.DataAnnotations;

namespace WebFramework.DTOs;

public class UserDto:IValidatableObject
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
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase) ||
            UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
            yield return new ValidationResult("User Name Cannot Be Test Or Admin");
    }
}