using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models.Classes;
using System.Data.SqlClient;
using System.Data.Entity;
using Sports.Models;



namespace Sports.Controllers
{
    public class HomeController : Controller
    {
        private SportContext db = new SportContext();
        
        public ActionResult Index()
        {

            //var sport = db.Sports.First();
            // ViewBag.sp = sport;
            
            //////////////////////////////////////////
            ViewBag.sport = db.Sports.First();
            /////////////////////////////////////////
            ViewBag.challenge = db.Challenges.First();
            ///////////////////////////////////////////
            ViewBag.ts = db.TypesOfSports.ToList();
            ViewBag.sp = db.TypesOfSports.First();
            /////////////////////////////////////////
            ViewBag.client = db.Clients.First();
            ViewBag.cl = db.Clients.ToList();
            /////////////////////////////////////////
            ViewBag.pricingplan = db.PricingPlans.First();
            ViewBag.pr = db.PricingPlans.ToList();
            //////////////////////////////////////////
            ViewBag.video = db.Videos.First();
            //////////////////////////////////////////
            ViewBag.subscribe = db.Subscribs.First();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.ts = db.TypesOfSports.ToList();
            ViewBag.sp = db.TypesOfSports.First();

            return View();
        }
        public ActionResult Client()
        {
            ViewBag.client = db.Clients.First();
            ViewBag.cl = db.Clients.ToList();
            /////////////////////////////////////////
            ViewBag.pricingplan = db.PricingPlans.First();
            ViewBag.pr = db.PricingPlans.ToList();
            return View();
        }
        public ActionResult Contact()
        {

            ViewBag.subscribe = db.Subscribs.First();
            return View();
        }
    }
}