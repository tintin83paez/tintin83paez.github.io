using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.DeviceDTOs;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UT.DevicesController_test
{
    public class GetDevicesForPurchase_test:AppForDevices4Sqlite
    {
        public GetDevicesForPurchase_test()
        {
            var models = new List<Model>()
            {
                new Model("Computer"),
                new Model("Phone")
            };

            var devices = new List<Device>() {
                new Device("iphone", "max", "apple", "white", 700, 3, 2023, models[1], 300, 5, Shared.QualityType.New),
                new Device("razerblade", "small", "razer", "black", 2000, 2, 2024, models[0], 500, 2, Shared.QualityType.New),
                new Device("thinkpad","medium", "lenovo", "black", 200, 0, 2021, models[0], 50, 2, Shared.QualityType.New),
            };
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> GenerateTestCasesForGetDevicesForPurchaseWithandWithoutFilters()
        {

            List<DeviceForPurchaseDTO> allDevices = new List<DeviceForPurchaseDTO>()
            {
                new DeviceForPurchaseDTO(1, "iphone", "apple", "white", 700,  "Phone"),
                new DeviceForPurchaseDTO(2, "razerblade", "razer", "black", 2000,  "Computer"),

            };
            List<DeviceForPurchaseDTO> devicesfiltername = new List<DeviceForPurchaseDTO>() { allDevices[0] };
            List<DeviceForPurchaseDTO> devicesfiltercolor = new List<DeviceForPurchaseDTO>() { allDevices[0] };
            List<DeviceForPurchaseDTO> devicesfiltermodel = new List<DeviceForPurchaseDTO>() { allDevices[1] };
            List<object[]> testcases = new List<object[]>()
            {
                new object[]{null,null,null, allDevices}, //Dejar solo name y color
                new object[]{"iphone",null, null, devicesfiltername},
                new object[]{null,"white", null, devicesfiltercolor},
                new object[]{null,null, "Computer", devicesfiltermodel},
            };
            return testcases;
        }

        [Theory]
        [MemberData(nameof(GenerateTestCasesForGetDevicesForPurchaseWithandWithoutFilters))]
        public async Task GetDevicesForPurchase_withoutFilters(string filterName, string filterColor, string? filterModel, List<DeviceForPurchaseDTO> expectedDevices)
        {
            //arrange
            Mock<ILogger<DevicesController>> mockLogger = new Mock<ILogger<DevicesController>>();
            DevicesController sut = new DevicesController(_context, mockLogger.Object);

            //List<DeviceForPurchaseDTO> expectedDevices = new List<DeviceForPurchaseDTO>()
            //{
            //    new DeviceForPurchaseDTO(1, "iphone", "apple", "white", 700,  "Phone"),
            //    new DeviceForPurchaseDTO(2, "razerblade", "razer", "black", 2000,  "Computer"),
            //};
            //act
            var result = await sut.GetDevicesForPurchase(filterName, filterColor, filterModel);
            //assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<List<DeviceForPurchaseDTO>>(okresult.Value);
            Assert.Equal(expectedDevices, actualresult);
        }

        //[Fact]
        //[Trait("LevelOfTesting", "Unit Testing")]
        //[Trait("TypeOfTesting", "Functional Testing")]
        ////driver code
        //public async Task ComputeDivision_Ok()
        //{
        //    //arrange-> What we need to run the test
        //    DevicesController sut = new DevicesController(_context, null);
        //    decimal expectedresult = 1.0m;
        //    //act
        //    var result = await sut.ComputeDivision(1.0m, 1.0m);
        //    //assert -> checks if the expected and the actual are the same
        //    var okresult = Assert.IsType<OkObjectResult>(result);
        //    var decimalresult = Assert.IsType<decimal>(okresult.Value);
        //    Assert.Equal(expectedresult, decimalresult);

        //}

        //[Fact]
        //public async Task ComputeDivision_badRequest()
        //{
        //    //arrange-> What we need to run the tes
        //    var mok = new Mock<ILogger<DevicesController>>();
        //    DevicesController sut = new DevicesController(_context, mok.Object);
        //    String expectedresult = "op2 must be different from 0";
        //    //act
        //    var result = await sut.ComputeDivision(1, 0);
        //    //assert -> checks if the expected and the actual are the same
        //    var badrequest = Assert.IsType<BadRequestObjectResult>(result);
        //    var stringresult = Assert.IsType<string>(badrequest.Value);
        //    Assert.Equal(expectedresult, stringresult);
        //}


    }
}
