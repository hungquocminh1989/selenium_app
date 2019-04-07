using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace selenium_app.Library
{
    class RemoteDriverConnector
    {
        /*
        API Doc : http://seleniumhq.github.io/selenium/docs/api/dotnet/index.html
         */
        private RemoteWebDriver rwdDriver = null;

        public RemoteDriverConnector(string hubUrl)
        {
            rwdDriver = SettingDriver(hubUrl);
        }

        public RemoteWebDriver SettingDriver(string hubUrl)
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("--disable-infobars");
            option.AddArguments("--start-maximized");
            rwdDriver = new RemoteWebDriver(new Uri(hubUrl), option.ToCapabilities());
            //rwdDriver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromMinutes(10));
            //rwdDriver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromMinutes(10));
            //rwdDriver.Manage().Timeouts().AsynchronousJavaScript.Add(TimeSpan.FromMinutes(10));

            /*DesiredCapabilities cap = new DesiredCapabilities();
            cap.SetCapability(CapabilityType.BrowserName, DriverSetting.ChromeBrowser);
            rwdDriver = new RemoteWebDriver(new Uri(hubUrl), cap);*/

            //cap.SetCapability(CapabilityType.Platform, PlatformType.Any);

            //rwdDriver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromMinutes(10));
            //rwdDriver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromMinutes(10));

            return rwdDriver;
        }

        public void CloseBrowser()
        {
            rwdDriver.Quit();
        }

        public void GoToLink(string url)
        {
            try
            {
                rwdDriver.Navigate().GoToUrl(url);
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
                    var ajaxIsComplete = (bool)(rwdDriver as IJavaScriptExecutor).ExecuteScript("return document.readyState == 'complete'");
                    if (ajaxIsComplete)
                        break;
                    Thread.Sleep(100);
                }

                //Waiting for ajax complete
                while (true) // Handle timeout somewhere
                {
                    var ajaxIsComplete = (bool)(rwdDriver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
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
                rwdDriver.FindElement(By.XPath(xpath));
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
                IWebElement oReturn = rwdDriver.FindElement(By.XPath(xpath));

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

        public bool SelectCheckbox(string xpath)
        {

            try
            {
                IWebElement checkbox = GetControl(xpath);
                if (checkbox.Selected == true)
                {
                    checkbox.Click();

                    return true;
                }
                
            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return false;

        }

        public bool SelectCombobox(string xpath, string value, string text)
        {

            try
            {

                IWebElement combobox = GetControl(xpath);
                var selectElement = new SelectElement(combobox);
                if (value != null)
                {
                    selectElement.SelectByValue(value);

                    return true;
                }
                if (text != null)
                {
                    selectElement.SelectByText(text);

                    return true;
                }

            }
            catch (Exception)
            {
                ExceptionHandler();
            }

            return false;

        }

    }
}
