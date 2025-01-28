using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldsBelly.Puppeteers.Models
{
    public class FoundNutrient
    {
        public string Measurement { get; set; }
        public List<Nutrient> FoundNutrients { get; set; }
    }
    public class Nutrient
    {
        public string EnglishName { get; set; }
        public string FoundEnglishName { get; set; }
        public string Measurement { get; set; }
        public decimal? Amount { get; set; }

        public Nutrient() { }

        public Nutrient(string englishName, string foundEnglishName, string measurement, decimal? amount)
        {
            EnglishName = englishName;
            FoundEnglishName = foundEnglishName;
            Measurement = measurement;
            Amount = amount;
        }
    }
}
