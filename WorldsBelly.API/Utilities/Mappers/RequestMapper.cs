using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WorldsBelly.API.Models;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.API.Utilities.Mappers
{ 
    public static class RequestMapper
    {
        private static int LanguageId = 0;
        private static int RecipeId = 0;
        public static Country Map(CountryView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Country()
            {
                Id = entity.Id,
                Code = entity.Code
            };
        }
        //public static TagSelectable Map(TagSelectableView entity)
        //{
        //    if (entity == null)
        //    {
        //        return null;
        //    }

        //    return new TagSelectable()
        //    {
        //        Id = entity.Id,
        //        Icon = entity.Icon,
        //        StepId = entity.StepId,
        //        RelatedTagId = entity.RelatedTagId,
        //        TagId = entity.TagId,
        //        OrderIndex = entity.OrderIndex,
        //        ExcludedTags = entity.ExcludedTags,
        //        DontAddTag = entity.DontAddTag
        //    };
        //}
        public static Recipe Map(RecipeView entity)
        {
            if (entity == null)
            {
                return null;
            }
            LanguageId = entity.LanguageId;
            RecipeId = entity.RecipeId;

            var translation = MapTranslation(entity);
            List<RecipeTranslation> translations = new List<RecipeTranslation>();
            translations.Add(translation);

            return new Recipe()
            {
                Id = entity.RecipeId,
                ImageUrl = entity.ImageUrl,
                VideoUrl = entity.VideoUrl,
                UpdatedAt = DateTime.UtcNow,
                //CreatedByUserId = entity.CreatedByUserId,
                CreatedAt = entity.CreatedAt,
                OriginCountryId = entity.OriginCountryId,
                Tags = entity.Tags?.Select(Map).ToList(),
                Steps = entity.Steps?.Select(Map).ToList(),
                IngredientLists = entity.IngredientLists?.Select(Map).ToList(),
                OriginalLanguageId = entity.OriginalLanguageId,
                TotalCookingTime = entity.TotalCookingTime,
                TotalPrepTime = entity.TotalPrepTime,
                TotalTime = entity.TotalTime,
                ServingAmount = entity.ServingAmount,
                DifficultyId = entity.DifficultyId,
                BestServedId = entity.BestServedId,

                Sweet = entity.Sweet,
                Bitter = entity.Bitter,
                Flavor = entity.Flavor,
                Salty = entity.Salty,
                Sour = entity.Sour,
                Spices = entity.Spices,

                ConsumerId = entity.ConsumerId,
                AgeGroupId = entity.AgeGroupId,
                CalculatedTotalComments = entity.TotalComments,
                Translations = translations
            };
        }
        public static RecipeTranslation MapTranslation(RecipeView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeTranslation()
            {
                Id = entity.Id,
                RecipeId = entity.RecipeId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                //CreatedByUserId = entity.TranslatedByUserId,
                LanguageId = entity.LanguageId,
                Title = entity.Title,
                Summary = entity.Summary
            };
        }
        public static RecipeStep Map(RecipeStepView entity)
        {
            if (entity == null)
            {
                return null;
            }

            var translation = MapTranslation(entity);
            List<RecipeStepTranslation> translations = new List<RecipeStepTranslation>();
            translations.Add(translation);

            return new RecipeStep()
            {
                Id = entity.Id,
                RecipeId = RecipeId,
                ImageUrl = entity.ImageUrl,
                VideoUrl = entity.VideoUrl,
                Translations = translations,
                OrderIndex = entity.OrderIndex
            };
        }
        public static RecipeStepTranslation MapTranslation(RecipeStepView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeStepTranslation(LanguageId)
            {
                RecipeStepId = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
            };
        }
        public static RecipeIngredientList Map(RecipeIngredientListView entity)
        {
            if (entity == null)
            {
                return null;
            }

            var translation = MapTranslation(entity);
            List<RecipeIngredientListTranslation> translations = new List<RecipeIngredientListTranslation>();
            translations.Add(translation);

            return new RecipeIngredientList()
            {
                Id = entity.Id,
                RecipeId = RecipeId,
                Translations = translations,
                Ingredients = entity.Ingredients?.Select(Map).ToList(),
                OrderIndex = entity.OrderIndex
            };
        }
        public static RecipeIngredientListTranslation MapTranslation(RecipeIngredientListView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeIngredientListTranslation(LanguageId)
            {
                RecipeIngredientListId = entity.Id,
                Title = entity.Title,
            };
        }
        public static RecipeIngredient Map(RecipeIngredientView entity)
        {
            if (entity == null)
            {
                return null;
            }

            var translation = MapTranslation(entity);
            List<RecipeIngredientTranslation> translations = new List<RecipeIngredientTranslation>();
            translations.Add(translation);

            return new RecipeIngredient()
            {
                Id = entity.Id,
                TempId = entity.TempId,
                RecipeIngredientListId = entity.RecipeIngredientListId,
                Amount = entity.Amount,
                //AmountPerServingDefaultNutrientMeasurement = entity.AmountPerServingDefaultNutrientMeasurement,
                MeasurementId = entity.MeasurementId,
                IngredientId = entity.IngredientId,
                Translations = translations,
                OrderIndex = entity.OrderIndex
            };
        }
        public static RecipeIngredientTranslation MapTranslation(RecipeIngredientView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeIngredientTranslation(LanguageId)
            {
                RecipeIngredientId = entity.Id,
                Description = entity.Description,
            };
        }
        public static Tag Map(TagView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Tag()
            {
                Id = entity.Id,
            };
        }
        public static RecipeRating Map(RecipeRatingView entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeRating()
            {
                RecipeId = entity.RecipeId,
                Rating = entity.Rating.GetValueOrDefault(),
            };
        }
    }
}
