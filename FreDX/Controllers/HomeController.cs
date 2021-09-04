using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.IO;
using FreDX.Providers;
using FreDX.Models;
using static FreDX.Models.helpers;

namespace FreDX.Controllers
{
  
    public class HomeController : Controller 
    {
       
        // GET: Home

        public ActionResult Index()
        {
            AppbProvider appb = new AppbProvider();

       try
            {
                HelpersDbContext db = new HelpersDbContext();
                List<News> ag = db.news.ToList();
                return View(ag);
            }
            catch
            {

            }
            return View();
        }

   
        //GET: Work
        public ActionResult Work()
        {

            return View();
        }
    }
}