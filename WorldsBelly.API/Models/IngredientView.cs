using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsBelly.API.Models
{
	public class IngredientView
	{
		public int Id { get; set; }
		public int DefaultMeasurementId { get; set; }
        public double? OneMilliliterInGram { get; set; }
        public double? OneCentimeterInGram { get; set; }
        public double? OneCentimeterInMilliliter { get; set; }
        public double? OnePieceInGram { get; set; }
        public double? OnePieceInMilliliter { get; set; }
        public double? OnePieceInCentimeter { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
    }
}
