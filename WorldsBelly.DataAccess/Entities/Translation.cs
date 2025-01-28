using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class EnglishTranslation : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string TextPlural { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ICollection<Translation> Translations { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            // Translations
            builder.Entity<Translation>().ToTable("Translations");
            builder.Entity<Translation>().HasKey(t => new { t.EnglishTranslationId, t.LanguageId });
            builder.Entity<EnglishTranslation>()
                .HasMany(c => c.Translations)
                .WithOne(e => e.EnglishTranslation)
                .HasForeignKey(e => e.EnglishTranslationId)
                .IsRequired();

            builder.Entity<TagSelectableCategory>()
                .HasOne(c => c.Title)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TagSelectableCategory>()
                .HasOne(c => c.Text)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<BestServed>()
            //    .HasOne(c => c.Translation)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
    public class Translation
    {
        public int EnglishTranslationId { get; set; }
        public EnglishTranslation EnglishTranslation { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public string Text { get; set; }
        public string TextPlural { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
