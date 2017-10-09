using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Beach
    {
        public int BeachId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Zoom { get; set; }

        public double Lattitude { get; set; }
        
        public double Longtidute { get; set; }

        public Beach(){
        }
        public Beach(int BeachId, string Name, string Description, double Zoom, double Lattitude, double Longtidute){
            this.BeachId = BeachId;
            this.Name = Name;
            this.Description = Description;
            this.Zoom = Zoom;
            this.Lattitude = Lattitude;
            this.Longtidute = Longtidute;
        }
    }
}