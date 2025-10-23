using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.PurchaseDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UT.PurchasesController_test
{
    public class GetPurchases_test : AppForDevices4Sqlite
    {
        public GetPurchases_test()
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

            var purchase = new Purchase("calle nueva", "carmen@uclm.es", "Carmen Noblejas", DateTime.Now, new List<PurchaseItem>(), Shared.PaymentMethodTypes.CreditCard, user);
            var purchaseItem = new PurchaseItem(devices[1], 1, purchase);
            purchase.PurchaseItems.Add(purchaseItem);

            _context.AddRange(models);
            _context.AddRange(devices);
            _context.AddRange(purchase);
            _context.SaveChanges();

        }

        [Fact]
        public async Task GetPurchase_Successful_Test()
        {
            //arrange 
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            //act
            PurchaseDetailDTO expectedPurchase = new PurchaseDetailDTO(1, "calle nueva", new List<PurchaseItemDTO>(), "carmen@uclm.es", "Carmen Noblejas", Shared.PaymentMethodTypes.CreditCard, DateTime.Now);
            expectedPurchase.PurchaseItems.Add(new PurchaseItemDTO(2, "razerblade", 2000, "Computer", 1));
            var result = await sut.GetPurchase(1);

            //Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var purchaseDetailActual = Assert.IsType<PurchaseDetailDTO>(okresult.Value);
            Assert.Equal(expectedPurchase, purchaseDetailActual);


        }

        [Fact]
        public async Task GetPurchase_NotFound_test()
        {
            //arrange
            Mock<ILogger<PurchasesController>> mockLogger = new Mock<ILogger<PurchasesController>>();
            PurchasesController sut = new PurchasesController(_context, mockLogger.Object);

            //act
            var result = await sut.GetPurchase(0);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
