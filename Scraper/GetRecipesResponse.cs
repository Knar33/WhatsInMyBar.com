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
        public int count { get; set; }
        public bool success { get; set; }
        public List<Recipe> recipes { get; set; }
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }

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
    }

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

    public class Title
    {
        public Title()
        {
        }

        public string rendered { get; set; }
    }

    public class Ingredient
    {
        public Ingredient()
        {
        }

        public string ingredient_id { get; set; }
        public string name { get; set; }
    }

    public class Basis
    {
        public Basis()
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
