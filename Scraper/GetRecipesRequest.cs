using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Configuration;

namespace Scraper
{
    class GetRecipesRequest
    {
        public string Ingredient { get; set; }
        public int Page { get; set; }

        public GetRecipesResponse Send()
        {
            RestClient client = new RestClient(ConfigurationManager.AppSettings["APIURL"]);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"action\"\r\n\r\nload_search_results\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ingredients[0]\n\"\r\n\r\n" + Ingredient + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"paged\"\r\n\r\n" + Page + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);

            var res = client.Execute<GetRecipesResponse>(request);

            return new GetRecipesResponse(res);
        }

        public GetRecipesRequest(string ingredient, int page)
        {
            Ingredient = ingredient;
            Page = page;
        }
    }
}
