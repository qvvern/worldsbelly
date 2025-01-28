using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldsBelly.Puppeteers.Models;
using WorldsBelly.Puppeteers.Service;

namespace WorldsBelly.Puppeteers.PuppeteerSharp.Steps
{
    public class FetchNutrients : PuppeteerService
    {
        public static List<Nutrient> Nutrients()
        {
            List<Nutrient> nutrients = new List<Nutrient>()
            {
                new Nutrient("Calories", "Calories", "kcal", null),
                new Nutrient("Kilojoules", "Kilojoules", "kJ", null),
                new Nutrient("Total Fat", "Total Fat", "g", null),
                new Nutrient("Saturated fat", "Saturated Fat", "g", null),
                new Nutrient("Polyunsaturated fat", "Polyunsaturated Fat", "g", null),
                new Nutrient("Monounsaturated fat", "Monounsaturated Fat", "g", null),
                new Nutrient("Trans fat", "Trans Fat", "g", null),
                new Nutrient("Omega-3", "Omega-3 Fatty Acids", "g", null),
                new Nutrient("Cholesterol", "Cholesterol", "mg", null),
                new Nutrient("Sodium", "Sodium", "mg", null),
                new Nutrient("Total Carbohydrate", "Total Carbohydrate", "g", null),
                new Nutrient("Dietary fiber", "Dietary Fiber", "g", null),
                new Nutrient("Sugar", "Sugars", "g", null),
                new Nutrient("Protein", "Protein", "g", null),
                new Nutrient("Calcium", "Calcium", "mg", null),
                new Nutrient("Potassium", "Potassium", "mg", null),
                new Nutrient("Iron", "Iron", "mg", null),
                new Nutrient("Vitamin A", "Vitamin A", "mg", null),
                new Nutrient("Vitamin B-6", "Vitamin B-6", "mg", null), // not on calorieking
                new Nutrient("Cobalamin", "Cobalamin", "mg", null), // not on calorieking
                new Nutrient("Vitamin C", "Vitamin C", "mg", null),
                new Nutrient("Vitamin D", "Vitamin D", "mg", null), // not on calorieking
                new Nutrient("Magnesium", "Magnesium", "mg", null), // not on calorieking
                new Nutrient("Caffeine", "Caffeine", "mg", null),
                new Nutrient("Alcohol", "Alcohol", "g", null),
            };
            return nutrients;
        }

    }
}