using System.Linq;
using WorldsBelly.API.Models;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Migrations;

namespace WorldsBelly.API.Utilities.Mappers
{
    public static class ResponseMapper
    {
        public static UserView Map(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserView()
            {
                Id = user.Id,
                Username = user.Username,
                Image = user.Image
            };
        }
        public static CountryView Map(Country entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CountryView()
            {
                Id = entity.Id,
                Name = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Name : entity.EnglishName,
                Code = entity.Code
            };
        }
        public static LanguageView Map(Language entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new LanguageView()
            {
                Id = entity.Id,
                Code = entity.Code,
                EnglishName = entity.EnglishName,
                NativeName = entity.NativeName,
            };
        }
        public static TagSelectableView Map(TagSelectable entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TagSelectableView()
            {
                Id = entity.Id,
                TagId = entity.TagId,
                Tag = Map(entity.Tag),
                Icon = entity.Icon,
                OrderIndex = entity.OrderIndex,
                ExcludedTags = entity.ExcludedTags,
                DontAddTag = entity.DontAddTag,
            };
        }
        public static TagSelectableCategoryView Map(TagSelectableCategory entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TagSelectableCategoryView()
            {
                Id = entity.Id,
                ExcludedTags = entity.ExcludedTags,
                //TitleId = entity.TitleId,
                Title = Map(entity.Title).Text,
                //TextId = entity.TextId,
                Text = Map(entity.Text).Text,
                TagSelectables = entity.TagSelectables?.Select(Map).ToList(),
            };
        }
        public static TagSelectableChoiceOrderView Map(TagSelectableChoiceOrder entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TagSelectableChoiceOrderView()
            {
                Id = entity.Id,
                TagId = entity.TagId,
                RelatedChoiceId = entity.RelatedChoiceId,
                Tag = Map(entity.Tag),
                OrderIndex = entity.OrderIndex,
                TagSelectableCategoryId = entity.TagSelectableCategoryId,
                TagSelectableCategory = Map(entity.TagSelectableCategory),
            };
        }
        public static TranslationView Map(EnglishTranslation response)
        {
            if (response == null)
            {
                return null;
            }

            return new TranslationView()
            {
                Id = response.Id,
                Text = response.Translations.FirstOrDefault().Text,
                TextPlural = response.Translations.FirstOrDefault().TextPlural,
                LanguageId = response.Translations.FirstOrDefault().LanguageId,
            };
        }
        public static RecipeView Map(Recipe entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeView()
            {
                RecipeId = entity.Id,
                Id = entity.Translations.FirstOrDefault().Id,
                OriginCountryId = entity.OriginCountryId,
                CreatedAt = entity.CreatedAt,
                CreatedByUser = Map(entity.CreatedByUser),
                TranslatedByUser = Map(entity.Translations.FirstOrDefault().CreatedByUser),

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
                ConsumerId = entity.ConsumerId,
                AgeGroupId = entity.AgeGroupId,
                TotalComments = entity.CalculatedTotalComments,
                Nutrients = entity.CalculatedNutrients?.Select(Map).ToList(),
                ImageUrl = entity.ImageUrl,
                VideoUrl = entity.VideoUrl,

                Sweet = entity.Sweet,
                Bitter = entity.Bitter,
                Flavor = entity.Flavor,
                Rating = entity.CalculatedRating,
                Salty = entity.Salty,
                Sour = entity.Sour,
                Spices = entity.Spices,
                Totaliked = entity.CalculatedTotaliked,
                TotalViews = entity.TotalViews,
                TotalRatings = entity.CalculatedTotalRatings,

                // translations
                Title = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Title : null,
                Summary = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Summary : null,
                LanguageId = entity.Translations.FirstOrDefault().LanguageId,
                IsPublished = entity.Translations.FirstOrDefault().IsPublished,
                PublishedAt = entity.Translations.FirstOrDefault().PublishedAt,
                IsApproved = entity.Translations.FirstOrDefault().IsApproved,
                ApprovedAt = entity.Translations.FirstOrDefault().ApprovedAt,
            };
        }
        public static RecipeNutrientView Map(RecipeCalculatedNutrient entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeNutrientView()
            {
                Id = entity.NutrientId,
                Amount = entity.Amount
            };
        }
        public static NutrientView Map(Nutrient entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new NutrientView()
            {
                Id = entity.Id,
                Name = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Name : entity.EnglishName,
                NamePlural = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().NamePlural : entity.EnglishNamePlural,
                Description = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Description : entity.EnglishDescription,
                MeasurementId = entity.DefaultMeasurementId
            };
        }
        public static RecipeStepView Map(RecipeStep entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeStepView()
            {
                Id = entity.Id,
                ImageUrl = entity.ImageUrl,
                VideoUrl = entity.VideoUrl,
                OrderIndex = entity.OrderIndex,
                Title = entity.Translations.FirstOrDefault().Title,
                Content = entity.Translations.FirstOrDefault().Content,
            };
        }
        public static RecipeIngredientListView Map(RecipeIngredientList entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeIngredientListView()
            {
                Id = entity.Id,
                RecipeId = entity.RecipeId,
                OrderIndex = entity.OrderIndex,
                Title = entity.Translations.FirstOrDefault()?.Title,
                Ingredients = entity.Ingredients?.Select(Map).ToList(),
            };
        }
        public static RecipeIngredientView Map(RecipeIngredient entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeIngredientView()
            {
                Id = entity.Id,
                RecipeIngredientListId = entity.RecipeIngredientListId,
                Amount = entity.Amount,
                //AmountPerServingDefaultNutrientMeasurement = entity.AmountPerServingDefaultNutrientMeasurement,
                MeasurementId = entity.MeasurementId,
                IngredientId = entity.IngredientId,
                Description = entity.Translations.FirstOrDefault()?.Description,
                Name = entity.Ingredient.Translations.FirstOrDefault()?.Name,
                NamePlural = entity.Ingredient.Translations.FirstOrDefault()?.NamePlural,
                OrderIndex = entity.OrderIndex
            };
        }
        public static TagView Map(Tag entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TagView()
            {
                Id = entity.Id,
                Name = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Name : entity.EnglishName,
                NamePlural = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().NamePlural : entity.EnglishNamePlural,
                Description = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Description : entity.EnglishDescription,
                Hidden = entity.Hidden,
                HideInCard = entity.HideInCard
            };
        }
        public static IngredientView Map(Ingredient entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new IngredientView()
            {
                Id = entity.Id,
                DefaultMeasurementId = entity.DefaultMeasurementId.GetValueOrDefault(),
                OneMilliliterInGram = entity.OneMilliliterInGram,
                OneCentimeterInGram = entity.OneCentimeterInGram,
                OneCentimeterInMilliliter = entity.OneCentimeterInMilliliter,
                OnePieceInGram = entity.OnePieceInGram,
                OnePieceInMilliliter = entity.OnePieceInMilliliter,
                OnePieceInCentimeter = entity.OnePieceInCentimeter,
                Name = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().Name : entity.EnglishName,
                NamePlural = entity.Translations.FirstOrDefault() != null ? entity.Translations.FirstOrDefault().NamePlural : entity.EnglishNamePlural,
                Description = entity.Translations.FirstOrDefault().Description,
            };
        }
        public static IngredientView Map(IngredientTranslation entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new IngredientView()
            {
                Id = entity.IngredientId,
                DefaultMeasurementId = entity.Ingredient.DefaultMeasurementId.GetValueOrDefault(),
                OneMilliliterInGram = entity.Ingredient.OneMilliliterInGram,
                OneCentimeterInGram = entity.Ingredient.OneCentimeterInGram,
                OneCentimeterInMilliliter = entity.Ingredient.OneCentimeterInMilliliter,
                OnePieceInGram = entity.Ingredient.OnePieceInGram,
                OnePieceInMilliliter = entity.Ingredient.OnePieceInMilliliter,
                OnePieceInCentimeter = entity.Ingredient.OnePieceInCentimeter,
                Name = entity.Name,
                NamePlural = entity.NamePlural,
                Description = entity.Description
            };
        }
        public static MeasurementView Map(Measurement entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new MeasurementView()
            {
                Id = entity.Id,
                TypeId = entity.TypeId,
                Name = entity.Translations.Count() > 0 ? entity.Translations.FirstOrDefault().Name : entity.EnglishName,
                NamePlural = entity.Translations.Count() > 0 ? entity.Translations.FirstOrDefault().NamePlural : entity.EnglishNamePlural,
                Description = entity.Translations.Count() > 0 ? entity.Translations.FirstOrDefault().Description : null,
                ConversionAmount = entity.ConversionAmount,
                Unit = entity.Translations.Count() > 0 ? entity.Translations.FirstOrDefault().Unit : entity.Unit,
            };
        }
        public static RecipeBestServedView Map(RecipeBestServed entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeBestServedView()
            {
                Id = entity.Id,
                TagId = entity.TagId,
                Label = entity.Tag.Translations.Count() > 0 ? entity.Tag.Translations.FirstOrDefault().Name : entity.Tag.EnglishName,
            };
        }
        public static RecipeConsumerView Map(RecipeConsumer entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeConsumerView()
            {
                Id = entity.Id,
                TagId = entity.TagId,
                Label = entity.Tag.Translations.Count() > 0 ? entity.Tag.Translations.FirstOrDefault().Name : entity.Tag.EnglishName,
            };
        }
        public static RecipeAgeGroupView Map(RecipeAgeGroup entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeAgeGroupView()
            {
                Id = entity.Id,
                TagId = entity.TagId,
                Label = entity.Tag.Translations.Count() > 0 ? entity.Tag.Translations.FirstOrDefault().Name : entity.Tag.EnglishName,
            };
        }
        public static RecipeDifficultyView Map(RecipeDifficulty entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeDifficultyView()
            {
                Id = entity.Id,
                TagId = entity.TagId,
                Label = entity.Tag.Translations.Count() > 0 ? entity.Tag.Translations.FirstOrDefault().Name : entity.Tag.EnglishName,
            };
        }
        public static RecipeRatingView Map(RecipeRating entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new RecipeRatingView()
            {
                RecipeId = entity.RecipeId,
                Rating = entity.Rating,
            };
        }
        public static NotificationView Map(Notification entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new NotificationView()
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                ReadAt = entity.ReadAt,
                IsRead = entity.IsRead,
                Title = entity.Template.Translations.FirstOrDefault().Title,
                Content = entity.Template.Translations.FirstOrDefault().Content,
                Sender = entity.Sender.Username,
                Action = entity.ActionUrl
            };
        }
    }
}
