using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETAPI.Models;

public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime ProcessDate { get; set; }

    public int AccountId { get; set; }
    public int? SourceWalletId { get; set; }
    public int? DestWalletId { get; set; }
    public int CategoryId { get; set; }

    public Account Account { get; set; }
    public Wallet SourceWallet { get; set; }
    public Wallet DestWallet { get; set; }
    public Category Category { get; set; }
}