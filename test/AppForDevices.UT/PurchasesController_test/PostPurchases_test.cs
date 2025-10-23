using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.PurchaseDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForDevices.UT.PurchasesController_test
{
    public class PostPurchases_test : AppForDevices4Sqlite
    {
        public PostPurchases_test()
        {

            var models = new List<Model>()
            {
                new Model("Computer"),
                new Model("Phone"),
            };

            var devices = new List<Device>()
            {
                 new Device("iphone", "max", "apple", "white", 700, 3, 2023, models[1], 300, 5, Shared.QualityType.New),
                new Device("razerblade", "small", "razer", "black", 2000, 2, 2024, models[0], 500, 2, Shared.QualityType.New),
                new Device("thinkpad","medium", "lenovo", "black", 200, 0, 2021, models[0], 50, 2, Shared.QualityType.New),
            };

            ApplicationUser user = new ApplicationUser("1", "carmen@uclm.es", "Carmen", "Noblejas");

            var purchase = new Purchase("calle nueva", "Carmen", "Noblejas", DateTime.Today, new List<PurchaseItem>(), Shared.PaymentMethodTypes.CreditCard, user);
            var purchaseItem = new PurchaseItem(devices[1], 1, purchase);
            purchase.PurchaseItems.Add(purchaseItem);

            _context.AddRange(models);
            _context.AddRange(devices);
            _context.AddRange(purchase);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase()
        {
           
            PurchaseForCreateDTO purchaseNoPurchaseItem = new PurchaseForCreateDTO("calle escudero", new List<PurchaseItemDTO>(), "carmen@uclm.es", "Carmen Noblejas", Shared.PaymentMethodTypes.CreditCard);
            PurchaseForCreateDTO purchaseDeviceNoExist = new PurchaseForCreateDTO("calle escudero", new List<PurchaseItemDTO>(), "carmen@uclm.es", "Carmen Noblejas", Shared.PaymentMethodTypes.CreditCard);
            purchaseDeviceNoExist.PurchaseItems.Add(new PurchaseItemDTO(0, "lenovo legion", 1600, "Computer", 2));
            PurchaseForCreateDTO purchaseQuantityTooHigh = new PurchaseForCreateDTO("calle escudero", new List<PurchaseItemDTO>(), "carmen@uclm.es", "Carmen Noblejas", Shared.PaymentMethodTypes.CreditCard);
            purchaseQuantityTooHigh.PurchaseItems.Add(new PurchaseItemDTO(1, "iphone", 700, "Phone", 20000));
            PurchaseForCreateDTO purchaseForCreateUserDoesNotExist = new PurchaseForCreateDTO("calle escudero", new List<PurchaseItemDTO>(), "alguien@uclm.es", "Alguien desconocido", Shared.PaymentMethodTypes.CreditCard);
            purchaseForCreateUserDoesNotExist.PurchaseItems.Add(new PurchaseItemDTO(1, "iphone", 700, "Phone", 1));

            List<object[]> testcases = new List<object[]>()
            {
                new object[] {purchaseDeviceNoExist, $"Error! Device named {purchaseDeviceNoExist.PurchaseItems[0].Name} with Id {purchaseDeviceNoExist.PurchaseItems[0].DeviceId} does not exist in the database" },
                new object[] {purchaseForCreateUserDoesNotExist, "Error! UserName is not registered" },
                new object[] {purchaseNoPurchaseItem, "Error! You must include at least one device to be purchased" },
                new object[] {purchaseQuantityTooHigh, $"Error! Device named {purchaseQuantityTooHigh.PurchaseItems[0].Name} only has 3 units available but {purchaseQuantityTooHigh.PurchaseItems[0].Quantity} were selected" },
            };
            return testcases;

        }
        [Theory]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreatePurchase_Test_Error(PurchaseForCreateDTO? purchaseforcreate, string errorExpected)
        {
            //arrange
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();

            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);
            //act
            var result = await sut.CreatePurchase(purchaseforcreate);
            //assert
            var badrequest = Assert.IsType<BadRequestObjectResult>(result);
           
            var problems = Assert.IsType<ValidationProblemDetails>(badrequest.Value);
            Assert.Equal(errorExpected, problems.Errors.First().Value[0]);
        }

        [Fact]
        public async Task CreatePurchase_Test_Successful()
        {
            //arrange
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            PurchaseForCreateDTO purchaseforcreate = new PurchaseForCreateDTO("calle nueva", new List<PurchaseItemDTO>(), "carmen@uclm.es", "Carmen Noblejas", Shared.PaymentMethodTypes.CreditCard);
            purchaseforcreate.PurchaseItems.Add(new PurchaseItemDTO(1, "iphone", 700, "Phone", 1)); 

            PurchaseDetailDTO purchaseexpectedDetailDto = new PurchaseDetailDTO(2, "calle nueva", new List<PurchaseItemDTO>(), "carmen@uclm.es", "Carmen Noblejas", Shared.PaymentMethodTypes.CreditCard, DateTime.Now);
            purchaseexpectedDetailDto.PurchaseItems.Add(new PurchaseItemDTO(1, "iphone", 700, "Phone", 1));


            //act
            var result = await sut.CreatePurchase(purchaseforcreate);

            //assert
            var createdpurchaeforcreate = Assert.IsType<CreatedAtActionResult>(result);
            var purchasedetail = Assert.IsType<PurchaseDetailDTO>(createdpurchaeforcreate.Value);
            Assert.Equal(purchaseexpectedDetailDto, purchasedetail);

        }


    }



    }
