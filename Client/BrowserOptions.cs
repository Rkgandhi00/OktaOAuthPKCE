using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OktaOAuthPOC.Models;

namespace OktaOAuthPOC.Client
{
    public class BrowserOptions
    {
        public SearchEngineTypeCode OpenBrowser(string url)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");

                    // Process.Start(url) was not working as expected, so this is a hack to open chrome from command prompt.
                    Process.Start(new ProcessStartInfo("cmd", $"/c start chrome {url}"));

                    return SearchEngineTypeCode.Chrome;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                    return SearchEngineTypeCode.Default;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                    return SearchEngineTypeCode.Default;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return SearchEngineTypeCode.None;
        }

        public void CloseBrowser(SearchEngineTypeCode searchEngine)
        {
            try
            {
                // currently this code handles closing the chrome browser only
                if (searchEngine == SearchEngineTypeCode.Chrome)
                {
                    var browserInstances = Process.GetProcessesByName("chrome");
                    foreach (var instance in browserInstances)
                    {
                        instance.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}