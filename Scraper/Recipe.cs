using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class Recipe
    {
        public Recipe()
        {
            IsNew = false;
        }

        public string id { get; set; }
        public Title title { get; set; }
        public string link { get; set; }
        public string thumbnail { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public List<Basis> bases { get; set; }
        public List<Flavor> flavors { get; set; }
        public string description { get; set; }
        public bool IsNew { get; set; }
    }
}
