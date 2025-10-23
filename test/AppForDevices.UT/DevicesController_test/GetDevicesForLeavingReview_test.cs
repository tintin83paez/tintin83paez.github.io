using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.DeviceDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForDevices.UT.DevicesController_test
{
    public class GetDevicesForLeavingReview_test : AppForDevices4Sqlite
    {
        // Arrange Step: Creating mock data and setting up the environment
        public GetDevicesForLeavingReview_test()
        {
            var models = new List<Model>()
            {
                new Model("Phone"),
                new Model("Tablet"),
            };

            var devices = new List<Device>()
            {
                new Device("Galaxy S21", "High-end smartphone", "Samsung", "Black", 2022, models[0]),
                new Device("iPad Pro", "Latest Apple tablet", "Apple", "Silver", 2023, models[1]),
            };

            // Add models and devices to the context
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();


        }

        public static IEnumerable<object[]> GenerateTestCasesForGetDevicesForLeavingReviewwithoutFilters()
        {
            //Case 1: Filter brand 'Samsung'
            List<DeviceForLeavingReviewDTO> expectedDevices1 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(1, "Galaxy S21", "Samsung", "Phone", "High-end smartphone", "Black", 2022),
            };

            //Case 2: Filter year '2023'
            List<DeviceForLeavingReviewDTO> expectedDevices2 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(2, "iPad Pro", "Apple", "Tablet", "Latest Apple tablet", "Silver", 2023),
            };

            // Case 3: Filter brand and year 'Apple' and 2023 
            List<DeviceForLeavingReviewDTO> expectedDevices3 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(2, "iPad Pro", "Apple", "Tablet", "Latest Apple tablet", "Silver", 2023),
            };

            //Case 4: No filters
            List<DeviceForLeavingReviewDTO> expectedDevices4 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(1, "Galaxy S21", "Samsung", "Phone", "High-end smartphone", "Black", 2022),
                new DeviceForLeavingReviewDTO(2, "iPad Pro", "Apple", "Tablet", "Latest Apple tablet", "Silver", 2023),
            };


            // Case 5: Filter name 'Galaxy S21'
            List<DeviceForLeavingReviewDTO> expectedDevices5 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(1, "Galaxy S21", "Samsung", "Phone", "High-end smartphone", "Black", 2022),
            };

            // Case 6: Filter brand and name
            List<DeviceForLeavingReviewDTO> expectedDevices6 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(1, "Galaxy S21", "Samsung", "Phone", "High-end smartphone", "Black", 2022),
            };

            // Case 7: Filter year and name 
            List<DeviceForLeavingReviewDTO> expectedDevices7 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(1, "Galaxy S21", "Samsung", "Phone", "High-end smartphone", "Black", 2022),
            };
            // Case 8: Filter brand, year and name
            List<DeviceForLeavingReviewDTO> expectedDevices8 = new List<DeviceForLeavingReviewDTO>()
            {
                new DeviceForLeavingReviewDTO(1, "Galaxy S21", "Samsung", "Phone", "High-end smartphone", "Black", 2022),
            };


            List<object[]> testcases = new List<object[]>()
            {
                new object[] { "Samsung", null, null, expectedDevices1 },
                new object[] { null, 2023, null, expectedDevices2 },
                new object[] { "Apple", 2023, null, expectedDevices3 },
                new object[] { null, null, null,expectedDevices4 },
                new object[] { null, null, "Galaxy S21", expectedDevices5 },
                new object[] { "Samsung", null, "Galaxy S21", expectedDevices6 },
                new object[] { null, 2022, "Galaxy S21", expectedDevices7 },
                new object[] { "Samsung", 2022, "Galaxy S21", expectedDevices8 },
            };

            return testcases;
        }


        [Theory]
        [MemberData(nameof(GenerateTestCasesForGetDevicesForLeavingReviewwithoutFilters))]
        public async Task GetDevicesForLeavingReview_withOutFilters(
           string filterBrand, int? filterYear, string? filterName, List<DeviceForLeavingReviewDTO> expectedDevices)
        {
            // Arrange
            Mock<ILogger<DevicesController>> mockLogger = new Mock<ILogger<DevicesController>>();
            DevicesController sut = new DevicesController(_context, mockLogger.Object);

            //List<DeviceForLeavingReviewDTO> expectedDevices = new List<DeviceForLeavingReviewDTO>()
            //{
            //    new DeviceForLeavingReviewDTO(1,"Galaxy S21", "Samsung", "Phone", "black", 2022),
            //    new DeviceForLeavingReviewDTO(2,"iPad Pro", "Apple", "Tablet", "black", 2023),
            //}

            // Act
            var result = await sut.GetDevicesForLeavingReview(filterBrand, filterYear, filterName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<DeviceForLeavingReviewDTO>>(okResult.Value);
            Assert.Equal(expectedDevices, actualResult);
        }
    }
}
