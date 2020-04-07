using Accidents.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accidents.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var users = db.Users.Include(u => u.Role).ToList();
                var roles = db.Roles.SqlQuery("SELECT * FROM [dbo].[Roles]").ToList();
                ViewBag.Users = users;
                ViewBag.Roles = roles;
            }

            return View();
        }
    }
}