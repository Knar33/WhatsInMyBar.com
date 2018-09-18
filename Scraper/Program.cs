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
            GetRecipesRequest request = new GetRecipesRequest("Vodka", 1);
            var response = request.Send();

            //Load existing ingredient and recipe list from the Database. If no ingredients are found in the database (first scrape), use hard coded list. 

            //Iterate over list of ingredients and do a GetRecipeRequest.
                //Calculate how many pages are in the response
                //Check if the each recipe is already in the Recipes collection. If not, add it and set IsNew = true.
                    //Check every ingredient in the added recipe to see if it's in the Ingredients collection. If not add it.
                //Set the Scraped boolean on the ingredient to True
            //Check to see if there any ingrednents that haven't been Scraped. If there are, repeat until there aren't. Otherwise terminate the program.

            //After all recipes are scraped, do a massive Merge into the database. Merge all Recipes and Ingredients.
            //for all Recipes with IsNew, download thumbnail image
        }
    }
}
