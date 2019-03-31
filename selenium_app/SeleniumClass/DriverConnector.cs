using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace selenium_app.Library
{
    class DriverConnector
    {
        /*
        API Doc : http://seleniumhq.github.io/selenium/docs/api/dotnet/index.html
         */
        private IWebDriver iwdDriver = null;
        protected string s_Root = Environment.CurrentDirectory;
        protected Int64 i_Timeout = 999999;
        protected string s_DriverPath = "";
        

        public DriverConnector(string s_Driver_Filename = "chromedriver.exe")
        {
            try
            {
                ChromeOptions options = CreateChromeOption();
                ChromeDriverService service = CreateChromeService(s_Driver_Filename);
                iwdDriver = StartChromeDriver(service, options);
            }
            catch (Exception)
            {
                ExceptionHandler();
            }
        }

        public ChromeDriver StartChromeDriver(ChromeDriverService service, ChromeOptions options)
        {
            try
            {
                ChromeDriver driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(i_Timeout));
                return driver;
            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return null;
        }

        public ChromeDriverService CreateChromeService(string s_Driver_Filename)
        {
            try
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(string.Concat(s_Root, @"\WebDrivers\"), s_Driver_Filename);
                //service.HideCommandPromptWindow = true;

                return service;
            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return null;
        }

        public ChromeOptions CreateChromeOption()
        {
            try
            {
                ChromeOptions co_Options = new ChromeOptions();
                co_Options.AddArgument("--disable-infobars");
                //co_Options.AddArgument("--auto-open-devtools-for-tabs");

                return co_Options;
            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return null;
        }

        public void CloseBrowser()
        {
            iwdDriver.Quit();
        }

        public void GoToLink(string url)
        {
            try
            {
                iwdDriver.Navigate().GoToUrl(url);
                WaitingForComplete();
            }
            catch (Exception)
            {
                ExceptionHandler();
            }
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
                ExceptionHandler();
            }
        }

        public bool SetInputField(string xpath, string value, Int16 i_Waiting = 0)
        {
            try
            {
                if (CheckElementXpath(xpath) == true)
                {
                    IWebElement iwe_Element = GetElementXpath(xpath);
                    if (iwe_Element != null)
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
            }
            catch (Exception)
            {
                ExceptionHandler();
            }
           
            return false;
        }

        public IWebElement GetControl(string xpath)
        {
            try
            {
                if (CheckElementXpath(xpath) == true)
                {
                    IWebElement iwe_Element = GetElementXpath(xpath);
                    if (iwe_Element != null)
                    {
                        return iwe_Element;
                    }
                }
            }
            catch (Exception)
            {
                ExceptionHandler();
            }
            
            return null;
        }

        public bool CheckElementXpath(string xpath)
        {
            try
            {
                WaitingForComplete();
                iwdDriver.FindElement(By.XPath(xpath));
                return true;
            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return false;
        }

        public IWebElement GetElementXpath(string xpath)
        {
            try
            {
                WaitingForComplete();
                IWebElement oReturn = iwdDriver.FindElement(By.XPath(xpath));

                return oReturn;
            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return null;
        }

        public void ExceptionHandler()
        {
            //Set log ...
            //Capture ...

            //Close
            CloseBrowser();
        }


    }
}
