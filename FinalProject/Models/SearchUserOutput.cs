using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class SearchUserOutput
    {
        public ApplicationUser User;

        [Display(Name = "Count of articles written by")]
        public int CountOfArticles;

        [Display(Name = "Role type")]
        public string RoleType;

        public SearchUserOutput(ApplicationUser user, int count, string roleType)
        {
            User = user;
            CountOfArticles = count;
            RoleType = roleType;
        }
    }

}