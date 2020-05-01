using Accidents.Models;
using Accidents.Models.Storage;
using PagedList;
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
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Accident", "Home");
        }

        [Authorize]
        public ActionResult Profession(int? page, string Search)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Profession> professions;
                if (Search != null && Search.Length > 0)
                {
                    professions = db.Professions.Where(a => a.Title.IndexOf(Search) != -1).ToList();
                }
                else professions = db.Professions.ToList();

                return View(professions.ToPagedList((page ?? 1), 10));
            }
        }

        [Authorize]
        public ActionResult Danger(int? page, string Search)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Danger> dangers;
                if (Search != null && Search.Length > 0)
                {
                    dangers = db.Dangers.Where(a => a.Title.IndexOf(Search) != -1).ToList();
                }
                else dangers = db.Dangers.ToList();

                return View(dangers.ToPagedList((page ?? 1), 10));
            }
        }

        [Authorize]
        public ActionResult SourceDanger(int? page, string Search)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                List<SourceDanger> sourceDangers;
                if (Search != null && Search.Length > 0)
                {
                    sourceDangers = db.SourceDangers.Where(a => a.Title.IndexOf(Search) != -1).ToList();
                }
                else sourceDangers = db.SourceDangers.ToList();

                return View(sourceDangers.ToPagedList((page ?? 1), 10));
            }
        }

        [Authorize]
        public ActionResult Accident(int? page, string Search, string SearchBy)
        {            
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Accident> accidents;
                if (Search != null && Search.Length > 0)
                {
                    if (SearchBy == "Title") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Title.IndexOf(Search) != -1).ToList();
                    else if (SearchBy == "Description") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Description.IndexOf(Search) != -1).ToList();
                    else if (SearchBy == "Profession") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Profession.Title.IndexOf(Search) != -1).ToList();
                    else if (SearchBy == "Danger") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Danger.Title.IndexOf(Search) != -1).ToList();
                    else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.SourceDanger.Title.IndexOf(Search) != -1).ToList();
                }
                else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).ToList();

                return View(accidents.ToPagedList((page ?? 1), 5));
            }
        }

    }
}