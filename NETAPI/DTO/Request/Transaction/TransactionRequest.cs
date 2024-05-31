using System.ComponentModel.DataAnnotations;
using NETAPI.Models;

namespace NETAPI.DTO.Request.Transaction;

public class TransactionRequest
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public int? SourceWalletId { get; set; }
    public int? DestWalletId { get; set; }

    public decimal Amount { get; set; } = 0;
    [Required]
    public TransactionType Type { get; set; }
}