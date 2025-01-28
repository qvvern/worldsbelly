using System;
using System.Collections.Generic;

namespace WorldsBelly.API.Models
{
    public class LanguageView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        //public string WikidataLanguageCode { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
    }
}
