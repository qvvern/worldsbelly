using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities.NutrientDb
{
    public class FoodNutrientConversionFactor
    {
        [Key]
        public int Id { get; set; }
        public int fdc_id { get; set; }
    }
}
