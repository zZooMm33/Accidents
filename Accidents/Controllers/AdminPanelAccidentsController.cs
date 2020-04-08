using Accidents.Models;
using Accidents.Models.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Accidents.Controllers
{
    public class AdminPanelAccidentsController : Controller
    {
        // GET: AdminPanelAccidents
        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            using (DatabaseContext db = new DatabaseContext())
            {                
                return View(db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).ToList());
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                Accident accident = db.Accidents.Where(a => a.Id == id).FirstOrDefault();
                if (accident != null)
                {
                    
                    SelectList dangers = new SelectList(db.Dangers.ToList(), "Id", "Title", accident.DangerId);
                    SelectList professions = new SelectList(db.Professions.ToList(), "Id", "Title", accident.ProfessionId);
                    SelectList sourceDangers = new SelectList(db.SourceDangers.ToList(), "Id", "Title", accident.SourceDangerId);
                    ViewBag.Dangers = dangers;
                    ViewBag.Professions = professions;
                    ViewBag.SourceDangers = sourceDangers;

                    return View(accident);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Accident accident)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                db.Entry(accident).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "AdminPanelAccidents");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ошибка при удалении, id не был задан", JsonRequestBehavior.AllowGet);
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                Accident accident = db.Accidents.Where(a => a.Id == id).FirstOrDefault();

                if (accident != null)
                {
                    db.Accidents.Remove(accident);
                    db.SaveChanges();

                    // Проверка
                    accident = db.Accidents.Where(a => a.Id == id).FirstOrDefault();
                    if (accident == null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("Успешно", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json("Ошибка при удалении", JsonRequestBehavior.AllowGet);
                        //new { success = false, responseText = "Ошибка при удалении" }
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Ошибка при удалении, НС не найден в БД", JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}