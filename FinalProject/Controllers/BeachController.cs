using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class BeachController : Controller
    {
        NewsDbContext db = new NewsDbContext();
        // GET: Beach
        public ActionResult Index()
        { 
            return View(db.Beaches.ToList());
        }

        // GET: Beach/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beach beach = db.Beaches.Find(id);
            if (beach == null)
            {
                return HttpNotFound();
            }
            return View(beach);
        }

        // GET: Beach/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beach/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Zoom,Lattitude,Longtidute")] Beach beach)
        {
            if (ModelState.IsValid)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);
                db.Beaches.Add(beach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beach);
        }

        // GET: Beach/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Beach/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Beach/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Beach/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
