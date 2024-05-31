using System.ComponentModel.DataAnnotations;

namespace NETAPI.DTO.Request.Wallet;

public class WalletRequest
{
    [Required]
    [MinLength(6)]
    public string Name { get; set; }
}