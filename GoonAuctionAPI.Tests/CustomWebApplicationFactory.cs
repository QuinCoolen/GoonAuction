using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
  private readonly string _dbName = Guid.NewGuid().ToString();
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      services.RemoveAll<IDbContextOptionsConfiguration<DbContext>>();

      services.AddDbContext<DbContext>(options =>
      {
        options.UseInMemoryDatabase(_dbName);
      });

      var sp = services.BuildServiceProvider();

      using var scope = sp.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<DbContext>();
      db.Database.EnsureDeleted();
      db.Database.EnsureCreated();

      try
      {
        // Seed the database with test data
        SeedTestData(db);
      }
      catch (Exception ex)
      {
        // Log the error but don't fail the test setup
        Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
      }
    });
  }

  private void SeedTestData(DbContext db)
  {
    // Check if data already exists
    if (db.Auctions.Any())
    {
      return; // Data already seeded
    }

    // Create test users
    var testUser1 = new ApplicationUser
    {
      Id = "test-user-1",
      UserName = "testuser1",
      Email = "testuser1@example.com",
      PasswordHash = "Password123!",
      EmailConfirmed = true,
      NormalizedUserName = "TESTUSER1",
      NormalizedEmail = "TESTUSER1@EXAMPLE.COM",
      SecurityStamp = Guid.NewGuid().ToString(),
      ConcurrencyStamp = Guid.NewGuid().ToString()
    };

    var testUser2 = new ApplicationUser
    {
      Id = "test-user-2",
      UserName = "testuser2",
      Email = "testuser2@example.com",
      EmailConfirmed = true,
      NormalizedUserName = "TESTUSER2",
      NormalizedEmail = "TESTUSER2@EXAMPLE.COM",
      SecurityStamp = Guid.NewGuid().ToString(),
      ConcurrencyStamp = Guid.NewGuid().ToString()
    };

    // Add users to database
    db.Users.AddRange(testUser1, testUser2);
    db.SaveChanges(); // Save users first to get their IDs

    // Create test auctions
    var auction1 = new Auction
    {
      Id = 1,
      Title = "Test Auction 1",
      Description = "This is a test auction for integration testing",
      Starting_price = 100,
      Current_price = 100,
      Increment = 10,
      Status = AuctionStatus.NotFinished,
      Image_url = "https://example.com/image1.jpg",
      End_date = DateTime.UtcNow.AddDays(7),
      UserId = testUser1.Id
    };

    var auction2 = new Auction
    {
      Id = 2,
      Title = "Test Auction 2",
      Description = "Another test auction for integration testing",
      Starting_price = 200,
      Current_price = 250,
      Increment = 25,
      Status = AuctionStatus.NotFinished,
      Image_url = "https://example.com/image2.jpg",
      End_date = DateTime.UtcNow.AddDays(14),
      UserId = testUser2.Id
    };

    var auction3 = new Auction
    {
      Id = 3,
      Title = "Finished Auction",
      Description = "A finished auction for testing",
      Starting_price = 50,
      Current_price = 75,
      Increment = 5,
      Status = AuctionStatus.Paid,
      Image_url = "https://example.com/image3.jpg",
      End_date = DateTime.UtcNow.AddDays(-1),
      UserId = testUser1.Id
    };

    // Add auctions to database
    db.Auctions.AddRange(auction1, auction2, auction3);
    db.SaveChanges(); // Save auctions

    // Create test bids
    var bid1 = new Bid
    {
      Id = 1,
      Amount = 110,
      Time = DateTime.UtcNow.AddHours(-2),
      AuctionId = 2,
      UserId = testUser1.Id
    };

    var bid2 = new Bid
    {
      Id = 2,
      Amount = 250,
      Time = DateTime.UtcNow.AddHours(-1),
      AuctionId = 2,
      UserId = testUser2.Id
    };

    var bid3 = new Bid
    {
      Id = 3,
      Amount = 75,
      Time = DateTime.UtcNow.AddDays(-2),
      AuctionId = 3,
      UserId = testUser2.Id
    };

    // Add bids to database
    db.Bids.AddRange(bid1, bid2, bid3);
    db.SaveChanges(); // Save bids
  }
}