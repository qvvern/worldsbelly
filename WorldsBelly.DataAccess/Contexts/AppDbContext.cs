
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.DataAccess.Contexts
{
    public class AppDbContext : DbContext
    {
        private const int MaxNumberOfQueryArguments = 2000;
        // Shared
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryTranslation> CountryTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<MeasurementTranslation> MeasurementTranslations { get; set; }
        public DbSet<MeasurementType> MeasurementTypes { get; set; }
        public DbSet<MeasurementTypeTranslation> MeasurementTypeTranslations { get; set; }
        public DbSet<RecipeBestServed> RecipeBestServed { get; set; }
        public DbSet<RecipeConsumer> RecipeConsumer { get; set; }
        public DbSet<RecipeAgeGroup> RecipeAgeGroup { get; set; }
        public DbSet<RecipeDifficulty> RecipeDifficulty { get; set; }

        // App
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeApproval> RecipeApprovals { get; set; }
        public DbSet<RecipeTranslation> RecipeTranslations { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<RecipeStepTranslation> RecipeStepTranslations { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeIngredientList> RecipeIngredientLists { get; set; }
        public DbSet<RecipeIngredientListTranslation> RecipeIngredientListTranslations { get; set; }
        public DbSet<RecipeComment> RecipeComments { get; set; }
        public DbSet<RecipeLike> RecipeLikes { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }
        public DbSet<RecipeCalculatedNutrient> RecipeCalculatedNutrients { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<NotificationTemplateTranslation> NotificationTemplateTranslations { get; set; }

        // Portal
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<NutrientTranslation> NutrientTranslations { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientTranslation> IngredientTranslations { get; set; }
        //public DbSet<UserTag> UserTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagTranslation> TagTranslations { get; set; }
        public DbSet<TagType> TagTypes { get; set; }
        public DbSet<TagSelectable> TagSelectables { get; set; }
        public DbSet<TagSelectableCategory> TagSelectableCategories { get; set; }
        public DbSet<TagSelectableChoiceOrder> TagSelectableChoiceOrders { get; set; }
        public DbSet<IngredientNutrient> IngredientNutrients { get; set; }
        public DbSet<EnglishTranslation> EnglishTranslations { get; set; }
        public DbSet<Translation> Translations { get; set; }
        //public DbSet<UserTagToRecipe> UserTagToRecipe { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Recipe.OnModelCreating(builder);
            Ingredient.OnModelCreating(builder);
            Nutrient.OnModelCreating(builder);
            Measurement.OnModelCreating(builder);
            Country.OnModelCreating(builder);
            Tag.OnModelCreating(builder);
            TagSelectableCategory.OnModelCreating(builder);
            User.OnModelCreating(builder);
            RecipeComment.OnModelCreating(builder);
            Notification.OnModelCreating(builder);
            EnglishTranslation.OnModelCreating(builder);
        }

        public void DeleteEntitiesRaw(string tableName, List<int> listOfIds)
        {
            if (!listOfIds.Any())
            {
                return;
            }

            List<List<int>> idsChunked = new List<List<int>>();

            if (listOfIds.Count > MaxNumberOfQueryArguments)
            {
                for (int i = 0; i < listOfIds.Count; i += MaxNumberOfQueryArguments)
                {
                    idsChunked.Add(listOfIds.GetRange(i, Math.Min(MaxNumberOfQueryArguments, listOfIds.Count - i)));
                }
            }
            else
            {
                idsChunked.Add(listOfIds);
            }

            DeleteEntitiesRaw(tableName, idsChunked);
        }

        private void DeleteEntitiesRaw(string tableName, ICollection<List<int>> idsLists)
        {
            foreach (List<int> idsList in idsLists)
            {
                if (!idsList.Any())
                {
                    continue;
                }

                if (idsList.Count > MaxNumberOfQueryArguments)
                {
                    throw new Exception($"Failed trying to bulk delete {tableName}. Max number of arguments is {MaxNumberOfQueryArguments} but found {idsList.Count}");
                }

                SqlParameter[] parameters = idsList.Select((id, index) => new SqlParameter($"@p{index}", id)).ToArray();
                string parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                string query = $"DELETE FROM {tableName} WHERE Id IN ({parameterNames})";
                Database.ExecuteSqlRaw(query, parameters);
            }
        }

        public void RawQuery(string query)
        {
            Database.ExecuteSqlRaw(query);
        }
    }
}
