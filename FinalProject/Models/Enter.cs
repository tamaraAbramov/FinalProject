using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Enter
    {
        [Key]
        public int Id { get; set; }
        public int ArticleId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Enter Date")]
        public DateTime EnterDate { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article EnterArticle { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser EnterUser { get; set; }

        public Enter(){
        }
        public Enter(int ArticleId, string UserId){
            this.ArticleId = ArticleId;
            this.UserId = UserId;
            this.EnterDate = DateTime.Now;
        }
    }
}