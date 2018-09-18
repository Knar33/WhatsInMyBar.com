using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class Flavor
    {
        public Flavor()
        {
        }

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
