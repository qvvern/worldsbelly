using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities.NutrientDb
{
    public class Food
    {
        [Key]
        public int Fdc_id { get; set; }
        public string Description { get; set; }
        public string Data_type { get; set; }
        public string Food_category_id { get; set; }
        public DateTime Publication_date { get; set; }
    }
}
