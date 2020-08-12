using System;
using System.Drawing;
using System.Linq;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TravianTools.Core.Driver.Models;
using TravianTools.Core.Extensions;

namespace TravianTools.Core.Driver
{
    public class TravianDriver : IDisposable
    {
        private readonly TravianDriverSettings _settings;
        private readonly ChromeDriver _driver;
        private bool _isLoggedIn;

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

        public VillageInfo GetVillageInfo(Point target, Point origin)
        {
            if (!_isLoggedIn)
            {
                Login();
            }
            
            var url = $"{_settings.HostUrl}/position_details.php?x={target.X}&y={target.Y}";
            _driver.Navigate().GoToUrl(url);
            
            var info = new VillageInfo();

            var tileElement = _driver.FindElementById("tileDetails");
            info.Name = tileElement.FindElement(By.TagName("h1")).Text;

            var villageInfoElem = _driver.FindElementsById("village_info").FirstOrDefault();
            if (villageInfoElem != null && !tileElement.GetAttribute("class").Contains("oasis"))
            {
                info.IsVillage = true;

                var villageInfoRows = villageInfoElem.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

                info.Nation = villageInfoRows[0].FindElement(By.TagName("td")).Text;
                info.PlayerName = villageInfoRows[2].FindElement(By.TagName("a")).Text;
                info.Population = int.Parse(villageInfoRows[3].FindElement(By.TagName("td")).Text);
                info.Distance = target.GetDistance(origin);
            }

            return info;
        }
        
        private void Login()
        {
            var url = $"{_settings.HostUrl}/login.php";
            _driver.Navigate().GoToUrl(url);
            
            var loginElement = _driver.FindElement(By.CssSelector("[name=\"name\"]"));
            var pwdElement = _driver.FindElement(By.CssSelector("[name=\"password\"]"));
            var submitElement = _driver.FindElement(By.CssSelector("[type=\"submit\"]"));
            
            loginElement.SendKeys(_settings.Login);
            pwdElement.SendKeys(_settings.Password);
            submitElement.Click();
            _isLoggedIn = true;
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}