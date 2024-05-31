using System.ComponentModel.DataAnnotations.Schema;

namespace NETAPI.Models;

public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int AccountId { get; set; }

    public Account Account { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}