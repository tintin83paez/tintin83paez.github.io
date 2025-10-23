using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.RepairDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForDevices.UT.ReceiptsController_test
{
    public class GetReceipts_test : AppForDevices4Sqlite
    {
        public GetReceipts_test()
        {
            var repairs = new List<Repair>()
            {
                new Repair("Repair Nikon",null,120.67f,new Scale("luxury")),
                new Repair("Repair Canon","We are going to repair the lens",69.67f,new Scale("medium"))
            };
            _context.AddRange(repairs);
            _context.SaveChanges();
            ApplicationUser user = new ApplicationUser("1", "agustin@uclm", "Agustín", "Prieto");

            var receipt = new Receipt(
                "Agustín Prieto",
                "Avda España s/n, Albacete 02071",
                Shared.PaymentMethodTypes.CreditCard,
                new List<ReceiptItem>(),
                user,
                DateTime.Today
                );
            receipt.ReceiptItems.Add(new ReceiptItem(repairs[0], receipt, "4500 SMDG"));

            _context.ApplicationUsers.Add(user);

            _context.AddRange(receipt);
            _context.SaveChanges();
        }
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReceipt_NotFound_test()
        {
            //Arrange
            Mock<ILogger<ReceiptsController>> mock = new Mock<ILogger<ReceiptsController>>();
            ILogger<ReceiptsController> logger = mock.Object;

            ReceiptsController sut = new ReceiptsController(_context, logger);
            //Act

            var result = await sut.GetReceipt(0);
            //Assert
            //we check that the response type is Ok and obtain the list of repairs
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task SaveReceipt_ReceiptsTableIsNull_Test()
        {
            // Arrange
            Mock<ILogger<ReceiptsController>> mockLogger = new Mock<ILogger<ReceiptsController>>();
            ILogger<ReceiptsController> logger = mockLogger.Object;
            ReceiptsController sut = new ReceiptsController(_context, logger);

            // Simulamos que la tabla Receipts es nula
            _context.Receipts = null;

            // Act
            var result = await sut.GetReceipt(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "UnitTesting")]
        [Trait("Database", "WithFixture")]
        public async Task GetReceipt_Ok_Test()
        {
            //Arrange
            Mock<ILogger<ReceiptsController>> mock = new Mock<ILogger<ReceiptsController>>();
            ReceiptsController sut = new ReceiptsController(_context, mock.Object);
            var expectedresult = new ReceiptDetailDTO(
               1,
               DateTime.Today,
               "agustin@uclm",
               Shared.PaymentMethodTypes.CreditCard,
               "Avda España s/n, Albacete 02071",
               "Agustín Prieto",
               new List<ReceiptItemDTO>()
               );
            expectedresult.ReceiptItems.Add(
                new ReceiptItemDTO(
                    1,
                    "Repair Nikon",
                    120.67f,
                    "luxury",
                    null,
                    "4500 SMDG"
                    )
                );

            //Act

            var result = await sut.GetReceipt(1);

            //Assert

            var okresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<ReceiptDetailDTO>(okresult.Value);

            Assert.Equal(expectedresult, actualresult);
        }
    }
}
