
//// STEP 2
//// Go to FoodData Central and search for keyword and select first result
//var page = await browser.GetPage();
//await page.GoToAsync($"https://fdc.nal.usda.gov/fdc-app.html#/?query={ingredient}");
////await page.WaitForTimeoutAsync(3000); // wait for page to load
//ElementHandle[] results = await page.QuerySelectorAllAsync($"[name='food-search-result-description']");
//await results[0].ClickAsync();
//await page.WaitForTimeoutAsync(8000); // wait for page to load

//// STEP 3
//// Get Portion amount
//ElementHandle[] portionOption = await page.QuerySelectorAllAsync("#nutrient-per-selection-Survey-or-branded option");
//var portionWithMesuament = await portionOption[0].GetPropertyAsync("innerText").Result.JsonValueAsync();
//var portion = portionWithMesuament.ToString().Split(" ")[0].Trim();
//var portionMeasure = portionWithMesuament.ToString().Split(" ")[1].Trim();

//// STEP 4
//// Get nutrients
//ElementHandle[] nutrientsTableRows = await page.QuerySelectorAllAsync("#nutrients-table tbody tr");


//var foundNutrients = new List<FoundNutrientData>();

//foreach (ElementHandle row in nutrientsTableRows)
//{
//    var rowName = await row.QuerySelectorAsync("[name='finalFoodNutrientName']");
//    var rowAverageAmount = await row.QuerySelectorAsync("[name='finalFoodNutrientValue']");
//    var rowMeasurement = await row.QuerySelectorAsync("[name='finalFoodNutrientStandardUnit']");

//    if (rowName != null && rowAverageAmount != null && rowMeasurement != null)
//    {
//        var nutrient = await rowName.GetPropertyAsync("innerText").Result.JsonValueAsync();
//        var amount = await rowAverageAmount.GetPropertyAsync("innerText").Result.JsonValueAsync();
//        var measurement = await rowMeasurement.GetPropertyAsync("innerText").Result.JsonValueAsync();
//        if (nutrient != null && amount != null && measurement != null)
//        {
//            foundNutrients.Add(new FoundNutrientData()
//            {
//                Nutrient = nutrient.ToString().Trim(),
//                Amount = amount.ToString().Trim(),
//                Measurement = measurement.ToString().Trim(),
//            });
//        }
//    };
//}
