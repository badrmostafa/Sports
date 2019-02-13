using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Sports.Models.Classes;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList;

namespace Sports.Controllers
{
    public class SubscribesController : Controller
    {
        private SportContext db = new SportContext();
        // GET: Subscribes
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
            var subscribes = from s in db.Subscribs select s;
            if (!string.IsNullOrEmpty(search))
            {
                subscribes = subscribes.Where(s => s.Head.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    subscribes = subscribes.OrderByDescending(s => s.Head);
                    break;
                default:
                    subscribes = subscribes.OrderBy(s => s.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(subscribes.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Subscribe subscribe = db.Subscribs.Find(id);
            if (subscribe==null)
            {
                return HttpNotFound();
            }
            return View(subscribe);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Subscribe subscribe)
        {
            if (ModelState.IsValid)
            {
                db.Subscribs.Add(subscribe);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(subscribe);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Subscribe subscribe = db.Subscribs.Find(id);
            if (subscribe == null)
            {
                return HttpNotFound();
            }
            return View(subscribe);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Subscribe subscribe)
        {
            try
            {
                db.Entry(subscribe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Subscribe)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The subscribe was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Subscribe)databaseEntry.ToObject();
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The " + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    subscribe.RowVersion = databaseValues.RowVersion;
                }
                
                
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
               
            
            return View(subscribe);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Subscribe subscribe = db.Subscribs.Find(id);
            if (subscribe == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (subscribe==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(subscribe);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Subscribe subscribe)
        {
            try
            {
                db.Entry(subscribe).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = subscribe.SubscribeID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty,"Unable to save changes.Try again,and if the problem persists contact your system administrator.");
            }
            return View(subscribe);
        }
    }
}