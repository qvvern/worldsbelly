using System;
using System.Collections.Generic;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.API.Models
{
    //public class EnglishTranslationView
    //{
    //    public int Id { get; set; }
    //    public string Text { get; set; }
    //    public string TextPlural { get; set; }
    //}
    public class TranslationView
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        //public LanguageView Language { get; set; }
        public string Text { get; set; }
        public string TextPlural { get; set; }
    }
}
