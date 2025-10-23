using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.ReviewDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AppForDevices.UT.ReviewsController_test
{
    public class PostReviewsController_test : AppForDevices4Sqlite
    {
        public PostReviewsController_test()
        {
            var devices = new List<Device>()
            {
                 new Device("Galaxy S21", "High-end smartphone", "Samsung", "Black", 2022, new Model("Phone")),
                new Device("iPad Pro", "Latest Apple tablet", "Apple", "Silver", 2023, new Model("Tablet"))
            };

            _context.Devices.AddRange(devices);
            _context.SaveChanges();
        }

        public static IEnumerable<Object[]> GenerateTestCasesForCreateReview()
        {
            // Case 1: No Review Items
            var case1 = new ReviewForCreateDTO
            {
                ReviewTitle = "Review without items",
                CustomerId = "customer123",
                CustomerCountry = "USA",
                ReviewItems = new List<ReviewItemDTO>()
            };
            string expectedError1 = "Error! You must include at least one device to review.";

            // Case 2: Device Not Found
            var case2 = new ReviewForCreateDTO
            {
                ReviewTitle = "Device not found test",
                CustomerId = "customer123",
                CustomerCountry = "USA",
                ReviewItems = new List<ReviewItemDTO>
                {
                    new ReviewItemDTO { DeviceId = 999, Comments = "Great product!", Rating = 4 }
                }
            };
            string expectedError2 = "Error! Device with Id 999 does not exist in the database.";

            // Case 3: Successful Review
            var case3 = new ReviewForCreateDTO
            {
                ReviewTitle = "Successful Review",
                CustomerId = "customer123",
                CustomerCountry = "USA",
                ReviewItems = new List<ReviewItemDTO>
                {
                    new ReviewItemDTO { DeviceId = 1, Comments = "Awesome product!", Rating = 5 }
                }
            };

            var allTestCases = new List<object[]>
            {
                new object[] { case1, expectedError1 },
                new object[] { case2, expectedError2 },
                new object[] { case3, null } // null expectedError = successful case
            };

            return allTestCases;
        }

        [Theory]
        [MemberData(nameof(GenerateTestCasesForCreateReview))]
        public async Task CreateReview_TestCases(ReviewForCreateDTO reviewForCreate, string expectedError)
        {
            // Arrange
            Mock<ILogger<ReviewsController>> mockLogger = new Mock<ILogger<ReviewsController>>();
            ReviewsController sut = new ReviewsController(_context, mockLogger.Object);

            // Act
            var result = await sut.CreateReview(reviewForCreate);

            // Assert
            if (expectedError == null) // Success :)
            {
                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                var createdReviewDto = Assert.IsType<ReviewConfirmationDTO>(createdResult.Value);

                Assert.Equal(reviewForCreate.ReviewTitle, createdReviewDto.ReviewTitle);
                Assert.Equal(reviewForCreate.CustomerId, createdReviewDto.CustomerId);
                Assert.Equal(reviewForCreate.CustomerCountry, createdReviewDto.CustomerCountry);

                Assert.Equal(reviewForCreate.ReviewItems.Count, createdReviewDto.ReviewItems.Count);
                for (int i = 0; i < reviewForCreate.ReviewItems.Count; i++)
                {
                    Assert.Equal(reviewForCreate.ReviewItems[i].DeviceId, createdReviewDto.ReviewItems[i].DeviceId); // Compare IDs
                    Assert.Equal(reviewForCreate.ReviewItems[i].Comments, createdReviewDto.ReviewItems[i].Comments);
                    Assert.Equal(reviewForCreate.ReviewItems[i].Rating, createdReviewDto.ReviewItems[i].Rating);
                }
                // Assert ReviewId is returned because now we show it in the post
                Assert.True(createdReviewDto.ReviewId > 0, "ReviewId should be greater than 0");
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
                Assert.Equal(expectedError, problemDetails.Errors.First().Value[0]);
            }
        }

    }



}
