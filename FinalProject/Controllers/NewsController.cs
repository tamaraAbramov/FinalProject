using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string DEFULT_RESULT = "###";

        // GET: News (Articales)
        public ActionResult Index()
        {
            if (User.IsInRole("NormalUser") || User.IsInRole("Admin") ||
                User.IsInRole("Author"))
            {
                var orderedArticles = db.Articles.OrderByDescending(a => a.SearchCount).ThenByDescending(a => a.PublishDate);
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

        //PrivilegeError
        public ActionResult PrivilegeError()
        {
            return View();
        }

        public ActionResult SurfingInfo()
        {
            return View();
        }

        public PartialViewResult SearchArticleResult(string articleTitle, string autherName, string articleText,
          DateTime? startDate, DateTime? endDate)
        {
            string whereQuery = String.Empty;

            if(articleTitle == DEFULT_RESULT)
            {
                articleTitle = string.Empty;
            }

            if (autherName == DEFULT_RESULT)
            {
                autherName = string.Empty;
            }

            if (articleText == DEFULT_RESULT)
            {
                articleText = string.Empty;
            }

            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }

            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            var articles = from a in db.Articles
                           where a.Title.Contains(articleTitle)
                           where a.Author.Contains(autherName)
                           where a.Text.Contains(articleText)
                           where a.PublishDate >= startDate
                           where a.PublishDate <= endDate
                           select a;

            UpdateStatisticResult(articles.ToList());

            return PartialView("../Shared/_ArticlesResult", articles.ToList());
        }

        private void UpdateStatisticResult(List<Article> articles)
        {
            foreach (Article article in articles)
            {
                article.SearchCount++;

                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
            }
            
        }

    }
}