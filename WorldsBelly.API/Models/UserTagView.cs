using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsBelly.API.Models
{
	public class UserTagView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int LanguageId { get; set; }
		public int RecipeReferencesAmount { get; set; }
	}
}
