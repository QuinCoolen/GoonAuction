using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using GoonAuctionBLL.Services;
using GoonAuctionBLL.Interfaces;
using GoonAuctionBLL.Dto;

namespace GoonAuctionBLL.Tests.Services
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _mockAuctionRepository;
        private readonly Mock<IBidRepository> _mockBidRepository;
        private readonly AuctionService _auctionService;

        public AuctionServiceTests()
        {
            _mockAuctionRepository = new Mock<IAuctionRepository>();
            _mockBidRepository = new Mock<IBidRepository>();
            _auctionService = new AuctionService(_mockAuctionRepository.Object, _mockBidRepository.Object);
        }

        [Fact]
        public void GetAuctions_WhenCalled_ReturnsAllAuctions()
        {
            // Arrange
            var expectedAuctions = new List<AuctionDto>
            {
                new AuctionDto { Id = 1, Title = "Auction 1", CurrentPrice = 100 },
                new AuctionDto { Id = 2, Title = "Auction 2", CurrentPrice = 200 }
            };
            _mockAuctionRepository.Setup(repo => repo.GetAuctions())
                .Returns(expectedAuctions);

            // Act
            var result = _auctionService.GetAuctions();

            // Assert
            Assert.Equal(expectedAuctions, result);
            _mockAuctionRepository.Verify(repo => repo.GetAuctions(), Times.Once);
        }

        [Fact]
        public void GetAuction_WithValidId_ReturnsAuction()
        {
            // Arrange
            var auctionId = 1;
            var expectedAuction = new FullAuctionDto 
            { 
                Id = auctionId, 
                Title = "Test Auction",
                User = new UserDto { Id = "1", Username = "testuser" }
            };
            _mockAuctionRepository.Setup(repo => repo.GetAuction(auctionId))
                .Returns(expectedAuction);

            // Act
            var result = _auctionService.GetAuction(auctionId);

            // Assert
            Assert.Equal(expectedAuction, result);
            _mockAuctionRepository.Verify(repo => repo.GetAuction(auctionId), Times.Once);
        }

        [Fact]
        public void GetAuctionsByUserId_WithValidUserId_ReturnsUserAuctions()
        {
            // Arrange
            var userId = "1";
            var userBids = new List<BidDto>
            {
                new BidDto { AuctionId = 1, UserId = userId, Amount = 100 },
                new BidDto { AuctionId = 2, UserId = userId, Amount = 200 }
            };
            var allAuctions = new List<AuctionDto>
            {
                new AuctionDto { Id = 1, Title = "Auction 1" },
                new AuctionDto { Id = 2, Title = "Auction 2" },
                new AuctionDto { Id = 3, Title = "Auction 3" }
            };

            _mockBidRepository.Setup(repo => repo.GetBidsByUserId(userId))
                .Returns(userBids);
            _mockAuctionRepository.Setup(repo => repo.GetAuctions())
                .Returns(allAuctions);

            // Act
            var result = _auctionService.GetAuctionsByUserId(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, a => a.Id == 1);
            Assert.Contains(result, a => a.Id == 2);
            Assert.DoesNotContain(result, a => a.Id == 3);
        }

        [Fact]
        public void CreateAuction_WithValidData_ReturnsCreatedAuction()
        {
            // Arrange
            var newAuction = new CreateEditAuctionDto
            {
                Title = "New Auction",
                Description = "Description",
                StartingPrice = 100,
                CurrentPrice = 100,
                Increment = 10,
                Status = "Active",
                ImageUrl = "image.jpg",
                EndDate = DateTime.UtcNow.AddDays(7),
                UserId = "1"
            };
            var expectedAuction = new AuctionDto
            {
                Id = 1,
                Title = newAuction.Title,
                CurrentPrice = newAuction.CurrentPrice
            };

            _mockAuctionRepository.Setup(repo => repo.CreateAuction(newAuction))
                .Returns(expectedAuction);

            // Act
            var result = _auctionService.CreateAuction(newAuction);

            // Assert
            Assert.Equal(expectedAuction, result);
            _mockAuctionRepository.Verify(repo => repo.CreateAuction(newAuction), Times.Once);
        }

        [Fact]
        public void UpdateCurrentPrice_WithHigherPrice_ReturnsTrue()
        {
            // Arrange
            var auctionId = 1;
            var currentPrice = 200;
            var existingAuction = new FullAuctionDto
            {
                Id = auctionId,
                Title = "Test Auction",
                CurrentPrice = 100,
                User = new UserDto { Id = "1", Username = "testuser" }
            };

            _mockAuctionRepository.Setup(repo => repo.GetAuction(auctionId))
                .Returns(existingAuction);
            _mockAuctionRepository.Setup(repo => repo.UpdateAuction(auctionId, It.IsAny<CreateEditAuctionDto>()))
                .Returns(new AuctionDto { Id = auctionId, CurrentPrice = currentPrice });

            // Act
            var result = _auctionService.UpdateCurrentPrice(auctionId, currentPrice);

            // Assert
            Assert.True(result);
            _mockAuctionRepository.Verify(repo => repo.UpdateAuction(auctionId, It.IsAny<CreateEditAuctionDto>()), Times.Once);
        }

        [Fact]
        public void UpdateCurrentPrice_WithLowerPrice_ReturnsFalse()
        {
            // Arrange
            var auctionId = 1;
            var currentPrice = 50;
            var existingAuction = new FullAuctionDto
            {
                Id = auctionId,
                Title = "Test Auction",
                CurrentPrice = 100,
                User = new UserDto { Id = "1", Username = "testuser" }
            };

            _mockAuctionRepository.Setup(repo => repo.GetAuction(auctionId))
                .Returns(existingAuction);

            // Act
            var result = _auctionService.UpdateCurrentPrice(auctionId, currentPrice);

            // Assert
            Assert.False(result);
            _mockAuctionRepository.Verify(repo => repo.UpdateAuction(It.IsAny<int>(), It.IsAny<CreateEditAuctionDto>()), Times.Never);
        }

        [Fact]
        public void UpdateAuctionStatus_WithValidStatus_ReturnsTrue()
        {
            // Arrange
            var auctionId = 1;
            var newStatus = "Completed";
            var existingAuction = new FullAuctionDto
            {
                Id = auctionId,
                Title = "Test Auction",
                Status = "Active",
                User = new UserDto { Id = "1", Username = "testuser" }
            };

            _mockAuctionRepository.Setup(repo => repo.GetAuction(auctionId))
                .Returns(existingAuction);
            _mockAuctionRepository.Setup(repo => repo.UpdateAuction(auctionId, It.IsAny<CreateEditAuctionDto>()))
                .Returns(new AuctionDto { Id = auctionId, Status = newStatus });

            // Act
            var result = _auctionService.UpdateAuctionStatus(auctionId, newStatus);

            // Assert
            Assert.True(result);
            _mockAuctionRepository.Verify(repo => repo.UpdateAuction(auctionId, It.IsAny<CreateEditAuctionDto>()), Times.Once);
        }

        [Fact]
        public void DeleteAuction_WithValidId_ReturnsTrue()
        {
            // Arrange
            var auctionId = 1;
            _mockAuctionRepository.Setup(repo => repo.DeleteAuction(auctionId))
                .Returns(true);

            // Act
            var result = _auctionService.DeleteAuction(auctionId);

            // Assert
            Assert.True(result);
            _mockAuctionRepository.Verify(repo => repo.DeleteAuction(auctionId), Times.Once);
        }
    }
} 