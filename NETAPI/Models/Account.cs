using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETAPI.Models;

public class Account
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [MinLength(6)]
    [JsonIgnore]
    public string Password { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Wallet> Wallets { get; set; }
    public ICollection<Category> Categories { get; set; }
}