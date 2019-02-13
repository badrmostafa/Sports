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
    public class VideosController : Controller
    {
        private SportContext db = new SportContext();
        // GET: videos
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Video = string.IsNullOrEmpty(sort) ? "video_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var videos = from p in db.Videos select p;
            if (!string.IsNullOrEmpty(search))
            {
                videos = videos.Where(p => p.Video_url.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "picture_desc":
                    videos = videos.OrderByDescending(p => p.Video_url);
                    break;
                default:
                    videos = videos.OrderBy(p => p.Video_url);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(videos.ToPagedList(PageNumber,PageSize));
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Video video)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }
        //Get details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video==null)
            {
                return HttpNotFound();
            }
            return View(video);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }
        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Video video)
        {
            try
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Video)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The picture was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Video)databaseEntry.ToObject();
                    
                         if (databaseValues.Video_url != clientValues.Video_url)
                        ModelState.AddModelError("Picture_url", "Curent Value:" + databaseValues.Video_url);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit"
                        + " Was modified by another user after you got the original value."
                        + " The edit operation was cancelled and the current values in the database"
                        + " Have been displayed.If you still want to edit this record"
                        + " Click,the save button again.Otherwise click the back to list hyperlink");
                    video.RowVersion = databaseValues.RowVersion;
                }
                
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again, and if the problem persists contact your system administrator. ");
            }
            
                
            
            return View(video);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (video==null)
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
            return View(video);
        }
        //Post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Video video)
        {
            try
            {
                db.Entry(video).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                return RedirectToAction("Delete", new { concurrencyError = true, id = video.VideoID });

            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
            }
            return View(video);
        }

    }
}