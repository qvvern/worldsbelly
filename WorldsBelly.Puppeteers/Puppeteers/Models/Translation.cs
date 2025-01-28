using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.Puppeteers.Models
{
    public class Translation
    {
        public string LanguageCode { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public int? Order { get; set; }
        public string Text { get; set; }
        public List<string> Texts { get; set; }
        public string Url { get; set; }

        public Translation() { }

        public Translation(int? order, string text, string languageCode, string englishName, string nativeName)
        {
            Order = order;
            Text = text;
            LanguageCode = languageCode;
            EnglishName = englishName;
            NativeName = nativeName;
            Texts = new List<string>();
        }
    }
}
