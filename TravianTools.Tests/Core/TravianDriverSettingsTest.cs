using System;
using System.IO;
using TravianTools.Core.Driver;
using Xunit;

namespace TravianTools.Tests.Core
{
    public class TravianDriverSettingsTest
    {
        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowInvalidDirectoryWithDriverLocation(string location)
        {
            var settings = GetValidSettings();
            settings.ChromeDriverLocation = location;
            
            Assert.Throws<DirectoryNotFoundException>(() => settings.ThrowIfNotValid());
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("123")]
        [InlineData("♣♠♦•☻")]
        [InlineData("/wswsws/wssw")]
        public void ThrowInvalidHostUrl(string url)
        {
            var settings = GetValidSettings();
            settings.HostUrl = url;
            
            Assert.Throws<ArgumentException>(() => settings.ThrowIfNotValid());
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ThrowInvalidLogin(string login)
        {
            var settings = GetValidSettings();
            settings.Login = login;
            
            Assert.Throws<ArgumentNullException>(() => settings.ThrowIfNotValid());
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ThrowInvalidPassword(string pwd)
        {
            var settings = GetValidSettings();
            settings.Password = pwd;
            
            Assert.Throws<ArgumentNullException>(() => settings.ThrowIfNotValid());
        }

        private TravianDriverSettings GetValidSettings()
        {
            return new TravianDriverSettings
            {
                ChromeDriverLocation = ".",
                HostUrl = "https://test.example",
                Login = "login",
                Password = "pwd"
            };
        }
    }
}