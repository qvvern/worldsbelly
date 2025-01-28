using System;
using System.Linq;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Utilities.Extensions
{
    public static class IngredientsQueryExtensions
    {
        public static IQueryable<Ingredient> ApplySorting(this IQueryable<Ingredient> query, SortingOptions sortingOptions)
        {
            if (!string.IsNullOrEmpty(sortingOptions?.SortBy))
                query = query.Sort(sortingOptions);

            return query;
        }

        private static IOrderedQueryable<Ingredient> Sort(this IQueryable<Ingredient> ingredients, SortingOptions sortingOptions)
        {

            if (string.IsNullOrEmpty(sortingOptions.SortBy))
            {
                throw new ApplicationException("Sorting failed. Property name is null or empty");
            }

            return sortingOptions.SortBy switch
            {
                "EnglishName" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.EnglishName) : ingredients.OrderByDescending(_ => _.EnglishName),
                "Id" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.Id) : ingredients.OrderByDescending(_ => _.Id),
                "WikidataId" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.WikidataId) : ingredients.OrderByDescending(_ => _.WikidataId),
                "EnglishNamePlural" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.EnglishNamePlural) : ingredients.OrderByDescending(_ => _.EnglishNamePlural),
                _ => throw new Exception($"Ingredients cannot be sorted by '{sortingOptions.SortBy}'")
            };
        }
        public static IQueryable<Tag> ApplySorting(this IQueryable<Tag> query, SortingOptions sortingOptions)
        {
            if (!string.IsNullOrEmpty(sortingOptions?.SortBy))
                query = query.Sort(sortingOptions);

            return query;
        }
        private static IOrderedQueryable<Tag> Sort(this IQueryable<Tag> ingredients, SortingOptions sortingOptions)
        {

            if (string.IsNullOrEmpty(sortingOptions.SortBy))
            {
                throw new ApplicationException("Sorting failed. Property name is null or empty");
            }

            return sortingOptions.SortBy switch
            {
                "EnglishName" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.EnglishName) : ingredients.OrderByDescending(_ => _.EnglishName),
                "Id" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.Id) : ingredients.OrderByDescending(_ => _.Id),
                "WikidataId" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.WikidataId) : ingredients.OrderByDescending(_ => _.WikidataId),
                "EnglishNamePlural" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.EnglishNamePlural) : ingredients.OrderByDescending(_ => _.EnglishNamePlural),
                _ => throw new Exception($"Tags cannot be sorted by '{sortingOptions.SortBy}'")
            };
        }
        public static IQueryable<EnglishTranslation> ApplySorting(this IQueryable<EnglishTranslation> query, SortingOptions sortingOptions)
        {
            if (!string.IsNullOrEmpty(sortingOptions?.SortBy))
                query = query.Sort(sortingOptions);

            return query;
        }
        private static IOrderedQueryable<EnglishTranslation> Sort(this IQueryable<EnglishTranslation> ingredients, SortingOptions sortingOptions)
        {

            if (string.IsNullOrEmpty(sortingOptions.SortBy))
            {
                throw new ApplicationException("Sorting failed. Property name is null or empty");
            }

            return sortingOptions.SortBy switch
            {
                "Text" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.Text) : ingredients.OrderByDescending(_ => _.Text),
                "Id" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.Id) : ingredients.OrderByDescending(_ => _.Id),
                "TextPlural" => sortingOptions.SortAscending ? ingredients.OrderBy(_ => _.TextPlural) : ingredients.OrderByDescending(_ => _.TextPlural),
                _ => throw new Exception($"Translations cannot be sorted by '{sortingOptions.SortBy}'")
            };
        }

    }
}
