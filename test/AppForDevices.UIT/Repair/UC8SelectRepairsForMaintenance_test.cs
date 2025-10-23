using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Repair
{
    public class UC8SelectRepairsForMaintenance_test : AppforDevices4UIT
    {
        LoginPage loginPagePO;
        SelectRepairsForMaintenancePageObject selectRepairsForMaintenancePageObjectPO;
        private const string repairName1 = "Repair Playstation 4";
        private const string repairsScale1 = "basic";
        private const string repairDescription1 = "We are going to clean the Playstation";
        private const string cost1 = "23,559999465942383";

        //second one
        private const string repairName2 = "Repair Xbox 364";
        private const string repairsScale2 = "medium";
        private const string repairDescription2 = "We are going to repair an Xbox 364 by changing the CPU and GPU";
        private const string cost2 = "45,45000076293945";
        public UC8SelectRepairsForMaintenance_test(ITestOutputHelper output) : base(output)
        {
            loginPagePO = new LoginPage(_driver, _output);
            selectRepairsForMaintenancePageObjectPO = new SelectRepairsForMaintenancePageObject(_driver, _output);

        }

        public void Precondition()
        {
            loginPagePO.Login("agus@uclm.es", "Password1234%");
            loginPagePO.WaitForBeingVisible(By.Id("repairs_menu"));
            _driver.FindElement(By.Id("repairs_menu")).Click();
        }
        [Theory]
        [InlineData(repairName1, repairsScale1, repairDescription1, cost1, repairName1, "")]
        [InlineData(repairName2, repairsScale2, repairDescription2, cost2, "", repairsScale2)]
        [Trait("TypeOfTesting", "FunctionalTesting")]
        public void UC8_2_UC8_3_Sce2_FilterRepairsByName_AF1(
            string expectedName,
            string expectedScale,
            string expectedDescription,
            string expectedCost,
            string filterName,
            string filterScale
            )
        {
            Precondition();
            selectRepairsForMaintenancePageObjectPO.WaitForBeingVisible(By.Id("repairsAvailable"));
            selectRepairsForMaintenancePageObjectPO.SearchRepairs(filterName, filterScale);
            List<string[]> expectedRepairs = new List<string[]>();
            expectedRepairs.Add(new string[] { expectedName, expectedScale, expectedDescription, expectedCost });

            Assert.True(selectRepairsForMaintenancePageObjectPO.CheckListOfRepairs(expectedRepairs));
        }

        [Fact]
        [Trait("TypeOfTesting", "FunctionalTesting")]
        public void UC8_13_Sce3_RemoveReceiptItem_AF2()
        {
            Precondition();
            
            selectRepairsForMaintenancePageObjectPO.AddRepairsToShoppingCart(3);
            selectRepairsForMaintenancePageObjectPO.RemoveReceiptItem(3);
            Assert.True(selectRepairsForMaintenancePageObjectPO.CheckReceiptItemIsNotInTheShoppingCart(3));
        }

        [Fact]
        [Trait("TypeOfTesting", "FunctionalTesting")]
        public void UC8_11_Sce7_No_repairs_available_AF0()
        {
            Precondition();
            selectRepairsForMaintenancePageObjectPO.SearchRepairs("", "b");
            selectRepairsForMaintenancePageObjectPO.WaitForBeingVisible(By.Id("loading_repairs_div"));
            selectRepairsForMaintenancePageObjectPO.WaitForTextToBePresentInElement(By.Id("loading_repairs_div"), "Loading repairs...");
            Assert.True(true);
        }
    }
}
