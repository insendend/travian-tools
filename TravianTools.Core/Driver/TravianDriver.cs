using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TravianTools.Core.Driver
{
    public class TravianDriver : ITravianDriver
    {
        private const string c_LoginPattern = @"{0}/login.php";
        private readonly string baseUrl = "https://ts4.anglosphere.travian.com/";
        private readonly string _login = "ivanf";
        private readonly string _password = "Qwerty1234";

        bool ITravianDriver.IsLoggedIn()
        {
            Driver.Navigate().GoToUrl(string.Format(c_LoginPattern, baseUrl));

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var elements = Driver.FindElements(By.CssSelector(".pass [name=\"password\"]"));
            return !elements.Any();
        }

        public ChromeDriver Driver { get; }

        public string CurrentAccountName { get; private set; }

        //TODO: check if driver is currently logged in

        public TravianDriver(bool headless = false)
        {
            var options = new ChromeOptions();

            if (headless)
            {
                options.AddArgument("--headless");
            }

            Driver = new ChromeDriver(options);
        }

        public void LoginDefault()
        {
            Login(_login, _password);
        }

        public void Login(string login, string password, List<Cookie> cookies = null)
        {
            Driver.Navigate().GoToUrl(string.Format(c_LoginPattern, baseUrl));

            // AcceptCookieIfExist(cookies);

            var loginElement = Driver.FindElement(By.CssSelector("[name=\"name\"]"));
            var passElement = Driver.FindElement(By.CssSelector("[name=\"password\"]"));
            loginElement.SendKeys(login);
            passElement.SendKeys(password);
            Driver.FindElement(By.CssSelector("[type=\"submit\"]")).Click();
            CurrentAccountName = login;
        }

        private void AcceptCookieIfExist(List<Cookie> cookies)
        {
            foreach (var cookie in cookies)
            {
                Driver.Manage().Cookies.AddCookie(cookie);
            }

            Driver.Navigate().Refresh();

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var elements = Driver.FindElements(By.CssSelector("#CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll.CybotCookiebotDialogBodyButton"));

            if (elements.Any())
            {
                elements.First().Click();
            }
        }

        public void Dispose()
        {
            Driver?.Dispose();
        }
    }
}