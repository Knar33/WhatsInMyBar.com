using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}