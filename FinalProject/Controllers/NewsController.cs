using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class NewsController : Controller
    {
        private NewsDbContext db = new NewsDbContext();
        private string DEFULT_RESULT = "###";

        // GET: News (Articales)
        public ActionResult Index()
        {
            if (User.IsInRole("NormalUser") || User.IsInRole("Admin") ||
                User.IsInRole("Author"))
            {
                var orderedArticles = db.Articles.OrderByDescending(a => a.PublishDate);
                return View(orderedArticles.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult FindWaves()
        {
            return View();
        }


        public string SearchArticleResult(string articleTitle, string autherName, string articleText,
          DateTime? startDate, DateTime? endDate)
        {

            return "WORK";
        }
    }
}