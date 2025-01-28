using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities.NutrientDb
{
    public class FoodProteinConversionFactor
    {
        [Key]
        public int Food_nutrient_conversion_factor_id { get; set; }
        public double Value { get; set; }
    }
}
