using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Comment
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        [Display(Name = "Comment Title")]
        public string CommentTitle { get; set; }

        public virtual User CommentUser { get; set; }

        public string Text { get; set; }

        public virtual Article ArticleComment { get; set; }
    }
}