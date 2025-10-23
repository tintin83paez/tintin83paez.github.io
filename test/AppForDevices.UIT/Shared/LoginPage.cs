using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AppForDevices.UIT.Shared
{
    public class LoginPage: PageObject
    {
        public LoginPage(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public void Login(string username, string password)
        {
            //locate the link to login

            _driver.FindElement(By.PartialLinkText("Login")).Click();
            WaitForBeingVisible(By.XPath("html/body/div[1]/main/article/div/div[1]/section/form/div[1]/input"));
            var usernameField = _driver.FindElement(By.XPath("/html/body/div[1]/main/article/div/div[1]/section/form/div[1]/input"));
            usernameField.SendKeys(username);

            var passwordField = _driver.FindElement(By.XPath("/html/body/div[1]/main/article/div/div[1]/section/form/div[2]/input"));
            passwordField.SendKeys(password);

            var loginButton = _driver.FindElement(By.XPath("/html/body/div[1]/main/article/div/div[1]/section/form/div[4]/button"));
            loginButton.Click();
        }
    }
}
