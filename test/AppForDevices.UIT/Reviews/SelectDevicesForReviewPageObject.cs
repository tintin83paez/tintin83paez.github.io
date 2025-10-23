using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Reviews
{
    public class SelectDevicesForReviewPageObject : PageObject
    {
        private By filterbyBrand = By.Id("filterbyBrand");
        private By filterbyYear = By.Id("filterbyYear");
        private By searchButton = By.Id("searchButton");
        public SelectDevicesForReviewPageObject(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void SearchDevices(string filterBrand, string? filterYear) // Se pone string el int porque el input se busca como string
        {
            WaitForBeingVisible(filterbyBrand);
            _driver.FindElement(filterbyBrand).SendKeys(filterBrand);
            WaitForBeingVisible(filterbyYear);
            if (filterYear != null) _driver.FindElement(filterbyYear).SendKeys(filterYear);
            WaitForBeingVisible(searchButton);
            _driver.FindElement(searchButton).Click();
        }

        public bool CheckListOfDevices(List<string[]> expectedDevices)
        {
            return CheckBodyTable(expectedDevices, By.Id("TableDevices"));
        }

        public void AddDeviceToReviewCart(string Id)
        {
            WaitForBeingClickable(By.Id($"buttonAddDevice_{Id}"));
            _driver.FindElement(By.Id($"buttonAddDevice_{Id}")).Click();
        }

        public void RemoveDeviceFromReviewCart(string Id)
        {
            WaitForBeingClickable(By.Id($"buttonRemoveDevice_{Id}"));
            _driver.FindElement(By.Id($"buttonRemoveDevice_{Id}")).Click();
        }

        public bool CheckDeviceInReviewCart(string Id)
        {
            WaitForBeingVisible(By.Id($"buttonRemoveDevice_{Id}"));
            return _driver.FindElements(By.Id($"buttonRemoveDevice_{Id}")).Count > 0;
        }

        public bool CheckDeviceNotInReviewCart(string Id)
        {
            return _driver.FindElements(By.Id($"buttonRemoveDevice_{Id}")).Count == 0;
        }
    }
}
