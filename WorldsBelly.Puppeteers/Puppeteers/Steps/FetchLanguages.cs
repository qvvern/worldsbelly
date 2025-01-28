using System;
using System.Collections.Generic;
using System.Linq;
using WorldsBelly.Puppeteers.Models;
using WorldsBelly.Puppeteers.Service;

namespace WorldsBelly.Puppeteers.PuppeteerSharp.Steps
{
    public class FetchLanguages : PuppeteerService
    {
        public static List<Translation> Languages()
        {
            List<Translation> languages = new List<Translation>()
            {
                new Translation(null, null, "af", "Afrikaans", "Afrikaans"),
                new Translation(null, null, "am", "Amharic", "አማርኛ"),
                new Translation(null, null, "ar", "Arabic", "العربية"),
                new Translation(null, null, "az", "Azerbaijani", "Azərbaycanca / آذربايجان"),
                new Translation(null, null, "be", "Belarusian", "Беларуская"),
                new Translation(null, null, "bg", "Bulgarian", "Български"),
                new Translation(null, null, "bn", "Bengali", "বাংলা"),
                new Translation(null, null, "bs", "Bosnian", "Bosanski"),
                new Translation(null, null, "ca", "Catalan", "Català"),
                new Translation(null, null, "cs", "Czech", "Česky"),
                new Translation(null, null, "da", "Danish", "Dansk"),
                new Translation(null, null, "de", "German", "Deutsch"),
                new Translation(null, null, "el", "Greek", "ΕλληνικKά"),
                new Translation(null, null, "en", "English", "English"),
                new Translation(null, null, "es", "Spanish", "Español"),
                new Translation(null, null, "et", "Estonian", "Eesti"),
                new Translation(null, null, "fa", "Persian", "فارسی"),
                new Translation(null, null, "fi", "Finnish", "Suomi"),
                new Translation(null, null, "fr", "French", "Français"),
                new Translation(null, null, "gu", "Gujarati", "ગુજરાતી"),
                new Translation(null, null, "iw", "Hebrew", "עברית"),
                new Translation(null, null, "hi", "Hindi", "हिन्दी"),
                new Translation(null, null, "hr", "Croatian", "Hrvatski"),
                new Translation(null, null, "ht", "Haitian", "Krèyol ayisyen"),
                new Translation(null, null, "hu", "Hungarian", "Magyar"),
                new Translation(null, null, "hy", "Armenian", "Հայերեն"),
                new Translation(null, null, "id", "Indonesian", "Bahasa Indonesia"),
                new Translation(null, null, "is", "Icelandic", "Íslenska"),
                new Translation(null, null, "it", "Italian", "Italiano"),
                new Translation(null, null, "ja", "Japanese", "日本語"),
                new Translation(null, null, "jw", "Javanese", "Basa Jawa"),
                new Translation(null, null, "ka", "Georgian", "ქართუKლი"),
                new Translation(null, null, "kk", "Kazakh", "Қазақша"),
                new Translation(null, null, "km", "Cambodian", "ភាសាខ្មែរ    "),
                new Translation(null, null, "kn", "Kannada", "ಕನ್ನಡ"),
                new Translation(null, null, "ko", "Korean", "한국어"),
                new Translation(null, null, "ku", "Kurdish (Kurmanji)", "Kurdî"),
                new Translation(null, null, "lt", "Lithuanian", "Lietuvių"),
                new Translation(null, null, "lv", "Latvian", "Latviešu"),
                new Translation(null, null, "mg", "Malagasy", "Malagasy"),
                new Translation(null, null, "mk", "Macedonian", "Македонски"),
                new Translation(null, null, "ml", "Malayalam", "മലയാളം"),
                new Translation(null, null, "mn", "Mongolian", "Монгол"),
                new Translation(null, null, "mr", "Marathi", "मराठी"),
                new Translation(null, null, "ne", "Nepali", "नेपाली"),
                new Translation(null, null, "nl", "Dutch", "Nederlands"),
                new Translation(null, null, "no", "Norwegian", "Norsk (bokmål / riksmål)"),
                new Translation(null, null, "pa", "Panjabi / Punjabi", "ਪੰਜਾਬੀ / पंजाबी / پنجابي"),
                new Translation(null, null, "pl", "Polish", "Polski"),
                new Translation(null, null, "ps", "Pashto", "پښتو"),
                new Translation(null, null, "pt", "Portuguese", "Português"),
                new Translation(null, null, "ro", "Romanian", "Română"),
                new Translation(null, null, "ru", "Russian", "Русский"),
                new Translation(null, null, "sd", "Sindhi", "सिनधि"),
                new Translation(null, null, "si", "Sinhalese", "සිංහල    "),
                new Translation(null, null, "sk", "Slovak", "Slovenčina"),
                new Translation(null, null, "sl", "Slovenian", "Slovenščina"),
                new Translation(null, null, "sq", "Albanian", "Shqip"),
                new Translation(null, null, "sr", "Serbian", "Српски"),
                new Translation(null, null, "sv", "Swedish", "Svenska"),
                new Translation(null, null, "th", "Thai", "ไทย / Phasa Thai"),
                new Translation(null, null, "tk", "Turkmen", "Туркмен / تركمن"),
                new Translation(null, null, "tl", "Tagalog", "Tagalog"),
                new Translation(null, null, "tr", "Turkish", "Türkçe"),
                new Translation(null, null, "uk", "Ukrainian", "Українська"),
                new Translation(null, null, "ur", "Urdu", "اردو"),
                new Translation(null, null, "vi", "Vietnamese", "Việtnam"),
                new Translation(null, null, "yo", "Yoruba", "Yorùbá"),
                new Translation(null, null, "zh-CN", "Chinese (Simplified)", "中文"),
                new Translation(null, null, "zh-TW", "Classical Chinese", "文言"),
                new Translation(null, null, "my", "Burmese", "Myanmasa"),
                new Translation(null, null, "ms", "Malay", "Bahasa Melayu"),
            };
            return languages.OrderBy(x => x.EnglishName).ToList();
        }

    }
}