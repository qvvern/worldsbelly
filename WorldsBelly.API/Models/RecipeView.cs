using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsBelly.API.Models
{
    public class RecipeView
	{
		public string ImageUrl { get; set; }
		public string VideoUrl { get; set; }
		//public CountryView OriginCountry { get; set; }
		public int? OriginCountryId { get; set; }
		public UserView CreatedByUser { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset? PublishedAt { get; set; }
		public DateTimeOffset? ApprovedAt { get; set; }
		public DateTimeOffset? UpdatedAt { get; set; }
		public int OriginalLanguageId { get; set; }
		public double TotalCookingTime { get; set; }
		public double TotalPrepTime { get; set; }
		public double TotalTime { get; set; }
		public int ServingAmount { get; set; }
		public int? DifficultyId { get; set; }
		public int? BestServedId { get; set; }
		public int ConsumerId { get; set; }
		public int AgeGroupId { get; set; }
		public bool IsPublished { get; set; }
		public bool IsApproved { get; set; }
		public ICollection<RecipeStepView> Steps { get; set; }
        public ICollection<RecipeIngredientListView> IngredientLists { get; set; }
		// Translations
		public int RecipeId { get; set; }
		public Guid Id { get; set; }
		public UserView TranslatedByUser { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public int LanguageId { get; set; }

		// Tags and user tags
		public ICollection<TagView> Tags { get; set; }
		public ICollection<UserTagView> UserTags { get; set; }

		// Calculations
		public double Sweet { get; set; }
		public double Sour { get; set; }
		public double Salty { get; set; }
		public double Bitter { get; set; }
		public double Spices { get; set; }
		public double Flavor { get; set; }
		public double Rating { get; set; }
		public int TotalComments { get; set; }
		public ICollection<RecipeNutrientView> Nutrients { get; set; }
		public int TotalViews { get; set; }
		public int Totaliked { get; set; }
		public int TotalRatings { get; set; }
		public int TotalFlavorRatings { get; set; }

	}
	public class RecipeNutrientView
	{
		public int Id { get; set; }
		public double Amount { get; set; }
	}
	public class RecipeIngredientListView 
	{
		public int Id { get; set; }
		public int RecipeId { get; set; }
		public int OrderIndex { get; set; }
		public ICollection<RecipeIngredientView> Ingredients { get; set; }
		// Translations
		public string Title { get; set; }
	}
	public class RecipeIngredientView
	{
		public int Id { get; set; }
		public string TempId { get; set; }
		public int RecipeIngredientId { get; set; }
		public int RecipeIngredientListId { get; set; }
		public double Amount { get; set; }
		public int MeasurementId { get; set; }
		public int OrderIndex { get; set; }
		public int IngredientId { get; set; }
		// Translations
		public string Name { get; set; }
		public string NamePlural { get; set; }
		public string Description { get; set; }
	}
	public class RecipeStepView
	{
		public int Id { get; set; }
		public int RecipeId { get; set; }
		public int OrderIndex { get; set; }
		public string ImageUrl { get; set; }
		public string VideoUrl { get; set; }
		// translations
		public string Title { get; set; }
		public string Content { get; set; }
	}

	public class CreateRecipeDraftView
    {
        public List<int> TagIds { get; set; }
		public int? LanguageId { get; set; }

	}

	public class RecipeRatingView
	{
        public int RecipeId { get; set; }
		public double? Rating { get; set; }
	}
	public class RecipeBestServedView
	{
		public int Id { get; set; }
		public int TagId { get; set; }
		public string Label { get; set; }
	}

	public class RecipeConsumerView
	{
		public int Id { get; set; }
		public int TagId { get; set; }
		public string Label { get; set; }
	}

	public class RecipeAgeGroupView
	{
		public int Id { get; set; }
		public int TagId { get; set; }
		public string Label { get; set; }
	}

	public class RecipeDifficultyView
	{
		public int Id { get; set; }
		public int TagId { get; set; }
		public string Label { get; set; }
	}
}
