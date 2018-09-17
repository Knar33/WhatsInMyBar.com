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

        public GetRecipesResponse Send()
        {
            RestClient client = new RestClient(ConfigurationManager.AppSettings["APIURL"]);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"action\"\r\n\r\nload_search_results\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ingredients[0]\n\"\r\n\r\nRedbreast\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var res = client.Execute<GetRecipesResponse>(request);

            return new GetRecipesResponse(res);
        }
    }
}
