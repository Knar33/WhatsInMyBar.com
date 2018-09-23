using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsInMyBar.Models
{
    public class RecipeIngredient
    {
        public int IngredientID { get; set; }
        public int RecipeID { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}