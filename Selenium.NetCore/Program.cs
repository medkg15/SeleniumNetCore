using BrowserStack;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

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
            };

            local.start(localArgs);

            // doesn't appear this will ever succeed:
            //while (!local.isRunning())
            //{
            //    Thread.Sleep(1000);
            //}

            OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddAdditionalCapability("browserstack.local", "true");
            options.AddAdditionalCapability("os_version", "10", true);
            options.AddAdditionalCapability("browser", "chrome", true);
            options.AddAdditionalCapability("browser_version", "latest", true);
            options.AddAdditionalCapability("os", "Windows", true);

            var driver = new RemoteWebDriver(
              new Uri($"{args[0]}:{args[1]}@hub-cloud.browserstack.com/wd/hub/"),
              options
            );

            driver.Navigate().GoToUrl("http://localhost");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(localDriver => localDriver.Title.ToLower().Contains("sample"));
        }
    }
}
