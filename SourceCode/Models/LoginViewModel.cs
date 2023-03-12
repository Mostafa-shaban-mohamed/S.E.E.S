using System.ComponentModel.DataAnnotations.Schema;

namespace SourceCode.Models;

[NotMapped]
public class LoginViewModel
{
    public string UserRole { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
