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
        [Required]
        public string Title { get; set; }

        public string AuthorID { get; set; }
        
        public string Author { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Article Text")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }
        
        public int SearchCount { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}