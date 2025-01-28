using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.Domain.Models
{
    public class TranslationView
    {
        public string Id { get; set; }
        public int LanguageId { get; set; } 
        public string LanguageCode { get; set; } 
        public string Text { get; set; } // Translation
        public string TextPlural { get; set; } // Translation Plural
        public string TextDescription { get; set; } // Translation
    }
}
