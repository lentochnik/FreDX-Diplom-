using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreDX.Models;

namespace FreDX.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SecurityController : Controller
    {
        // GET: Admin
        
        public ActionResult AdminTools()
        {
            return View();
        }

        // GET
        public ActionResult UsersTools()
        {
            UserContext db = new UserContext();
            List<User> ag = db.Users.ToList();
            return View(ag);
        }

        public ActionResult AddUser()
        {
            UserContext db = new UserContext();
            var RoleList = db.Roles.ToList();
            SelectList list = new SelectList(RoleList, "Id", "Name");
            ViewBag.RoleList = list;
            return View();

        }

        public ActionResult EditUser()
        {

            return View();

        }
        public ActionResult ConfEditUser()
        {

            return View();

        }
    }
}