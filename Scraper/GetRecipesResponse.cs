using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class GetRecipesResponse
    {
        public int count { get; set; }
        public bool success { get; set; }
        public Recipe[] recipes { get; set; }
    }

    public class Recipe
    {
        public string id { get; set; }
        public Title title { get; set; }
        public string link { get; set; }
        public string thumbnail { get; set; }
        public Ingredient[] ingredients { get; set; }
        public Basis[] bases { get; set; }
        public Flavor[] flavors { get; set; }
        public string description { get; set; }
    }

    public class Title
    {
        public string rendered { get; set; }
    }

    public class Ingredient
    {
        public string ingredient_id { get; set; }
        public string name { get; set; }
    }

    public class Basis
    {
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

    public class Flavor
    {
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
