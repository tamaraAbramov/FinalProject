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
        private NewsDbContext db = new NewsDbContext();

        // GET: Article
        public ActionResult Index()
        {
            return View(db.Articles.ToList());
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
        public ActionResult Create([Bind(Include = "ID,Title,PublishDate,Text,Image,Video")] Article article, HttpPostedFileBase ImageUploud, HttpPostedFileBase VideoUpload)
        {

            if (ModelState.IsValid)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                article.Author = user.FirstName + " " + user.LastName;
                article.AuthorID = user.Id;
                
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

                //Post article into facebook
                string messageToPost = "New Article was posted by " + article.Author + " at " +
                    article.PublishDate + ": " + "\r\n" + article.Text;
                var facebookClient = new FacebookClient();
                var facebookService = new FacebookService(facebookClient);
                var getAccountTask = facebookService.GetAccountAsync(FacebookSettings.AccessToken);
                Task.WaitAll(getAccountTask);
                var account = getAccountTask.Result;
                var postOnWallTask = facebookService.PostOnWallAsync(FacebookSettings.AccessToken, messageToPost);
                Task.WaitAll(postOnWallTask);


                article.PublishDate = System.DateTime.Now;
                db.Articles.Add(article);

                db.SaveChanges();
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
        public ActionResult Edit([Bind(Include = "ID,Title,PublishDate,Text,Image,Video")] Article article, HttpPostedFileBase NewImage, HttpPostedFileBase NewVideo)
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
    }
}
