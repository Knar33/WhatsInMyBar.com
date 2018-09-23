using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsInMyBar.Models
{
    public class IngredientCategory
    {
        public int IngredientID { get; set; }
        public int CategoryID { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}