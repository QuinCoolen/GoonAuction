using System.Net;
using System.Text.Json;
using GoonAuctionBLL.Dto;
using GoonAuctionBLL.Interfaces;

namespace GoonAuctionAPI.Tests.IntegrationTests
{
    public class CheckoutControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task CreateCheckoutSession_WithValidAuctionId_ReturnsOk()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // Ensure auction with ID 1 exists in your seed data
            var response = await client.PostAsync("/create-checkout-session?auctionId=1", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("url", content); // just check the response has a 'url' property
        }

        [Fact]
        public async Task CreateCheckoutSession_WithInvalidAuctionId_ReturnsNotFound()
        {
            using var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var response = await client.PostAsync("/create-checkout-session?auctionId=999", null);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
