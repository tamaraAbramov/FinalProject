using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        // GET: Beach/json
        public ActionResult Json()
        {
            return this.Json(db.Beaches.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Map()
        {
            return View();
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
        public ActionResult Edit(int? id)
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

        // POST: Beach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Beach beach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beach);
        }

        // GET: Beach/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beach beach = db.Beaches.Find(id);
            if (beach == null){
                return HttpNotFound();
            }
            return View(beach);
        }

        // POST: Beach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Beach beach = db.Beaches.Find(id);
            db.Beaches.Remove(beach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
