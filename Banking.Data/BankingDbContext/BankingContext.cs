using Banking.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Banking.Data.BankingDbContext;

public class BankingContext : IdentityDbContext<IdentityUser>
{
    public BankingContext(DbContextOptions<BankingContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Applying unique constraint on PersonalId, Email
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.PersonalId)
            .IsUnique();
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

}
