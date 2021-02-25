using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using TravianTools.Core.Driver.Models;
using TravianTools.DAL;

namespace TravianTools.Core.Driver
{
    public class CountryInformation : ICountryInformation
    {
        private bool A;
        private const string _c_VillageInfoPattern = @"{0}/position_details.php?x={1}&y={2}";
        private readonly string baseUrl = "https://ts4.anglosphere.travian.com";

        //private readonly object locker = new object();
        public ITravianDriver Driver { get; }

        public CountryInformation(ITravianDriver travianDriver)
        {
            Driver = travianDriver;
        }

        public bool TryParseVillage(Point villagePoint, out NeighborsVillageInfo neighborVillage)
        {
            neighborVillage = new NeighborsVillageInfo
            {
                PointX = villagePoint.X,
                PointY = villagePoint.Y
            };

            var url = string.Format(_c_VillageInfoPattern, baseUrl, villagePoint.X, villagePoint.Y);
            Driver.Driver.Navigate().GoToUrl(url);

            neighborVillage.IsVillage = IsVillage();

            //check that village in any protection
            var elements = Driver.Driver.FindElements(By.CssSelector(".option .arrow.disabled"));

            if (elements.Any())
            {
                if (Driver.Driver.FindElements(By.CssSelector("#CybotCookiebotDialog")).Any())
                {
                    var script = "$(document.querySelector('[name=\\'CybotCookiebotDialog\\']').style.display = 'none')";
                    Driver.Driver.ExecuteJavaScript(script);
                }

                new Actions(Driver.Driver).MoveToElement(elements[0]).Perform();

                var text = Driver.Driver.FindElementByCssSelector(".tip-contents .text.elementText").Text;

                var dtAsString = text
                    .Replace("Player is under beginner's protection until ", "", StringComparison.OrdinalIgnoreCase)
                    .Replace(",", "")
                    .TrimEnd('.');

                if (DateTime.TryParseExact(dtAsString, "dd.MM.yy HH:mm", null, DateTimeStyles.AllowInnerWhite, out var dt))
                {
                    neighborVillage.UntilProtectionTime = dt;
                }
            }

            return neighborVillage.IsVillage;
        }

        private bool IsVillage()
        {
            var tileElement = Driver.Driver.FindElementById("tileDetails");

            var villageInfoElements = Driver
                .Driver
                .FindElementsById("village_info")
                .FirstOrDefault();

            return villageInfoElements != null && !tileElement.GetAttribute("class").Contains("oasis");
        }
    }
}