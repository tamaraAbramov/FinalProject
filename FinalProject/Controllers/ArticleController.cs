using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using FinalProject.Models.Facebook;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class ArticleController : Controller
    {
        static Dictionary<int, string> months = new Dictionary<int, string>()
                                                            {
                                                                {1, "January"},
                                                                {2, "February"},
                                                                {3, "March"},
                                                                {4, "April"},
                                                                {5, "May"},
                                                                {6, "June"},
                                                                {7, "July"},
                                                                {8, "August"},
                                                                {9, "September"},
                                                                {10, "October"},
                                                                {11, "November"},
                                                                {12, "December"},
                                                            };
        private NewsDbContext db = new NewsDbContext();

        // GET: Article
        public ActionResult Index()
        {
            string strCurrentUserId = User.Identity.GetUserId();
            var articlesToShow = db.Articles.ToList().Where(a => a.AuthorID == strCurrentUserId);
            return View(articlesToShow.ToList());
        }

        // GET: Article/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        
        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,PublishDate,Text,Image,Video, SearchCount")] Article article, HttpPostedFileBase ImageUploud, HttpPostedFileBase VideoUpload)
        {

            if (ModelState.IsValid)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                article.Author = user.FirstName + " " + user.LastName;
                article.AuthorID = user.Id;

                article.PublishDate = System.DateTime.Now;
                article.SearchCount = 0;

                //Post article into facebook
                string messageToPost = "New Article was posted by " + article.Author + " at " +
                    article.PublishDate + ": " + "\r\n" + article.Text;

                var facebookClient = new FacebookClient();
                var facebookService = new FacebookService(facebookClient);
                var postOnWallTask = facebookService.PostOnWallAsync(FacebookSettings.AccessToken, messageToPost);


                if (ImageUploud != null)
                {
                    ImageUploud.SaveAs(HttpContext.Server.MapPath("~/Visual/Images/")
                                                      + ImageUploud.FileName);
                    article.Image = ImageUploud.FileName;
                }

                if (VideoUpload != null)
                {
                    VideoUpload.SaveAs(HttpContext.Server.MapPath("~/Visual/Videos/")
                                                      + VideoUpload.FileName);
                    article.Video = VideoUpload.FileName;
                }
                
                db.Articles.Add(article);

                db.SaveChanges();
                postOnWallTask.Wait(5000);

                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Author, AuthorID, PublishDate,Text,Image,Video, SearchCount")] Article article, HttpPostedFileBase NewImage, HttpPostedFileBase NewVideo)
        {
            if (ModelState.IsValid)
            {
       
                if (NewImage != null)
                {
                    NewImage.SaveAs(HttpContext.Server.MapPath("~/Visual/Images/")
                                                      + NewImage.FileName);
                    article.Image = NewImage.FileName;
                }

                if (NewVideo != null)
                {
                    NewVideo.SaveAs(HttpContext.Server.MapPath("~/Visual/Videos/")
                                                      + NewVideo.FileName);
                    article.Video = NewVideo.FileName;
                }

                article.PublishDate = System.DateTime.Now;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult NewComment([Bind(Include = "ID,ArticleID,CommentTitle,CommentUser,Text, PublishDate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                comment.ArticleComment = db.Articles.Find(comment.ArticleID);
                comment.CommentUser = user.FirstName + " " + user.LastName;

                comment.PublishDate = System.DateTime.Now;

                db.Comments.Add(comment);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = comment.ArticleID });
            }

            return RedirectToAction("Index");
        }
        
        private class PerMonth
        {
            public string Month;
            public int Total;
            public PerMonth(int Month, int NumberOfArticles){
                months.TryGetValue(Month, out this.Month);
                this.Total = NumberOfArticles;
            }
        };
        // GET: Articale/ByMonth
        public ActionResult ByMonth()
        {
            List<PerMonth> temp = new List<PerMonth>();
            for (int i = 1; i < months.Count + 1; i++){
                temp.Add(new PerMonth(i, 0));
            }
            var articlesByMonth = db.Articles
            .GroupBy(c => new {
                Month = c.PublishDate.Month
            })
            .Select(c => new {
                Month = c.Key.Month,
                Total = c.Count()
            })
            .OrderByDescending(a => a.Month)
            .ToList();
            foreach (var month in articlesByMonth){
                temp[month.Month - 1].Total = month.Total;
            }
            return this.Json(temp, JsonRequestBehavior.AllowGet);
        }

        // GET: Articale/Graph
        public ActionResult Graph()
        {
            return View();
        }
    }
}
