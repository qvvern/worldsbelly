using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldsBelly.DataAccess.Entities
{
    public class Ingredient : IEntity
    {
        public string WikidataId { get; set; }
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string EnglishNamePlural { get; set; }
        public string EnglishDescription { get; set; }
        public string ImageUrl { get; set; }
        public int? DefaultMeasurementId { get; set; }
        public int? DefaultNutrientMeasurementId { get; set; }
        public Measurement DefaultMeasurement { get; set; }
        public Measurement DefaultNutrientMeasurement { get; set; }
        public ICollection<IngredientNutrient> Nutrients { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<IngredientTranslation> Translations { get; set; }
        public double? OneMilliliterInGram { get; set; }
        public double? OneCentimeterInGram { get; set; }
        public double? OneCentimeterInMilliliter { get; set; }
        public double? OnePieceInGram { get; set; }
        public double? OnePieceInMilliliter { get; set; }
        public double? OnePieceInCentimeter { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Published { get; set; }
        public bool Archived { get; set; }
        public int? NutrientsAmount { get; set; }
        public int? TranslationsAmount { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Ingredient>()
                .HasMany(i => i.Tags)
                .WithMany(i => i.Ingredients)
                .UsingEntity(e => e.ToTable("TagToIngredient"));

            builder.Entity<IngredientNutrient>()
                .HasKey(i => new { i.IngredientId, i.NutrientId });

            builder.Entity<IngredientNutrient>()
                .HasOne<Ingredient>()
                .WithMany(i => i.Nutrients)
                .HasForeignKey(t => t.IngredientId);

            builder.Entity<IngredientNutrient>().ToTable("IngredientNutrients");
            builder.Entity<IngredientNutrient>().HasKey(t => new { t.IngredientId, t.NutrientId });
            builder.Entity<IngredientNutrient>()
                .HasOne(t => t.Ingredient)
                .WithMany(t => t.Nutrients)
                .HasForeignKey(t => t.IngredientId);

            // Translations
            builder.Entity<IngredientTranslation>().ToTable("IngredientTranslations");
            builder.Entity<IngredientTranslation>().HasKey(t => new { t.IngredientId, t.LanguageId });
            builder.Entity<Ingredient>()
                .HasMany(c => c.Translations)
                .WithOne(e => e.Ingredient)
                .HasForeignKey(e => e.IngredientId)
                .IsRequired();
        }
    }

    public class IngredientTranslation
    {
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public int IngredientId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public Ingredient Ingredient { get; set; }
    }

    public class IngredientNutrient
    {
        public double Amount { get; set; }
        public int IngredientId { get; set; }
        public int NutrientId { get; set; }
        public Nutrient Nutrient { get; set; }
        public Ingredient Ingredient { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
