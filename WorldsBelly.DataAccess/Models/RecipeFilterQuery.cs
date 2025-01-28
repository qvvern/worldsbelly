using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.DataAccess.Models
{
    public class RecipeFilterQuery
    {
        private const int MaxPageSize = 1000;

        public int Page { get; set; } = 1;

        private int _pageSize = 50;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value < MaxPageSize) ? value : MaxPageSize;
        }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public string Search { get; set; }
        public List<int> Tags { get; set; }
        //public List<int> UTags { get; set; }
        public List<int> BestServed { get; set; }
        public List<int> Difficulty { get; set; }
        public List<int> Includeingredients { get; set; }
        public List<int> Excludeingredients { get; set; }
        public List<RecipeFilterQueryNutrient> Nutrients { get; set; }
        public List<int> Includecuisines { get; set; }
        public List<int> Excludecuisines { get; set; }
        public RecipeFilterQueryTime Time { get; set; }
        public int? Fromrating { get; set; }
        public int? Torating { get; set; }
        public int? Fromsweet { get; set; }
        public int? Tosweet { get; set; }
        public int? Fromsour { get; set; }
        public int? Tosour { get; set; }
        public int? Fromsalty { get; set; }
        public int? Tosalty { get; set; }
        public int? Frombitter { get; set; }
        public int? Tobitter { get; set; }
        public int? Fromspices { get; set; }
        public int? Tospices { get; set; }
        public int? Fromflavor { get; set; }
        public int? Toflavor { get; set; }
        public int? Servings { get; set; }
    }
    public class RecipeRelatedToUserFilterQuery
    {
        private const int MaxPageSize = 1000;

        public int Page { get; set; } = 1;

        private int _pageSize = 50;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value < MaxPageSize) ? value : MaxPageSize;
        }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
        public int? CreatedBy { get; set; }
        public int? LikedBy { get; set; }
        public int? ReviewedBy { get; set; }
    }
    public class RecipeFilterQueryNutrient
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public Option? Option { get; set; }
    }
    public class RecipeFilterQueryTime
    {
        public double Amount { get; set; }
        public Option? Option { get; set; }
    }
    public enum Option
    {
        LessThan = 0,  // 0
        MoreThan = 1,  // 1
    }
}
