using System;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TravianTools.Core.Driver
{
    public class TravianDriver : IDisposable
    {
        private readonly TravianDriverSettings _settings;
        private readonly IWebDriver _driver;

        public TravianDriver(IOptions<TravianDriverSettings> settings)
        {
            _settings = settings.Value;
            _settings.ThrowIfNotValid();
            
            var options = new ChromeOptions();

            if (_settings.Headless)
            {
                options.AddArgument("--headless");
            }

            _driver = new ChromeDriver(_settings.ChromeDriverLocation, options);
        }

        public IWebDriver Authorize()
        {
            var url = $"{_settings.HostUrl}/login.php";
            _driver.Navigate().GoToUrl(url);
            
            var loginElement = _driver.FindElement(By.CssSelector("[name=\"name\"]"));
            var pwdElement = _driver.FindElement(By.CssSelector("[name=\"password\"]"));
            var submitElement = _driver.FindElement(By.CssSelector("[type=\"submit\"]"));
            
            loginElement.SendKeys(_settings.Login);
            pwdElement.SendKeys(_settings.Password);
            submitElement.Click();
            
            return _driver;
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}