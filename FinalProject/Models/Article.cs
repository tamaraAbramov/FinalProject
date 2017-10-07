using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Article
    {
        public int ID { get; set; }

        [Display(Name = "Article Title")]
        public string Title { get; set; }

        //Check
        public virtual User Author { get; set; }

        public DateTime PublishDate { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}