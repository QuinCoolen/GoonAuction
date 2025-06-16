using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using GoonAuctionBLL.Services;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Tests.Services
{
    public class BidServiceTests
    {
        private readonly Mock<IBidRepository> _mockBidRepository;
        private readonly Mock<IAuctionService> _mockAuctionService;
        private readonly BidService _bidService;

        public BidServiceTests()
        {
            _mockBidRepository = new Mock<IBidRepository>();
            _mockAuctionService = new Mock<IAuctionService>();
            _bidService = new BidService(_mockBidRepository.Object, _mockAuctionService.Object);
        }

        [Fact]
        public void GetBidsByUserId_WithValidUserId_ReturnsUserBids()
        {
            // Arrange
            var userId = "1";
            var expectedBids = new List<BidDto>
            {
                new BidDto { Id = 1, UserId = userId, AuctionId = 1, Amount = 100 },
                new BidDto { Id = 2, UserId = userId, AuctionId = 2, Amount = 200 }
            };
            _mockBidRepository.Setup(repo => repo.GetBidsByUserId(userId))
                .Returns(expectedBids);

            // Act
            var result = _bidService.GetBidsByUserId(userId);

            // Assert
            Assert.Equal(expectedBids, result);
            _mockBidRepository.Verify(repo => repo.GetBidsByUserId(userId), Times.Once);
        }

        [Fact]
        public void GetBidsByAuctionId_WithValidAuctionId_ReturnsAuctionBids()
        {
            // Arrange
            var auctionId = 1;
            var expectedBids = new List<BidDto>
            {
                new BidDto { Id = 1, UserId = "1", AuctionId = auctionId, Amount = 100 },
                new BidDto { Id = 2, UserId = "2", AuctionId = auctionId, Amount = 200 }
            };
            _mockBidRepository.Setup(repo => repo.GetBidsByAuctionId(auctionId))
                .Returns(expectedBids);

            // Act
            var result = _bidService.GetBidsByAuctionId(auctionId);

            // Assert
            Assert.Equal(expectedBids, result);
            _mockBidRepository.Verify(repo => repo.GetBidsByAuctionId(auctionId), Times.Once);
        }

        [Fact]
        public void PlaceBid_WithValidBid_ReturnsTrue()
        {
            // Arrange
            var bidDto = new BidDto
            {
                UserId = "1",
                AuctionId = 1,
                Amount = 200
            };
            var auction = new FullAuctionDto
            {
                Id = 1,
                CurrentPrice = 100,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            _mockAuctionService.Setup(service => service.GetAuction(bidDto.AuctionId))
                .Returns(auction);
            _mockAuctionService.Setup(service => service.UpdateCurrentPrice(bidDto.AuctionId, bidDto.Amount))
                .Returns(true);
            _mockBidRepository.Setup(repo => repo.PlaceBid(bidDto))
                .Returns(true);

            // Act
            var result = _bidService.PlaceBid(bidDto);

            // Assert
            Assert.True(result);
            _mockAuctionService.Verify(service => service.UpdateCurrentPrice(bidDto.AuctionId, bidDto.Amount), Times.Once);
            _mockBidRepository.Verify(repo => repo.PlaceBid(bidDto), Times.Once);
        }

        [Fact]
        public void PlaceBid_WithLowerAmount_ReturnsFalse()
        {
            // Arrange
            var bidDto = new BidDto
            {
                UserId = "1",
                AuctionId = 1,
                Amount = 50
            };
            var auction = new FullAuctionDto
            {
                Id = 1,
                CurrentPrice = 100,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            _mockAuctionService.Setup(service => service.GetAuction(bidDto.AuctionId))
                .Returns(auction);

            // Act
            var result = _bidService.PlaceBid(bidDto);

            // Assert
            Assert.False(result);
            _mockAuctionService.Verify(service => service.UpdateCurrentPrice(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockBidRepository.Verify(repo => repo.PlaceBid(It.IsAny<BidDto>()), Times.Never);
        }

        [Fact]
        public void PlaceBid_WithExpiredAuction_ReturnsFalse()
        {
            // Arrange
            var bidDto = new BidDto
            {
                UserId = "1",
                AuctionId = 1,
                Amount = 200
            };
            var auction = new FullAuctionDto
            {
                Id = 1,
                CurrentPrice = 100,
                EndDate = DateTime.UtcNow.AddDays(-1) // Expired auction
            };

            _mockAuctionService.Setup(service => service.GetAuction(bidDto.AuctionId))
                .Returns(auction);

            // Act
            var result = _bidService.PlaceBid(bidDto);

            // Assert
            Assert.False(result);
            _mockAuctionService.Verify(service => service.UpdateCurrentPrice(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockBidRepository.Verify(repo => repo.PlaceBid(It.IsAny<BidDto>()), Times.Never);
        }

        [Fact]
        public void PlaceBid_WithNonExistentAuction_ReturnsFalse()
        {
            // Arrange
            var bidDto = new BidDto
            {
                UserId = "1",
                AuctionId = 999,
                Amount = 200
            };

            _mockAuctionService.Setup(service => service.GetAuction(bidDto.AuctionId))
                .Returns((FullAuctionDto)null);

            // Act
            var result = _bidService.PlaceBid(bidDto);

            // Assert
            Assert.False(result);
            _mockAuctionService.Verify(service => service.UpdateCurrentPrice(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockBidRepository.Verify(repo => repo.PlaceBid(It.IsAny<BidDto>()), Times.Never);
        }
    }
} 