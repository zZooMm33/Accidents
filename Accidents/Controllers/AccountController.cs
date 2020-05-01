using Accidents.Models;
using Accidents.Models.Storage.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace Accidents.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Accident", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login log)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (DatabaseContext db = new DatabaseContext())
                {
                    var provider = MD5.Create();
                    string salt = WebConfigurationManager.AppSettings["Salt"];
                    byte[] bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + log.Password));
                    string passHash = BitConverter.ToString(bytes);

                    user = db.Users.FirstOrDefault(u => u.Email == log.Email && u.Password == passHash);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(log.Email, true);
                    return RedirectToAction("Accident", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(log);
        }

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Accident", "Home");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register reg)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (DatabaseContext db = new DatabaseContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == reg.Email);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (DatabaseContext dbAdd = new DatabaseContext())
                    {
                        var provider = MD5.Create();
                        string salt = WebConfigurationManager.AppSettings["Salt"];
                        byte[] bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + reg.Password));
                        string passHash = BitConverter.ToString(bytes);

                        dbAdd.Users.Add(new User { Email = reg.Email, Password = passHash, DateBirth = reg.DateBirth, RoleId = 2 });
                        dbAdd.SaveChanges();

                        user = dbAdd.Users.Where(u => u.Email == reg.Email && u.Password == passHash).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(reg.Email, true);
                        return RedirectToAction("Accident", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким Email адресом уже существует");
                }
            }

            return View(reg);
        }

        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Accident", "Home");
        }
    }
}