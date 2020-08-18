using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TravianTools.Core.Driver
{
    public interface ITravianDriver : IDisposable
    {
        void Login(string hostUrl, string login, string password, List<Cookie> cookies = null);

        bool IsLoggedIn(string hostUrl);

        ChromeDriver Driver { get; }

        string CurrentAccountName { get; }
    }
}