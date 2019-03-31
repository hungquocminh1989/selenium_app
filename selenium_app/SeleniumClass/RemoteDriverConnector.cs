using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;

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
            rwdDriver.Navigate().GoToUrl(url);
            WaitingForComplete();
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

            }
        }

        public bool SetInputField(string xpath, string value, int i_Waiting = 0)
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

        public IWebElement GetControl(string xpath)
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
                return false;
            }
        }

        public IWebElement GetElementXpath(string xpath)
        {
            WaitingForComplete();
            IWebElement oReturn = rwdDriver.FindElement(By.XPath(xpath));

            return oReturn;
        }



    }
}
