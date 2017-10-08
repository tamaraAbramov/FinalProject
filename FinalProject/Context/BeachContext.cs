using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FinalProject.Context
{
    public class BeachContext : DbContext
    {
        public DbSet<Beach> Beaches { get; set; }
    }
}