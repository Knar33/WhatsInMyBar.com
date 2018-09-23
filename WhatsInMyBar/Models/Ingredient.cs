using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsInMyBar.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public List<Category> Categories { get; set; }
    }
}