using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using WhatsInMyBar.Extensions;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

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

        public void DownloadThumbnail()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string thumbnailURI = string.Format("http://{0}", thumbnail.TrimStart('/'));
                    byte[] data = webClient.DownloadData(thumbnailURI);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var yourImage = Image.FromStream(mem))
                        {
                            string imageURL = string.Format("{0}{1}.jpg", ConfigurationManager.AppSettings["LocalImagePath"], id);
                            yourImage.Save(imageURL, ImageFormat.Jpeg);
                        }
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
