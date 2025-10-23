using AppForDevices.API.Data;
using Microsoft.Data.Sqlite;
using AppForDevices.UT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForDevices.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForDevices.Shared.DeviceDTOs;
using AppForDevices.API.Controllers;

namespace AppForDevices.UT.DevicesController_test
{
    public class GetDevicesForRental_test : AppForDevices4Sqlite
    {
        public GetDevicesForRental_test()
        {
            var models = new List<Model>
            {
            new Model("iPhone"),
            new Model("Xiaomi")
            };
            var devices = new List<Device>
            {
                new Device("IPhone 16", "nothing", "Apple", "Green", 1200.0, 2, 2024, models[0], 120, 123, 0),  //Este es el actual
                new Device("Xiaomi Redmi", "nothing", "Xiaomi", "Blue", 1000.0, 2, 2022, models[1], 100, 123, (Shared.QualityType)2)    //Este es el actual

            };
            _context.AddRange(models);
            _context.AddRange(devices); //creo la base de datos con los datos metidos arriba
            _context.SaveChanges();
            //model y devices de ejemlplo
        }


        public static IEnumerable<object[]> TestCasesFor_GetDevicesForRental()
        {
            var deviceDTOs = new List<DeviceForRentalDTO>()
        {
            new DeviceForRentalDTO(1, "IPhone 16", "iPhone", "Apple", "Green", 123, 120.0, 0),
            new DeviceForRentalDTO(2, "Xiaomi Redmi", "Xiaomi", "Xiaomi", "Blue", 123, 100.0, (Shared.QualityType)2)

        };
            var deviceDTOsTC1 = new List<DeviceForRentalDTO>() { deviceDTOs[0], deviceDTOs[1] }
            .OrderBy(d => d.Name).ToList();

            var deviceDTOsTC2 = new List<DeviceForRentalDTO>() { deviceDTOs[0] };
            var deviceDTOsTC3 = new List<DeviceForRentalDTO>() { deviceDTOs[1] };
            var deviceDTOsTC4 = new List<DeviceForRentalDTO>() { deviceDTOs[1] };

            var allTests = new List<object[]>
        {
            new object[] { null, null, null ,deviceDTOsTC1 }, //Filtra ambos
            new object[] {"iPhone", null, null, deviceDTOsTC2}, //Filtra IPhone por modelo
            new object[] {null, 100.0, null, deviceDTOsTC3 }, //Filtra Xiaomi por priceForRental, por eso tengo d.priceForRent == priceForRental en el DeviceController.cs
            new object[] {null, null, "Blue", deviceDTOsTC4 } //Filtra Xiaomi por color azul
        };
            return allTests;

        }


        [Theory]
        [MemberData(nameof(TestCasesFor_GetDevicesForRental))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task TestCasesFor_GetDevicesForRental_test(string? filterModel, double? filterPriceForRental, string? filterColor, IList<DeviceForRentalDTO> expectedDevices)
        {
            // Arrange
            var controller = new DevicesController(_context, null);

            // Act
            var result = await controller.GetDevicesForRental(filterModel, filterPriceForRental, filterColor);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var deviceDTOsActual = Assert.IsType<List<DeviceForRentalDTO>>(okResult.Value);
            Assert.Equal(expectedDevices, deviceDTOsActual);
        }
    }
}