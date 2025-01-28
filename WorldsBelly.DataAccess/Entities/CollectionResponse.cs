using System.Collections.Generic;


namespace WorldsBelly.DataAccess.Models
{
    public class RecipeCollectionResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool ExactMatch { get; set; }
        public ICollection<T> Recipes { get; set; }
    }
}