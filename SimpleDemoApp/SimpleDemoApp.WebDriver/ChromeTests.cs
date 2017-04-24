using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;

namespace SimpleDemoApp.WebDriver
{
    [TestClass]
    public class ChromeTests
    {

        private static RemoteWebDriver _webDriver = null;
        private static string _webAppBaseURL;
        // A website kept running and not changed. Use to show working in VS, remove if prefer to show failure
        private static string _defaultWebAppBaseURL = "http://deployedsimplewebapp.azurewebsites.net/";

        [ClassInitialize()]
        // All tests in this class are going to use Chrome, therefore set the Chrome driver up once here
        public static void ClassInit(TestContext context)
        {
            // Chrome specifics
            _webDriver = new ChromeDriver(@"C:\tools\selenium");

            //Set page load timeout to 20 seconds (occasionally 5 secs is too tight after a deployment)
            _webDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(20));

            try
            {
                // Get the URL for the current environment (e.g. Dev, QA, Prod) as set in the release environment
                string releaseEnvironmentAppBaseURL = Environment.GetEnvironmentVariable("WebAppName");
                if (releaseEnvironmentAppBaseURL != null)
                {
                    _webAppBaseURL = "http://" + releaseEnvironmentAppBaseURL + ".azurewebsites.net";
                    Console.WriteLine("WebApp Base URL found: " + _webAppBaseURL);
                }
                else
                {
                    // The environment variable exists but has no value, so use a default
                    _webAppBaseURL = _defaultWebAppBaseURL;
                    Console.WriteLine("WebApp Base URL not set, using default: " + _defaultWebAppBaseURL);
                }
            }
            catch (Exception Ex)
            {
                // The environment variable probably doesn't exist (might be called from within VS)
                Console.WriteLine("Exception thrown accessing environment variable: " + Ex.Message);
                Console.WriteLine("Using default: " + _defaultWebAppBaseURL);
                _webAppBaseURL = _defaultWebAppBaseURL;
            }
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            if (_webDriver != null)
            {
                _webDriver.Quit();
            }
        }

        [TestMethod]
        [TestCategory("UI")]
        public void IndexTitleChromeTest()
        {
            string expectedTitle = "ASP.NET";
            
            _webDriver.Url = _webAppBaseURL + "/Home/Index";
            RemoteWebElement titleElement = (RemoteWebElement)_webDriver.FindElementByTagName("H1");

            Assert.AreEqual(expectedTitle, titleElement.Text);
        }

        [TestMethod]
        [TestCategory("UI")]
        public void HomePageFoundChromeTest()
        {
            _webDriver.Url = _webAppBaseURL;

            string actualPageTitle = _webDriver.Title;
            string expectedPageTitle = "Home Page - My ASP.NET Application";

            Assert.AreEqual(expectedPageTitle, actualPageTitle);
        }

        [TestMethod]
        [TestCategory("UI")]
        public void AboutPageFoundChromeTest()
        {
            _webDriver.Url = _webAppBaseURL + "/Home/About";

            string actualPageTitle = _webDriver.Title;
            string expectedPageTitle = "About - My ASP.NET Application";

            Assert.AreEqual(expectedPageTitle, actualPageTitle);
        }

        [TestMethod]
        [TestCategory("UI")]
        public void ContactPageFoundChromeTest()
        {
            _webDriver.Url = _webAppBaseURL + "/Home/Contact";

            string actualPageTitle = _webDriver.Title;
            string expectedPageTitle = "Contact - My ASP.NET Application";

            Assert.AreEqual(expectedPageTitle, actualPageTitle);
        }

        [TestMethod]
        [TestCategory("UI")]
        public void SupportEmailAddressChromeTest()
        {
            string supportEmailAddress = "Support@example.com";

            _webDriver.Url = _webAppBaseURL + "/Home/Contact";
            RemoteWebElement supportEmailElement = (RemoteWebElement)_webDriver.FindElementByLinkText(supportEmailAddress);

            Assert.AreEqual(supportEmailAddress, supportEmailElement.Text);
        }

        [TestMethod]
        [TestCategory("UI")]
        public void MarketingEmailAddressChromeTest()
        {
            string marketingEmailAddress = "Marketing@example.com";

            _webDriver.Url = _webAppBaseURL + "/Home/Contact";
            RemoteWebElement marketingEmailElement = (RemoteWebElement)_webDriver.FindElementByLinkText(marketingEmailAddress);

            Assert.AreEqual(marketingEmailAddress, marketingEmailElement.Text);
        }

    }
}
