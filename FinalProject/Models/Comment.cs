using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [Display(Name = "Article")]
        public int ArticleID { get; set; }

        [Display(Name = "Comment Title")]
        [Required]
        public string CommentTitle { get; set; }

        [Display(Name = "Written By")]
        public string CommentUser { get; set; }

        public string Text { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        [ForeignKey("ArticleID")]
        public virtual Article ArticleComment { get; set; }
    }
}