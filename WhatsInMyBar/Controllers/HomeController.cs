using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsInMyBar.Extensions;
using WhatsInMyBar.Models;

namespace WhatsInMyBar.Controllers
{
    public class HomeController : Controller
    {
        public List<Recipe> Recipes { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Category> Categories { get; set; }

        public HomeController()
        {
            Recipes = new List<Recipe>();
            Ingredients = new List<Ingredient>();
            Categories = new List<Category>();
            List<RecipeIngredient> RecipeIngredients = new List<RecipeIngredient>();
            List<IngredientCategory> IngredientCategories = new List<IngredientCategory>();

            using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                dbConnection.Open();
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "GetAllIngredients";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ingredients.Add(new Ingredient
                            {
                                IngredientID = reader.GetValueOrDefault<int>("IngredientID"),
                                Name = reader.GetValueOrDefault<string>("Name")
                            });
                        }
                    }
                }

                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "GetAllRecipes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Recipes.Add(new Recipe
                            {
                                RecipeID = reader.GetValueOrDefault<int>("RecipeID"),
                                Name = reader.GetValueOrDefault<string>("Name"),
                                Link = reader.GetValueOrDefault<string>("Link"),
                                Thumbnail = reader.GetValueOrDefault<string>("Thumbnail"),
                                Description = reader.GetValueOrDefault<string>("Description")
                            });
                        }
                    }
                }

                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "GetAllCategories";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categories.Add(new Category
                            {
                                CategoryID = reader.GetValueOrDefault<int>("CategoryID"),
                                Name = reader.GetValueOrDefault<string>("Name")
                            });
                        }
                    }
                }

                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "GetAllRecipeIngredients";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecipeIngredients.Add(new RecipeIngredient
                            {
                                IngredientID = reader.GetValueOrDefault<int>("IngredientID"),
                                RecipeID = reader.GetValueOrDefault<int>("RecipeID")
                            });
                        }
                    }
                }

                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "GetAllIngredientCategories";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IngredientCategories.Add(new IngredientCategory
                            {
                                IngredientID = reader.GetValueOrDefault<int>("IngredientID"),
                                CategoryID = reader.GetValueOrDefault<int>("CategoryID")
                            });
                        }
                    }
                }
            }

            foreach (Recipe recipe in Recipes)
            {
                recipe.Ingredients = new List<Ingredient>();
                List<RecipeIngredient> recipeIngredients = RecipeIngredients.Where(x => x.RecipeID == recipe.RecipeID).ToList();
                foreach (RecipeIngredient recipeIngredient in recipeIngredients)
                {
                    recipe.Ingredients.Add(Ingredients.FirstOrDefault(x => x.IngredientID == recipeIngredient.IngredientID));
                }
            }

            foreach (Ingredient ingredient in Ingredients)
            {
                ingredient.Categories = new List<Category>();
                List<IngredientCategory> ingredientCategories = IngredientCategories.Where(x => x.IngredientID == ingredient.IngredientID).ToList();
                foreach (IngredientCategory ingredientCategory in ingredientCategories)
                {
                    ingredient.Categories.Add(Categories.FirstOrDefault(x => x.CategoryID == ingredientCategory.CategoryID));
                }
            }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}