using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auction.Wallet.Infrastructure.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Freezing> Freezings { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
