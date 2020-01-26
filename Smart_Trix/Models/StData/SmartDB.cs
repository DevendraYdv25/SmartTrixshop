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
        public DbSet<StSidebarDT> Sidebar { get; set; }
        public DbSet<StCategoryDT> Category { get; set; }
        public DbSet<StProductDT> Product { get; set; }
        public DbSet<StUserDT> Users { get; set; }
        public DbSet<StRoleDT> Roles { get; set; }
        public DbSet<StUselRoleDT> UserRoles { get; set; }
        public DbSet<StOrderDT> Orders { get; set; }
        public DbSet<StOrderDetailDT> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.StPages.PageVM> PageVMs { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.StPages.SidebarVM> SidebarVMs { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.StShop.CategoryVM> CategoryVMs { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.StShop.ProductVM> ProductVMs { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.Account.UserVM> UserVMs { get; set; }

        public System.Data.Entity.DbSet<Smart_Trix.Models.StViewModel.Account.UserProfileVM> UserProfileVMs { get; set; }
    }
}