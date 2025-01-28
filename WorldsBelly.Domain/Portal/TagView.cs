using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldsBelly.Domain.Models;

namespace WorldsBelly.Domain.Portal
{
    public class TagView
    {
        public string EnglishName { get; set; }
        public string EnglishNamePlural { get; set; }
        public string EnglishDescription { get; set; }
        public string Image { get; set; }
        public string WikidataId { get; set; }
        public List<TranslationView> Translations { get; set; }
    }
}
