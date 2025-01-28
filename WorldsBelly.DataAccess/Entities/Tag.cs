using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldsBelly.DataAccess.Entities
{

    public class Tag : IEntity
    {
        public string WikidataId { get; set; }
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string EnglishNamePlural { get; set; }
        public string EnglishDescription { get; set; }
        public string Image { get; set; }
        public ICollection<TagTranslation> Translations { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Published { get; set; }
        public int? TranslationsAmount { get; set; }
        public int? TagTypeId { get; set; }
        public TagType TagType { get; set; }
        public bool? Hidden { get; set; }
        public bool? HideInCard { get; set; }
        public bool? ExcludeAlways { get; set; }
        public bool? IncludeAlways { get; set; }
        public int RecipeReferencesAmount { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TagTranslation>().ToTable("TagTranslations");
            builder.Entity<TagTranslation>().HasKey(t => new { t.TagId, t.LanguageId });
            builder.Entity<TagTranslation>()
                .HasOne<Tag>()
                .WithMany(t => t.Translations)
                .HasForeignKey(t => t.TagId);
        }
    }

    public class TagType
    {
        public int Id { get; set; }
        public string Name { get; set; } // searchable at this level, searchable at this level 2 
    }
    public class TagTranslation
    {
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public int TagId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
