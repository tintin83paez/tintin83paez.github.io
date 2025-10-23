using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using Moq;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AppForDevices.Shared.RepairDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using AppForDevices.Shared;
using AppForDevices.API.Data;
using Microsoft.EntityFrameworkCore;
namespace AppForDevices.UT.ReceiptsController_test
{
    public class PostReceipts_test:AppForDevices4Sqlite
    {
        public PostReceipts_test()
        {
            var repairs = new List<Repair>()
            {
                new Repair("Repair Playstation 4",null,20.64f,new Scale("basic")),
                new Repair("Repair Xbox 364","We are going to repair an Xbox 364",10.19f,new Scale("basic"))
            };
            var user = new ApplicationUser("1", "agus@uclm.es", "Agustín", "Prieto");
            _context.Add(user);
            _context.AddRange(repairs);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> GenerateTestCasesForPostingAReceipt()
        {
            List<ReceiptForCreateDTO> receipts = new List<ReceiptForCreateDTO>
            {
                new ReceiptForCreateDTO(
                    
                    "Blasco Ibañez 24",
                    "Agustín Prieto",
                    "agus@uclm.es",
                    new List<ReceiptItemDTO>(),
                    PaymentMethodTypes.CreditCard
                    ),
                new ReceiptForCreateDTO(
                    
                    "Blasco Ibañez 24",
                    "Carmen Noblejas",
                    "carmen@uclm",
                    new List<ReceiptItemDTO>(){
                        new ReceiptItemDTO(
                            1,
                            "Repair Playstation 4",
                            20.64d,
                            "basic",
                            null,
                            "Slim 1TB"
                            )
                    },
                    PaymentMethodTypes.CreditCard
                    ),
                new ReceiptForCreateDTO(
                    
                    "Blasco Ibañez 24",
                    "Agustín Prieto",
                    "agus@uclm.es",
                    new List<ReceiptItemDTO>(){
                         new ReceiptItemDTO(
                            0,
                            "Repair Playstation", //repair.name != item.name on ReceiptsController on line 112
                            20.64d,
                            "basic",
                            null,
                            "Slim 1TB"
                            )
                    },
                    PaymentMethodTypes.CreditCard
                    )
            };
            List<object[]> testcases = new List<object[]>()
            {
                new object[]{receipts[0],"Error! You must include at least one repair to be repaired"},
                new object[]{ receipts[1],"Error! UserName is not registered" },
                new object[]{ receipts[2], "Error! Repair with Id '0' is not available" }
            };
            return testcases;
        }
        
        [Theory]
        [MemberData(nameof(GenerateTestCasesForPostingAReceipt))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithFixture")]
        public async Task CreateRepair_error_test(ReceiptForCreateDTO receiptForCreate, String errorExpected)
        {
            //Arrange
            Mock<ILogger<ReceiptsController>> mock = new Mock<ILogger<ReceiptsController>>();
            ReceiptsController sut = new ReceiptsController(_context, mock.Object);
            //Act
            var result = await  sut.CreateReceipt(receiptForCreate);
            //Assert
            var message = Assert.IsType<BadRequestObjectResult>(result);
            var notactualresult = Assert.IsType<ValidationProblemDetails>(message.Value);

            var actualresult = notactualresult.Errors.First().Value[0];
            Assert.Equal(actualresult,errorExpected);
        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithFixture")]
        public async Task CreateRental_Ok_test()
        {
            //Arrange
            Mock<ILogger<ReceiptsController>> mock = new Mock<ILogger<ReceiptsController>>();
            ReceiptsController sut = new ReceiptsController(_context, mock.Object);
            ReceiptForCreateDTO parameter = new ReceiptForCreateDTO(
                
                "Avda España s/n, Albacete 02071",
                "Agustín Prieto",
                "agus@uclm.es",
                new List<ReceiptItemDTO>()
                {
                    new ReceiptItemDTO(
                    1,
                    "Repair Playstation 4",
                    20.64f,
                   "basic",
                   null,
                   "Slim 1tb"
                    ),
                    new ReceiptItemDTO(
                    2,
                    "Repair Xbox 364",
                    10.19f,
                    "basic",
                    "We are going to repair an Xbox 364",
                    "Series"
                    )
                },
                PaymentMethodTypes.Cash
                );

            ReceiptDetailDTO expectedresult = new ReceiptDetailDTO(
                1,
                DateTime.Today,
                "agus@uclm.es",
                PaymentMethodTypes.Cash,
                "Avda España s/n, Albacete 02071",
                "Agustín Prieto",
                new List<ReceiptItemDTO>()
                {
                    new ReceiptItemDTO(
                    1,
                    "Repair Playstation 4",
                    20.64f,
                   "basic",
                   null,
                   "Slim 1tb"
                    ),
                    new ReceiptItemDTO(
                    2,
                    "Repair Xbox 364",
                    10.19f,
                    "basic",
                    "We are going to repair an Xbox 364",
                    "Series"
                    )
                }
                );

            //Act
            var result = await sut.CreateReceipt(parameter);
            //Assert
            var okresult = Assert.IsType<CreatedAtActionResult>(result);
            var actualresult = Assert.IsType<ReceiptDetailDTO>(okresult.Value);
            Assert.Equal( actualresult, expectedresult );
        }
    }
}
