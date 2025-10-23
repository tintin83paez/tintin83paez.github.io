using AppForDevices.UIT.Rental;
using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppForDevices.UIT.Rentals

{
    public class UC3RentalDevices_test : AppforDevices4UIT
    {
        LoginPage loginPagePO;
        SelectDevicesForRentalPageObject selectDevicesForRentalPO;
        private const string deviceName1 = "iphone16";
        private const string devicePrice1 = "200";
        private const string deviceModel1 = "Phone";

        private const string deviceName2 = "razerblade";
        private const string devicePrice2 = "500";
        private const string deviceModel2 = "Computer";
        public UC3RentalDevices_test(ITestOutputHelper output) : base(output)
        {
            loginPagePO = new LoginPage(_driver, _output);
            selectDevicesForRentalPO = new SelectDevicesForRentalPageObject(_driver, _output);
        }

        public void Precondition()
        {
            loginPagePO.Login("javier.garcia126@alu.uclm.es", "Javier1234.");
            loginPagePO.WaitForBeingVisible(By.Id("RentalDevices"));
            _driver.FindElement(By.Id("RentalDevices")).Click();

        }

        [Theory]
        [InlineData(deviceName1, deviceModel1, devicePrice1, deviceModel1, "")]
        [InlineData(deviceName2, deviceModel2, devicePrice2, "", devicePrice2)]

        public void AF1_FilterDevices_UC3(string expectedName, string expectedModel, string expectedPrice, string filterbyModel, string? filterByPriceForRent)
        {
            Precondition();
            selectDevicesForRentalPO.SearchDevices(filterbyModel, filterByPriceForRent);
            selectDevicesForRentalPO.WaitForBeingVisible(By.Id("devicesAvailable"));
            List<string[]> expectedDevices = new List<string[]>();
            expectedDevices.Add(new string[] { expectedName, expectedModel, expectedPrice, "Add Device"}); // He añadido el "Add Device" ya que cuando compara el row también ve el botón
            
            Assert.True(selectDevicesForRentalPO.CheckListOfDevices(expectedDevices));
        }

        [Fact]
        public void AF2_RemovePurchaseItem_UC3()
        {
            Precondition();
            selectDevicesForRentalPO.AddDevicesToShoppingCart(deviceName2);
            selectDevicesForRentalPO.RemoveDevicesFromShoppingCart(deviceName2);
            Assert.True(selectDevicesForRentalPO.CheckRentalItemNotInTheShoppingCart(deviceName2));
        }
    }
}
