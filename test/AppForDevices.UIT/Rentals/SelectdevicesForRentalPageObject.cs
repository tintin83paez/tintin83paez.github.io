using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Rental
{
    public class SelectDevicesForRentalPageObject : PageObject
    {
        private By inputModel = By.Id("filterbyModel");
        private By inputPrice = By.Id("filterByPriceForRent");
        private By searchDevices = By.Id("buttonSearch");
        public SelectDevicesForRentalPageObject(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void SearchDevices(string filtermodel, string filterByPriceForRent)
        {
            WaitForBeingVisible(inputModel);
            _driver.FindElement(inputModel).SendKeys(filtermodel);
            WaitForBeingVisible(inputPrice);
            _driver.FindElement(inputPrice).SendKeys(filterByPriceForRent);
            WaitForBeingVisible(searchDevices);
            _driver.FindElement(searchDevices).Click();
        }

        public bool CheckListOfDevices(List<string[]> expectedDevices)
        {
            return CheckBodyTable(expectedDevices, By.Id("devicesAvailable"));
        }

        public void AddDevicesToShoppingCart(string NameOfDevice)
        {
            WaitForBeingClickable(By.Id($"adddevice_{NameOfDevice}"));
            _driver.FindElement(By.Id($"adddevice_{NameOfDevice}")).Click();
        }
        public void RemoveDevicesFromShoppingCart(string NameOfDevice)
        {
            WaitForBeingClickable(By.Id($"removeDevice_{NameOfDevice}"));
            _driver.FindElement(By.Id($"removeDevice_{NameOfDevice}")).Click();
        }
        public bool CheckRentalItemNotInTheShoppingCart(string NameOfDevice)
        {
            var webelements = _driver.FindElements(By.Id($"removeDevice_{NameOfDevice}"));
            if (webelements.Count() == 0)
            {
                return true;
            }
            else return false;
        }
    }
}