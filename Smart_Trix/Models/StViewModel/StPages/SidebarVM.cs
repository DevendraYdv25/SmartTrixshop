using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Smart_Trix.Models.StData;
using System.Web.Mvc; //Allow  Html code

namespace Smart_Trix.Models.StViewModel.StPages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }
        public SidebarVM(StSidebarDT yadav)
        {
            Id = yadav.Id;
            Body = yadav.Body;
        }
        [Key]
        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}