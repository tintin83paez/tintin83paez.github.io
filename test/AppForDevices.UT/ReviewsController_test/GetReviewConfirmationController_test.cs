using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.ReviewDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UT.ReviewsController_test
{
    public class GetReviewConfirmationController_test : AppForDevices4Sqlite
    {
        public GetReviewConfirmationController_test()
        {
            // Arrange
            var devices = new List<Device>()
            {
                new Device("Galaxy S21", "High-end smartphone", "Samsung", "Black", 2022, new Model("Phone")),
                new Device("iPad Pro", "Latest Apple tablet", "Apple", "Silver", 2023, new Model("Tablet"))
            };

            var reviews = new List<Review>()
            {
                new Review("Great Devices", DateTime.UtcNow, "USA", "customer123", new List<ReviewItem>
                {
                    new ReviewItem(1, new Review(), 1, devices[0], "Fantastic device!", 5),
                    new ReviewItem(2, new Review(), 2, devices[1], "Good tablet!", 4)
                })
            };

            _context.Devices.AddRange(devices);
            _context.Reviews.AddRange(reviews);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReviewConfirmation_Success_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var controller = new ReviewsController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetReviewConfirmation(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewConfirmation = Assert.IsType<ReviewConfirmationDTO>(okResult.Value);

            Assert.Equal(0, reviewConfirmation.ReviewId); // Assert id
            Assert.Equal("Great Devices", reviewConfirmation.ReviewTitle);
            Assert.Equal(2, reviewConfirmation.ReviewItems.Count); // Review has two items
            Assert.Equal(4.5, reviewConfirmation.OverallRating, 1); // Average
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReviewConfirmation_NotFound_Test() 
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var controller = new ReviewsController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetReviewConfirmation(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
