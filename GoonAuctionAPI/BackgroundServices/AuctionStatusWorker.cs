using GoonAuctionBLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GoonAuctionAPI.BackgroundServices
{
    public class AuctionStatusWorker : BackgroundService
    {
        private readonly ILogger<AuctionStatusWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Check every minute

        public AuctionStatusWorker(ILogger<AuctionStatusWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Auction Status Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    CheckAndUpdateExpiredAuctions();
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Auction Status Worker stopped");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking expired auctions");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Wait 5 minutes before retrying
                }
            }
        }

        private void CheckAndUpdateExpiredAuctions()
        {
            using var scope = _serviceProvider.CreateScope();
            var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

            var updatedCount = auctionRepository.UpdateExpiredAuctions();

            if (updatedCount > 0)
            {
                _logger.LogInformation("Updated {Count} expired auctions from NotFinished to Unpaid", updatedCount);
            }
        }
    }
}
