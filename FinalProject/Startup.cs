using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(FinalProject.Startup))]
namespace FinalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            PopulateTopTenBeaches();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  
                var user = new ApplicationUser
                {
                    UserName = "Admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admino",
                    Email = "Admin@gmail.com",
                    BirthDate = new DateTime(2001, 01, 01, 9, 0, 0),
                    Gender = "Female"
                };

                var chkUser = UserManager.Create(user, "123Admin*");

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating NormalUser role 
            if (!roleManager.RoleExists("Author"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Author";
                roleManager.Create(role);

                //Create a user                 
                var user = new ApplicationUser
                {
                    UserName = "Plony@gmail.com",
                    FirstName = "Plony",
                    LastName = "Almony",
                    Email = "Plony@gmail.com",
                    BirthDate = new DateTime(2001, 01, 01, 9, 0, 0),
                    Gender = "male"

                };

                var chkUser = UserManager.Create(user, "123Plony*");

                //Add default User to Role  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Author");
                    string authorName = user.FirstName + " " + user.LastName;
                    AddArticles(authorName, user.Id);
                }

                //Create a user                 
                var anotherUser = new ApplicationUser
                {
                    UserName = "Else@gmail.com",
                    FirstName = "Someone",
                    LastName = "Else",
                    Email = "Else@gmail.com",
                    BirthDate = new DateTime(1990, 01, 01, 9, 0, 0),
                    Gender = "male"

                };

                var chkAnotherUser = UserManager.Create(anotherUser, "123Else*");

                //Add default User to Role  
                if (chkAnotherUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(anotherUser.Id, "Author");
                }

            }

            // creating Creating NormalUser role    
            if (!roleManager.RoleExists("NormalUser"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "NormalUser";
                roleManager.Create(role);

            }
        }


        private void PopulateTopTenBeaches()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var result = db.Beaches;

            //  Check if the DB is empty
            if (!result.Any())
            {  
                string thisFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
                string beachesJsonFile = thisFilePath.Replace("Startup.cs", "beaches.json");

                foreach (var entity in db.Beaches)
                    db.Beaches.Remove(entity);
                db.SaveChanges();

                using (StreamReader r = new StreamReader(beachesJsonFile))
                {
                    string json = r.ReadToEnd();
                    List<Beach> Beaches = JsonConvert.DeserializeObject<List<Beach>>(json);
                    foreach (Beach beach in Beaches)
                    {
                        db.Beaches.Add(beach);
                        db.SaveChanges();
                    }
                }
            }
           
        }

        private void AddArticles(string authorAlmony, string authorIDAlmony)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var result = db.Articles;

            //  Check if the DB is empty
            if (!result.Any())
            {
                string thisFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
                string articlesJsonFile = thisFilePath.Replace("Startup.cs", "Articles.json");

                foreach (var entity in db.Articles)
                    db.Articles.Remove(entity);
                db.SaveChanges();

                using (StreamReader r = new StreamReader(articlesJsonFile))
                {
                    

                    string json = r.ReadToEnd();

                    List<Article> Articles = JsonConvert.DeserializeObject<List<Article>>(json);

                    foreach (Article article in Articles)
                    {
                        Random rnd = new Random(DateTime.Now.Millisecond);
                        int month = rnd.Next(0, 10);
                        int minusMonths = (-1) * month;
                        article.Author = authorAlmony;
                        article.AuthorID = authorIDAlmony;
                        article.PublishDate = DateTime.Now.AddMonths(minusMonths) ;

                        db.Articles.Add(article);
                        db.SaveChanges();
                    }
                }
            }
            

        }

    }

    
}
