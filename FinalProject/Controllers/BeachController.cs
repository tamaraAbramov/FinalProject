using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Beach/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beach/Create
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
