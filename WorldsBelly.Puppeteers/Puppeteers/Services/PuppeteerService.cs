using System.Threading.Tasks;
using PuppeteerSharp;
using SmartWorkers.FAPP.Runner.Utils.Extensions;
using System.Linq;
using System.Collections.Generic;
using WorldsBelly.Puppeteers.PuppeteerSharp.Steps;
using WorldsBelly.Puppeteers.Models;
using WorldsBelly.Puppeteers.Service.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Web;
using WorldsBelly.Puppeteers.Puppeteers.Models;
using System.Collections.Concurrent;

namespace WorldsBelly.Puppeteers.Service
{
    public class PuppeteerService : IPuppeteerService
    {
        public async Task<List<Translation>> TranslateAsync(List<string> translations)
        {
            var translation = string.Join("%0A", translations.ToArray());
            List<Translation> languages = FetchLanguages.Languages();
            Browsers browsers = new Browsers();

            object threadLock = new object();

            BrowserContext mainBrowser = await FetchBrowser.OpenIncognitoBrowser();
            await mainBrowser.NewPageAsync();
            var firstPage = await mainBrowser.GetPage(1);
            await firstPage.GoToAsync("https://translate.google.com/");
            var buttons = await firstPage.QuerySelectorAllAsync("button");
            await buttons.Last().ClickAsync();
            //await firstPage.WaitForTimeoutAsync(1500);

            browsers.Windows.Add(new Window(mainBrowser));

            Task.Run(async () =>
            {
                await Task.WhenAll(
                    from partition in Partitioner.Create(languages).GetPartitions(12)
                    select Task.Run(async delegate
                    {
                        using (partition)
                        {
                            while (partition.MoveNext())
                            {
                                Translation language = partition.Current;
                                string properties = $"sl=en&tl={language.LanguageCode}&text={translation}&op=translate";
                                var url = $"https://translate.google.com/?{properties}";
                                language.Url = url;
                                language.Texts = translations.ToList();

                                if (language.LanguageCode != "en")
                                {
                                    var currentWindow = browsers.Windows.Last();
                                    Page page = await currentWindow.Browser.NewPageAsync();
                                    await page.GoToAsync(url);

                                    currentWindow.Pages.Add(page);
                                }
                                else
                                {
                                    language.Text = translation.Trim().TrimEnd('.');
                                }
                            }
                        }
                    }))
                    .ConfigureAwait(false);
            }).Wait();


            foreach (Window window in browsers.Windows)
            {
                BrowserContext browser = window.Browser;
                foreach (Page page in window.Pages)
                {
                    await page.BringToFrontAsync();

                    await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
                    var text = await page.CopyFromClipboardAsync(); // 

                    Uri myUri = new Uri(page.Url);
                    string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("tl");
                    languages.Find(_ => _.LanguageCode == param1).Url = myUri.ToString(); 

                    await page.ClickElementByXpathAsync("//button/i[text()='swap_horiz']");
                    await page.WaitForTimeoutAsync(500);
                    await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
                    var englishText = await page.CopyFromClipboardAsync();

                    List<string> texts = text.Split('\n').ToList();
                    List<string> englishTexts = englishText.Split('\n').ToList();
                    var index = 0;
                    foreach(var translatedText in texts)
                    {
                        if (englishTexts[index].ToString().ToLower().Contains(translations[index].ToString().ToLower()) || translations[index].ToString().ToLower().Contains(englishTexts[index].ToString().ToLower()))
                        {
                            languages.Find(_ => _.LanguageCode == param1).Texts[index] = translatedText.Trim().TrimEnd('.');
                            languages.Find(_ => _.LanguageCode == param1).Order = 3;
                            if (translatedText.ToLower() == translations[index].ToLower())
                            {
                                languages.Find(_ => _.LanguageCode == param1).Order = 2;
                            }
                        }
                        else
                        {
                            languages.Find(_ => _.LanguageCode == param1).Texts[index] = null;
                            languages.Find(_ => _.LanguageCode == param1).Order = 1;
                        }
                        index++;
                    }

                    //if (englishText.ToLower().Contains(translation.ToLower()) || translation.ToLower().Contains(englishText.ToLower()))
                    //{
                    //    languages.Find(_ => _.LanguageCode == param1).Text = text.Trim().TrimEnd('.');
                    //    languages.Find(_ => _.LanguageCode == param1).Order = 3;
                    //    if(text.ToLower() == translation.ToLower())
                    //    {
                    //        languages.Find(_ => _.LanguageCode == param1).Order = 2;
                    //    }
                    //}
                    //else
                    //{
                    //    languages.Find(_ => _.LanguageCode == param1).Order = 1;
                    //}

                }

                await browser.CloseIncognitoAsync();


            }

            return languages.OrderBy(x => x.Order).ThenBy(x => x.Text).ThenBy(x => x.EnglishName).ToList();
        }

        public async Task<FoundNutrient> FindNutrientsAsync(string url)
        {
            // STEP 1
            // OPEN BROWSER
            Browser browser = await FetchBrowser.OpenBrowser();
            //Page page = await browser.GetPage();

            // STEP 2
            // Go to website
            var page = await browser.GetPage();
            await page.GoToAsync(url);

            // add amount
            ElementHandle[] quantityInputs = await page.QuerySelectorAllAsync($".MuiCardHeader-root input");
            var multiplierAmount = 9999999999999999999;
            await page.CopyToClipboardAsync(multiplierAmount.ToString());
            await quantityInputs[0].FocusAsync();
            await page.ClearFocusedElementAsync();
            await page.PasteIntoFocusedElementAsync();

            // choose measurement
            var unit = "";
            ElementHandle[] measurementInputs = await page.QuerySelectorAllAsync($".MuiCardHeader-root .MuiSelect-selectMenu");
            await measurementInputs[0].ClickAsync();
            await page.WaitForTimeoutAsync(1000);
            ElementHandle[] choices = await page.QuerySelectorAllAsync($".MuiPaper-root ul li");
            foreach (ElementHandle choice in choices)
            {
                var choiceText = choice.GetPropertyAsync("textContent").Result.JsonValueAsync().Result.ToString();
                if (choiceText == "ml")
                {
                    await choice.ClickAsync();
                    unit = "ml";
                    break;
                }
                else if (choiceText == "g")
                {
                    await page.Keyboard.PressAsync("g");
                    await page.Keyboard.PressAsync("Enter");
                    //await choice.ClickAsync();
                    unit = "g";
                    break;
                }
            }


            // STEP 3
            // Get nutrients
            ElementHandle[] nutrientsTableRows = await page.QuerySelectorAllAsync(".MuiCollapse-container .MuiTable-root tbody tr.MuiTableRow-root");

            var nutrientsList = FetchNutrients.Nutrients();
            int counter = 1;
            foreach (ElementHandle row in nutrientsTableRows)
            {

                try
                {

                    var rowHeader = await row.QuerySelectorAsync("th");
                    var rowItems = await row.QuerySelectorAllAsync("td");

                    if (counter == 1)
                    {
                        var caloriesString = rowHeader.GetPropertyAsync("innerText").Result.JsonValueAsync().Result.ToString();
                        decimal nutrientCalories = decimal.Parse(Regex.Match(caloriesString, @"[0-9]+(\.[0-9]+)?").Value);
                        var kilojoulesString = rowItems[0].GetPropertyAsync("innerText").Result.JsonValueAsync().Result.ToString();
                        decimal nutrientKilojoules = decimal.Parse(Regex.Match(kilojoulesString, @"[0-9]+(\.[0-9]+)?").Value);
                        nutrientsList.First(_ => _.FoundEnglishName == "Calories").Amount = nutrientCalories / multiplierAmount;
                        nutrientsList.First(_ => _.FoundEnglishName == "Kilojoules").Amount = nutrientKilojoules / multiplierAmount;
                    }
                    else
                    {
                        if (rowHeader != null && rowItems.Length > 0 && rowItems[0] != null)
                        {
                            var nutrientTitle = rowHeader.GetPropertyAsync("innerText").Result.JsonValueAsync().Result.ToString();
                            var nutrientString = rowItems[0].GetPropertyAsync("innerText").Result.JsonValueAsync().Result.ToString();
                            var nutrientAmountString = nutrientString.Split(" ")[0].Trim().ToString();
                            decimal nutrientAmount = 0;
                            if (nutrientAmountString != "0")
                            {
                                nutrientAmount = decimal.Parse(Regex.Match(nutrientAmountString, @"[0-9]+(\.[0-9]+)?").Value);
                            }

                            if (nutrientTitle != null)
                            {
                                nutrientsList.First(_ => _.FoundEnglishName == nutrientTitle).Amount = nutrientAmount / multiplierAmount;
                            };
                        }
                    }
                }
                catch (FormatException e)
                {
                    // do nothing
                }

                counter++;

            }

            // STEP 5
            // CLose Browser and return nutrients
            await browser.CloseAsync();
            return new FoundNutrient()
            {
                Measurement = unit,
                FoundNutrients = nutrientsList
            };
        }
    }
}
