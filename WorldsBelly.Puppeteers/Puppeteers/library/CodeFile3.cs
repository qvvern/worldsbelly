//using System.Threading.Tasks;
//using PuppeteerSharp;
//using SmartWorkers.FAPP.Runner.Utils.Extensions;
//using System.Linq;
//using System.Collections.Generic;
//using WorldsBelly.Puppeteers.PuppeteerSharp.Steps;
//using WorldsBelly.Puppeteers.Models;
//using WorldsBelly.Puppeteers.Service.Interfaces;
//using WorldsBelly.Domain.ViewModel;

//namespace WorldsBelly.Puppeteers.Service
//{
//    public class PuppeteerService : IPuppeteerService
//    {

//        public async Task<List<Translation>> TranslateAsync(string translation)
//        {
//            // STEP 1
//            // OPEN BROWSER
//            Browser browser = await FetchBrowser.OpenBrowser();
//            //Page page = await browser.GetPage();

//            // STEP 2
//            // Loop Over All languages and open google translate with language and text in a new tab and add translation to list
//            var translations = new List<Translation>();
//            int count = 0;
//            foreach (LanguageCode language in FetchLanguages.Languages())
//            {
//                var page = await browser.NewPageAsync();
//                string properties = $"sl=en&tl={language.Code}&text={translation}&op=translate";
//                await page.GoToAsync($"https://translate.google.com/?{properties}");
//                if (count == 0)
//                {
//                    var buttons = await page.QuerySelectorAllAsync("button");
//                    await buttons.Last().ClickAsync();
//                }
//                await page.WaitForTimeoutAsync(3000); // wait for google to translate
//                ElementHandle[] translatedData = await page.QuerySelectorAllAsync($"[data-language-for-alternatives='{language.Code}']");
//                if (translatedData.Length > 0)
//                {
//                    var text = await translatedData[0].GetPropertyAsync("innerText").Result.JsonValueAsync();
//                    translations.Add(new Translation()
//                    {
//                        Language = language.EnglishName,
//                        Text = text.ToString().Trim()
//                    });
//                }
//                await page.CloseAsync();
//                count += 1;
//            }

//            // STEP 3
//            // CLose Browser and return translations
//            await browser.CloseAsync();
//            return translations;
//        }

//        public async Task<FoundNutrient> FindNutrientsAsync(string ingredient)
//        {
//            // STEP 1
//            // OPEN BROWSER
//            Browser browser = await FetchBrowser.OpenBrowser();
//            //Page page = await browser.GetPage();

//            // STEP 2
//            // Go to FoodData Central and search for keyword and select first result
//            var page = await browser.GetPage();
//            await page.GoToAsync($"https://fdc.nal.usda.gov/fdc-app.html#/?query={ingredient}");
//            //await page.WaitForTimeoutAsync(3000); // wait for page to load
//            ElementHandle[] results = await page.QuerySelectorAllAsync($"[name='food-search-result-description']");
//            await results[0].ClickAsync();
//            await page.WaitForTimeoutAsync(8000); // wait for page to load

//            // STEP 3
//            // Get Portion amount
//            ElementHandle[] portionOption = await page.QuerySelectorAllAsync("#nutrient-per-selection-Survey-or-branded option");
//            var portionWithMesuament = await portionOption[0].GetPropertyAsync("innerText").Result.JsonValueAsync();
//            var portion = portionWithMesuament.ToString().Split(" ")[0].Trim();
//            var portionMeasure = portionWithMesuament.ToString().Split(" ")[1].Trim();

//            // STEP 4
//            // Get nutrients
//            ElementHandle[] nutrientsTableRows = await page.QuerySelectorAllAsync("#nutrients-table tbody tr");


//            var foundNutrients = new List<FoundNutrientData>();

//            foreach (ElementHandle row in nutrientsTableRows)
//            {
//                var rowName = await row.QuerySelectorAsync("[name='finalFoodNutrientName']");
//                var rowAverageAmount = await row.QuerySelectorAsync("[name='finalFoodNutrientValue']");
//                var rowMeasurement = await row.QuerySelectorAsync("[name='finalFoodNutrientStandardUnit']");

//                if (rowName != null && rowAverageAmount != null && rowMeasurement != null)
//                {
//                    var nutrient = await rowName.GetPropertyAsync("innerText").Result.JsonValueAsync();
//                    var amount = await rowAverageAmount.GetPropertyAsync("innerText").Result.JsonValueAsync();
//                    var measurement = await rowMeasurement.GetPropertyAsync("innerText").Result.JsonValueAsync();
//                    if (nutrient != null && amount != null && measurement != null)
//                    {
//                        foundNutrients.Add(new FoundNutrientData()
//                        {
//                            Nutrient = nutrient.ToString().Trim(),
//                            Amount = amount.ToString().Trim(),
//                            Measurement = measurement.ToString().Trim(),
//                        });
//                    }
//                };
//            }

//            // STEP 5
//            // CLose Browser and return nutrients
//            await browser.CloseAsync();
//            return new FoundNutrient()
//            {
//                Url = page.Url,
//                Portion = portion,
//                PortionMeasure = portionMeasure,
//                FoundNutrients = foundNutrients
//            };
//        }
//    }
//}
