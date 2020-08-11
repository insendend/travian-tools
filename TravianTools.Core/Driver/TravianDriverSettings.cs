using System;
using System.IO;
using TravianTools.Core.Common;

namespace TravianTools.Core.Driver
{
    public class TravianDriverSettings : IValidateThrowable
    {
        /// <summary>
        /// Path to chromedriver.exe
        /// </summary>
        public string ChromeDriverLocation { get; set; }
        public bool Headless { get; set; }
        
        /// <summary>
        /// Url to server (example: https://ts5.travian.ru) 
        /// </summary>
        public string HostUrl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        
        public void ThrowIfNotValid()
        {
            if (!Directory.Exists(ChromeDriverLocation))
            {
                throw new DirectoryNotFoundException($"incorrect <{nameof(ChromeDriverLocation)}> param");
            }
            
            if (!Uri.IsWellFormedUriString(HostUrl, UriKind.Absolute))
            {
                throw new ArgumentException($"incorrect <{nameof(HostUrl)}> param");
            }
            
            if (string.IsNullOrEmpty(Login))
            {
                throw new ArgumentNullException(nameof(Login));
            }
            
            if (string.IsNullOrEmpty(Password))
            {
                throw new ArgumentNullException(nameof(Password));
            }
        }
    }
}