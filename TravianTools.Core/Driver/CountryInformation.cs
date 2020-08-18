using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using TravianTools.DAL;

namespace TravianTools.Core.Driver
{
    public class CountryInformation : ICountryInformation
    {
        private const string VillageInfoRoutePattern = "position_details.php";

        //private readonly object locker = new object();
        public ITravianDriver Driver { get; }

        public CountryInformation(ITravianDriver travianDriver)
        {
            Driver = travianDriver;
        }

        public bool TryParseVillage(string hostUrl, Point villagePoint, out NeighborsVillageInfo neighborVillage)
        {
            neighborVillage = new NeighborsVillageInfo
            {
                PointX = villagePoint.X,
                PointY = villagePoint.Y
            };

            var url = $"{hostUrl}/{VillageInfoRoutePattern}?x={villagePoint.X}&y={villagePoint.Y}";
            Driver.Driver.Navigate().GoToUrl(url);

            var addInfo = GetAdditionalInfo();

            neighborVillage.IsVillage = addInfo.IsVillage;
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

        private (int Population, bool IsVillage) GetAdditionalInfo()
        {
            var result = (p: 0, v: false);
            
            try
            {
                var tileElement = Driver.Driver.FindElementById("tileDetails");

                var villageInfoElements = Driver
                    .Driver
                    .FindElementsById("village_info")
                    .FirstOrDefault();

                result.v = villageInfoElements != null && !tileElement.GetAttribute("class").Contains("oasis");
                
                var pElem = villageInfoElements?.FindElement(By.XPath(@"//*[@id=""village_info""]/tbody/tr[4]/td"));
                if (pElem != null)
                {
                    var population = pElem?.Text;
                    int.TryParse(population, out result.p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return result;
        }
    }
}