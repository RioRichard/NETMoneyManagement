using System.ComponentModel.DataAnnotations;

namespace NETAPI.DTO.Request.Wallet;

public class WalletQuery : BaseQuery
{
    [MinLength(2)] public string Name { get; set; }
}