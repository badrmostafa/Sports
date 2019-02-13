using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sports.Models.Classes;
using System.Data.Entity.Infrastructure;
using PagedList.Mvc;
using PagedList;


namespace Sports.Controllers
{
    public class HobbiesController : Controller
    {
        private SportContext db = new SportContext();

        // GET: Hobbies
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
            var hobbies = db.Hobbies.Include(h => h.Client).Include(h => h.TypeOfSport);
            if (!string.IsNullOrEmpty(search))
            {
                hobbies = hobbies.Where(h => h.TypeOfSport.Head.ToUpper().Contains(search.ToUpper()) ||
                  h.Client.Head.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    hobbies = hobbies.OrderByDescending(h => h.TypeOfSport.Head);
                    break;
                default:
                    hobbies = hobbies.OrderBy(h => h.TypeOfSport.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(hobbies.ToPagedList(PageNumber,PageSize));
        }

        // GET: Hobbies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hobby hobby = db.Hobbies.Find(id);
            if (hobby == null)
            {
                return HttpNotFound();
            }
            return View(hobby);
        }

        // GET: Hobbies/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "Head");
            ViewBag.TypeOfSportID = new SelectList(db.TypesOfSports, "TypeOfSportID", "Head");
            return View();
        }

        // POST: Hobbies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HobbyID,Name,TypeOfSportID,ClientID,RowVersion")] Hobby hobby)
        {
            if (ModelState.IsValid)
            {
                db.Hobbies.Add(hobby);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientID", hobby.ClientID);
            ViewBag.TypeOfSportID = new SelectList(db.TypesOfSports, "TypeOfSportID", "TypeOfSportID", hobby.TypeOfSportID);
            return View(hobby);
        }

        // GET: Hobbies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hobby hobby = db.Hobbies.Find(id);
            if (hobby == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientID", hobby.ClientID);
            ViewBag.TypeOfSportID = new SelectList(db.TypesOfSports, "TypeOfSportID", "TypeOfSportID", hobby.TypeOfSportID);
            return View(hobby);
        }

        // POST: Hobbies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HobbyID,Name,TypeOfSportID,ClientID,RowVersion")] Hobby hobby)
        {
            try
            {
                db.Entry(hobby).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Hobby)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The hobby was deleted by another user. ");
                }
                else
                {
                    var databaseValues = (Hobby)databaseEntry.ToObject();
                    
                       
                    if (databaseValues.TypeOfSportID != clientValues.TypeOfSportID)
                        ModelState.AddModelError("TypeOfSportID", "Current Value:" + db.TypesOfSports.Find(databaseValues.TypeOfSportID));
                    if (databaseValues.ClientID != clientValues.ClientID)
                        ModelState.AddModelError("ClientID", "Current Value:" + db.Clients.Find(databaseValues.ClientID));
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit"
                        + " Was modified by another user after you got the original value."
                        + " The edit operation was cancelled and the current values in the database"
                        + " Have been displayed.If you still want to edit this record"
                        + " Click,the save button again.Otherwise click the back to list hyperlink");
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again, and if the problem persists contact your system administrator. ");

            }



            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientID", hobby.ClientID);
            ViewBag.TypeOfSportID = new SelectList(db.TypesOfSports, "TypeOfSportID", "TypeOfSportID", hobby.TypeOfSportID);
            return View(hobby);
        }

        // GET: Hobbies/Delete/5
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hobby hobby = db.Hobbies.Find(id);
            if (hobby == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (hobby==null)
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
            return View(hobby);
        }

        // POST: Hobbies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Hobby hobby)
        {
            try
            {
                db.Entry(hobby).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                return RedirectToAction("Delete", new { concurrencyError = true, id = hobby.HobbyID });
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again, and if the problem persists contact your system administrator.");
            }
            return View(hobby);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
