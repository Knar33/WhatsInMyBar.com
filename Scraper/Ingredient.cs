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
            Scraped = false;
        }

        public Ingredient(int id, string name)
        {
            ingredient_id = id;
            this.name = name;
            Scraped = false;
        }

        public int ingredient_id { get; set; }
        public int? Category { get; set; }
        public string name { get; set; }
        public bool Scraped { get; set; }
    }
}
