using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Receipt
{
    public class CreateReceiptPageObject : PageObject
    {
        By nameinput = By.Id("name_surname");
        By addressinput = By.Id("delivery_address");
        By paymentinput = By.Id("payment");
        public CreateReceiptPageObject(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void AddUsersData(string namesurname, string address, string typeOfPayment)
        {
            WaitForBeingVisible(nameinput);
            _driver.FindElement(nameinput).Clear();
            _driver.FindElement(nameinput).SendKeys(namesurname);
            WaitForBeingVisible(addressinput);
            _driver.FindElement(addressinput).Clear();
            _driver.FindElement(addressinput).SendKeys(address);
            WaitForBeingVisible(paymentinput);
            SelectElement select = new SelectElement(_driver.FindElement(paymentinput));
            select.SelectByText(typeOfPayment);

        }

        public void AddModelToReceiptItem(int repairId, string model)
        {
            By modelinpunt = By.Id($"model_{repairId}");
            WaitForBeingVisible(modelinpunt);
            _driver.FindElement(modelinpunt).Clear();
            _driver.FindElement(modelinpunt).SendKeys(model);
        }

        public void Checkout()
        {
            _driver.FindElement(By.Id("submit")).Click();
        }

        public bool CheckErrorShown(string errorMessage)
        {
            ImplicitWait(1);
            var errorElements = _driver.FindElements(By.ClassName("validation-message"));
            if (errorElements.Any(error => error.Text.Contains(errorMessage)))
            {
                return true;
            }
            else
            {
                _output.WriteLine($"Error message is not shown: {errorMessage}");
                return false;
            }
        }

        public void ConfirmCheckOut()
        {
            this.PressOkModalDialog();
        }
    }
}
