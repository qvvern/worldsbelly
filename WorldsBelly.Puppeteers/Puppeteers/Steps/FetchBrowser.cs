using PuppeteerSharp;
using System.Threading.Tasks;
using WorldsBelly.Puppeteers.Service;

namespace WorldsBelly.Puppeteers.PuppeteerSharp.Steps
{
    public class FetchBrowser : PuppeteerService
    {
        public static async Task<Browser> OpenBrowser()
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                ExecutablePath = "C:/Program Files (x86)/Google/Chrome/Application/chrome.exe",
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }, // , "--start-maximized"
                DefaultViewport = null,
                Devtools = true,

            });

            return browser;
        }
        public static async Task<BrowserContext> OpenIncognitoBrowser()
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                ExecutablePath = "C:/Program Files (x86)/Google/Chrome/Application/chrome.exe",
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox", "--incognito", "--start-maximized" },
                DefaultViewport = null,
                Devtools = true,

            });
            BrowserContext context = await browser.CreateIncognitoBrowserContextAsync();

            return context;
        }
    }
}
