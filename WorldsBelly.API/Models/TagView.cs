using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsBelly.API.Models
{
	public class TagView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string NamePlural { get; set; }
		public string Description { get; set; }
		public bool? Hidden { get; set; }
        public bool? HideInCard { get; set; }

    }
}
