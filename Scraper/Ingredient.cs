using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyBar.Extensions;

namespace Scraper
{
    public class Ingredient
    {
        public Ingredient()
        {
            Scraped = false;
        }

        public Ingredient(int id, string name)
        {
            ingredient_id = id;
            this.name = name;
            Scraped = false;
        }

        public int ingredient_id { get; set; }
        public int? Category { get; set; }
        public string name { get; set; }
        public bool Scraped { get; set; }

        public void Insert()
        {
            try
            {
                using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    dbConnection.Open();
                    using (var cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "CreateIngredients";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                        cmd.AddParameter("@IngredientID", ingredient_id);
                        cmd.AddParameter("@Name", name);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InsertRecipeIngredient(int RecipeID)
        {
            try
            {
                using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    dbConnection.Open();
                    using (var cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "CreateRecipeIngredients";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                        cmd.AddParameter("@IngredientID", ingredient_id);
                        cmd.AddParameter("@RecipeID", RecipeID);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public static List<Ingredient> GetIngredientsFromDatabase()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
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
                                ingredients.Add(new Ingredient
                                {
                                    ingredient_id = reader.GetValueOrDefault<int>("IngredientID"),
                                    name = reader.GetValueOrDefault<string>("Name"),
                                    Scraped = false
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

            return ingredients;
        }

        public static List<Ingredient> GetHardCodedIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            ingredients.Add(new Ingredient(10772, "Absinthe"));
            ingredients.Add(new Ingredient(1860, "Brandy"));
            ingredients.Add(new Ingredient(6630, "Cointreau"));
            ingredients.Add(new Ingredient(59, "Grand Marnier"));
            ingredients.Add(new Ingredient(10060, "Orange juice"));
            ingredients.Add(new Ingredient(227, "St-Germain "));
            ingredients.Add(new Ingredient(2043, "Agave nectar"));
            ingredients.Add(new Ingredient(463, "Campari"));
            ingredients.Add(new Ingredient(1888, "Cranberry juice"));
            ingredients.Add(new Ingredient(1922, "Grapefruit juice"));
            ingredients.Add(new Ingredient(2010, "Pineapple juice"));
            ingredients.Add(new Ingredient(1872, "Sweet vermouth"));
            ingredients.Add(new Ingredient(2198, "Aperol"));
            ingredients.Add(new Ingredient(1910, "Champagne"));
            ingredients.Add(new Ingredient(10307, "Dry vermouth"));
            ingredients.Add(new Ingredient(2049, "Green Chartreuse"));
            ingredients.Add(new Ingredient(7433, "Pisco"));
            ingredients.Add(new Ingredient(10276, "Tequila"));
            ingredients.Add(new Ingredient(268, "Bénédictine"));
            ingredients.Add(new Ingredient(1947, "Club soda"));
            ingredients.Add(new Ingredient(1867, "Egg white"));
            ingredients.Add(new Ingredient(468, "Grenadine"));
            ingredients.Add(new Ingredient(10516, "Rum"));
            ingredients.Add(new Ingredient(1892, "Triple sec"));
            ingredients.Add(new Ingredient(1937, "Coffee liqueur "));
            ingredients.Add(new Ingredient(10096, "Gin"));
            ingredients.Add(new Ingredient(1865, "Lemon juice"));
            ingredients.Add(new Ingredient(10334, "Scotch"));
            ingredients.Add(new Ingredient(1966, "Vodka"));
            ingredients.Add(new Ingredient(6568, "Bourbon"));
            ingredients.Add(new Ingredient(696, "Cognac"));
            ingredients.Add(new Ingredient(1915, "Ginger beer"));
            ingredients.Add(new Ingredient(1893, "Lime juice"));
            ingredients.Add(new Ingredient(19, "Simple syrup"));
            ingredients.Add(new Ingredient(1935, "Rye whiskey"));

            foreach (Ingredient ingredient in ingredients)
            {
                ingredient.Insert();
            }

            return ingredients;
        }
    }
}
