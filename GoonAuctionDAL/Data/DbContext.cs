using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class DbContext(DbContextOptions<DbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
  public DbSet<Auction> Auctions { get; set; }
  public DbSet<Bid> Bids { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // modelBuilder.Entity<Auction>()
    //   .HasOne(a => a.User)
    //   .WithMany(u => u.Auctions)
    //   .HasForeignKey(a => a.UserId)
    //   .OnDelete(DeleteBehavior.Cascade);

    // modelBuilder.Entity<Bid>()
    //     .HasOne(b => b.User)
    //     .WithMany(u => u.Bids)
    //     .HasForeignKey(b => b.UserId)
    //     .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Bid>()
        .HasOne(b => b.Auction)
        .WithMany(a => a.Bids)
        .HasForeignKey(b => b.AuctionId)
        .OnDelete(DeleteBehavior.NoAction);
  }
}