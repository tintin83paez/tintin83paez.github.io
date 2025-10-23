using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Repair
{
    public class SelectRepairsForMaintenancePageObject : PageObject
    {
        private By inputName = By.Id("filterbyName");
        private By inputScale = By.Id("filterbyScale");
        private By searchrepairs = By.Id("buttonSearch");
        public SelectRepairsForMaintenancePageObject(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchRepairs(string filterName, string filterScale)
        {
            WaitForBeingVisible(inputName);
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(filterName);
            WaitForBeingVisible(inputScale);
            WaitForBeingClickable(inputScale);
            _driver.FindElement(inputScale).SendKeys(filterScale);
            WaitForBeingVisible(searchrepairs);
            WaitForBeingClickable(searchrepairs);
            _driver.FindElement(searchrepairs).Click();
        }
        public bool CheckListOfRepairs(List<string[]> expectedRepairs)
        {
            return CheckBodyTable(expectedRepairs, By.Id("repairsAvailable"));
        }

        public void AddRepairsToShoppingCart(int idOfTheRepair)
        {
            WaitForBeingClickable(By.Id($"addRepair_{idOfTheRepair}"));
            _driver.FindElement(By.Id($"addRepair_{idOfTheRepair}")).Click();
        }

        public void RemoveReceiptItem(int idOfTheRepair)
        {
            _driver.FindElement(By.Id($"removeRepair_{idOfTheRepair}")).Click();
        }

        public bool CheckReceiptItemIsNotInTheShoppingCart(int idOfTheRepair)
        {
            var webelements = _driver.FindElements(By.Id($"removeRepair_{idOfTheRepair}"));
            if (webelements.Count == 0)
                return true;
            return false;
        }
    }
}
