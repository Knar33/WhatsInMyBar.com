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

            var res = client.Execute<GetRecipesResponse>(request);

            return new GetRecipesResponse(res);
        }
    }
}
