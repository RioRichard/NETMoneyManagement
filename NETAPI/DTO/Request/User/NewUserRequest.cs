using System.ComponentModel.DataAnnotations;

namespace NETAPI.DTO.Request.User;

public class NewUserRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string PasswordConfirmation { get; set; }
}