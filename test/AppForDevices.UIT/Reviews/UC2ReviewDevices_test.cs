using AppForDevices.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForDevices.UIT.Reviews
{
    public class UC2ReviewDevices_test : AppforDevices4UIT
    {
        LoginPage loginPageP0;
        SelectDevicesForReviewPageObject selectDevicesForReviewP0;
        private const string deviceId1 = "3";
        private const string deviceName1 = "Galaxy S21";
        private const string deviceDescription1 = "High-end smartphone";
        private const string deviceColor1 = "Black";
        private const string deviceModel1 = "Phone";
        private const string deviceBrand1 = "Samsung";
        private const string deviceYear1 = "2022";

        private const string deviceId2 = "4";
        private const string deviceName2 = "iPad Pro";
        private const string deviceDescription2 = "Latest Apple tablet";
        private const string deviceColor2 = "Silver";
        private const string deviceModel2 = "Tablet";
        private const string deviceBrand2 = "Apple";
        private const string deviceYear2 = "2023";

        public UC2ReviewDevices_test(ITestOutputHelper output) : base(output)
        {
            loginPageP0= new LoginPage(_driver, _output);
            selectDevicesForReviewP0 = new SelectDevicesForReviewPageObject(_driver, _output);

        }

        public void Precondition()
        {
            loginPageP0.Login("lucian@uclm.es", "APassword1234%");
            loginPageP0.WaitForBeingVisible(By.Id("ReviewDevices"));
            _driver.FindElement(By.Id("ReviewDevices")).Click();
            loginPageP0.WaitForBeingVisible(By.Id("TableDevices"));
        }

        [Theory]
        [InlineData(deviceId1, deviceName1, deviceDescription1, deviceColor1, deviceModel1, deviceBrand1, deviceYear1, "Samsung", "")]
        [InlineData(deviceId2, deviceName2, deviceDescription2, deviceColor2, deviceModel2, deviceBrand2, deviceYear2, "", "2023")]
        [Trait("TypeofTesting", "FunctionalTesting")]
        public void AF1_FilterDevices_UC_2_UC_documentoTestPlanPorDefinir(string expectedId, string expectedName, string expectedDescription, string expectedColor, string expectedModel, string expectedBrand, string expectedYear, string filterBrand, string? filterYear)
        {
            Precondition();
            selectDevicesForReviewP0.SearchDevices(filterBrand, filterYear);

            List<string[]> expectedDevices = new List<string[]>();
            expectedDevices.Add(new string[] { expectedId, expectedName, expectedDescription, expectedColor, expectedModel, expectedBrand, expectedYear });
            Assert.True(selectDevicesForReviewP0.CheckListOfDevices(expectedDevices));
        }

        [Fact]
        [Trait("TypeofTesting", "FunctionalTesting")]
        public void AF2_AddAndRemoveDevicesFromReviewCart_UC_2()
        {
            Precondition();

            // Add the first device to the review cart
            selectDevicesForReviewP0.AddDeviceToReviewCart(deviceId1);
            Assert.True(selectDevicesForReviewP0.CheckDeviceInReviewCart(deviceId1));
            
            // Remove the device from the review cart
            selectDevicesForReviewP0.RemoveDeviceFromReviewCart(deviceId1);
            Assert.True(selectDevicesForReviewP0.CheckDeviceNotInReviewCart(deviceId1));
        }


    }
}
