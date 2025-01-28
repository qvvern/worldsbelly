using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorldsBelly.DataAccess.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public Guid ADObjectId { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public int TotalRecipes { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Disabled { get; set; }
        public bool Anonymous { get; set; }
        public string Summary { get; set; }
        public string Title { get; set; }
        public string References { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }
        public IEnumerable<RecipeTranslation> RecipeTranslations { get; set; }
        public IEnumerable<RecipeRating> RecipeRatings { get; set; }
        public IEnumerable<RecipeComment> Comments { get; set; }
        public IEnumerable<RecipeLike> RecipeLikes { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(c => c.Recipes)
                .WithOne(e => e.CreatedByUser)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<User>()
                .HasMany(c => c.RecipeTranslations)
                .WithOne(e => e.CreatedByUser)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<User>()
                .HasMany(t => t.Comments)
                .WithOne(e => e.CreatedByUser)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<User>()
                .HasMany(t => t.RecipeLikes)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<User>()
                .HasMany(t => t.RecipeRatings)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
