using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsInMyBar.Models;

namespace WhatsInMyBar.Controllers
{
    public class HomeController : Controller
    {
        public List<Recipe> Recipes { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public HomeController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}