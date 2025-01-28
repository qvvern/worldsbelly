using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities.NutrientDb
{
    public class FoodNutrient
    {
        [Key]
        public int Id { get; set; }
        public int Fdc_id { get; set; }
        public int Nutrient_id { get; set; }
        public string Amount { get; set; }
        public string data_points { get; set; }
    }
    public class FoodNutrientResponse
    {
        public int Id { get; set; }
        public int Fdc_id { get; set; }
        public int Nutrient_id { get; set; }
        public string NutrientName { get; set; }
        public string Amount { get; set; }
        public int ReferenceId { get; set; }
        public string Measurement { get; set; }
        public int MeasurementId { get; set; }

    }
}
