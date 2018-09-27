﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyBar.Extensions;

namespace Scraper
{
    public class Ingredient
    {
        public Ingredient()
        {
            Scraped = false;
        }

        public Ingredient(int id, string name)
        {
            ingredient_id = id;
            this.name = name;
            Scraped = false;
        }

        public int ingredient_id { get; set; }
        public int? Category { get; set; }
        public string name { get; set; }
        public bool Scraped { get; set; }

        public void Insert()
        {
            try
            {
                using (DbConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    dbConnection.Open();
                    using (var cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "CreateIngredients";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = Int32.Parse(ConfigurationManager.AppSettings["SQLTimeout"]);

                        cmd.AddParameter("@IngredientID", ingredient_id);
                        cmd.AddParameter("@Name", name);

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
