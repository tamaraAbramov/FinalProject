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
            if (User.IsInRole("Admin"))
            {
                return View(db.Beaches.ToList());
            }
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Beach/json
        public ActionResult Json()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("NormalUser"))
            {
                return this.Json(db.Beaches.ToList(), JsonRequestBehavior.AllowGet);
            }
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Map()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("NormalUser"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult BeachStatistics()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("NormalUser"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        // GET: Beach/Details/5
        public ActionResult Details(int? id)
        {
            if (User.IsInRole("Admin"))
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
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Beach/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Beach/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Zoom,Lattitude,Longtidute")] Beach beach)
        {
            if (User.IsInRole("Admin"))
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
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Beach/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole("Admin"))
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
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Beach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Beach beach)
        {
            if (User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(beach).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(beach);
            }
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Beach/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Admin"))
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
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Beach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (User.IsInRole("Admin"))
            {
                Beach beach = db.Beaches.Find(id);
                db.Beaches.Remove(beach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (User.IsInRole("NormalUser") || User.IsInRole("Author"))
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
