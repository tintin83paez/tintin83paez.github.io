using AppForDevices.UIT.Repair;
using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Receipt
{
    public class UC8DetailReceipt_test: AppforDevices4UIT
    {
        SelectRepairsForMaintenancePageObject selectRepairsForMaintenancePO;
        CreateReceiptPageObject createReceiptPO;
        DetailReceiptPageObject detailReceiptPO;
        LoginPage loginPagePO;

        public UC8DetailReceipt_test(ITestOutputHelper output) : base(output)
        {
            loginPagePO = new LoginPage(_driver, _output);
            selectRepairsForMaintenancePO = new SelectRepairsForMaintenancePageObject(_driver, _output);
            createReceiptPO = new CreateReceiptPageObject(_driver, _output);
            detailReceiptPO = new DetailReceiptPageObject(_driver, _output);
        }

        public void Precondition()
        {
            loginPagePO.Login("agus@uclm.es", "Password1234%");
            loginPagePO.WaitForBeingVisible(By.Id("repairs_menu"));
            _driver.FindElement(By.Id("repairs_menu")).Click();
        }
        [Fact]
        public void UC8_1_Sce1_BF_basic_flow()
        {
            Precondition();
            
            
            for (int i = 1; i < 4; i++)
            {
                if (!selectRepairsForMaintenancePO.CheckReceiptItemIsNotInTheShoppingCart(i))
                {
                    selectRepairsForMaintenancePO.WaitForBeingVisible(By.Id($"removeRepair_{i}"));
                    selectRepairsForMaintenancePO.WaitForBeingClickable(By.Id($"removeRepair_{i}"));
                    _driver.FindElement(By.Id($"removeRepair_{i}")).Click();
                }
            }

            selectRepairsForMaintenancePO.AddRepairsToShoppingCart(1);
            selectRepairsForMaintenancePO.WaitForBeingVisible(By.Id("rentRepairsButton"));
            selectRepairsForMaintenancePO.WaitForBeingClickable(By.Id("rentRepairsButton"));
            _driver.FindElement(By.Id("rentRepairsButton")).Click();
            createReceiptPO.AddUsersData("Agustin Prieto", "Avda Espana s/n", "PayPal");
            createReceiptPO.AddModelToReceiptItem(1, "Slim 1TB");
            createReceiptPO.Checkout();
            createReceiptPO.ConfirmCheckOut();
            Assert.True(detailReceiptPO.CheckRepairData("Repair Playstation 4", "Slim 1TB"));
            Assert.True(detailReceiptPO.CheckUsersDataAndTotalPrice("Agustin Prieto", "Avda Espana s/n", "PayPal", "23,559999465942383 €")); 
            
        }
    }
}
