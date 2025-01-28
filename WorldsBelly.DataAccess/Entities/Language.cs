using System;

namespace WorldsBelly.DataAccess.Entities
{
    public class Language : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string WikidataLanguageCode { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public bool IsHidden { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
