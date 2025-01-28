using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace WorldsBelly.DataAccess.Entities
{
    public class RecipeComment : IEntity
    {
        public int Id { get; set; }
        public int RootId { get; set; }
        public int Level { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public User DeletedByUser { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public User ModifiedByUser { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public List<RecipeCommentTranslation> Translations { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RecipeComment>().ToTable("RecipeComments");

            builder.Entity<RecipeComment>()
                .Property(c => c.RootId)
                .IsRequired();

            builder.Entity<RecipeComment>()
                .Property(c => c.Level)
                .IsRequired();

            builder.Entity<RecipeComment>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey("CreatedByUserId")
                .IsRequired();

            builder.Entity<RecipeComment>()
                .HasOne(c => c.DeletedByUser)
                .WithMany()
                .HasForeignKey("DeletedByUserId")
                .IsRequired(false);

            builder.Entity<RecipeComment>()
                .HasOne(c => c.ModifiedByUser)
                .WithMany()
                .HasForeignKey("ModifiedByUserId")
                .IsRequired(false);

            builder.Entity<RecipeComment>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder.Entity<RecipeCommentTranslation>().ToTable("RecipeCommentTranslations");
            builder.Entity<RecipeCommentTranslation>().HasKey(t => new { t.CommentId, t.LanguageId });
            builder.Entity<RecipeCommentTranslation>()
                .HasOne(t => t.Comment)
                .WithMany(t => t.Translations)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class RecipeCommentTranslation
    {
        public int CommentId { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
        public bool IsOriginal { get; set; }

        public RecipeComment Comment { get; set; }
        public Language Language { get; set; }
    }
}
