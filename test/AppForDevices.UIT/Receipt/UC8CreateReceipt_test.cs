using AppForDevices.UIT.Repair;
using AppForDevices.UIT.Shared;
using OpenQA.Selenium.DevTools.V129.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Receipt
{
    public class UC8CreateReceipt_test: AppforDevices4UIT
    {
        LoginPage loginPagePO;
        SelectRepairsForMaintenancePageObject selectRepairsForMaintenancePO;
        CreateReceiptPageObject createReceiptPO;

        private const string name_surname = "Agustin Prieto";
        private const string delivery_address = "Avda Espana s/n";
        private const string payment_method = "PayPal";
        private const string model = "model1";




        public UC8CreateReceipt_test(ITestOutputHelper output) : base(output)
        {
            loginPagePO = new LoginPage(_driver, _output);
            selectRepairsForMaintenancePO = new SelectRepairsForMaintenancePageObject(_driver, _output);
            createReceiptPO = new CreateReceiptPageObject(_driver, _output);
        }

        public void Precondition()
        {
            loginPagePO.Login("agus@uclm.es", "Password1234%");
            loginPagePO.WaitForBeingVisible(By.Id("repairs_menu"));
            _driver.FindElement(By.Id("repairs_menu")).Click();
        }
        [Theory]
        [InlineData("", delivery_address, payment_method, model, "The CustomerNameSurname field is required.")]
        [InlineData("a b", delivery_address, payment_method, model, "The field CustomerNameSurname must be a string with a minimum length of 5 and a maximum length of 40.")]
        [InlineData("Agustín Prieto", delivery_address, payment_method, model, "The field CustomerNameSurname must match the regular expression '^[A-Za-z]+ [A-Za-z]+$'.")]
        [InlineData(name_surname, "", payment_method, model, "The DeliveryAddress field is required.")]
        [InlineData(name_surname, "avda", payment_method, model, "The field DeliveryAddress must be a string with a minimum length of 5 and a maximum length of 50.")]
        [Trait("TypeOfTesting", "FunctionalTesting")]
        public void UC8_4_UC8_9_Sce5_Mandatory_Data_have_not_been_filled_AF4(
            string name_and_surname,
            string delivery_address,
            string payment_method,
            string model,
            string expected_error
            )
        {
            Precondition();


            
            selectRepairsForMaintenancePO.AddRepairsToShoppingCart(2);
            
            selectRepairsForMaintenancePO.AddRepairsToShoppingCart(3);
            
            selectRepairsForMaintenancePO.WaitForBeingVisible(By.Id("rentRepairsButton"));
            selectRepairsForMaintenancePO.WaitForBeingClickable(By.Id("rentRepairsButton"));
            _driver.FindElement(By.Id("rentRepairsButton")).Click();


            

            createReceiptPO.AddUsersData(name_and_surname, delivery_address, payment_method);
            createReceiptPO.AddModelToReceiptItem(2, model);
            createReceiptPO.AddModelToReceiptItem(3, model);
            createReceiptPO.Checkout();
            Assert.True(createReceiptPO.CheckErrorShown(expected_error));

        }

        [Fact]
        [Trait("TypeOfTesting", "FunctionalTesting")]
        public void UC8_12_Sce6_modify_items_on_receipt_AF5()
        {
            Precondition();

            
            selectRepairsForMaintenancePO.AddRepairsToShoppingCart(2);

            selectRepairsForMaintenancePO.AddRepairsToShoppingCart(3);
            selectRepairsForMaintenancePO.WaitForBeingVisible(By.Id("rentRepairsButton"));
            selectRepairsForMaintenancePO.WaitForBeingClickable(By.Id("rentRepairsButton"));
            _driver.FindElement(By.Id("rentRepairsButton")).Click();



            createReceiptPO.WaitForBeingVisible(By.Id("modifyReceiptItems"));
            createReceiptPO.WaitForBeingClickable(By.Id("modifyReceiptItems"));
            _driver.FindElement(By.Id("modifyReceiptItems")).Click();

            selectRepairsForMaintenancePO.WaitForBeingVisible(By.Id("removeRepair_2"));
            selectRepairsForMaintenancePO.WaitForBeingClickable(By.Id("removeRepair_2"));
            _driver.FindElement(By.Id("removeRepair_2")).Click();


            
            Assert.False(selectRepairsForMaintenancePO.CheckReceiptItemIsNotInTheShoppingCart(3));

        }
    }
}
