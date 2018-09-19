using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Recipe> recipes = GetRecipesFromDatabase();
            List<Ingredient> ingredients = GetIngredientsFromDatabase();
            if (ingredients.Count() == 0)
            {
                ingredients = GetHardCodedIngredients();
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
                        foreach (Recipe recipe in innerResponse.recipes)
                        {
                            if (!recipes.Any(x => x.id == recipe.id))
                            {
                                recipe.IsNew = true;
                                recipes.Add(recipe);
                                //database insert new recipe
                                //download thumbnail image
                                foreach (Ingredient newIngredient in recipe.ingredients)
                                {
                                    if (!ingredients.Any(x => x.ingredient_id == newIngredient.ingredient_id) && !newIngredients.Any(x => x.ingredient_id == newIngredient.ingredient_id))
                                    {
                                        ingredient.IsNew = true;
                                        newIngredients.Add(newIngredient);
                                        //database insert new recipe
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

            Console.ReadLine();
        }
        
        public static List<Ingredient> GetIngredientsFromDatabase()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            return ingredients;
        }

        public static List<Recipe> GetRecipesFromDatabase()
        {
            List<Recipe> recipes = new List<Recipe>();
            return recipes;
        }

        public static List<Ingredient> GetHardCodedIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            ingredients.Add(new Ingredient("10772", "Absinthe"));
            ingredients.Add(new Ingredient("1860", "Brandy"));
            ingredients.Add(new Ingredient("6630", "Cointreau"));
            ingredients.Add(new Ingredient("59", "Grand Marnier"));
            ingredients.Add(new Ingredient("10060", "Orange juice"));
            ingredients.Add(new Ingredient("227", "St-Germain "));
            ingredients.Add(new Ingredient("2043", "Agave nectar"));
            ingredients.Add(new Ingredient("463", "Campari"));
            ingredients.Add(new Ingredient("1888", "Cranberry juice"));
            ingredients.Add(new Ingredient("1922", "Grapefruit juice"));
            ingredients.Add(new Ingredient("2010", "Pineapple juice"));
            ingredients.Add(new Ingredient("1872", "Sweet vermouth"));
            ingredients.Add(new Ingredient("2198", "Aperol"));
            ingredients.Add(new Ingredient("1910", "Champagne"));
            ingredients.Add(new Ingredient("10307", "Dry vermouth"));
            ingredients.Add(new Ingredient("2049", "Green Chartreuse"));
            ingredients.Add(new Ingredient("7433", "Pisco"));
            ingredients.Add(new Ingredient("10276", "Tequila"));
            ingredients.Add(new Ingredient("268", "Bénédictine"));
            ingredients.Add(new Ingredient("1947", "Club soda"));
            ingredients.Add(new Ingredient("1867", "Egg white"));
            ingredients.Add(new Ingredient("468", "Grenadine"));
            ingredients.Add(new Ingredient("10516", "Rum"));
            ingredients.Add(new Ingredient("1892", "Triple sec"));
            ingredients.Add(new Ingredient("1937", "Coffee liqueur "));
            ingredients.Add(new Ingredient("10096", "Gin"));
            ingredients.Add(new Ingredient("1865", "Lemon juice"));
            ingredients.Add(new Ingredient("10334", "Scotch"));
            ingredients.Add(new Ingredient("1966", "Vodka"));
            ingredients.Add(new Ingredient("6568", "Bourbon"));
            ingredients.Add(new Ingredient("696", "Cognac"));
            ingredients.Add(new Ingredient("1915", "Ginger beer"));
            ingredients.Add(new Ingredient("1893", "Lime juice"));
            ingredients.Add(new Ingredient("19", "Simple syrup"));
            ingredients.Add(new Ingredient("1935", "Rye whiskey"));
            return ingredients;
        }
    }
}
