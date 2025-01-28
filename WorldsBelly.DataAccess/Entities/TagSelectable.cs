using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldsBelly.DataAccess.Entities
{
    public class TagSelectableCategory : IEntity
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public EnglishTranslation Title { get; set; }
        public int? TextId { get; set; }
        public EnglishTranslation Text { get; set; }
        public string ExcludedTags { get; set; }
        public ICollection<TagSelectable> TagSelectables { get; set; }
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TagSelectableCategory>()
                .HasMany(i => i.TagSelectables)
                .WithMany(i => i.Categories)
                .UsingEntity(e => e.ToTable("TagSelectableCategoryToTagSelectable"));
        }
    }
    public class TagSelectable : IEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string Icon { get; set; }
        public int OrderIndex { get; set; }
        public string ExcludedTags { get; set; }
        public bool DontAddTag { get; set; }
        public ICollection<TagSelectableCategory> Categories { get; set; }
    }
    public class TagSelectableChoiceOrder : IEntity
    {
        public int Id { get; set; }
        public int? TagId { get; set; }
        public Tag Tag { get; set; }
        public int OrderIndex { get; set; }
        public int? RelatedChoiceId { get; set; }
        public int TagSelectableCategoryId { get; set; }
        public TagSelectableCategory TagSelectableCategory { get; set; }
    }
}
