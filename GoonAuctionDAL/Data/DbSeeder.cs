using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoonAuctionDAL.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync(ct);

        const string username = "John Doe";
        const string email = "johndoe@example.com";
        const string password = "Passw0rd!";

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Failed to create seed user: {errors}");
            }
        }

        if (!await context.Set<Auction>().AnyAsync(ct))
        {
            var now = DateTime.UtcNow;

            var auctions = new List<Auction>
            {
                new Auction
                {
                    Title = "1952 Topps #311 Yankees Outfielder Rookie Card — High‑Grade Slab",
                    Description = "Iconic 1952 Topps #311 rookie card of a New York outfielder, presented in a graded slab (label in photos). Bright color and sharp corners. Ships insured.",
                    Starting_price = 50000,
                    Current_price = 50000,
                    Increment = 1000,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/baseball.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Estate Statement Jewelry Set — Crystal & Pearl Necklaces with Earrings",
                    Description = "Curated estate lot of statement necklaces, earrings, and rings. Rhinestone and faux‑pearl accents; light wear; display props not included.",
                    Starting_price = 1200,
                    Current_price = 1200,
                    Increment = 25,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/collection.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Baroque‑Style Living Room Suite — Sofa, Loveseat, Armchair & Tables",
                    Description = "Carved and gilt wood suite with tufted upholstery; coffee and side tables included. Minor finish wear; pickup or white‑glove freight recommended.",
                    Starting_price = 3500,
                    Current_price = 3500,
                    Increment = 100,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/furniture.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Apple iPhone SE — Rose Gold, Unlocked",
                    Description = "First‑gen SE in rose gold with clear case. Unlocked; powers on. Battery health and accessories not verified. Sold as‑is.",
                    Starting_price = 80,
                    Current_price = 80,
                    Increment = 5,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/iphone.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Gilt‑Framed Landscape Oil on Canvas — Pastoral Valley Scene",
                    Description = "Large landscape oil on canvas in ornate gilt frame depicting a valley with figures and sheep. Signature not visible; light craquelure; ready to hang.",
                    Starting_price = 1000,
                    Current_price = 1000,
                    Increment = 50,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/painting.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Handwoven Wool Area Rug — Persian‑Style Floral",
                    Description = "Wool pile rug with rich red field and repeating floral medallions; bordered vines. Even pile; edges sound. Professional cleaning recommended; ships rolled.",
                    Starting_price = 400,
                    Current_price = 400,
                    Increment = 10,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/rug.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Thriller — Vinyl LP, Gatefold, Inner Sleeve",
                    Description = "Classic 1980s pop LP on Epic with gatefold jacket and lyric inner. Vinyl shows light hairlines; play‑graded VG/VG+.",
                    Starting_price = 25,
                    Current_price = 25,
                    Increment = 2,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/vinyl.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
                new Auction
                {
                    Title = "Patek Philippe Aquanaut Stainless Steel Automatic",
                    Description = "Stainless‑steel sports watch with black textured dial, date at 3, and black rubber strap. Running at time of listing; service history not known. Pouch shown included.",
                    Starting_price = 15000,
                    Current_price = 15000,
                    Increment = 250,
                    Status = AuctionStatus.NotFinished,
                    Image_url = "/images/watch.png",
                    End_date = now.AddDays(7),
                    UserId = user.Id
                },
            };

            await context.Set<Auction>().AddRangeAsync(auctions, ct);
            await context.SaveChangesAsync(ct);
        }
    }
}