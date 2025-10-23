using AppForDevices.API.Controllers;
using AppForDevices.API.Models;
using AppForDevices.Shared.RepairDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForDevices.UT.RepairsController_test
{
    public class GetRepairsForMaintenance_test : AppForDevices4Sqlite
    {
        public GetRepairsForMaintenance_test()
        {
            var repairs = new List<Repair>()
            {
                new Repair("Repair Samsung","We are going to repair a samsung",21.35f,new Scale("basic")),
                new Repair("Repair iphone","We are going to repair an iphone",31.61f,new Scale("luxury")),
                new Repair("Repair xiaomi","We are going to repair an iphone",9.61f,new Scale("medium"))
            };
            _context.AddRange(repairs);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> GenerateTestCasesForGetRepairsForMaintenanceWithandWithoutFilters()
        {
            List<RepairDTO> allRepairs = new List<RepairDTO>()
            {
                new RepairDTO(1,"Repair Samsung",21.35f,"basic","We are going to repair a samsung"),
                new RepairDTO(2,"Repair iphone",31.61f,"luxury","We are going to repair an iphone"),
                new RepairDTO(3,"Repair xiaomi",9.61f,"medium","We are going to repair an iphone")
            };
            List<RepairDTO> repairsForFilterWithRepairName = new List<RepairDTO>() {
                allRepairs[0]
            };
            List<RepairDTO> repairsForFilterWithScaleName = new List<RepairDTO>() {
                allRepairs[1]
            };
            List<RepairDTO> repairs_for_filter_cost_less_or_equal = new List<RepairDTO>() {
                allRepairs[2]
            };
            List<object[]> testcases = new List<object[]>()
            {
                new object[]{null,null,null,allRepairs},
                new object[]{ "Sam", null,null,repairsForFilterWithRepairName},
                new object[]{null, "luxury", null,repairsForFilterWithScaleName},
                new object[]{null, null, 10.00f, repairs_for_filter_cost_less_or_equal }
            };
            return testcases;
        }

        [Theory]
        [MemberData(nameof(GenerateTestCasesForGetRepairsForMaintenanceWithandWithoutFilters))]
        public async Task GetRepairsForMaintenance_withAndWithoutFilters(
            string filterRepairName,
            string filterScaleName,
            float? filter_cost_less_or_equal,
            List<RepairDTO> expectedRepairs
            )
        {
            //arrange
            Mock<ILogger<RepairsController>> mockLogger = new Mock<ILogger<RepairsController>>();
            RepairsController sut = new RepairsController(_context, mockLogger.Object);


            //act
            var result = await sut.GetRepairsForMaintenance(filterRepairName, filterScaleName,filter_cost_less_or_equal);


            //assert

            var okresult = Assert.IsType<OkObjectResult>(result);
            var actualresult = Assert.IsType<List<RepairDTO>>(okresult.Value);
            Assert.Equal(expectedRepairs, actualresult);
        }
    }
}
