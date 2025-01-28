using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class Country : IEntity
    {
		public int Id { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public string Code { get; set; }
        public int DialCode { get; set; }
        public string Continent { get; set; }
        public string Capital { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsHidden { get; set; }

        public Language PrimaryLanguage { get; set; }
        public ICollection<CountryAlternativeLanguage> AlternativeLanguages { get; set; }
        public ICollection<CountryTranslation> Translations { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CountryAlternativeLanguage>().ToTable("CountryAlternativeLanguages");
            builder.Entity<CountryAlternativeLanguage>().HasKey(l => new { l.CountryId, l.LanguageId });
            builder.Entity<CountryAlternativeLanguage>()
                .HasOne<Country>()
                .WithMany(c => c.AlternativeLanguages)
                .HasForeignKey(l => l.CountryId);

            builder.Entity<CountryTranslation>().ToTable("CountryTranslations");
            builder.Entity<CountryTranslation>().HasKey(t => new { t.CountryId, t.LanguageId });
            builder.Entity<CountryTranslation>()
                .HasOne<Country>()
                .WithMany(t => t.Translations)
                .HasForeignKey(t => t.CountryId);
        }
    }

    public class CountryTranslation
    {
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
    }

    public class CountryAlternativeLanguage
    {
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
