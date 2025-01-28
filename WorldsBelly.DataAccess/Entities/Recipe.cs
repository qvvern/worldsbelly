using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WorldsBelly.Domain.Interfaces;

namespace WorldsBelly.DataAccess.Entities
{
	public class Recipe : IEntity
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public string VideoUrl { get; set; }
		public Country OriginCountry { get; set; }
		public int? OriginCountryId { get; set; }
		public int CreatedByUserId { get; set; }
		public User CreatedByUser { get; set; }
		public int OriginalLanguageId { get; set; }
		public int ServingAmount { get; set; }
        public int? BestServedId { get; set; }
        public RecipeBestServed BestServed { get; set; }
        public int ConsumerId { get; set; }
        public RecipeConsumer Consumer { get; set; }
        public int AgeGroupId { get; set; }
        public RecipeAgeGroup AgeGroup { get; set; }
        public int? DifficultyId { get; set; }
        public RecipeDifficulty Difficulty { get; set; }
        public double TotalCookingTime { get; set; }
        //public int BestServedId { get; set; }
        //public BestServed BestServed { get; set; }
        public double TotalPrepTime { get; set; }
		public double TotalTime { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ICollection<Tag> Tags { get; set; }
        //public ICollection<UserTag> UserTags { get; set; }
        public ICollection<RecipeStep> Steps { get; set; }
        public ICollection<RecipeIngredientList> IngredientLists { get; set; }
        public ICollection<RecipeTranslation> Translations { get; set; }
        public IEnumerable<RecipeCalculatedNutrient> CalculatedNutrients { get; set; }
        public ICollection<RecipeComment> Comments { get; set; }
        public int TotalViews { get; set; }
        public int CalculatedTotaliked { get; set; }
        public int CalculatedTotalComments { get; set; }
        public int CalculatedTotalRatings { get; set; }
        public double CalculatedRating { get; set; }
        public double Sweet { get; set; }
        public double Sour { get; set; }
        public double Salty { get; set; }
        public double Bitter { get; set; }
        public double Spices { get; set; }
        public double Flavor { get; set; }
        public ICollection<RecipeLike> Likes { get; set; }
        public ICollection<RecipeRating> Ratings { get; set; }

        public Recipe()
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.Tags = new List<Tag>();
            //this.UserTags = new List<UserTag>();
            this.Steps = new List<RecipeStep>();
            this.IngredientLists = new List<RecipeIngredientList>();
            this.Translations = new List<RecipeTranslation>();
        }
        public Recipe(int CreatedByUserId, int OriginalLanguageId)
        {
            this.CreatedAt = DateTime.UtcNow;
            this.CreatedByUserId = CreatedByUserId;
            this.OriginalLanguageId = OriginalLanguageId;
            this.Tags = new List<Tag>();
            //this.UserTags = new List<UserTag>();
            this.Steps = new List<RecipeStep>();
            this.IngredientLists = new List<RecipeIngredientList>();
            this.Translations = new List<RecipeTranslation>();
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Recipe>()
                .HasMany(i => i.Tags)
                .WithMany(i => i.Recipes)
                .UsingEntity(e => e.ToTable("TagToRecipe"));

            builder.Entity<RecipeTranslation>()
                .HasKey(b => b.Id);

            //builder.Entity<Recipe>()
            //    .HasMany(i => i.UserTags)
            //    .WithMany(i => i.Recipes)
            //    .UsingEntity(e => e.ToTable("UserTagToRecipe"));

            builder.Entity<Recipe>()
                .HasMany(c => c.IngredientLists)
                .WithOne(e => e.Recipe)
                .HasForeignKey(e => e.RecipeId)
                .IsRequired();

            builder.Entity<Recipe>()
                .HasMany(c => c.Steps)
                .WithOne(e => e.Recipe)
                .HasForeignKey(e => e.RecipeId)
                .IsRequired();

            builder.Entity<RecipeIngredientList>()
                .HasMany(c => c.Ingredients)
                .WithOne(e => e.RecipeIngredientList)
                .HasForeignKey(e => e.RecipeIngredientListId)
                .IsRequired();

            builder.Entity<Recipe>()
                .HasMany(c => c.CalculatedNutrients)
                .WithOne(e => e.Recipe)
                .HasForeignKey(e => e.RecipeId)
                .IsRequired();

            builder.Entity<Recipe>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Recipe)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<RecipeComment>().ToTable("RecipeComments");
            //builder.Entity<RecipeComment>().HasKey(c => new { c.RecipeId, c.Id});
            //builder.Entity<RecipeComment>()
            //    .HasOne(c => c.Comment)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Recipe>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Recipe)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RecipeLike>().ToTable("RecipeLikes");
            builder.Entity<RecipeLike>().HasKey(c => new { c.RecipeId, c.UserId });

            builder.Entity<RecipeRating>().ToTable("RecipeRatings");
            builder.Entity<RecipeRating>().HasKey(c => new { c.RecipeId, c.UserId });

            builder.Entity<RecipeCalculatedNutrient>().HasKey(l => new { l.RecipeId, l.NutrientId });
            builder.Entity<RecipeCalculatedNutrient>()
                .HasOne(e => e.Nutrient);


            // Translations
            builder.Entity<RecipeTranslation>().ToTable("RecipeTranslations");
            builder.Entity<RecipeTranslation>().HasKey(t => new { t.RecipeId, t.LanguageId });
            builder.Entity<RecipeTranslation>()
                .HasOne<Recipe>()
                .WithMany(t => t.Translations)
                .HasForeignKey(t => t.RecipeId);

            builder.Entity<RecipeIngredientListTranslation>().ToTable("RecipeIngredientListTranslations");
            builder.Entity<RecipeIngredientListTranslation>().HasKey(t => new { t.RecipeIngredientListId, t.LanguageId });
            builder.Entity<RecipeIngredientList>()
                .HasMany(c => c.Translations)
                .WithOne(e => e.RecipeIngredientList)
                .HasForeignKey(e => e.RecipeIngredientListId)
                .IsRequired();

            builder.Entity<RecipeIngredientTranslation>().ToTable("RecipeIngredientTranslations");
            builder.Entity<RecipeIngredientTranslation>().HasKey(t => new { t.RecipeIngredientId, t.LanguageId });
            builder.Entity<RecipeIngredient>()
                .HasMany(c => c.Translations)
                .WithOne(e => e.RecipeIngredient)
                .HasForeignKey(e => e.RecipeIngredientId)
                .IsRequired();

            builder.Entity<RecipeStepTranslation>().ToTable("RecipeStepTranslations");
            builder.Entity<RecipeStepTranslation>().HasKey(t => new { t.RecipeStepId, t.LanguageId });
            builder.Entity<RecipeStep>()
                .HasMany(c => c.Translations)
                .WithOne(e => e.RecipeStep)
                .HasForeignKey(e => e.RecipeStepId)
                .IsRequired();

            builder.Entity<Recipe>()
                .HasOne(c => c.BestServed)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecipeBestServed>()
                .HasOne(c => c.Tag)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasOne(c => c.Consumer)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecipeConsumer>()
                .HasOne(c => c.Tag)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasOne(c => c.AgeGroup)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecipeAgeGroup>()
                .HasOne(c => c.Tag)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasOne(c => c.Difficulty)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecipeDifficulty>()
                .HasOne(c => c.Tag)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class RecipeBestServed : IEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string Text { get; set; }
    }

    public class RecipeConsumer : IEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string Text { get; set; }
    }

    public class RecipeAgeGroup : IEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string Text { get; set; }
    }

    public class RecipeDifficulty : IEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string Text { get; set; }
    }

    public class RecipeLike
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public class RecipeRating
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public double Rating { get; set; }
    }

    public class RecipeCalculatedNutrient
    {
        public int NutrientId { get; set; }
        public Nutrient Nutrient { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public double Amount { get; set; }
    }

    public class RecipeTranslation
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int RecipeId { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public bool IsApproved { get; set; }

        public RecipeTranslation() { }
        public RecipeTranslation(int CreatedByUserId, int LanguageId)
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.LanguageId = LanguageId;
            this.CreatedByUserId = CreatedByUserId;
        }
    }

    // IngredientList
    public class RecipeIngredientList : IEntity, IOrderIndex
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int OrderIndex { get; set; }
        public ICollection<RecipeIngredient> Ingredients { get; set; }
        public IEnumerable<RecipeIngredientListTranslation> Translations { get; set; }

        public RecipeIngredientList()
        {
            this.Ingredients = new List<RecipeIngredient>();
            this.Translations = new List<RecipeIngredientListTranslation>();
        }
    }

    public class RecipeIngredientListTranslation
    {
        public int RecipeIngredientListId { get; set; }
        public RecipeIngredientList RecipeIngredientList { get; set; }
        public string Title { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public RecipeIngredientListTranslation(int LanguageId)
        {
            this.LanguageId = LanguageId;
        }
    }


    // Ingredient
    public class RecipeIngredient : IEntity, IOrderIndex
    {
        public int Id { get; set; }
        public int RecipeIngredientListId { get; set; }
        public RecipeIngredientList RecipeIngredientList { get; set; }
        public double Amount { get; set; }
        public double AmountPerServingDefaultNutrientMeasurement { get; set; }
        public int MeasurementId { get; set; }
        public Measurement Measurement { get; set; }
        public int OrderIndex { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public IEnumerable<RecipeIngredientTranslation> Translations { get; set; }
        [NotMapped]
        public string TempId { get; set; }


        public RecipeIngredient()
        {
            this.Translations = new List<RecipeIngredientTranslation>();
        }
    }
    public class RecipeIngredientTranslation
    {
        public string Description { get; set; }
        public int RecipeIngredientId { get; set; }
        public RecipeIngredient RecipeIngredient { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public RecipeIngredientTranslation(int LanguageId)
        {
            this.LanguageId = LanguageId;
        }
    }

    // Steps
    public class RecipeStep : IEntity, IOrderIndex
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int OrderIndex { get; set; }
        public IEnumerable<RecipeStepTranslation> Translations { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }

        public RecipeStep()
        {
            this.Translations = new List<RecipeStepTranslation>();
        }
    }

    public class RecipeStepTranslation
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int RecipeStepId { get; set; }
        public RecipeStep RecipeStep { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public RecipeStepTranslation(int LanguageId)
        {
            this.LanguageId = LanguageId;
        }
    }
}
