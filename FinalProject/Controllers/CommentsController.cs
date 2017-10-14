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

namespace FinalProject.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index(string articleTitle, string commentTitle, string commentUserName)
        {
           

            if (User.IsInRole("Admin"))
            {
                var comments = from a in db.Comments select a;

                if (!String.IsNullOrEmpty(articleTitle))
                {
                    comments = comments.Where(s => s.ArticleComment.Title.Contains(articleTitle));
                }

                if (!String.IsNullOrEmpty(commentTitle))
                {
                    comments = comments.Where(s => s.CommentTitle.Contains(commentTitle));
                }

                if (!String.IsNullOrEmpty(commentUserName))
                {
                    comments = comments.Where(s => s.CommentUser.Contains(commentUserName));
                }

                return View(comments.ToList());
            }
            else if (User.Identity.IsAuthenticated)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                var userName = user.FirstName + " " + user.LastName;

                var comments = db.Comments.Include(c => c.ArticleComment).Where(c => c.CommentUser == userName);

                if (!String.IsNullOrEmpty(articleTitle))
                {
                    comments = comments.Where(s => s.ArticleComment.Title.Contains(articleTitle));
                }

                if (!String.IsNullOrEmpty(commentTitle))
                {
                    comments = comments.Where(s => s.CommentTitle.Contains(commentTitle));
                }

                if (!String.IsNullOrEmpty(commentUserName))
                {
                    comments = comments.Where(s => s.CommentUser.Contains(commentUserName));
                }

                return View(comments.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin"))
            {
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                var userName = user.FirstName + " " + user.LastName;

                if (userName == comment.CommentUser)
                {
                    return View(comment);
                }
                else
                {
                    return RedirectToAction("PrivilegeError", "News");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }            
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ArticleID = new SelectList(db.Articles, "ID", "Title");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ArticleID,CommentTitle,CommentUser,Text, PublishDate")] Comment comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    string strCurrentUserId = User.Identity.GetUserId();
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

                    comment.CommentUser = user.FirstName + " " + user.LastName;

                    comment.PublishDate = System.DateTime.Now;

                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ArticleID = new SelectList(db.Articles, "ID", "Title", comment.ArticleID);
                return View(comment);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            ViewBag.ArticleID = new SelectList(db.Articles, "ID", "Title", comment.ArticleID);

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ArticleID,CommentTitle,CommentUser,Text, PublishDate")] Comment comment)
        {

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                if (ModelState.IsValid)
                {
                    comment.PublishDate = System.DateTime.Now;
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ArticleID = new SelectList(db.Articles, "ID", "Title", comment.ArticleID);
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


            
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {                
                return View(comment);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            string strCurrentUserId = User.Identity.GetUserId();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(strCurrentUserId);

            var userName = user.FirstName + " " + user.LastName;
            Comment comment = db.Comments.Find(id);

            if (User.IsInRole("Admin") || (userName == comment.CommentUser))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PrivilegeError", "News");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            
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
