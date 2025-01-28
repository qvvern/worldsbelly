using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WorldsBelly.Domain.Interfaces;

namespace WorldsBelly.DataAccess.Entities
{
	public class RecipeApproval : IEntity
	{
		public int Id { get; set; }
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public int LanguageId { get; set; }
        public int CreatedById { get; set; }
        public Guid RecipeTranslationId { get; set; }
        public int UnPublishedIngredients { get; set; }
        public string UnPublishedIngredientIds { get; set; }
        public string UnPublishedIngredientNames { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int AmountOfIngredients { get; set; }
        public int AmountOfSteps { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RecipeApproval>()
                .HasKey(b => b.Id);
        }
    }
}
