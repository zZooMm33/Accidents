using Accidents.Models;
using Accidents.Models.Storage;
using Accidents.Utils;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

                    List<Danger> dangersList = db.Dangers.ToList();
                    dangersList.Insert(0, new Danger());
                    List<Profession> professionsList = db.Professions.ToList();
                    professionsList.Insert(0, new Profession());
                    List<SourceDanger> sourceDangerList = db.SourceDangers.ToList();
                    sourceDangerList.Insert(0, new SourceDanger());

                    SelectList dangers = new SelectList(dangersList, "Id", "Title", accident.DangerId);                    
                    SelectList professions = new SelectList(professionsList, "Id", "Title", accident.ProfessionId);
                    SelectList sourceDangers = new SelectList(sourceDangerList, "Id", "Title", accident.SourceDangerId);

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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult LoadExcelFile(HttpPostedFileBase excelFile)
        {
            XLWorkbook workBook;

            if (excelFile == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ошибка при загруке файла на сервер", JsonRequestBehavior.AllowGet);
            }

            try
            {
                workBook = new XLWorkbook(excelFile.InputStream, XLEventTracking.Disabled);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Неверный формат файла", JsonRequestBehavior.AllowGet);
            }

            foreach (IXLWorksheet worksheet in workBook.Worksheets)
            {
                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                {
                    try
                    {
                        Accident accident = new Accident();

                        accident.Title = row.Cell(1).Value.ToString();
                        accident.Description = row.Cell(2).Value.ToString();
                        accident.Date = DateTime.Parse(row.Cell(3).Value.ToString());
                        accident.Link = row.Cell(7).Value.ToString();

                        using (DatabaseContext db = new DatabaseContext())
                        {
                            string Profession = row.Cell(4).Value.ToString();
                            string Danger = row.Cell(5).Value.ToString();
                            string SourceDanger = row.Cell(6).Value.ToString();

                            // Проверяем есть ли такой НС в БД
                            Accident accidentTmp = db.Accidents.Where(a => a.Title == accident.Title).FirstOrDefault();

                            if (accidentTmp == null)
                            {
                                if (Danger.Length > 3)
                                {
                                    // Есть ли опасность в БД
                                    Danger danger = db.Dangers.Where(d => d.Title == Danger).FirstOrDefault();

                                    if (danger != null)
                                    {
                                        accident.DangerId = danger.Id;
                                    }
                                    else
                                    {
                                        // добавляем в БД
                                        db.Dangers.Add(new Danger { Title = Danger });
                                        db.SaveChanges();

                                        danger = db.Dangers.Where(d => d.Title == Danger).FirstOrDefault();
                                        if (danger != null)
                                        {
                                            accident.DangerId = danger.Id;
                                        }
                                    }
                                }

                                if (Profession.Length > 3)
                                {
                                    // Есть ли опасность в БД
                                    Profession profession = db.Professions.Where(d => d.Title == Profession).FirstOrDefault();

                                    if (profession != null)
                                    {
                                        accident.ProfessionId = profession.Id;
                                    }
                                    else
                                    {
                                        // добавляем в БД
                                        db.Professions.Add(new Profession { Title = Profession });
                                        db.SaveChanges();

                                        profession = db.Professions.Where(d => d.Title == Profession).FirstOrDefault();
                                        if (profession != null)
                                        {
                                            accident.ProfessionId = profession.Id;
                                        }
                                    }
                                }

                                if (SourceDanger.Length > 3)
                                {
                                    // Есть ли опасность в БД
                                    SourceDanger sourceDanger = db.SourceDangers.Where(d => d.Title == SourceDanger).FirstOrDefault();

                                    if (sourceDanger != null)
                                    {
                                        accident.DangerId = sourceDanger.Id;
                                    }
                                    else
                                    {
                                        // добавляем в БД
                                        db.SourceDangers.Add(new SourceDanger { Title = SourceDanger });
                                        db.SaveChanges();

                                        sourceDanger = db.SourceDangers.Where(d => d.Title == SourceDanger).FirstOrDefault();
                                        if (SourceDanger != null)
                                        {
                                            accident.SourceDangerId = sourceDanger.Id;
                                        }
                                    }
                                }

                                db.Accidents.Add(accident);
                                db.SaveChanges();
                            }
                        }

                    }
                    catch (Exception)
                    {
                        // записать в лог ошибку
                    }
                }
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("НС были успешно добавлены в БД", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Export()
        {
            List<Accident> accidents;
            using (DatabaseContext db = new DatabaseContext())
            {
                accidents = db.Accidents.Include(p => p.Profession).Include(d => d.Danger).Include(s => s.SourceDanger).ToList();
            }

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("НС");
                worksheet.Cell("A1").Value = "Название";                
                worksheet.Cell("B1").Value = "Описание";
                worksheet.Cell("C1").Value = "Дата";
                worksheet.Cell("D1").Value = "Профессия";
                worksheet.Cell("E1").Value = "Опасность";
                worksheet.Cell("F1").Value = "Источник опасности";
                worksheet.Cell("G1").Value = "Источник (Ссылка)";
                
                worksheet.Row(1).Style.Font.Bold = true;

                worksheet.Column(1).Width = 50;
                worksheet.Column(2).Width = 150;
                worksheet.Column(3).Width = 15;
                worksheet.Column(5).Width = 50;
                worksheet.Column(6).Width = 50;
                worksheet.Column(7).Width = 50;
                worksheet.Column(4).Width = 100;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < accidents.Count; i++)
                {
                    
                    worksheet.Cell(i + 2, 1).Value = accidents[i].Title;
                    worksheet.Cell(i + 2, 2).Value = accidents[i].Description;
                    worksheet.Cell(i + 2, 3).Value = accidents[i].Date;                    
                    if (accidents[i].ProfessionId != null) worksheet.Cell(i + 2, 4).Value = accidents[i].Profession.Title;
                    if (accidents[i].DangerId != null) worksheet.Cell(i + 2, 5).Value = accidents[i].Danger.Title;
                    if (accidents[i].SourceDangerId != null) worksheet.Cell(i + 2, 6).Value = accidents[i].SourceDanger.Title;

                    worksheet.Cell(i + 2, 7).Value = accidents[i].Link;
                    worksheet.Row(i + 2).Height = (int)(accidents[i].Description.Length / 150)*21 + 20;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"accidents_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}