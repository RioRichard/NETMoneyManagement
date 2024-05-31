using System.ComponentModel.DataAnnotations.Schema;

namespace NETAPI.Models;

public class Wallet
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int AccountId { get; set; }

    public Account Account { get; set; }
    public ICollection<Transaction> TransactionsOut { get; set; }
    public ICollection<Transaction> TransactionsIn { get; set; }

}