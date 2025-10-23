using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.RentalDTOs;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UT.RentalsController_test
{
    public class PostRentals_test : AppForDevices4Sqlite
    {
        public PostRentals_test()
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

            ApplicationUser user = new ApplicationUser("1", "javier.126@alu.uclm.es", "Javier", "Garcia");

            var rental = new Rental("Javier", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", DateTime.Now, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 1200.0, Shared.PaymentMethodTypes.CreditCard, user);
            rental.RentDevices.Add(new RentDevice(devices[0], 1, rental));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(devices);
            _context.AddRange(models);
            _context.Add(rental);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase()
        {
            var rentalNoITem = new RentalForCreateDTO("Javier", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RentalItemDTO>());
            // El new List<RentalItemDTO>() crea una especie de Struct como en C donde añadirá el new RentalItemDTO("IPhone", 0, 120, 1, 1) con los valores del dispositivo a rentar
            // En este caso string de modelo, calidad, precio de renta, Id del device y cantidad (justo la línea de abajo). Todo es añadido a la variable rentalItems

            var rentalItems = new List<RentalItemDTO>() { new RentalItemDTO("IPhone", 0, 120, 1, 1) };

            var rentalFromBeforeToday = new RentalForCreateDTO("javier.126@alu.uclm.es", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard, DateTime.Today, DateTime.Today.AddDays(5), rentalItems);

            var rentalToBeforeFrom = new RentalForCreateDTO("javier.126@alu.uclm.es", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard, DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), rentalItems);

            var RentalApplicationUser = new RentalForCreateDTO("elena@uclm.es", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard, DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), rentalItems);

            var rentalDeviceNotAvailable = new RentalForCreateDTO("javier.126@alu.uclm.es", "Javier Garcia", "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RentalItemDTO>() { new RentalItemDTO("Xiaomi", (Shared.QualityType)2, 100, 2, 1) });


            var allTests = new List<object[]>
            {             //input for createpurchase - Error expected
                new object[] { rentalNoITem, "Error! You must include at least one device to be rented",  },
                new object[] { rentalFromBeforeToday, "Error! Your rental date must start later than today", },
                new object[] { rentalToBeforeFrom, "Error! Your rental must end later than it starts", },
                new object[] { RentalApplicationUser, "Error! UserName is not registered", },
                new object[] { rentalDeviceNotAvailable, "Error! Device is not available for being rented for these days", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreateRental_Error_test(RentalForCreateDTO rentalDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateRental_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            var rentalDTO = new RentalForCreateDTO("javier.126@alu.uclm.es", "Javier Garcia",
                "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(6), DateTime.Today.AddDays(7), new List<RentalItemDTO>()
                { new RentalItemDTO(1, "IPhone 16", "IPhone", 1, 120.0) }); //Name y model cambiado para que funcione

            var expectedrentalDetailDTO = new RentalDetailDTO(2, DateTime.Now,
                "javier.126@alu.uclm.es", "Javier Garcia",
                "Muñoz Seca 20, Albacete 02002", Shared.PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(6), DateTime.Today.AddDays(7), new List<RentalItemDTO>()
                { new RentalItemDTO(1, "IPhone 16", "IPhone", 1, 120.0) });

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<RentalDetailDTO>(createdResult.Value);

            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO);

        }
    }
}