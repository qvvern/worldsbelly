using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class Measurement : IEntity
    {
        public string WikidataId { get; set; }
        public int Id { get; set; }
        public string Unit { get; set; }
        public string EnglishName { get; set; }
        public string EnglishNamePlural { get; set; }
        public string EnglishDescription { get; set; }
        public bool IsMetric { get; set; }
        public bool IsImperial { get; set; }
        public bool IsHidden { get; set; }
        public int? BaseUnitId { get; set; }
        public double ConversionAmount { get; set; }
        public int TypeId { get; set; }
        public MeasurementType Type { get; set; }
        public ICollection<MeasurementTranslation> Translations { get; set; }
        public bool HasMultipleConversions { get; set; }
        public ICollection<MeasurementMultipleConversion> MultipleConversions { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MeasurementMultipleConversion>().ToTable("MeasurementMultipleConversions");
            builder.Entity<MeasurementMultipleConversion>().HasKey(m => new { m.MeasurementId, m.LanguageId });
            builder.Entity<MeasurementMultipleConversion>()
                .HasOne<Measurement>()
                .WithMany(m => m.MultipleConversions)
                .HasForeignKey(m => m.MeasurementId);

            builder.Entity<MeasurementTranslation>().ToTable("MeasurementTranslations");
            builder.Entity<MeasurementTranslation>().HasKey(t => new { t.MeasurementId, t.LanguageId });
            builder.Entity<MeasurementTranslation>()
                .HasOne<Measurement>()
                .WithMany(t => t.Translations)
                .HasForeignKey(t => t.MeasurementId);

            builder.Entity<MeasurementTypeTranslation>().ToTable("MeasurementTypeTranslations");
            builder.Entity<MeasurementTypeTranslation>().HasKey(t => new { t.MeasurementTypeId, t.LanguageId });
            builder.Entity<MeasurementTypeTranslation>()
                .HasOne<MeasurementType>()
                .WithMany(t => t.Translations)
                .HasForeignKey(t => t.MeasurementTypeId);
        }
    }

    public class MeasurementTranslation
    {
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int MeasurementId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }


    public class MeasurementType : IEntity
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public ICollection<MeasurementTypeTranslation> Translations { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class MeasurementTypeTranslation
    {
        public string Name { get; set; }
        public int MeasurementTypeId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }

    public class MeasurementMultipleConversion
    {
        public int MeasurementId { get; set; }
        public double ConversionAmount { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
