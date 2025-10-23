using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.RentalDTOs;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using AppForDevices.Shared.DeviceDTOs;
using AppForDevices.Shared.PurchaseDTOs;

namespace AppForDevices.UT.RentalsController_test
{
    public class GetRentals_test : AppForDevices4Sqlite
    {
        public GetRentals_test()
        {

            var models = new List<Model>() {
                new Model("IPhone"),
                new Model("Xiaomi"),
            };

            var devices = new List<Device>
            {
                new Device("IPhone 16", "nothing", "Apple", "Green", 1200.0, 2, 2024, models[0], 120, 123, 0),  //Este es el actual
                new Device("Xiaomi Redmi", "nothing", "Xiaomi", "Blue", 1000.0, 2, 2022, models[1], 100, 123, (Shared.QualityType)2)    //Este es el actual

            };

            ApplicationUser user = new ApplicationUser("1", "javier@uclm.es", "Javier", "Garcia");

            var rental = new Rental("Javier", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", DateTime.Now, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 1200.0, Shared.PaymentMethodTypes.CreditCard, user);
            rental.RentDevices.Add(new RentDevice(devices[0], 1, rental));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(devices);
            _context.AddRange(models);
            _context.Add(rental);
            _context.SaveChanges();
        }
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.GetRental(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task GetRental_Successful_Test()
        {
            //arrange 
            Mock<ILogger<RentalsController>> mockLogger = new Mock<ILogger<RentalsController>>();
            RentalsController sut = new RentalsController(_context, mockLogger.Object);

            //act
            RentalDetailDTO expectedRental = new RentalDetailDTO(1, DateTime.Now, "Javier", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RentalItemDTO>());
            expectedRental.RentalItems.Add(new RentalItemDTO("IPhone", 0, 120, 1, 1));
            var result = await sut.GetRental(1);

            //Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var rentalDetailActual = Assert.IsType<RentalDetailDTO>(okresult.Value);
            Assert.Equal(expectedRental, rentalDetailActual);


        }
    }
}