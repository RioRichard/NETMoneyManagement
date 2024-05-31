using System.ComponentModel.DataAnnotations;

namespace NETAPI.DTO.Request.Session;

public class NewSessionRequest
{
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}