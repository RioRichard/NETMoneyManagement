using Microsoft.EntityFrameworkCore;

namespace NETAPI.Models;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Account>().ToTable("Accounts");
        modelBuilder.Entity<Account>().HasIndex(p => p.Email).IsUnique();
        modelBuilder.Entity<Account>().HasMany(p => p.Categories)
            .WithOne(p => p.Account)
            .HasForeignKey(p => p.AccountId);
        modelBuilder.Entity<Account>().HasMany(p => p.Transactions)
            .WithOne(p => p.Account)
            .HasForeignKey(p => p.AccountId);
        modelBuilder.Entity<Account>().HasMany(p => p.Wallets)
            .WithOne(p => p.Account)
            .HasForeignKey(p => p.AccountId);

        modelBuilder.Entity<Wallet>().HasMany(p => p.TransactionsOut)
            .WithOne(p => p.SourceWallet)
            .HasForeignKey(p => p.SourceWalletId);
        modelBuilder.Entity<Wallet>().HasMany(p => p.TransactionsIn)
            .WithOne(p => p.DestWallet)
            .HasForeignKey(p => p.DestWalletId);

        modelBuilder.Entity<Category>().HasMany(p => p.Transactions)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Transaction>().ToTable("Transaction");
    }
}