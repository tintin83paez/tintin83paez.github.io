using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Purchase
{
    public class SelectDevicesForPurchasePageObject : PageObject
    {
        private By inputName = By.Id("filterbyName");
        private By inputColor = By.Id("filterbyColor");
        private By searchDevices = By.Id("buttonSearch");
        public SelectDevicesForPurchasePageObject(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void SearchDevices(string filtername, string filtercolor)
        {
            WaitForBeingVisible(inputName);
            _driver.FindElement(inputName).SendKeys(filtername);
            WaitForBeingVisible(inputColor);
            _driver.FindElement(inputColor).SendKeys(filtercolor);
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
            WaitForBeingClickable(By.Id($"adddevice_{NameOfDevice}"));
            _driver.FindElement(By.Id($"removeDevice_{NameOfDevice}")).Click();
        }
        public bool CheckPurchaseItemNotInTheShoppingCart(string NameOfDevice)
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
