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
        private const string LoginRoutePattern = "login.php";

        bool ITravianDriver.IsLoggedIn(string hostUrl)
        {
            Driver.Navigate().GoToUrl($"{hostUrl}/{LoginRoutePattern}");

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var elements = Driver.FindElements(By.CssSelector(".pass [name=\"password\"]"));
            return !elements.Any();
        }

        public ChromeDriver Driver { get; }

        public string CurrentAccountName { get; private set; }

        public TravianDriver(bool headless = false)
        {
            var options = new ChromeOptions();

            if (headless)
            {
                options.AddArgument("--headless");
            }

            Driver = new ChromeDriver(options);
        }

        public void Login(string hostUrl, string login, string password, List<Cookie> cookies = null)
        {
            Driver.Navigate().GoToUrl($"{hostUrl}/{LoginRoutePattern}");

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