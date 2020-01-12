using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema; //For Structure
using System.ComponentModel.DataAnnotations; //For attribute key

namespace Smart_Trix.Models.StData
{
    [Table("tblSidebar")]
    public class StSidebarDT
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
    }
}