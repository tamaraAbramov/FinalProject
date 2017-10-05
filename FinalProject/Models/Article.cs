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
        public string ArticleTitle { get; set; }

        public virtual User ArticleAuthor { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Text { get; set; }

        public string PostImage { get; set; }

        public string PostVideo { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}