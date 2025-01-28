using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities.NutrientDb
{
    public class Nutrient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit_name { get; set; }
        public double? Nutrient_nbr { get; set; }
        public int? Rank { get; set; }
        public int? ReferenceId { get; set; }
    }
}
