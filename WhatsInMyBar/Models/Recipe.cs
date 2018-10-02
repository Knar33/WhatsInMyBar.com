using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WhatsInMyBar.Extensions;

namespace WhatsInMyBar.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public static List<Recipe> GetRecipesFromDatabase()
        {
            List<Recipe> recipes = new List<Recipe>();

            try
            {
                using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    dbConnection.Open();
                    using (var cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "GetAllRecipes";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                recipes.Add(new Recipe
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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return recipes;
        }

        public void Insert()
        {
            try
            {
                using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    dbConnection.Open();
                    using (var cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "CreateRecipes";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                        cmd.AddParameter("@RecipeID", RecipeID);
                        cmd.AddParameter("@Name", Name);
                        cmd.AddParameter("@Link", Link);
                        cmd.AddParameter("@Thumbnail", Thumbnail);
                        cmd.AddParameter("@Description", Description);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}