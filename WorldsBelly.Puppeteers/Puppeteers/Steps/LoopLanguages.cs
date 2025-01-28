using PuppeteerSharp;
using SmartWorkers.FAPP.Runner.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsBelly.Puppeteers.Models;
using WorldsBelly.Puppeteers.Service;

namespace WorldsBelly.Puppeteers.PuppeteerSharp.Steps
{
    public class LoopLanguages : PuppeteerService
    {
        public static async Task ForGoogleTranslate(List<Translation> translations, string translation, int skip, int take)
        {
            // STEP 1
            // OPEN BROWSER
            BrowserContext browser = await FetchBrowser.OpenIncognitoBrowser();
            //Page page = await browser.GetPage();

            // STEP 2
            // Loop Over All languages and open google translate with language and text in a new tab and add translation to list
            int count = 0;
            foreach (Translation language in translations.Skip(skip).Take(take).Where(_ => _.Text == null))
            {

                try
                {
                    var page = await browser.NewPageAsync();
                    string properties = $"sl=en&tl={language.LanguageCode}&text={translation}&op=translate";
                    var url = $"https://translate.google.com/?{properties}";
                    language.Url = url;
                    await page.GoToAsync(url);
                    if (count == 0)
                    {
                        var buttons = await page.QuerySelectorAllAsync("button");
                        await buttons.Last().ClickAsync();
                    }

                    await page.WaitForTimeoutAsync(3500); // wait for google to translate

                    ElementHandle[] translatedData = await page.QuerySelectorAllAsync($"[data-result-index='0'] span [data-language-for-alternatives='{language.LanguageCode}'] span");
                    if (translatedData.Length > 0)
                    {
                        var text = await translatedData[0].GetPropertyAsync("innerText").Result.JsonValueAsync();
                        language.Text = text.ToString().Trim().TrimEnd('.');
                    }

                    //await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
                    //var text = await page.CopyFromClipboardAsync();


                    //char[] a = text.ToString().ToCharArray();
                    //a[0] = char.ToUpper(a[0]);
                    //var textWithUpperCase = new string(a);


                    await page.CloseAsync();
                    count += 1;
                }
                catch (FormatException e)
                {
                    // do nothing
                }
            }

            // STEP 3
            // CLose Browser and return translations
            await browser.CloseAsync();

            return;
        }
    }
}
