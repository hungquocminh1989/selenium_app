using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace selenium_app.Library
{
    class DriverConnector
    {
        /*
        API Doc : http://seleniumhq.github.io/selenium/docs/api/dotnet/index.html
         */
        private IWebDriver iwdDriver = null;
        protected String s_Root = Environment.CurrentDirectory;
        protected Int64 i_Timeout = 999999;
        protected String s_DriverPath = "";
        

        public DriverConnector(String s_Driver_Filename = "chromedriver.exe")
        {
            
            ChromeOptions options = CreateChromeOption();
            ChromeDriverService service = CreateChromeService(s_Driver_Filename);
            iwdDriver = StartChromeDriver(service, options);

        }

        public ChromeDriver StartChromeDriver(ChromeDriverService service, ChromeOptions options)
        {
            ChromeDriver driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(i_Timeout));

            return driver;
        }

        public ChromeDriverService CreateChromeService(String s_Driver_Filename)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(String.Concat(s_Root, @"\WebDrivers\"), s_Driver_Filename);
            service.HideCommandPromptWindow = true;

            return service;
        }

        public ChromeOptions CreateChromeOption()
        {
            ChromeOptions co_Options = new ChromeOptions();
            co_Options.AddArgument("--disable-infobars");
            //co_Options.AddArgument("--auto-open-devtools-for-tabs");

            return co_Options;
        }

        public void CloseBrowser()
        {
            iwdDriver.Quit();
        }

        public void GoToLink(String url)
        {
            iwdDriver.Navigate().GoToUrl(url);
        }

        public void WaitingForComplete()
        {

            try
            {
                //Waiting for page complete
                while (true) // Handle timeout somewhere
                {
                    var ajaxIsComplete = (bool)(iwdDriver as IJavaScriptExecutor).ExecuteScript("return document.readyState == 'complete'");
                    if (ajaxIsComplete)
                        break;
                    Thread.Sleep(100);
                }

                //Waiting for ajax complete
                while (true) // Handle timeout somewhere
                {
                    var ajaxIsComplete = (bool)(iwdDriver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                    if (ajaxIsComplete)
                        break;
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {

            }

        }

        public Boolean SetInputField(String xpath, String value, Int16 i_Waiting = 0)
        {
            if (CheckElementXpath(xpath) == true)
            {
                IWebElement iwe_Element = GetElementXpath(xpath);
                if (iwe_Element != null )
                {
                    iwe_Element.Clear();
                    iwe_Element.SendKeys(value);
                    if (i_Waiting != 0)
                    {
                        Thread.Sleep(i_Waiting);
                    }

                    return true;
                }
            }

            return false;
        }

        public IWebElement GetControl(String xpath)
        {
            if (CheckElementXpath(xpath) == true)
            {
                IWebElement iwe_Element = GetElementXpath(xpath);
                if (iwe_Element != null)
                {
                    return iwe_Element;
                }
            }

            return null;
        }

        public Boolean CheckElementXpath(String xpath)
        {
            try
            {
                WaitingForComplete();
                iwdDriver.FindElement(By.XPath(xpath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IWebElement GetElementXpath(String xpath)
        {
            WaitingForComplete();
            IWebElement oReturn = iwdDriver.FindElement(By.XPath(xpath));

            return oReturn;
        }



    }
}
