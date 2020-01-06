using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Smart_Trix.Models.StData
{
    public class SmartDB : DbContext
    {
        public SmartDB()
            : base("StConnection")
        {
            
        }

        public DbSet<StPageDT> Pages { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.StPages.PageVM> PageVMs { get; set; }
    }
}