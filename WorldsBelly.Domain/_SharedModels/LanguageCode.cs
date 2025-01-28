using System;

namespace WorldsBelly.Domain.Models
{
    public class LanguageCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public bool IsUISupported { get; set; }

        public LanguageCode() { }

        public LanguageCode(int id, string code, string englishName, string nativeName, bool isUISupported = false)
        {
            Id = id;
            Code = code;
            EnglishName = englishName;
            NativeName = nativeName;
            IsUISupported = isUISupported;
        }
    }
}
