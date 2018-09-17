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
            GetRecipesRequest request = new GetRecipesRequest();
            var response = request.Send();
        }
    }
}
