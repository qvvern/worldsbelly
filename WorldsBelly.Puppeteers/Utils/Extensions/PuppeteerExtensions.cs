using System;
using System.Linq;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace SmartWorkers.FAPP.Runner.Utils.Extensions
{
    public static class PuppeteerExtensions
    {
        private const string IframeSelector = "iframe";

        public static async Task<Page> GetPage(this Browser browser, int number = 1)
        {
            number--;

            var pages = await browser.PagesAsync();

            if (number < pages.Length)
            {
                return pages[number];
            }

            throw new Exception($"Page {number + 1} is not available");
        }
        public static async Task CloseIncognitoAsync(this BrowserContext browser)
        {
            await browser.Browser.CloseAsync();
            return;
        }
        public static async Task<Page> GetPage(this BrowserContext browser, int number = 1)
        {
            number--;

            var pages = await browser.PagesAsync();

            if (number < pages.Length)
            {
                return pages[number];
            }

            throw new Exception($"Page {number + 1} is not available");
        }

        public static async Task AddToInputAsync(this Page page, string selector, string value, bool clearFirst = false)
        {
            await page.WaitForSelectorAsync(selector, new WaitForSelectorOptions
            {
                Timeout = 0
            });
            await page.FocusAsync(selector);

            if (clearFirst)
            {
                await page.Keyboard.PressAsync("Home");
                await page.Keyboard.DownAsync("Shift");
                await page.Keyboard.PressAsync("End");
                await page.Keyboard.UpAsync("Shift");
                await page.Keyboard.PressAsync("Backspace");
            }

            await page.Keyboard.TypeAsync(value);
        }

        public static async Task ClearFocusedElementAsync(this Page page)
        {
            await page.Keyboard.DownAsync("Control");
            await page.Keyboard.PressAsync("A");
            await page.Keyboard.UpAsync("Control");
            await page.Keyboard.PressAsync("Backspace");
        }
        public static async Task PasteIntoFocusedElementAsync(this Page page)
        {
            await page.Keyboard.DownAsync("Control");
            await page.Keyboard.PressAsync("V");
            await page.Keyboard.UpAsync("Control");
        }

        public static async Task CopyToClipboardAsync(this Page page, string value)
        {
            var javascriptCopyToClipboard = @$" 
                            var input = document.createElement('textarea');
                            input.innerHTML = `{value}`;
                            document.body.appendChild(input);
                            input.select();
                            var result = document.execCommand('copy');
                            document.body.removeChild(input);
                        ";
            await page.EvaluateExpressionHandleAsync(javascriptCopyToClipboard);

        }

        public static async Task<string> CopyFromClipboardAsync(this Page page)
        {
            var javascript = @$" 
                            var g = document.createElement('textarea');
                            g.id = 'PuPpEtEeRsHaRpojipsdojfPOHPOUH8787076976DSFDSFgfdfgsewwbytmuyi';
                            document.body.appendChild(g);
                        ";
            await page.EvaluateExpressionHandleAsync(javascript);
            var textarea = await page.QuerySelectorAsync("#PuPpEtEeRsHaRpojipsdojfPOHPOUH8787076976DSFDSFgfdfgsewwbytmuyi");
            await textarea.FocusAsync();
            await page.PasteIntoFocusedElementAsync();

            var text = await textarea.GetElementPropertyValue("value");



            var execute = @$" 
                            document.getElementById('PuPpEtEeRsHaRpojipsdojfPOHPOUH8787076976DSFDSFgfdfgsewwbytmuyi').outerHTML = '';
                        ";
            await page.EvaluateExpressionHandleAsync(execute);
            return text.ToString();

        }

        public static async Task AddToInputAsync(this Frame frame, string selector, string value)
        {
            await frame.WaitForSelectorAsync(selector, new WaitForSelectorOptions
            {
                Timeout = 0
            });
            await frame.FocusAsync(selector);
            await frame.TypeAsync(selector, value);
        }

        public static async Task ClickElementAsync(this Page page, string selector, int? timeout = null)
        {
            if (timeout.HasValue)
            {
                await page.WaitForTimeoutAsync(timeout.Value);
            }

            await page.WaitForSelectorAsync(selector);
            await page.ClickAsync(selector);
        }

        public static async Task ClickElementAsync(this Frame frame, string selector, int? timeout = null)
        {
            if (timeout.HasValue)
            {
                await frame.WaitForTimeoutAsync(timeout.Value);
            }

            await frame.WaitForSelectorAsync(selector);
            await frame.ClickAsync(selector);
        }

        public static async Task ClickElementByXpathAsync(this Page page, string xpath)
        {
            var elementHandle = (await page.XPathAsync(xpath))[0];
            await elementHandle.ClickAsync();
        }

        public static async Task ClickElementByXpathAsync(this Frame frame, string xpath)
        {
            var elementHandle = (await frame.XPathAsync(xpath))[0];
            await elementHandle.ClickAsync();
        }

        public static async Task HoverElementAsync(this Page page, string selector, int? timeout = null)
        {
            if (timeout.HasValue)
            {
                await page.WaitForTimeoutAsync(timeout.Value);
            }

            await page.WaitForSelectorAsync(selector);
            await page.HoverAsync(selector);
        }

        public static Frame GetChildFrame(this Frame frame, string name)
        {
            var childFrame = frame.ChildFrames.SingleOrDefault(_ => _.Name == name);

            if (childFrame == null)
                throw new Exception($"frame {name} name {frame.Name} was not found");

            return childFrame;
        }

        public static async Task<Frame> GetContentFrame(this Page page, int index)
        {
            var elementHandles = await page.QuerySelectorAllAsync(IframeSelector);
            var elementHandle = elementHandles[index];

            //TODO: Exception handling
            return await elementHandle.ContentFrameAsync();
        }

        public static async Task<Frame> GetContentFrame(this Frame frame)
        {
            var elementHandle = await frame.QuerySelectorAsync(IframeSelector);

            //TODO: Exception handling
            return await elementHandle.ContentFrameAsync();
        }

        public static async Task<string> GetElementPropertyValue(this ElementHandle field, string property)
        {
            var fieldProperty = await field.GetPropertyAsync(property);
            var fieldPropertyToJson = await fieldProperty.JsonValueAsync();
            return fieldPropertyToJson?.ToString();
        }
    }
}