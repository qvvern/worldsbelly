using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsBelly.API.Models
{
	public class MeasurementView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string NamePlural { get; set; }
		public string Description { get; set; }
		public double ConversionAmount { get; set; }
		public string Unit { get; set; }
		public int TypeId { get; set; }
	}
}
