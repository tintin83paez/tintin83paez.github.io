using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AppForDevices.UIT
{
    public abstract class AppforDevices4UIT:IDisposable
    {
        protected bool _pipeline = false;
        //private static string _browser = "Chrome";
        //private static string _browser = "Firefox";
        protected string _browser = "Edge";
        protected string _URI = "https://localhost:7147/";
        protected IWebDriver _driver;
        //this may be used whenever some result should be printed in the Test Explorer
        protected readonly ITestOutputHelper _output;
        public AppforDevices4UIT(ITestOutputHelper output)
        {
            _output = output;

            switch (_browser)
            {
                case "Firefox":
                    SetUp_FireFox4UIT();
                    break;
                case "Edge":
                    SetUp_EdgeFor4UIT();
                    break;
                default:
                    //by default Chrome will be used
                    SetUp_Chrome4UIT();
                    break;
            }
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_URI);
        }

        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SetUp_Chrome4UIT()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            //For pipelines use this option for hiding the browser
            if (_pipeline) optionsc.AddArgument("--headless");

            _driver = new ChromeDriver(optionsc);

        }

        public void SetUp_FireFox4UIT()
        {
            var optionsff = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            //For pipelines use this option for hiding the browser
            if (_pipeline) optionsff.AddArgument("--headless");

            _driver = new FirefoxDriver(optionsff);

        }

        public void SetUp_EdgeFor4UIT()
        {
            var optionsEdge = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            //For pipelines use this option for hiding the browser
            if (_pipeline) optionsEdge.AddArgument("--headless");

            _driver = new EdgeDriver(optionsEdge);

        }
    }
}
