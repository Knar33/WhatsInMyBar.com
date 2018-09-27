using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using WhatsInMyBar.Extensions;

namespace Scraper
{
    public class Recipe
    {
        public int id { get; set; }
        public Title title { get; set; }
        public string link { get; set; }
        public string thumbnail { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public List<Basis> bases { get; set; }
        public List<Flavor> flavors { get; set; }
        public string description { get; set; }

        public void Insert()
        {
            try
            {
                using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    dbConnection.Open();
                    using (var cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "CreateRecipes";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                        cmd.AddParameter("@RecipeID", id);
                        cmd.AddParameter("@Name", title.rendered);
                        cmd.AddParameter("@Link", link);
                        cmd.AddParameter("@Thumbnail", thumbnail);
                        cmd.AddParameter("@Description", description);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
