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
    [Authorize]
    public class SportsController : Controller
    {
        private SportContext db = new SportContext();
        // GET: Sports
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
            var sports = from s in db.Sports
                         select s;
            if (!string.IsNullOrEmpty(search))
            {
                sports = sports.Where(s => s.Head1.ToUpper().Contains(search.ToUpper()) ||
                  s.Head2.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    sports = sports.OrderByDescending(s => s.Head1);
                    break;
                default:
                    sports = sports.OrderBy(s => s.Head1);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 1;
            return View(sports.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Sport sport)
        {
            if (ModelState.IsValid)
            {
                db.Sports.Add(sport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sport);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Sport sport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(sport).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Sport)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty,
                  "Unable to save changes.The sport was deleted by another user.");

                }
                else
                {
                    var databaseValues = (Sport)databaseEntry.ToObject();
                    if (databaseValues.Head1 != clientValues.Head1)
                        ModelState.AddModelError("Head1", "Current Value:" + databaseValues.Head1);
                    if (databaseValues.Head2 != clientValues.Head2)
                        ModelState.AddModelError("Head2", "Current Value:" + databaseValues.Head2);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit"
                        + " Was modified by another user after you got the original value."
                        + " The edit operation was cancelled and the current values in the database"
                        + " Have been displayed.If you still want to edit this record"
                        + " Click,the save button again.Otherwise click the back to list hyperlink");
                    sport.RowVersion = databaseValues.RowVersion;

                }

            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again,and if the problem is persists contact your system administrator. ");
            }
            return View(sport);
        }
        //Get Delete
        public ActionResult Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                if (concurrencyError == true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (sport == null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete"
                      + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }

                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " +
                        "was modified by another user after you got the original values. "
                        + "The delete operation was canceled and the current values in the "
                        + "database have been displayed. If you still want to delete this "
                        + "record, click the Delete button again. Otherwise "
                        + "click the Back to List hyperlink.";

                }

            }
            return View(sport);
        }


        //Post Delete
        [HttpPost]
        
        public ActionResult Delete(Sport sport)
        {
            try
            {
                db.Entry(sport).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = sport.SportID });
                
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }
            return View(sport);
        }
    }
}    
