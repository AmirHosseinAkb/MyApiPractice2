using System.ComponentModel.DataAnnotations;
using Entities.User;

namespace WebFramework.ViewModels;

public class UserViewModel
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public GenderType Gender { get; set; }
}