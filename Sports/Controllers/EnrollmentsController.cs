using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models.Classes;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList;

namespace Sports.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SportContext db = new SportContext();
        // GET: Enrollments
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.FullName = string.IsNullOrEmpty(sort) ? "fullname_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var enrollments = from e in db.Enrollments select e;
            if (!string.IsNullOrEmpty(search))
            {
                enrollments = enrollments.Where(e => e.FullName.ToUpper().Contains(search.ToUpper()) ||
                  e.Email.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "fullname_desc":
                    enrollments = enrollments.OrderByDescending(e => e.FullName);
                    break;
                default:
                    enrollments = enrollments.OrderBy(e => e.FullName);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(enrollments.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment==null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
              
            }
            return View(enrollment);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Enrollment enrollment)
        {
            try
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Enrollment)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The Enrollment was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Enrollment)databaseEntry.ToObject();
                    if (databaseValues.FullName != clientValues.FullName)
                        ModelState.AddModelError("FullName", "Current Value:" + databaseValues.FullName);
                    if (databaseValues.Email != clientValues.Email)
                        ModelState.AddModelError("Email", "Current Value:" + databaseValues.Email);
                    if (databaseValues.Age != clientValues.Age)
                        ModelState.AddModelError("Age", "Current Value:" + databaseValues.Age);
                    if (databaseValues.Grade != clientValues.Grade)
                        ModelState.AddModelError("Grade", "Current Value:" + databaseValues.Grade);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The " + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    enrollment.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again ,and if the problem persists contact your system administrator.");
            }
            return View(enrollment);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (enrollment==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(enrollment);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Enrollment enrollment)
        {
            try
            {
                db.Entry(enrollment).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = enrollment.EnrollmentID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }
            return View(enrollment);
        }

    }
}