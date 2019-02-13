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
    public class ClientsController : Controller
    {
        private SportContext db = new SportContext();
        // GET: Clients
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
            var clients = db.Clients.Include(c => c.PricingPlan);
            if (!string.IsNullOrEmpty(search))
            {
                clients = clients.Where(c => c.Head.ToUpper().Contains(search.ToUpper()) ||
                  c.Description.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    clients = clients.OrderByDescending(c => c.Head);
                    break;
                default:
                    clients = clients.OrderBy(c => c.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(clients.ToPagedList(PageNumber,PageSize));
        }
        //Get details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Client client = db.Clients.Find(id);
            if (client==null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        //Get Create
        public ActionResult Create()
        {
            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "PricingPlanID", "PricingPlanID");
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PricingPlan = new SelectList(db.PricingPlans, "PricingPlanID", "PricingPlanID", client.PricingPlanID);
            return View(client);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "PricingPlanID", "PricingPlanID", client.PricingPlanID);
            return View(client);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Client client)
        {
            try
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Client)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes, the client was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Client)databaseEntry.ToObject();
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    if (databaseValues.PricingPlanID != clientValues.PricingPlanID)
                        ModelState.AddModelError("PricingPlanID", "Current Value:" + db.PricingPlans.Find(databaseValues.PricingPlanID));
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The " + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    client.RowVersion = databaseValues.RowVersion;
                }   
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "PricingPlanID", "PricingPlanID", client.PricingPlanID);
            return View(client);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (client == null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }

            }
            return View(client);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Client client)
        {
            try
            {
                db.Entry(client).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = client.ClientID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }
            return View(client);
        }

        
    }
}