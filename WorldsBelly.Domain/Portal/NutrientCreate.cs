using System;
using System.Collections.Generic;
using WorldsBelly.Domain.Models;

namespace WorldsBelly.Domain.Portal
{
    public class NutrientCreate
    {
        public string EnglishName { get; set; }
        public string Measurement { get; set; } // ml eller g
        public List<TranslationView> Translations { get; set; }
    }
}
