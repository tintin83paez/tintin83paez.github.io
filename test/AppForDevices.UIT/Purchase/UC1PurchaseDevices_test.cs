using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppForDevices.UIT.Purchase
{
    public class UC1PurchaseDevices_test : AppforDevices4UIT
    {
        LoginPage loginPagePO;
        SelectDevicesForPurchasePageObject selectDevicesForPurchasePO;
        private const string deviceName1 = "iphone16";
        private const string deviceColor1 = "white";
        private const string devicePrice1 = "700";
        private const string deviceModel1 = "Phone";
        private const string deviceBrand1 = "apple";

        private const string deviceName2 = "iPad Pro";
        private const string deviceColor2 = "Silver";
        private const string devicePrice2 = "0";
        private const string deviceModel2 = "Tablet";
        private const string deviceBrand2 = "Apple";
        public UC1PurchaseDevices_test(ITestOutputHelper output) : base(output)
        {
            loginPagePO = new LoginPage(_driver, _output);
            selectDevicesForPurchasePO = new SelectDevicesForPurchasePageObject(_driver, _output);
        }

        public void Precondition()
        {
            loginPagePO.Login("carmen@uclm.es", "Password1234%");
            loginPagePO.WaitForBeingVisible(By.Id("purchasemenu"));
            _driver.FindElement(By.Id("purchasemenu")).Click();
        }

        [Theory]
        [InlineData(deviceName1, deviceColor1,deviceModel1, deviceBrand1, devicePrice1, deviceName1, "")]
        [InlineData(deviceName2, deviceColor2,deviceModel2, deviceBrand2, devicePrice2, "", deviceColor2)]
        public void AF1_FilterDevices_UC1(string expectedName, string expectedColor, string expectedModel, string expectedBrand, string expectedPrice, string filterName, string filterColor)
        {
            Precondition();

            selectDevicesForPurchasePO.SearchDevices(filterName, filterColor);
            selectDevicesForPurchasePO.WaitForBeingVisible(By.Id("devicesAvailable"));
            List<string[]> expectedDevices = new List<string[]>();
            expectedDevices.Add(new string[] { expectedName, expectedModel, expectedBrand, expectedColor, expectedPrice});
            Assert.True(selectDevicesForPurchasePO.CheckListOfDevices(expectedDevices));
        }

        [Fact]
        public void AF2_RemovePurchaseItem_UC1()
        {
            Precondition();
            selectDevicesForPurchasePO.AddDevicesToShoppingCart(deviceName1);
            selectDevicesForPurchasePO.RemoveDevicesFromShoppingCart(deviceName1);
            Assert.True(selectDevicesForPurchasePO.CheckPurchaseItemNotInTheShoppingCart(deviceName1));
        }
    }
}
