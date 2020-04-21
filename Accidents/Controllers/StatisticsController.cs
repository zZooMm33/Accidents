using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accidents.Models;
using Accidents.Models.Storage;
using System.Data.Entity;
using PagedList;

namespace Accidents.Controllers
{    
    public class StatisticsController : Controller
    {        
        public ActionResult Profession(int id, int? page, string Search, string SearchBy)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Accident> accidents;
                if (Search != null && Search.Length > 0)
                {
                    if (SearchBy == "Title") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Title.IndexOf(Search) != -1).Where(p => p.ProfessionId == id).ToList();
                    else if (SearchBy == "Description") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Description.IndexOf(Search) != -1).Where(p => p.ProfessionId == id).ToList();
                    else if (SearchBy == "Profession") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Profession.Title.IndexOf(Search) != -1).Where(p => p.ProfessionId == id).ToList();
                    else if (SearchBy == "Danger") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Danger.Title.IndexOf(Search) != -1).Where(p => p.ProfessionId == id).ToList();
                    else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.SourceDanger.Title.IndexOf(Search) != -1).Where(p => p.ProfessionId == id).ToList();
                }
                else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(p => p.ProfessionId == id).ToList();

                ViewBag.StatisticsTitle = db.Professions.Where(p => p.Id == id).FirstOrDefault().Title;

                return View(accidents.ToPagedList((page ?? 1), 5));
            }
        }

        public ActionResult Danger(int id, int? page, string Search, string SearchBy)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Accident> accidents;
                if (Search != null && Search.Length > 0)
                {
                    if (SearchBy == "Title") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Title.IndexOf(Search) != -1).Where(p => p.DangerId == id).ToList();
                    else if (SearchBy == "Description") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Description.IndexOf(Search) != -1).Where(p => p.DangerId == id).ToList();
                    else if (SearchBy == "Profession") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Profession.Title.IndexOf(Search) != -1).Where(p => p.DangerId == id).ToList();
                    else if (SearchBy == "Danger") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Danger.Title.IndexOf(Search) != -1).Where(p => p.DangerId == id).ToList();
                    else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.SourceDanger.Title.IndexOf(Search) != -1).Where(p => p.DangerId == id).ToList();
                }
                else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(p => p.DangerId == id).ToList();

                ViewBag.StatisticsTitle = db.Dangers.Where(p => p.Id == id).FirstOrDefault().Title;

                return View(accidents.ToPagedList((page ?? 1), 5));
            }
        }

        public ActionResult SourceDanger(int id, int? page, string Search, string SearchBy)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                List<Accident> accidents;
                if (Search != null && Search.Length > 0)
                {
                    if (SearchBy == "Title") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Title.IndexOf(Search) != -1).Where(p => p.SourceDangerId == id).ToList();
                    else if (SearchBy == "Description") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Description.IndexOf(Search) != -1).Where(p => p.SourceDangerId == id).ToList();
                    else if (SearchBy == "Profession") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Profession.Title.IndexOf(Search) != -1).Where(p => p.SourceDangerId == id).ToList();
                    else if (SearchBy == "Danger") accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.Danger.Title.IndexOf(Search) != -1).Where(p => p.SourceDangerId == id).ToList();
                    else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(a => a.SourceDanger.Title.IndexOf(Search) != -1).Where(p => p.SourceDangerId == id).ToList();
                }
                else accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).Where(p => p.SourceDangerId == id).ToList();

                ViewBag.StatisticsTitle = db.SourceDangers.Where(p => p.Id == id).FirstOrDefault().Title;

                return View(accidents.ToPagedList((page ?? 1), 5));
            }
        }
    }
}
