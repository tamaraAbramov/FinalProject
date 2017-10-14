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

        private class Output{
            public string articleTitle;
            public int total;
            public Output(string articleTitle, int total){
                this.articleTitle = articleTitle;
                this.total = total;
            }
        }

        public ActionResult Json(){
            if (User.IsInRole("Admin") || User.IsInRole("Author") || User.IsInRole("NormalUser"))
            {
                var articleEnters = db.Enters
                    .Join(
                        db.Articles,
                        enter => enter.ArticleId,
                        article => article.ID,
                        (enter, article ) => new { Article = article, Enter = enter}
                    )
                    .GroupBy(c => new {
                                ArticleTitle = c.Article.Title
                            }) 
                    .Select(c => new {
                                articleTitle = c.Key.ArticleTitle,
                                total = c.Count()
                    })
                    .OrderBy(a => a.total)
                    .ToList();
                List<Output> jsonOutput = new List<Output>();
                foreach (var a in articleEnters){
                    jsonOutput.Add(new Output(a.articleTitle, a.total));
                }
                return this.Json(jsonOutput, JsonRequestBehavior.AllowGet);
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

        public ActionResult Diagnostics(){
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
