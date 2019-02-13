using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Sports.Models.Classes;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList.Mvc;
using PagedList;

namespace Sports.Controllers
{
    public class TypesOfSportsController : Controller
    {
        private SportContext db = new SportContext();
        // GET: TypesOfSports
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Head = string.IsNullOrEmpty(sort) ? "head_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search; 
            var typeofsport = db.TypesOfSports.Include(t => t.Sport);
            if (!string.IsNullOrEmpty(search))
            {
                typeofsport = typeofsport.Where(t => t.Head.ToUpper().Contains(search.ToUpper()) ||
                  t.Description.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    typeofsport = typeofsport.OrderByDescending(t => t.Head);
                    break;
                default:
                    typeofsport = typeofsport.OrderBy(t => t.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(typeofsport.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfSport typeOfSport = db.TypesOfSports.Find(id);
            if (typeOfSport==null)
            {
                return HttpNotFound();
            }
            return View(typeOfSport);
        }
        //Get Create
        public ActionResult Create()
        {
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportID");
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(TypeOfSport typeOfSport)
        {
            if (ModelState.IsValid)
            {
                db.TypesOfSports.Add(typeOfSport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportID", typeOfSport.SportID);
            return View(typeOfSport);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfSport typeOfSport = db.TypesOfSports.Find(id);
            if (typeOfSport == null)
            {
                return HttpNotFound();
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportID", typeOfSport.SportID);
            return View(typeOfSport);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(TypeOfSport typeOfSport)
        {
            try
            {
                db.Entry(typeOfSport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (TypeOfSport)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes,the typeofsport was deleted by another user.");
                }
                else
                {
                    var databaseValues = (TypeOfSport)databaseEntry.ToObject();
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    if (databaseValues.Image1 != clientValues.Image1)
                        ModelState.AddModelError("Image1", "Current Value:" + databaseValues.Image1);
                    if (databaseValues.SportID != clientValues.SportID)
                        ModelState.AddModelError("SportID", "Current Value:" + db.TypesOfSports.Find(databaseValues.SportID));
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The " + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    typeOfSport.RowVersion = databaseValues.RowVersion;
                }

               
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportID", typeOfSport.SportID);
            return View(typeOfSport);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOfSport typeOfSport = db.TypesOfSports.Find(id);
            if (typeOfSport==null)
            {
                if (concurrencyError==null)
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (typeOfSport==null)
                {
                    ViewBag.ConcurrencyErrorMessage= "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
                
            }
            return View(typeOfSport);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(TypeOfSport typeOfSport)
        {
            try
            {
                db.Entry(typeOfSport).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                return RedirectToAction("Delete", new { concurrencyError = true, id = typeOfSport.TypeOfSportID });
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }
            return View(typeOfSport);
          
        }

    }
}