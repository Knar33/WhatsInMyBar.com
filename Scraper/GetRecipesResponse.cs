using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Scraper
{
    public class GetRecipesResponse
    {
        public GetRecipesResponse()
        {
        }

        public GetRecipesResponse(IRestResponse<GetRecipesResponse> res)
        {
            StatusCode = res.StatusCode;
            if (res.StatusCode == HttpStatusCode.OK)
            {
                count = res.Data.count;
                success = res.Data.success;
                recipes = res.Data.recipes;
            }
            else
            {
                Content = res.Content;
            }
        }

        public int count { get; set; }
        public bool success { get; set; }
        public List<Recipe> recipes { get; set; }
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
