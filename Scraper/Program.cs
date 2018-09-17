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
            GetRecipesRequest request = new GetRecipesRequest("Vodka");
            var response = request.Send();

            //Load existing ingredient and recipe list from the Database. If no ingredients are found in the database (first scrape), use hard coded list. 

            //Iterate over list of ingredients and do a GetRecipeRequest.
                //Check if the the recipe is already in the Recipes collection. If not, add it.
                    //Check every ingredient in the added recipe to see if it's in the Ingredients collection. If not add it.
                //Set the Scraped boolean on the ingredient to True
            //Check to see if there any ingrednents that haven't been Scraped. If there are, repeat until there aren't. Otherwise terminate the program.

            //After all recipes are scraped, do a massive Merge into the database. Merge all Recipes and Ingredients.
        }
    }
}
