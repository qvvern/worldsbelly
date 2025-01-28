using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities.NutrientDb
{
    public class FoodCalorieConversionFactor
    {
        [Key]
        public int Food_nutrient_conversion_factor_id { get; set; }
        public double protein_value { get; set; }
        public double fat_value { get; set; }
        public double carbohydrate_value { get; set; }
    }
}
