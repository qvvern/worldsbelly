using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class Nutrient : IEntity
    {
        public int Id { get; set; }
        public int NutrientDbId { get; set; }
        public bool IsCommon { get; set; }
        public string EnglishName { get; set; }
        public string WikidataId { get; set; }
        public string EnglishNamePlural { get; set; }
        public string EnglishDescription { get; set; }
        public int DefaultMeasurementId { get; set; }
        public Measurement DefaultMeasurement { get; set; }
        public ICollection<NutrientTranslation> Translations { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NutrientTranslation>().ToTable("NutrientTranslations");
            builder.Entity<NutrientTranslation>().HasKey(t => new { t.NutrientId, t.LanguageId });
            builder.Entity<NutrientTranslation>()
                .HasOne<Nutrient>()
                .WithMany(t => t.Translations)
                .HasForeignKey(t => t.NutrientId);
        }
    }

    public class NutrientTranslation
    {
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public int NutrientId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
