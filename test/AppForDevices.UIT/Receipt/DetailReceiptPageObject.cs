using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Receipt
{
    public class DetailReceiptPageObject : PageObject
    {
        By nametext = By.Id("NameSurname");
        By addresstext = By.Id("DeliveryAddress");
        By paymenttext = By.Id("PaymentMethod");
        By totalPricetext = By.Id("TotalPrice");
        public DetailReceiptPageObject(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public bool CheckUsersDataAndTotalPrice(string namesurname, string address, string typeOfPayment, string totalPrice)
        {
            WaitForBeingVisible(nametext);
            if (!_driver.FindElement(nametext).Text.Equals(namesurname))
            {
                _output.WriteLine($"Expected name: {namesurname} but found {_driver.FindElement(nametext).Text}");
                return false;
            }

            if (!_driver.FindElement(addresstext).Text.Equals(address))
            {
                _output.WriteLine($"Expected address: {address} but found {_driver.FindElement(addresstext).Text}");
                return false;
            }
            if (!_driver.FindElement(paymenttext).Text.Equals(typeOfPayment))
            {
                _output.WriteLine($"Expected payment: {typeOfPayment} but found {_driver.FindElement(paymenttext).Text}");
                return false;
            }
            if (!_driver.FindElement(totalPricetext).Text.Equals(totalPrice))
            {
                _output.WriteLine($"Expected totalPrice: {totalPrice} but found {_driver.FindElement(totalPricetext).Text}");
                return false;
            }
            return true;
        }

        public bool CheckRepairData(string selectedRepair, string model)
        {
            WaitForBeingVisible(By.Id("SelectedRepairs"));
            var actualcols = _driver
                 .FindElement(By.Id("ReceiptItem_1"))
                 .FindElements(By.TagName("td"))
                 .Select(m => m.Text)
                 //we obtain just the columns of the row where the repair is shown
                 .ToList();
            if (!actualcols[0].Equals(selectedRepair)) return false;
            if (!actualcols[4].Equals(model)) return false;
            return true;

        }
    }
}
