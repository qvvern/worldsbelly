

//var page = await browser.NewPageAsync();
//// STEP 2
//// SELECT ALL LANGUAGES
//await page.ClickElementAsync(".selectContainer");
//await page.WaitForTimeoutAsync(100);
//var languagesToPick = await page.QuerySelectorAllAsync(".listContainer .listItem");

//foreach (ElementHandle lang in languagesToPick)
//{
//    var language = await lang.GetPropertyAsync("innerText").Result.JsonValueAsync();
//    await page.AddToInputAsync(".selectContainer input", language.ToString().Trim());
//    await page.WaitForTimeoutAsync(150);
//    await page.Keyboard.PressAsync("Enter");
//    await page.ClickElementAsync(".selectContainer");
//}

//var languagesToPickCheck = await page.QuerySelectorAllAsync(".listContainer .listItem");
//await page.Keyboard.PressAsync("Enter");
//if (languagesToPickCheck.Length > 0)
//{
//    foreach (var i in Enumerable.Range(0, 100))
//    {
//        await page.ClickElementAsync(".selectContainer");
//        var languagesToPickCheck2 = await page.QuerySelectorAllAsync(".listContainer .listItem");
//        if (languagesToPickCheck2.Length == 0)
//        {
//            break;
//        }
//        await page.WaitForTimeoutAsync(100);
//        await page.Keyboard.PressAsync("Enter");
//    }
//}

//// STEP 3
//// TRANSLATE TEXT
//await page.WaitForTimeoutAsync(100);
//await page.AddToInputAsync("#focusOnMe", translation.Trim());
//await page.WaitForTimeoutAsync(1500);




//// STEP 4
//// GET TRANSLATIONS
//var translations = new List<Translation>();
//var elementTranslations = await page.QuerySelectorAllAsync(".translation");

//for (int i = 0; i < elementTranslations.Length; i++)
//{
//    var languageTranslation = await elementTranslations[i].GetPropertyAsync("innerText").Result.JsonValueAsync();
//    var language = await languagesToPick[i].GetPropertyAsync("innerText").Result.JsonValueAsync();
//    await page.WaitForTimeoutAsync(50);
//    translations.Add(new Translation()
//    {
//        Language = language.ToString().Trim(),
//        Text = languageTranslation.ToString().Trim()
//    });
//}

//await browser.CloseAsync();
//return translations;

// open multiple pages in browsers and execute..
//var theBrowser1 = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = browser.WebSocketEndpoint });
//var theBrowser2 = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = browser.WebSocketEndpoint });
//var page1 = await theBrowser1.NewPageAsync();
//var page2 = await theBrowser2.NewPageAsync();

//await Task.WhenAll(
//    page1.GoToAsync("https://www.stackoverflow.com"),
//    page2.GoToAsync("https://serverfault.com/")
//);