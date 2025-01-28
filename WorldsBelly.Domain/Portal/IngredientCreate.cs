using System;
using System.Collections.Generic;
using WorldsBelly.Domain.Models;

namespace WorldsBelly.Domain.Portal
{
    public class IngredientCreate
    {
        public int? DefaultMeasurementId { get; set; }
        public int? DefaultNutrientMeasurementId { get; set; }
        public string EnglishName { get; set; }
        public string EnglishNamePlural { get; set; }
        public string EnglishDescription { get; set; }
        public string WikidataId { get; set; }
        public string Image { get; set; }
        public List<TranslationView> Translations { get; set; }
        public List<NutrientView> Nutrients { get; set; }
        public List<TagView> Tags { get; set; }
    }
    public class IngredientImport
    {
        public string Item { get; set; } 
        public string Label { get; set; }
        public string Subclasses { get; set; }
        public string Instances { get; set; }
        public string Categories { get; set; }
    }
    public class UpdateIngredientsFromWiki
    {
        public List<int> Ids { get; set; }
        public List<string> WikiIds { get; set; }
        public bool? OverrideFieldsWithExistingValues { get; set; }
        public int? FromId { get; set; }
        public int? ToId { get; set; }
    }
}
