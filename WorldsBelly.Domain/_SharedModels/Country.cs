using System;
using System.Collections.Generic;

namespace WorldsBelly.Domain.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public string CountryCode { get; set; }


        // Primary language
        public int PrimaryLanguageCodeId { get; set; }
        public string PrimaryLanguageCode { get; set; }
    }
}
