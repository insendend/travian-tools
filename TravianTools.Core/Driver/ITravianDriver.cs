using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TravianTools.Core.Driver
{
    public interface ITravianDriver : IDisposable
    {
        void LoginDefault();

        void Login(string login, string password, List<Cookie> cookies = null);

        bool IsLoggedIn();

        ChromeDriver Driver { get; }

        string CurrentAccountName { get; }
    }
}