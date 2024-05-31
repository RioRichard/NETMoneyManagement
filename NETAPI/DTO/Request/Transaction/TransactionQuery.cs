using System.ComponentModel.DataAnnotations;
using NETAPI.Models;

namespace NETAPI.DTO.Request.Transaction;

public class TransactionQuery : BaseQuery
{
    [MinLength(3)] public string Name { get; set; }
    public int? WalletId { get; set; }
    public int? CategoryId { get; set; }
    public decimal? FromAmount { get; set; }
    public decimal? ToAmount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime? FromProcessDate { get; set; }
    public DateTime? ToProcessDate { get; set; }
}