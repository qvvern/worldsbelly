
// WORKS


// STEP 1
// OPEN BROWSER
//    BrowserContext browser = await FetchBrowser.OpenIncognitoBrowser();

//await browser.NewPageAsync();
//var test = await browser.GetPage(1);
//await test.GoToAsync("https://translate.google.com/");
//var buttons = await test.QuerySelectorAllAsync("button");
//await buttons.Last().ClickAsync();
//await test.WaitForTimeoutAsync(1500);


//// STEP 2

//List<Page> pages = new List<Page>();
//int tabLimit = 20;

//foreach (Translation language in translations.Take(tabLimit))
//{
//    Page page = await browser.NewPageAsync();
//    string properties = $"sl=en&tl={language.LanguageCode}&text={translation}&op=translate";
//    var url = $"https://translate.google.com/?{properties}";

//    language.Url = url;
//    await page.GoToAsync(url);

//    pages.Add(page);
//};

//int tabCounter = 0;

//foreach(Translation language in translations)
//{
//    var page = pages[tabCounter];
//    await page.BringToFrontAsync();

//    await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
//    var text = await page.CopyFromClipboardAsync();

//    Uri myUri = new Uri(page.Url);
//    string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("tl");

//    await page.ClickElementByXpathAsync("//button/i[text()='swap_horiz']");
//    await page.WaitForTimeoutAsync(500);
//    await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
//    var englishText = await page.CopyFromClipboardAsync();
//    if(englishText.ToLower().Contains(translation.ToLower()) || translation.ToLower().Contains(englishText.ToLower()))
//    {
//        translations.Find(_ => _.LanguageCode == param1).Text = text.Trim().TrimEnd('.');
//    }


//    var firstNullTranslation = translations.First(_ => _.Url == null);
//    string properties = $"sl=en&tl={firstNullTranslation.LanguageCode}&text={translation}&op=translate";
//    var url = $"https://translate.google.com/?{properties}";

//    language.Url = url;
//    await page.GoToAsync(url);

//    // reset tabCounter
//    tabCounter++;
//    if(tabCounter >= tabLimit)
//    {
//        tabCounter = 0;
//    }
//}

//foreach(Page page in pages)
//{
//    await page.BringToFrontAsync();

//    await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
//    var text = await page.CopyFromClipboardAsync();

//    Uri myUri = new Uri(page.Url);
//    string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("tl");
//    translations.Find(_ => _.LanguageCode == param1).Url = myUri.ToString();

//    await page.ClickElementByXpathAsync("//button/i[text()='swap_horiz']");
//    await page.WaitForTimeoutAsync(500);
//    await page.ClickElementByXpathAsync("//button/i[text()='content_copy']");
//    var englishText = await page.CopyFromClipboardAsync();
//    if (englishText.ToLower().Contains(translation.ToLower()) || translation.ToLower().Contains(englishText.ToLower()))
//    {
//        translations.Find(_ => _.LanguageCode == param1).Text = text;
//    }

//    translations.Find(_ => _.LanguageCode == param1).Text = text.Trim().TrimEnd('.');

//}

//// STEP 3
//// CLose Browser and return translations
//await browser.CloseIncognitoAsync();


//await Task.WhenAll(
//    LoopLanguages.ForGoogleTranslate(translations, translation, 0, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 16, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 16, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 24, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 32, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 40, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 48, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 56, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 64, 8),
//    LoopLanguages.ForGoogleTranslate(translations, translation, 72, 2)
//);

//if(translations.Any(_ => _.Text == null))
//{
//    await LoopLanguages.ForGoogleTranslate(translations, translation, 0, 100);
//}

