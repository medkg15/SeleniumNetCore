using BrowserStack;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Selenium.NetCore
{
    public class Program
    {
        // start with args: username key
        public static void Main(string[] args)
        {
            var local = new Local();

            var localArgs = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("key", args[1]),
                new KeyValuePair<string, string>("v", "true"),
                new KeyValuePair<string, string>("force", "true"),
            };

            local.start(localArgs);

            while (!Process.GetProcessesByName("BrowserStackLocal").Any())
            {
                Thread.Sleep(1000);
            }

            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.BrowserVersion = "latest";

            var browserstackOptions = new Dictionary<string, object>
            {
                { "os", "Windows" },
                { "osVersion", "10" },
                { "local", "true" },
                { "userName", args[0] },
                { "accessKey", args[1] }
            };
            options.AddAdditionalOption("bstack:options", browserstackOptions);

            var driver = new RemoteWebDriver(
              new Uri($"https://hub-cloud.browserstack.com/wd/hub/"),
              options
            );

            try
            {
                driver.Navigate().GoToUrl("http://localhost");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(localDriver => localDriver.Title.ToLower().Contains("Sitecore Active Directory Login"));
            }
            finally
            {
                driver.Dispose();
            }
        }
    }
}
