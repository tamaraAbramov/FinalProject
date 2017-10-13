using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class EnterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Enter
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Enters.ToList());
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

        // GET: Enter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Enter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Enter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Enter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Enter/Edit/5
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

        // GET: Enter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Enter/Delete/5
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

        public ActionResult Json(){
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("NormalUser"))
            {
                var articleEnters = db.Enters
                    .GroupBy(c => new {
                                ArticleId = c.ArticleId
                            }) 
                    .Select(c => new {
                                articleId = c.Key.ArticleId,
                                total = c.Count()
                    })
                    .OrderBy(a => a.articleId)
                    .ToList();
                return this.Json(articleEnters, JsonRequestBehavior.AllowGet);
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

        public ActionResult Graph(){
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
    }
}
