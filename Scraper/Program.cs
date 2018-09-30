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
using System.Text.RegularExpressions;

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

            List<Recipe> recipes = Recipe.GetRecipesFromDatabase();
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
                var response = GetSpecificRecipe(recipe.id);
                if (response.Data.ping_status == "open")
                {
                    recipesMissing++;
                    Console.WriteLine(string.Format("{0} - {1}", response.Data.title.rendered, response.Data.id));
                }
            }
            Console.WriteLine("================================================ End of missing recipes ================================================ ");

            if (recipesMissing > 0)
            {
                List<Ingredient> ingredients = Ingredient.GetIngredientsFromDatabase();
                if (ingredients.Count() == 0)
                {
                    ingredients = Ingredient.GetHardCodedIngredients();
                }

                bool ingredientsLeft = true;
                
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
                                        recipe.Insert();
                                        recipe.DownloadThumbnail();
                                        foreach (Ingredient newIngredient in recipe.ingredients)
                                        {
                                            if (!ingredients.Any(x => x.ingredient_id == newIngredient.ingredient_id) && !newIngredients.Any(x => x.ingredient_id == newIngredient.ingredient_id))
                                            {
                                                newIngredients.Add(newIngredient);
                                                newIngredient.Insert();
                                            }
                                            ingredient.InsertRecipeIngredient(recipe.id);
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
            Regex alphaNumeric = new Regex("[^a-zA-Z0-9]");
            List<Ingredient> ingredients = Ingredient.GetIngredientsFromDatabase();
            Dictionary<string, int> categories = new Dictionary<string, int>();

            foreach (Ingredient ingredient in ingredients)
            {
                string[] words = ingredient.name.Replace('-', ' ').Split(' ').Where(x => x.Length > 2).Select(x => alphaNumeric.Replace(x, "").ToLower().StripDiacritics()).GroupBy(x => x).Select(x => x.First()).ToArray();
                foreach (string word in words) 
                {
                    List<string> matchingKeys = categories.Where(x => x.Key == word).Select(x => x.Key).ToList();
                    if (matchingKeys.Count() > 0)
                    {
                        foreach (string match in matchingKeys)
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
                Console.WriteLine("{0},{1}", category.Key, category.Value);
            }
        }

        public static IRestResponse<List<ProtoRecipe>> GetProtoRecipes(int page)
        {
            string url = string.Format("{0}?page={1}", ConfigurationManager.AppSettings["APIURL2"], page);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            return client.Execute<List<ProtoRecipe>>(request);
        }

        public static IRestResponse<SpecificRecipe> GetSpecificRecipe(int recipeID)
        {
            string url = string.Format("{0}/{1}", ConfigurationManager.AppSettings["APIURL2"], recipeID);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            return client.Execute<SpecificRecipe>(request);
        }
    }
}
