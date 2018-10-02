using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using WhatsInMyBar.Extensions;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RestSharp;

namespace Scraper
{
    public class AdminRecipe
    {
        public int id { get; set; }
        public Title title { get; set; }
        public string link { get; set; }
        public string thumbnail { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public List<Basis> bases { get; set; }
        public List<Flavor> flavors { get; set; }
        public string description { get; set; }

        public static IRestResponse<List<AdminRecipe>> GetAdminRecipes(string ingredient, int page)
        {
            RestClient client = new RestClient(ConfigurationManager.AppSettings["APIURL"]);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"action\"\r\n\r\nload_search_results\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ingredients[0]\n\"\r\n\r\n" + ingredient + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"paged\"\r\n\r\n" + page + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);

            return client.Execute<List<AdminRecipe>>(request);
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

                        cmd.AddParameter("@RecipeID", id);
                        cmd.AddParameter("@Name", title.rendered);
                        cmd.AddParameter("@Link", link);
                        cmd.AddParameter("@Thumbnail", thumbnail);
                        cmd.AddParameter("@Description", description);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DownloadThumbnail()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string thumbnailURI = string.Format("http://{0}", thumbnail.TrimStart('/'));
                    byte[] data = webClient.DownloadData(thumbnailURI);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var yourImage = Image.FromStream(mem))
                        {
                            string imageURL = string.Format("{0}{1}.jpg", ConfigurationManager.AppSettings["LocalImagePath"], id);
                            yourImage.Save(imageURL, ImageFormat.Jpeg);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<AdminRecipe> GetRecipesFromDatabase()
        {
            List<AdminRecipe> recipes = new List<AdminRecipe>();

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
                                recipes.Add(new AdminRecipe
                                {
                                    id = reader.GetValueOrDefault<int>("RecipeID"),
                                    title = new Title { rendered = reader.GetValueOrDefault<string>("Name") },
                                    link = reader.GetValueOrDefault<string>("Link"),
                                    thumbnail = reader.GetValueOrDefault<string>("Thumbnail"),
                                    description = reader.GetValueOrDefault<string>("Description")
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

        public class Title
        {
            public Title()
            {
            }

            public string rendered { get; set; }
        }

        public class Flavor
        {
            public Flavor()
            {
            }

            public int term_id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public int term_group { get; set; }
            public int term_taxonomy_id { get; set; }
            public string taxonomy { get; set; }
            public string description { get; set; }
            public int parent { get; set; }
            public int count { get; set; }
            public string filter { get; set; }
        }

        public class Basis
        {
            public Basis()
            {
            }

            public int term_id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public int term_group { get; set; }
            public int term_taxonomy_id { get; set; }
            public string taxonomy { get; set; }
            public string description { get; set; }
            public int parent { get; set; }
            public int count { get; set; }
            public string filter { get; set; }
        }
    }
}
