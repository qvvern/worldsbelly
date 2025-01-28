using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldsBelly.DataAccess.Entities
{

    public class UserTag : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int RecipeReferencesAmount { get; set; }
        public int? TagTypeId { get; set; }
        public TagType TagType { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

    }
    public class UserTagToRecipe
    {
        public int RecipeId { get; set; }
        public int UserTagId { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual UserTag UserTag { get; set; }

    }
}
