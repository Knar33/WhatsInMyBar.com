using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using WhatsInMyBar.Extensions;
using RestSharp;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScrapeRecipes();
            CategorizeIngredients();
        }

        public static void ScrapeRecipes()
        {
            //This whole thing is way too procedural, should be broken out into methods to be cleaner
            //It's just a quick and dirty scraper to get the job done

            var response22 = GetSpecifitRecipe(197);

            List<ProtoRecipe> protoRecipes = new List<ProtoRecipe>();
            var res = GetProtoRecipes(1);
            int pageCount = 1;
            if (res.IsSuccessful)
            {
                pageCount = Convert.ToInt32(res.Headers.FirstOrDefault(x => x.Name == "X-WP-TotalPages").Value);
                for (int i = 1; i <= pageCount; i++)
                {
                    var response = GetProtoRecipes(i);
                    if (response.IsSuccessful)
                    {
                        foreach (ProtoRecipe protoRecipe in response.Data)
                        {
                            protoRecipes.Add(protoRecipe);
                        }
                    }
                }
            }

            List<Recipe> recipes = GetRecipesFromDatabase();
            List<ProtoRecipe> missingRecipes = new List<ProtoRecipe>();
            foreach (ProtoRecipe protoRecipe in protoRecipes)
            {
                if (!recipes.Any(x => x.id == protoRecipe.id))
                {
                    missingRecipes.Add(protoRecipe);
                }
            }

            Console.WriteLine("================================================ missing recipes ================================================ ");
            int recipesMissing = 0;
            foreach (ProtoRecipe recipe in missingRecipes)
            {
                var response = GetSpecifitRecipe(recipe.id);
                if (response.Data.ping_status == "open")
                {
                    recipesMissing++;
                    Console.WriteLine(string.Format("{0} - {1}", response.Data.title.rendered, response.Data.id));
                }
            }
            Console.WriteLine("================================================  End of missing recipes ================================================ ");

            if (recipesMissing > 0)
            {
                List<Ingredient> ingredients = GetIngredientsFromDatabase();
                if (ingredients.Count() == 0)
                {
                    ingredients = GetHardCodedIngredients();
                }

                bool ingredientsLeft = true;

                //TODO: Keep track of the RecipesMissing, and once all of the missing recipes are scraped, end the process
                while (ingredientsLeft)
                {
                    List<Ingredient> newIngredients = new List<Ingredient>();
                    foreach (Ingredient ingredient in ingredients.Where(x => !x.Scraped))
                    {
                        Console.WriteLine(ingredient.name);
                        GetRecipesResponse response = (new GetRecipesRequest(ingredient.name, 1)).Send();

                        decimal pages = Math.Ceiling(response.count / 24m);
                        for (int i = 1; i <= pages; i++)
                        {
                            GetRecipesResponse innerResponse = (new GetRecipesRequest(ingredient.name, i)).Send();
                            if (innerResponse.recipes?.Count() > 0)
                            {
                                foreach (Recipe recipe in innerResponse.recipes)
                                {
                                    if (!recipes.Any(x => x.id == recipe.id))
                                    {
                                        recipes.Add(recipe);
                                        InsertRecipe(recipe);
                                        DownloadThumbnail(recipe);
                                        foreach (Ingredient newIngredient in recipe.ingredients)
                                        {
                                            if (!ingredients.Any(x => x.ingredient_id == newIngredient.ingredient_id) && !newIngredients.Any(x => x.ingredient_id == newIngredient.ingredient_id))
                                            {
                                                newIngredients.Add(newIngredient);
                                                InsertIngredient(newIngredient);
                                            }
                                            InsertRecipeIngredient(newIngredient, recipe.id);
                                        }
                                    }
                                }
                            }
                        }
                        ingredient.Scraped = true;
                    }

                    foreach (Ingredient newIngredient in newIngredients)
                    {
                        ingredients.Add(newIngredient);
                    }

                    ingredientsLeft = ingredients.Any(x => !x.Scraped);
                }
            }
            else
            {
                Console.WriteLine("All Recipes are in the Database");
            }

            Console.ReadLine();
        }

        public static void CategorizeIngredients()
        {
            List<Ingredient> ingredients = GetIngredientsFromDatabase();
            Dictionary<string, int> categories = new Dictionary<string, int>();

            foreach (Ingredient ingredient in ingredients)
            {
                string[] words = ingredient.name.Split(' ').Where(x => x.Length > 2).GroupBy(x => x).Select(x => x.First()).ToArray();
                foreach (string word in words)
                {
                    var matchingKeys = categories.Where(x => x.Key.Contains(word) || word.Contains(x.Key) || x.Key.ToLower().Contains(word.ToLower()) || word.ToLower().Contains(x.Key.ToLower())).Select(x => x.Key).ToList();
                    if (matchingKeys.Count() > 0)
                    {
                        foreach (var match in matchingKeys)
                        {
                            categories[match]++;
                        }
                    }
                    else
                    {
                        categories.Add(word, 1);
                    }
                }
            }

            foreach (KeyValuePair<string, int> category in categories.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("{0},{1}", category.Key.Replace(",", ""), category.Value);
            }
        }

        public static void DownloadThumbnail(Recipe recipe)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string thumbnailURI = string.Format("http://{0}", recipe.thumbnail.TrimStart('/'));
                    byte[] data = webClient.DownloadData(thumbnailURI);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var yourImage = Image.FromStream(mem))
                        {
                            string imageURL = string.Format("{0}{1}.jpg", ConfigurationManager.AppSettings["LocalImagePath"], recipe.id);
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

        public static void InsertRecipe(Recipe recipe)
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

                        cmd.AddParameter("@RecipeID", recipe.id);
                        cmd.AddParameter("@Name", recipe.title.rendered);
                        cmd.AddParameter("@Link", recipe.link);
                        cmd.AddParameter("@Thumbnail", recipe.thumbnail);
                        cmd.AddParameter("@Description", recipe.description);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void InsertIngredient(Ingredient ingredient)
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

                        cmd.AddParameter("@IngredientID", ingredient.ingredient_id);
                        cmd.AddParameter("@Name", ingredient.name);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void InsertRecipeIngredient(Ingredient ingredient, int RecipeID)
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

                        cmd.AddParameter("@IngredientID", ingredient.ingredient_id);
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

        public static IRestResponse<List<ProtoRecipe>> GetProtoRecipes(int page)
        {
            string url = string.Format("{0}?page={1}", ConfigurationManager.AppSettings["APIURL2"], page);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            return client.Execute<List<ProtoRecipe>>(request);
        }

        public static IRestResponse<SpecificRecipe> GetSpecifitRecipe(int recipeID)
        {
            string url = string.Format("{0}/{1}", ConfigurationManager.AppSettings["APIURL2"], recipeID);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            return client.Execute<SpecificRecipe>(request);
        }

        public static List<Ingredient> GetHardCodedIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            ingredients.Add(new Ingredient(10772, "Absinthe"));
            InsertIngredient(new Ingredient(10772, "Absinthe"));
            ingredients.Add(new Ingredient(1860, "Brandy"));
            InsertIngredient(new Ingredient(1860, "Brandy"));
            ingredients.Add(new Ingredient(6630, "Cointreau"));
            InsertIngredient(new Ingredient(6630, "Cointreau"));
            ingredients.Add(new Ingredient(59, "Grand Marnier"));
            InsertIngredient(new Ingredient(59, "Grand Marnier"));
            ingredients.Add(new Ingredient(10060, "Orange juice"));
            InsertIngredient(new Ingredient(10060, "Orange juice"));
            ingredients.Add(new Ingredient(227, "St-Germain "));
            InsertIngredient(new Ingredient(227, "St-Germain "));
            ingredients.Add(new Ingredient(2043, "Agave nectar"));
            InsertIngredient(new Ingredient(2043, "Agave nectar"));
            ingredients.Add(new Ingredient(463, "Campari"));
            InsertIngredient(new Ingredient(463, "Campari"));
            ingredients.Add(new Ingredient(1888, "Cranberry juice"));
            InsertIngredient(new Ingredient(1888, "Cranberry juice"));
            ingredients.Add(new Ingredient(1922, "Grapefruit juice"));
            InsertIngredient(new Ingredient(1922, "Grapefruit juice"));
            ingredients.Add(new Ingredient(2010, "Pineapple juice"));
            InsertIngredient(new Ingredient(2010, "Pineapple juice"));
            ingredients.Add(new Ingredient(1872, "Sweet vermouth"));
            InsertIngredient(new Ingredient(1872, "Sweet vermouth"));
            ingredients.Add(new Ingredient(2198, "Aperol"));
            InsertIngredient(new Ingredient(2198, "Aperol"));
            ingredients.Add(new Ingredient(1910, "Champagne"));
            InsertIngredient(new Ingredient(1910, "Champagne"));
            ingredients.Add(new Ingredient(10307, "Dry vermouth"));
            InsertIngredient(new Ingredient(10307, "Dry vermouth"));
            ingredients.Add(new Ingredient(2049, "Green Chartreuse"));
            InsertIngredient(new Ingredient(2049, "Green Chartreuse"));
            ingredients.Add(new Ingredient(7433, "Pisco"));
            InsertIngredient(new Ingredient(7433, "Pisco"));
            ingredients.Add(new Ingredient(10276, "Tequila"));
            InsertIngredient(new Ingredient(10276, "Tequila"));
            ingredients.Add(new Ingredient(268, "Bénédictine"));
            InsertIngredient(new Ingredient(268, "Bénédictine"));
            ingredients.Add(new Ingredient(1947, "Club soda"));
            InsertIngredient(new Ingredient(1947, "Club soda"));
            ingredients.Add(new Ingredient(1867, "Egg white"));
            InsertIngredient(new Ingredient(1867, "Egg white"));
            ingredients.Add(new Ingredient(468, "Grenadine"));
            InsertIngredient(new Ingredient(468, "Grenadine"));
            ingredients.Add(new Ingredient(10516, "Rum"));
            InsertIngredient(new Ingredient(10516, "Rum"));
            ingredients.Add(new Ingredient(1892, "Triple sec"));
            InsertIngredient(new Ingredient(1892, "Triple sec"));
            ingredients.Add(new Ingredient(1937, "Coffee liqueur "));
            InsertIngredient(new Ingredient(1937, "Coffee liqueur "));
            ingredients.Add(new Ingredient(10096, "Gin"));
            InsertIngredient(new Ingredient(10096, "Gin"));
            ingredients.Add(new Ingredient(1865, "Lemon juice"));
            InsertIngredient(new Ingredient(1865, "Lemon juice"));
            ingredients.Add(new Ingredient(10334, "Scotch"));
            InsertIngredient(new Ingredient(10334, "Scotch"));
            ingredients.Add(new Ingredient(1966, "Vodka"));
            InsertIngredient(new Ingredient(1966, "Vodka"));
            ingredients.Add(new Ingredient(6568, "Bourbon"));
            InsertIngredient(new Ingredient(6568, "Bourbon"));
            ingredients.Add(new Ingredient(696, "Cognac"));
            InsertIngredient(new Ingredient(696, "Cognac"));
            ingredients.Add(new Ingredient(1915, "Ginger beer"));
            InsertIngredient(new Ingredient(1915, "Ginger beer"));
            ingredients.Add(new Ingredient(1893, "Lime juice"));
            InsertIngredient(new Ingredient(1893, "Lime juice"));
            ingredients.Add(new Ingredient(19, "Simple syrup"));
            InsertIngredient(new Ingredient(19, "Simple syrup"));
            ingredients.Add(new Ingredient(1935, "Rye whiskey"));
            InsertIngredient(new Ingredient(1935, "Rye whiskey"));
            return ingredients;
        }
    }
}
