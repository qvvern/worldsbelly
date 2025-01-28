using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WorldsBelly.DataAccess.Entities.NutrientDb;

namespace WorldsBelly.DataAccess.Contexts
{
    public class NutrientDbContext : DbContext
    {
        public DbSet<Food> food { get; set; }
        public DbSet<Nutrient> nutrient { get; set; }
        public DbSet<FoodNutrient> food_nutrient { get; set; }
        public DbSet<FoodCalorieConversionFactor> food_calorie_conversion_factor { get; set; }
        public DbSet<FoodNutrientConversionFactor> food_nutrient_conversion_factor { get; set; }
        public DbSet<FoodProteinConversionFactor> food_protein_conversion_factor { get; set; }

        public NutrientDbContext(DbContextOptions options) : base(options)
        {
        }

        public void RawQuery(string query)
        {
            Database.ExecuteSqlRaw(query);
        }
    }
}
