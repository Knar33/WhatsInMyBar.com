using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class Ingredient
    {
        public Ingredient()
        {
            IsNew = false;
        }

        public string ingredient_id { get; set; }
        public string name { get; set; }
        public bool IsNew { get; set; }
    }
}
