﻿using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;

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
                    UserName = "AdminAdmino@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admino",
                    Email = "AdminAdmino@gmail.com",
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


            // creating Creating Employee role    
            if (!roleManager.RoleExists("NormalUser"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "NormalUser";
                roleManager.Create(role);

            }
        }


        private void PopulateTopTenBeaches()
        {
            NewsDbContext db = new NewsDbContext();
            string thisFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            string beachesJsonFile = thisFilePath.Replace("Startup.cs", "beaches.json");

            foreach (var entity in db.Beaches)
                db.Beaches.Remove(entity);
            db.SaveChanges();

            using (StreamReader r = new StreamReader(beachesJsonFile)){
                string json = r.ReadToEnd();
                List<Beach> Beaches = JsonConvert.DeserializeObject<List<Beach>>(json);
                foreach (Beach beach in Beaches){
                    db.Beaches.Add(beach);
                    db.SaveChanges();
                }
            }
           
        }
    }

    
}
