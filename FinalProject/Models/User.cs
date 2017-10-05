using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class User
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public int UserType { get; set; }
    }

    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}