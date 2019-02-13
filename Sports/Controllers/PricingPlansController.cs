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
using PagedList;

namespace Sports.Controllers
{
    public class PricingPlansController : Controller
    {
        private SportContext db = new SportContext();
        // GET: PricingPlans
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
            var pricingplans = from p in db.PricingPlans select p;
            if (!string.IsNullOrEmpty(search))
            {
                pricingplans = pricingplans.Where(p => p.Head.ToUpper().Contains(search.ToUpper()) ||
                  p.Head1.ToUpper().Contains(search.ToUpper()) ||
                  p.Head2.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    pricingplans = pricingplans.OrderByDescending(p => p.Head);
                    break;
                default:
                    pricingplans = pricingplans.OrderBy(p => p.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(pricingplans.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            if (pricingPlan==null)
            {
                return HttpNotFound();
            }
            return View(pricingPlan);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(PricingPlan pricingPlan)
        {
            if (ModelState.IsValid)
            {
                db.PricingPlans.Add(pricingPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pricingPlan);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            if (pricingPlan == null)
            {
                return HttpNotFound();
            }
            return View(pricingPlan);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(PricingPlan pricingPlan)
        {
            try
            {
                db.Entry(pricingPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (PricingPlan)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The pricingplan was deleted by another user.");
                }
                else
                {
                    var databaseValues = (PricingPlan)databaseEntry.ToObject();
                    if (databaseValues.Title != clientValues.Title)
                        ModelState.AddModelError("Title", "Current Value:" + databaseValues.Title);
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Price != clientValues.Price)
                        ModelState.AddModelError("Price", "Curent Value:" + string.Format("{0:c}", databaseValues.Price));
                    if (databaseValues.Head1 != clientValues.Head1)
                        ModelState.AddModelError("Head1", "Current Value:" + databaseValues.Head1);
                    if (databaseValues.Head2 != clientValues.Head2)
                        ModelState.AddModelError("Head2", "Current Value:" + databaseValues.Head2);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The " + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    pricingPlan.RowVersion = databaseValues.RowVersion;
                }   
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(pricingPlan);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            if (pricingPlan == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (pricingPlan==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }

            }
            return View(pricingPlan);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(PricingPlan pricingPlan)
        {
            try
            {
                db.Entry(pricingPlan).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = pricingPlan.PricingPlanID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }
            return View(pricingPlan);
        }

    }
}